//===========================================================================================================================================================================================================================================================================
// Name:                Spline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Brief:               A parent class for all splines 
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A parent class for all splines 
    /// </summary>
    [System.Serializable]
    public abstract class Spline
    {
        #region Public Methods
        /// <summary>
        /// The returns the total length of the spline
        /// </summary>
        /// <returns>The total length of the spline</returns>
        public abstract float GetTotalLength();

        /// <summary>
        /// Returns the point at which the spline starts
        /// </summary>
        /// <returns>The point at which the spline starts</returns>
        public abstract Vector3 GetEndPoint();
        /// <summary>
        /// Returns a point along the spline at the given unit interval value
        /// </summary>
        /// <param name="t">The unit interval for how far along the spline the point should</param>
        /// <returns>Get calculated point at the t value</returns>
        public abstract Vector3 GetPointAtTime(float t);
        /// <summary>
        /// Returns the point at which the spline starts
        /// </summary>
        /// <returns>The point at which the spline starts</returns>
        public abstract Vector3 GetStartPoint();

        /// <summary>
        /// Sets the position that line spline should start at
        /// </summary>
        /// <param name="startPoint">The position that line spline should start at</param>
        public abstract void SetStartPoint(Vector3 startPoint);
        /// <summary>
        /// Sets the point that the Line Spline should end at
        /// </summary>
        /// <param name="endPoint">The point that the Line Spline should end at</param>
        public abstract void SetEndPoint(Vector3 endPoint);
        #endregion
    }
}


