//======================================================================================================================================================================================================================================================================================================
//  Name:           SplineWrapper.cs
//  Author:         Matthew Mason
//  Date Created:   04/10/2021
//  Brief:          The parent class for any of the MonoBehaviour Script that contain and control a spline in the unity interface
//======================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// The parent class for any of the MonoBehaviour Script that contain and control a spline in the unity interface
    /// </summary>
    public abstract class SplineWrapper : MonoBehaviour
    {
        #region Unity Methods
        protected virtual void Start()
        {
            UpdateWorldPositions();
        }

        protected virtual void OnValidate()
        {
            UpdateWorldPositions();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The point at which at which the line spline ends, would be the position at t 1
        /// </summary>
        public abstract Vector3 WorldEndPosition
        {
            get;
            set;
        }
        /// <summary>
        /// The Point at which the line spline starts, would be the position at t 0
        /// </summary>
        public abstract Vector3 WorldStartPosition
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
        /// Sets the end point of the spline in terms of world coordinates and correct the spline's local values
        /// </summary>
        /// <param name="endPoint">The new world coordinates to set as the end point of the spline</param>
        public abstract void SetWorldEndPointAndUpdateLocal(Vector3 endPoint);
        /// <summary>
        /// Sets the start point of the spline in terms of world coordinates and correct the spline's local values
        /// </summary>
        /// <param name="startPoint">The new world coordinates to set as the start point of the spline</param>
        public abstract void SetWorldStartPointAndUpdateLocal(Vector3 startPoint);
        /// <summary>
        /// Updates the world positions based on the local ones of the contained spline
        /// </summary>
        public abstract void UpdateWorldPositions();
        #endregion
    }
}