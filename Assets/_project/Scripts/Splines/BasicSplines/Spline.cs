//===========================================================================================================================================================================================================================================================================
// Name:                Spline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Brief:               A parent class for all splines 
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Utility.Splines
{
    /// <summary>
    /// A parent class for all splines 
    /// </summary>
    [System.Serializable]
    public abstract class Spline
    {
        #region Public Constants
        /// <summary>
        /// The maximum value that t can be given for assessing a point along the spline
        /// </summary>
        public const float maxTValue = 1.0f;

        /// <summary>
        /// The minimum value
        /// </summary>
        public const float minTValue = 0.0f;
        #endregion

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

        /// <summary>
        /// Gets the length of a defined Bezier curve spline with a single control point
        /// </summary>
        /// <param name="startPoint">The point at which the spline starts</param>
        /// <param name="endPoint">The point at which the spline ends</param>
        /// <param name="controlPoint">The single control point of the Bezier curve</param>
        /// <param name="distancePrecision">How many points will be samples along the spline to get its length</param>
        /// <returns>The length of the spline, slightly smaller than its real length</returns>
        public static float GetTotalLengthOfSpline(Spline spline, float distancePrecision)
        {
            float distance = 0.0f;

            // Sampling a given number of points to get the distance between them to get the whole length of the spline
            Vector3 lastPosition = spline.StartPosition;
            float tIncrement = 1.0f / distancePrecision;
            float t = 0;
            int i = 0;
            for (; i <= distancePrecision; ++i)
            {
                Vector3 newPosition = spline.GetPointAtTime(t);
                distance += Vector3.Distance(newPosition, lastPosition);
                lastPosition = newPosition;
                t += tIncrement;
            }
            // If distance precision is a decimal then get the end section
            if (i - 1 != distancePrecision)
            {
                Vector3 newPosition = spline.GetPointAtTime(1f);
                distance += Vector3.Distance(newPosition, lastPosition);
            }
            return distance;
        }

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
        /// Returns a direction of the spline at the given t value, uses the length of the spline to judge how far ahead to check for the direction
        /// </summary>
        /// <param name="t">The t value to check which direction the spline is going at</param>
        /// <returns>A normalized vector for the direction the spline is moving at a given value</returns>
        public virtual Vector3 GetDirection(float t)
        {
            float baseStep = 0.1f;
            float length = GetTotalLength();
            if (length < baseStep)
            {
                baseStep = Mathf.Max(float.Epsilon, length * 0.001f);
            }
            return GetDirection(t, baseStep / GetTotalLength());
        }
        /// <summary>
        /// Returns a direction of the spline at the given t value
        /// </summary>
        /// <param name="t">The t value to check which direction the spline is going at</param>
        /// <param name="stepDistance">Distance to check the next step at, to calculate the direction</param>
        /// <returns>A normalized vector for the direction the spline is moving at a given value</returns>
        public virtual Vector3 GetDirection(float t, float stepDistance)
        {
            Mathf.Clamp01(t);
            // If t is one then don
            if (t == maxTValue)
            {
                return (GetPointAtTime(t) - GetPointAtTime(t - stepDistance)).normalized;
            }
            if (t + stepDistance > maxTValue)
            {
                return (GetPointAtTime(maxTValue) - GetPointAtTime(t)).normalized;
            }
            return (GetPointAtTime(t + stepDistance) - GetPointAtTime(t)).normalized;
        }
        #endregion
    }
}


