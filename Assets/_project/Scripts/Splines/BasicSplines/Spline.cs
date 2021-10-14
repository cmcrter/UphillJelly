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
        #region Public Properties
        /// <summary>
        /// The point at which at which the line spline ends, would be the position at t 1
        /// </summary>
        public abstract Vector3 EndPosition
        {
            get;
            set;
        }
        /// <summary>
        /// The Point at which the line spline starts, would be the position at t 0
        /// </summary>
        public abstract Vector3 StartPosition
        {
            get;
            set;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The returns the total length of the spline
        /// </summary>
        /// <returns>The total length of the spline</returns>
        public abstract float GetTotalLength();

        /// <summary>
        /// Returns a point along the spline at the given unit interval value
        /// </summary>
        /// <param name="t">The unit interval for how far along the spline the point should</param>
        /// <returns>Get calculated point at the t value</returns>
        public abstract Vector3 GetPointAtTime(float t);

        /// <summary>
        /// Returns a direction of the spline at the given t value
        /// </summary>
        /// <param name="t">The t value to check which direction the spline is going at</param>
        /// <param name="stepDistance">Distance to check the next step at, to calculate the direction</param>
        /// <returns>A normalized vector for the direction the spline is moving at a given value</returns>
        public Vector3 GetDirection(float t, float stepDistance)
        {
            return (GetPointAtTime(t + stepDistance) - GetPointAtTime(t)).normalized;
        }
        #endregion
    }
}


