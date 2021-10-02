//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       25-Mar-2020
//  Date Last Modified: 21-Apr-2020
//  Brief:              A child of the spline component for showing make a linked set of Hermite splines
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A child of the spline component for showing make a linked set of Hermite splines
/// </summary>
public class HermiteSplineSequence : Spline
{
    /// <summary>
    /// All of the splines included in the sequence 
    /// </summary>
    [SerializeField]
    private HermiteSpline[] hermiteSplines;

    /// <summary>
    /// If the end of the last spline links to the start of the first spline
    /// </summary>
    [SerializeField]
    private bool isCircuit;

    public override float GetTotalLength()
    {
        float totalLength = 0f;
        if (hermiteSplines != null)
        {
            for (int i = 0; i < hermiteSplines.Length; ++i)
            {
                totalLength += hermiteSplines[i].GetTotalLength();
            }
        }
        return totalLength;
    }

    public override Vector3 GetPointAtTime(float t)
    {
        return GetLengthBasedPoint(t);
    }

    /// <summary>
    /// Returns a point at the given unit interval along the set of splines, weighting all the splines based on their total length
    /// </summary>
    /// <param name="t">The unit interval for how far along the point is in the sequence</param>
    /// <returns>The point at the given unit interval along the set of splines assume, weighting all the splines based on their total length</returns>
    public Vector3 GetLengthBasedPoint(float t)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Using t to find the length along the spline sequence to aim for
        float targetLength = GetTotalLength() * t;

        for (int index = 0; index < hermiteSplines.Length; ++index)
        {
            if (hermiteSplines[index] != null)
            {
                float sectionDistance = hermiteSplines[index].GetTotalLength();
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    return hermiteSplines[index].GetPointAtTime(targetLength / sectionDistance);
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            else
            {
                Debug.LogWarning("A spline was null when getting length based point", gameObject);
            }
        }
        // Return the end point of the last spline if the target length was not within any of the splines
        if (hermiteSplines.Length > 0)
        {
            return hermiteSplines[hermiteSplines.Length - 1].EndPoint;
        }
        else
        {
            Debug.LogError("SplineSequence contained no splines when GetLengthBasedPoint was called", gameObject);
            return Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        if (hermiteSplines != null)
        {
            for (int i = 0; i < hermiteSplines.Length - 1; ++i)
            {
                if (hermiteSplines.Length > 1)
                {
                    if (hermiteSplines[i] != null)
                    {
                        if (hermiteSplines[i + 1] != null)
                        {
                            hermiteSplines[i].ConnectToStartOfHermiteSpline(hermiteSplines[i + 1].WorldStartPoint, hermiteSplines[i + 1].StartTangent);
                        }
                    }
                }
            }
        }
    }

    // TODO: MAKE A CUSTOM INSPECTOR FOR THIS
}
