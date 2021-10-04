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
        //[SerializeField]
        //[Tooltip("The spline that this class wraps up and controls")]
        //protected SplineType spline;

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

        public abstract Vector3 GetWorldStartPoint();

        public abstract Vector3 GetWorldEndPoint();

        public abstract void SetWorldStartPoint(Vector3 startPoint);

        public abstract void SetWorldEndPoint(Vector3 endPoint);

        public abstract void UpdateWorldPositions();
    }
}