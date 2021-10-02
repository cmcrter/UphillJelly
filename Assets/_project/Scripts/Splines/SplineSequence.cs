//===========================================================================================================================================================================================================================================================================
// Name:                SplineSequence.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A component for getting a sequence of splines and lerping through them
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component for getting a sequence of splines and lerping through them
/// </summary>
public class SplineSequence : MonoBehaviour
{
    #region Serialized Private Fields
    /// <summary>
    /// If checked it will make sure that the ends of each spline connect to the start of the next one
    /// </summary>
    [SerializeField] [Tooltip("If checked it will make sure that the ends of each spline connect to the start of the next one")]
    private bool connectEnds = false;

    /// <summary>
    /// If checked it will make sure that the end of the last spline connects to the start of the first one
    /// </summary>
    [SerializeField] [Tooltip("If checked it will make sure that the end of the last spline connects to the start of the first one")]
    private bool isCircuit = false;

    /// <summary>
    /// The different splines contained within the sequence
    /// </summary>
    [SerializeField]
    [Tooltip("The different splines contained within the sequence")]
    private List<Spline> containedSplines = new List<Spline>();
    #endregion

    #region Publicly Retrievable Variables
    /// <summary>
    /// The total length of all the splines combined
    /// </summary>
    public float totalLength
    {
        get;
        private set;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Returns how many spline are within the this sequence
    /// </summary>
    /// <returns>How many spline are within the this sequence</returns>
    public int GetNumberOfSplines()
    {
        return containedSplines.Count;
    }

    /// <summary>
    /// Returns a spline at the given index
    /// </summary>
    /// <param name="index">The index to get the spline at</param>
    /// <returns>A spline at the given index, will be null if it is out of range</returns>
    public Spline GetSpline(int index)
    {
        if (containedSplines.Count > index && index > 0)
        {
            return containedSplines[index];
        }
        return null;
    }

    /// <summary>
    /// Returns a point at the given unit interval along the set of splines, giving all the splines the same weighting in terms of length
    /// </summary>
    /// <param name="t">The unit interval for how far along the point is in the sequence</param>
    /// <returns>The point at the given unit interval along the set of splines assume, giving all the splines the same weighting in terms of length</returns>
    public Vector3 GetPoint(float t)
    {
        // Ensuring t is a unit interval
        t = Mathf.Clamp01(t);

        // Calculating which spline the point is on and how far it is along that spline
        int numberOfSections = containedSplines.Count - 1;
        float longT = (float)numberOfSections * t;  // Get the value for how many splines it is along the sequence
        int index = Mathf.FloorToInt(longT);        // Get the index for the spline that the point is on
        float newT = longT - (float)index;          // Get the unit interval for how far along the point on that spline

        if (containedSplines[index] != null)
        {
            return containedSplines[index].GetPointAtTime(newT); // Use this index and unit interval to get the point from the spline
        }
        else
        {
            Debug.Log("A spline at index " + index.ToString() + " was null when referenced in GetPoint method", gameObject);
            return Vector3.zero;
        }
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
        float targetLength = totalLength * t;

        for (int index = 0; index < containedSplines.Count; ++index)
        {
            if (containedSplines[index] != null)
            {
                float sectionDistance = containedSplines[index].GetTotalLength();
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    return containedSplines[index].GetPointAtTime(targetLength / sectionDistance);
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
        if (containedSplines.Count > 0)
        {
            return containedSplines[containedSplines.Count - 1].EndPoint;
        }
        else
        {
            Debug.LogError("SplineSequence contained no splines when GetLengthBasedPoint was called", gameObject);
            return Vector3.zero;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spline">The spline to add to the sequence, it will have its start point corrected to fit the sequence</param>
    public void AddSpline(Spline spline)
    {
        containedSplines.Add(spline);
        UpdateSplines();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Updates the positions of splines based on the given booleans and contained splines
    /// </summary>
    private void UpdateSplines()
    {
        if (connectEnds)
        {
            if (containedSplines.Count > 1)
            {
                for (int i = 0; i < containedSplines.Count - 1; ++i)
                {
                    if (containedSplines[i] != null && containedSplines[i + 1] != null)
                    {
                        containedSplines[i].WorldEndPoint = containedSplines[i + 1].WorldStartPoint;
                    }

                }
            }
        }
        if (isCircuit)
        {
            if (containedSplines.Count > 1)
            {
                if (containedSplines[containedSplines.Count - 1] != null && containedSplines[0] != null)
                {
                    containedSplines[containedSplines.Count - 1].WorldEndPoint = containedSplines[0].WorldStartPoint;
                }
            }
        }
        UpdateTotalDistance();
    }

    /// <summary>
    /// Update the total distance based on the contained splines
    /// </summary>
    private void UpdateTotalDistance()
    {
        totalLength = 0;
        for (int i = 0; i < containedSplines.Count; ++i)
        {
            if (containedSplines[i] != null)
            {
                totalLength += containedSplines[i].GetTotalLength();
            }
            else
            {
                Debug.LogWarning("A spline was null when updating distance", gameObject);
            }
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        UpdateSplines();
    }
    private void OnDrawGizmos()
    {
        UpdateSplines();
    }
    #endregion
}
