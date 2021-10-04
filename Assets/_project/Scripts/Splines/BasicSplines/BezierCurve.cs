//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       25-Mar-2020
//  Date Last Modified: 25-Mar-2020
//  Brief:              A child of the spline component for showing make a Bezier curve 
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A child of the spline component for showing make a Bezier curve 
/// </summary>
public class BezierCurve : Spline
{
    #region Public Constants
    /// <summary>
    /// The index in the additional points array the first additional point would be
    /// </summary>
    public const int firstAddtionalPointIndex = 0;
    /// <summary>
    /// The index in the additional points array the second additional point would be
    /// </summary>
    public const int secondAddtionalPointIndex = 1;
    #endregion

    #region Private Serialized Fields
    /// <summary>
    /// If the Bezier curve should be based around 4 points instead of 3
    /// </summary>
    [SerializeField]
    [Tooltip("If the Bezier curve should be based around 4 points instead of 3")]
    public bool isFourPoint = false;

    /// <summary>
    /// The name of positions that are sampled along the length of the spline to calculate its distance
    /// </summary>
    [SerializeField]
    [Min(float.Epsilon)]
    [Tooltip("The number of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
    public float distancePrecision = 10;

    /// <summary>
    /// The additional points needed on top of the start and end point
    /// </summary>
    [SerializeField] [HideInInspector]
    public Vector3[] controlPoints = null;

    private Vector3 startPoint;

    private Vector3 endPoint;
    #endregion

    public static Vector3 GetThreePointPositionAtTime(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(startPoint, controlPoint, t),
                Vector3.Lerp(controlPoint, endPoint, t), t);
    }

    public static Vector3 GetFourPointPositionAtTime(Vector3 startPoint, Vector3 endPoint, 
        Vector3 controlPointOne, Vector3 controlPointTwo, float t)
    {
        Vector3[] lerpsBetweenPointsInSequence = new Vector3[3]
        {
                Vector3.Lerp(startPoint, controlPointOne, t),       // Between point 0 and 1
                Vector3.Lerp(controlPointOne, controlPointTwo, t),  // Between point 1 and 2
                Vector3.Lerp(controlPointTwo, endPoint,t)           // Between point 2 and 3
        };

        return Vector3.Lerp(Vector3.Lerp(lerpsBetweenPointsInSequence[0], lerpsBetweenPointsInSequence[1], t),
            Vector3.Lerp(lerpsBetweenPointsInSequence[1], lerpsBetweenPointsInSequence[2], t), t);
    }

    public static float GetThreePointTotalLength(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float distancePrecision)
    {
        float distance = 0.0f;

        // Sampling a given number of points to get the distance between them to get the whole length of the spline
        Vector3 lastPosition = startPoint;
        float tIncrement = 1.0f / distancePrecision;
        for (float t = 0; t <= 1f; t += tIncrement)
        {
            Vector3 newPosition = GetThreePointPositionAtTime(startPoint, endPoint, controlPoint, t);
            distance += Vector3.Distance(newPosition, lastPosition);
            lastPosition = newPosition;
        }
        return distance;
    }

    public static float GetFourPointTotalLength(Vector3 startPoint, Vector3 endPoint, 
        Vector3 controlPointOne, Vector3 controlPointTwo, float distancePrecision)
    {
        float distance = 0.0f;

        // Sampling a given number of points to get the distance between them to get the whole length of the spline
        Vector3 lastPosition = startPoint;
        float tIncrement = 1.0f / distancePrecision;
        for (float t = 0; t <= 1f; t += tIncrement)
        {
            Vector3 newPosition = GetFourPointPositionAtTime(startPoint, endPoint, controlPointOne, controlPointTwo, t);
            distance += Vector3.Distance(newPosition, lastPosition);
            lastPosition = newPosition;
        }
        return distance;
    }


    #region Public Methods
    public override float GetTotalLength()
    {
        if (isFourPoint)
        {
            return GetFourPointTotalLength(startPoint, endPoint, 
                controlPoints[firstAddtionalPointIndex], controlPoints[secondAddtionalPointIndex], distancePrecision);
        }
        else
        {
            return GetThreePointTotalLength(startPoint, endPoint, controlPoints[firstAddtionalPointIndex], distancePrecision);
        }
    }

    public override Vector3 GetPointAtTime(float t)
    {
        if (isFourPoint)
        {
            return GetFourPointPositionAtTime(t);
        }
        else
        {
            return GetThreePointPositionAtTime(t);
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Returns the a point along the spline at a given unit interval assuming it has is working with 3 different points
    /// </summary>
    /// <param name="t">The unit interval</param>
    /// <returns>A point along the spline at a given unit interval</returns>
    private Vector3 GetThreePointPositionAtTime(float t)
    {
        return GetThreePointPositionAtTime(startPoint, endPoint, controlPoints[firstAddtionalPointIndex], t);
    }

    /// <summary>
    /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
    /// </summary>
    /// <param name="t">The unit interval</param>
    /// <returns>A point along the spline at a given unit interval</returns>
    private Vector3 GetFourPointPositionAtTime(float t)
    {
        return GetFourPointPositionAtTime(startPoint, endPoint,
            controlPoints[firstAddtionalPointIndex], controlPoints[secondAddtionalPointIndex], t);
    }

    public override Vector3 GetStartPoint()
    {
        return startPoint;
    }

    public override Vector3 GetEndPoint()
    {
        return endPoint;
    }

    public override void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
    }

    public override void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }
    #endregion
}
