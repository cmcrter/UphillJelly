//===========================================================================================================================================================================================================================================================================
// Name:                HermiteSpline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  07-10-2021
// Brief:               A spline containing additional tangents for the start and end point that dictate the curve of the spline
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A spline containing additional tangents for the start and end point that dictate the curve of the spline
    /// </summary>
    [System.Serializable]
    public class HermiteSpline : Spline
    {
        #region Public Serialized Fields
        [SerializeField]
        [Min(float.Epsilon)]
        [Tooltip("The name of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
        private float distancePrecision = 100f;

        [SerializeField]
        [Tooltip("The point at which at which the line spline ends, would be the position at t 1")]
        private Vector3 endPoint;
        [SerializeField]
        [Tooltip("The tangent attached to the end of the spline")]
        private Vector3 endTangent;
        [SerializeField]
        [Tooltip("The point at which at which the line spline start, would be the position at t 0")]
        private Vector3 startPoint;
        [SerializeField]
        [Tooltip("The tangent attached to the start of the spline")]
        private Vector3 startTangent;
        #endregion

        #region Private Variables
        /// <summary>
        /// The total length the spline covers
        /// </summary>
        private float totalLength;
        #endregion

        #region Public Properties
        #region Overrides
        public override Vector3 EndPosition
        {
            get
            {
                return endPoint;
            }
            set
            {
                this.endPoint = value;
                UpdateLength();
            }
        }
        public override Vector3 StartPosition
        {
            get
            {
                return startPoint;
            }
            set
            {
                this.startPoint = value;
                UpdateLength();
            }
        }
        #endregion
        /// <summary>
        /// The name of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0
        /// </summary>
        public float DistancePrecision
        {
            get
            {
                return distancePrecision;
            }
            set
            {
                this.distancePrecision = value;
                UpdateLength();
            }
        }

        /// <summary>
        /// The tangent attached to the end of the spline
        /// </summary>
        public Vector3 EndTangent
        {
            get
            {
                return endTangent;
            }
            set
            {
                endTangent = value;
                UpdateLength();
            }
        }
        /// <summary>
        /// The tangent attached to the start of the spline
        /// </summary>
        public Vector3 StartTangent
        {
            get
            {
                return startTangent;
            }
            set
            {
                startTangent = value;
                UpdateLength();
            }
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Returns a given a point on a defined hermite spline at given unit interval t
        /// </summary>
        /// <param name="startPoint">The point at which the hermite spline starts</param>
        /// <param name="endPoint">The point at which the hermite spline ends</param>
        /// <param name="startTangent">The tangent for the start of the hermite spline</param>
        /// <param name="endTangent">The tangent for the end of the hermite<</param>
        /// <param name="t"></param>
        /// <returns>A given a point on a defined hermite spline at given unit interval t</returns>
        public static Vector3 GetPointAtTime(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent, float t)
        {
            // Making sure t is within parameters
            t = Mathf.Clamp01(t);

            // Hermite Spline Calculation
            float tsq = t * t;
            float tcub = tsq * t;

            float h00 = 2 * tcub - 3 * tsq + 1;
            float h01 = -2 * tcub + 3 * tsq;
            float h10 = tcub - 2 * tsq + t;
            float h11 = tcub - tsq;

            return h00 * startPoint + h10 * startTangent + h01 * endPoint + h11 * endTangent;
        }

        /// <summary>
        /// Returns the approximate length of the defined spline
        /// </summary>
        /// <param name="startPoint">The point that the spline starts at</param>
        /// <param name="endPoint">The point that the spline ends at</param>
        /// <param name="startTangent">The tangent for the start spline</param>
        /// <param name="endTangent">The tangent for the end of the spline</param>
        /// <param name="distancePrecision">The number of points that will be </param>
        /// <returns>Approximate length of the defined spline</returns>
        public static float GetTotalLength(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent, float distancePrecision)
        {
            float distance = 0.0f;

            // Sampling a given number of points to get the distance between them to get the whole length of the spline
            Vector3 lastPosition = startPoint;
            float tIncrement = 1.0f / distancePrecision;
            float t = 0;
            int i = 0;
            for (; i <= distancePrecision; ++i)
            {
                Vector3 newPosition = GetPointAtTime(startPoint, endPoint, startTangent, endTangent, t);
                distance += Vector3.Distance(newPosition, lastPosition);
                lastPosition = newPosition;
                t += tIncrement;
            }
            // If distance precision is a decimal then get the end section
            if (i - 1 != distancePrecision)
            {
                Vector3 newPosition = GetPointAtTime(startPoint, endPoint, startTangent, endTangent, Spline.maxTValue);
                distance += Vector3.Distance(newPosition, lastPosition);
            }
            return distance;
        }
        #endregion

        #region Public Methods
        #region Overrides
        /// <summary>
        /// Returns the approximate length of the spline
        /// </summary>
        /// <returns>The approximate length of the spline</returns>
        public override float GetTotalLength()
        {
            return totalLength;
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return GetPointAtTime(startPoint, endPoint, startTangent, endTangent, t);
        }
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the total length to be correct based on the current spline
        /// </summary>
        private void UpdateLength()
        {
            totalLength = GetTotalLength(startPoint, endPoint, startTangent, endTangent, distancePrecision);
        }
        #endregion
    }
}
