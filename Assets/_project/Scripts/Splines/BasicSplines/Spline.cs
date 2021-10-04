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
[System.Serializable]
public abstract class Spline
{
    //#region Public Fields
    ///// <summary>
    ///// The point at which the spline starts
    ///// </summary>
    //public Vector3 startPoint;

    ///// <summary>
    ///// The point at which the spline ends
    ///// </summary>
    //public Vector3 endPoint;
    //#endregion

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

    public abstract Vector3 GetStartPoint();

    public abstract Vector3 GetEndPoint();

    public abstract void SetStartPoint(Vector3 startPoint);

    public abstract void SetEndPoint(Vector3 endPoint);
}
