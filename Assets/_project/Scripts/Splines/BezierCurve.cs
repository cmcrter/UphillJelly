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
    #region private consts
    /// <summary>
    /// The index in the additional points array the first additional point would be
    /// </summary>
    private const int firstAddtionalPointIndex = 0;
    /// <summary>
    /// The index in the additional points array the second additional point would be
    /// </summary>
    private const int secondAddtionalPointIndex = 1;
    #endregion

    #region Private Serialized Fields
    /// <summary>
    /// If the Bezier curve should be based around 4 points instead of 3
    /// </summary>
    [SerializeField]
    [Tooltip("If the Bezier curve should be based around 4 points instead of 3")]
    private bool isFourPoint = false;

    /// <summary>
    /// The name of positions that are sampled along the length of the spline to calculate its distance
    /// </summary>
    [SerializeField]
    [Min(float.Epsilon)]
    [Tooltip("The number of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
    private float distancePrecision = 10;

    /// <summary>
    /// The additional points needed on top of the start and end point
    /// </summary>
    [SerializeField] [HideInInspector]
    private Vector3[] addtionalPoints = null;
    #endregion

    #region Private Variables
    /// <summary>
    /// The additional points needed on top of the start and end point in world space
    /// </summary>
    private Vector3[] worldAddtionalPoints;
    #endregion

    #region Public Methods
    public override float GetTotalLength()
    {
        float distance = 0.0f;

        // Sampling a given number of points to get the distance between them to get the whole length of the spline
        Vector3 lastPosition = StartPoint;
        float tIncrement = 1.0f / distancePrecision;
        for (float t = 0; t <= 1f; t += tIncrement)
        {
            Vector3 newPosition = GetPointAtTime(t);
            distance += Vector3.Distance(newPosition, lastPosition);
            lastPosition = newPosition;
        }
        return distance;
    }

    public Vector3 GetAdditionalWorldPoint(int index)
    {
        UpdateWorldPositions();
        if (index < worldAddtionalPoints.Length)
        {
            return worldAddtionalPoints[index];
        }
        return Vector3.zero;
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

    public void SetAdditionalWorldPoint(int index, Vector3 position)
    {
        UpdateWorldPositions();
        if (index < worldAddtionalPoints.Length)
        {
            worldAddtionalPoints[index] = position;
            addtionalPoints[index] = transform.InverseTransformPoint(worldAddtionalPoints[index]);
        }
    }
    #endregion

    #region Protected Methods
    public override void UpdateWorldPositions()
    {
        base.UpdateWorldPositions();
        worldAddtionalPoints = new Vector3[addtionalPoints.Length];
        for (int i = 0; i < worldAddtionalPoints.Length; ++i)
        {
            worldAddtionalPoints[i] = transform.TransformPoint(addtionalPoints[i]);
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
        return Vector3.Lerp(Vector3.Lerp(WorldStartPoint, worldAddtionalPoints[firstAddtionalPointIndex], t),
            Vector3.Lerp(worldAddtionalPoints[firstAddtionalPointIndex], WorldEndPoint, t), t);
    }

    /// <summary>
    /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
    /// </summary>
    /// <param name="t">The unit interval</param>
    /// <returns>A point along the spline at a given unit interval</returns>
    private Vector3 GetFourPointPositionAtTime(float t)
    {
        if (worldAddtionalPoints == null)
        {
            UpdateWorldPositions();
        }
        else if ( worldAddtionalPoints.Length == 1)
        {
            UpdateWorldPositions();
        }

        Vector3[] lerpsBetweenPointsInSequence = new Vector3[3]
            {
                 Vector3.Lerp(WorldStartPoint, worldAddtionalPoints[firstAddtionalPointIndex], t),                               // Between point 0 and 1
                 Vector3.Lerp(worldAddtionalPoints[firstAddtionalPointIndex], worldAddtionalPoints[secondAddtionalPointIndex], t),    // Between point 1 and 2
                 Vector3.Lerp(worldAddtionalPoints[secondAddtionalPointIndex], WorldEndPoint,t)                                  // Between point 2 and 3
            };

        return Vector3.Lerp(Vector3.Lerp(lerpsBetweenPointsInSequence[0], lerpsBetweenPointsInSequence[1], t),
            Vector3.Lerp(lerpsBetweenPointsInSequence[1], lerpsBetweenPointsInSequence[2], t), t);

    }
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        UpdateWorldPositions();

        for (int i =0; i < addtionalPoints.Length; ++i)
        {
            Gizmos.DrawWireCube(worldAddtionalPoints[i], new Vector3(1,1,1));
        }

        // Drawing line between all the sampled points to represent the spline
        float tIncrement = 1.0f / distancePrecision;
        for (float f = 0.0f; f < 1f;)
        {
            Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
            Gizmos.DrawLine(GetPointAtTime(f), GetPointAtTime(f += tIncrement));
        }
    }
    #endregion
}
