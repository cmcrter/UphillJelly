//===========================================================================================================================================================================================================================================================================
// Name:                Spline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
// Brief:               A parent component for any splines that are contained within
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A parent class for all the splines 
/// </summary>
public abstract class Spline : MonoBehaviour
{
    #region Public Fields
    /// <summary>
    /// The point at which the spline starts local to the object attached to
    /// </summary>
    [SerializeField]
    [Tooltip("The point at which the spline starts")]
    private Vector3 startPoint;
    public Vector3 StartPoint
    {
        get
        {
            return startPoint;
        }
        set
        {
            startPoint = value;
            worldStartPoint = transform.TransformPoint(value);
        }
    }

    /// <summary>
    /// The point at which the spline ends local to the object attached to
    /// </summary>
    [SerializeField]
    [Tooltip("The point at which the spline ends")]
    private Vector3 endPoint;
    public Vector3 EndPoint
    {
        get
        {
            return endPoint;
        }
        set
        {
            endPoint = value;
            worldEndPoint = transform.TransformPoint(value);

        }
    }
    #endregion

    #region Public Variables
    /// <summary>
    /// The actual start point in world space
    /// </summary>
    private Vector3 worldStartPoint;
    /// <summary>
    /// The actual start point in world space (Property accessor)
    /// </summary>
    public Vector3 WorldStartPoint
    {
        get
        {
            return worldStartPoint;
        }
        set
        {
            worldStartPoint = value;
            startPoint = transform.InverseTransformPoint(value);
        }
    }

    /// <summary>
    /// The actual start point in world space
    /// </summary>
    private Vector3 worldEndPoint;
    /// <summary>
    /// The actual start point in world space (Property accessor)
    /// </summary>
    public Vector3 WorldEndPoint
    {
        get
        {
            return worldEndPoint;
        }
        set
        {
            worldEndPoint = value;
            endPoint = transform.InverseTransformPoint(value);
        }
    }
    #endregion

/// <summary>
/// Returns a point along the spline at the given unit interval value
/// </summary>
/// <param name="t">The unit interval for how far along the spline the point should</param>
/// <returns>Get calculated point at the t value</returns>
public abstract Vector3 GetPointAtTime(float t);

    /// <summary>
    /// The returns the total length of the spline
    /// </summary>
    /// <returns>The total length of the spline</returns>
    public abstract float GetTotalLength();

    /// <summary>
    /// Updates the world positions of the point that make up the spline based on the local ones
    /// </summary>
    public virtual void UpdateWorldPositions()
    {
        worldStartPoint = transform.TransformPoint(startPoint);
        worldEndPoint = transform.TransformPoint(endPoint);
    }

    protected virtual void Start()
    {
        UpdateWorldPositions();
    }

    protected virtual void OnValidate()
    {
        UpdateWorldPositions();
    }
}
