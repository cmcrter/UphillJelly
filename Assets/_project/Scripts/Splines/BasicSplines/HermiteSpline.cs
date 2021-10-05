//===========================================================================================================================================================================================================================================================================
// Name:                HermiteSpline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  25-Mar-2020
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
    public class HermiteSpline : Spline
    {
        #region Public Variables
        public Vector3 startPoint;

        public Vector3 endPoint;

        /// <summary>
        /// The tangent connected to the start point
        /// </summary>
        public Vector3 startTangent;

        /// <summary>
        /// The tangent connected to the end point
        /// </summary>
        public Vector3 endTangent;
        #endregion

        #region Serialized Private Fields
        /// <summary>
        /// The name of positions that are sampled along the length of the spline to calculate its distance
        /// </summary>
        [SerializeField]
        [Min(float.Epsilon)]
        [Tooltip("The name of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
        public float distancePrecision = 100f;
        #endregion

        #region Private Variables
        /// <summary>
        /// The total length the spline covers
        /// </summary>
        private float totalLength;
        #endregion

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

        public static float GetTotalLength(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent, float distancePrecision)
        {
            float distance = 0.0f;

            // Sampling a given number of points to get the distance between them to get the whole length of the spline
            Vector3 lastPosition = startPoint;
            float tIncrement = 1.0f / distancePrecision;
            for (float t = 0; t <= 1f; t += tIncrement)
            {
                Vector3 newPosition = GetPointAtTime(startPoint, endPoint, startTangent, endTangent, t);
                distance += Vector3.Distance(newPosition, lastPosition);
                lastPosition = newPosition;
            }
            return distance;
        }

        #region Public Methods
        public override float GetTotalLength()
        {
            return totalLength;
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return GetPointAtTime(startPoint, endPoint, startTangent, endTangent, t);
        }

        public void ConnectToStartOfHermiteSpline(Vector3 startPoint, Vector3 startTangent)
        {
            endPoint = startPoint;
            endTangent = startTangent;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the total length to be correct based on the current spline
        /// </summary>
        private void UpdateLength()
        {
            totalLength = GetTotalLength(startPoint, endPoint, startTangent, endTangent, distancePrecision);
        }

        public override Vector3 GetStartPoint()
        {
            return startPoint;
        }

        public override Vector3 GetEndPoint()
        {
            return endPoint;
        }

        public override void SetStartPoint(Vector3 startPoint)
        {
            this.startPoint = startPoint;
        }

        public override void SetEndPoint(Vector3 endPoint)
        {
            this.endPoint = endPoint;
        }
        #endregion


    }
}
