//======================================================================================================================================================================================================================================================================================================
//  Name:           SplineWrapper.cs
//  Author:         Matthew Mason
//  Date Created:   04/10/2021
//  Brief:          The parent class for any of the MonoBehaviour Script that contain and control a spline
//======================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// The parent class for any of the MonoBehaviour Script that contain and control a spline
    /// </summary>
    /// <typeparam name="SplineType">The spline type that is wrapped up in the inheriting class</typeparam>
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

        #region Public Methods
        /// <summary>
        /// The returns the total length of the spline
        /// </summary>
        /// <returns>The total length of the spline</returns>
        public abstract float GetTotalLength();

        /// <summary>
        /// Returns the point at which at which the line spline ends, would be the position at t 1
        /// </summary>
        /// <returns>The point at which at which the line spline ends, would be the position at t 1</returns>
        public abstract Vector3 GetWorldEndPoint();
        /// <summary>
        /// Returns a point along the spline at the given unit interval value
        /// </summary>
        /// <param name="t">The unit interval for how far along the spline the point should</param>
        /// <returns>Get calculated point at the t value</returns>
        public abstract Vector3 GetPointAtTime(float t);
        /// <summary>
        /// Returns the point at which the line spline starts, would be the position at t 0
        /// </summary>
        /// <returns>The point at which at which the line spline ends, would be the position at t 0</returns>
        public abstract Vector3 GetWorldStartPoint();

        /// <summary>
        /// Sets the end point of the spline in terms of world coordinates
        /// </summary>
        /// <param name="endPoint">The new world coordinates to set as the end point of the spline</param>
        /// <param name="updateLocalPosition">If the splines local coordinates should be updated as well (so the local coordinates changes can be applied to the world by making this false)</param>
        public abstract void SetWorldEndPoint(Vector3 endPoint, bool updateLocalPosition);
        /// <summary>
        /// Sets the start point of the spline in terms of world coordinates
        /// </summary>
        /// <param name="startPoint">The new world coordinates to set as the start point of the spline</param>
        /// <param name="updateLocalPosition">If the splines local coordinates should be updated as well (so the local coordinates changes can be applied to the world by making this false)</param>
        public abstract void SetWorldStartPoint(Vector3 startPoint, bool updateLocalPosition);
        /// <summary>
        /// Updates the world positions based on the local ones of the contained spline
        /// </summary>
        public abstract void UpdateWorldPositions();
        #endregion
    }
}