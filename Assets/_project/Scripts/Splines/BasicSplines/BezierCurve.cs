//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       25-Mar-2020
//  Date Last Modified: 25/10/2021
//  Brief:              A child of the spline component for showing make a Bezier curve 
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A child of the spline component for showing make a Bezier curve 
    /// </summary>
    [System.Serializable]
    public class BezierCurve : Spline
    {
        #region Public Constants
        /// <summary>
        /// The index in the additional points array the first additional point would be
        /// </summary>
        public const int firstAddtionalPointIndex = 0;
        /// <summary>
        /// The index in the additional points array the second additional point would be
        /// </summary>
        public const int secondAddtionalPointIndex = 1;
        #endregion

        #region Private Serialized Fields
        /// <summary>
        /// If the Bezier curve should be based around 4 points instead of 3
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private bool isTwoControlPoint = false;

        /// <summary>
        /// The name of positions that are sampled along the length of the spline to calculate its distance
        /// </summary>
        [SerializeField]
        [Min(float.Epsilon)]
        [Tooltip("The number of positions that are sampled along the length of the spline to calculate its distance, Cannot be less that 0")]
        public float distancePrecision = 10;

        [SerializeField]
        [Tooltip("The additional points needed on top of the start and end point")]
        public Vector3[] controlPoints = new Vector3[2];
        [SerializeField]
        [Tooltip("The point at which the spline ends")]
        private Vector3 endPoint;
        [SerializeField]
        [Tooltip("The point at which the spline starts")]
        private Vector3 startPoint;
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Gets the length of a defined Bezier curve spline with a single control point
        /// </summary>
        /// <param name="startPoint">The point at which the spline starts</param>
        /// <param name="endPoint">The point at which the spline ends</param>
        /// <param name="controlPoint">The single control point of the Bezier curve</param>
        /// <param name="distancePrecision">How many points will be samples along the spline to get its length</param>
        /// <returns>The length of the spline, slightly smaller than its real length</returns>
        public static float GetTotalLengthSingleControlPoint(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float distancePrecision)
        {
            float distance = 0.0f;

            // Sampling a given number of points to get the distance between them to get the whole length of the spline
            Vector3 lastPosition = startPoint;
            float tIncrement = 1.0f / distancePrecision;
            for (float t = 0; t <= 1f; t += tIncrement)
            {
                Vector3 newPosition = GetPositionAtTimeSingleControlPoint(startPoint, endPoint, controlPoint, t);
                distance += Vector3.Distance(newPosition, lastPosition);
                lastPosition = newPosition;
            }
            return distance;
        }
        /// <summary>
        /// Gets the length of a defined Bezier curve spline with a control point for the start and end
        /// </summary>
        /// <param name="startPoint">The point at which the spline starts</param>
        /// <param name="endPoint">The point at which the spline ends</param>
        /// <param name="controlPointOne">The control point for the beginning of the Bezier curve</param>
        /// <param name="controlPointTwo">The control point for the end of the Bezier curve</param>
        /// <param name="distancePrecision">How many points will be samples along the spline to get its length</param>
        /// <returns>The length of the spline, slightly smaller than its real length</returns>
        public static float GetTotalLengthTwoControlPoints(Vector3 startPoint, Vector3 endPoint,
            Vector3 controlPointOne, Vector3 controlPointTwo, float distancePrecision)
        {
            float distance = 0.0f;

            // Sampling a given number of points to get the distance between them to get the whole length of the spline
            Vector3 lastPosition = startPoint;
            float tIncrement = 1.0f / distancePrecision;
            for (float t = 0; t <= 1f; t += tIncrement)
            {
                Vector3 newPosition = GetPositionAtTimeTwoControlPoints(startPoint, endPoint, controlPointOne, controlPointTwo, t);
                distance += Vector3.Distance(newPosition, lastPosition);
                lastPosition = newPosition;
            }
            return distance;
        }

        /// <summary>
        /// Used to get a position at t along a defined Bezier curve spline using a single control point 
        /// </summary>
        /// <param name="startPoint">The point at which the spline starts</param>
        /// <param name="endPoint">The point at which the spline ends</param>
        /// <param name="controlPoint">The single control point of the Bezier curve</param>
        /// <param name="t">The unit interval define how far along the spline to get the point</param>
        /// <returns>The position at the unit interval along the given spline parameters using the Bezier formula</returns>
        public static Vector3 GetPositionAtTimeSingleControlPoint(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float t)
        {
            return Vector3.Lerp(Vector3.Lerp(startPoint, controlPoint, t),
                    Vector3.Lerp(controlPoint, endPoint, t), t);
        }
        /// <summary>
        /// Used to get a position at t along a defined Bezier curve spline using a control point for the start and end
        /// </summary>
        /// <param name="startPoint">The point at which the spline starts</param>
        /// <param name="endPoint">The point at which the spline ends</param>
        /// <param name="controlPointOne">The control point for the beginning of the Bezier curve</param>
        /// <param name="controlPointTwo">The control point for the end of the Bezier curve</param>
        /// <param name="t">The unit interval define how far along the spline to get the point</param>
        /// <returns>The position at the unit interval along the given spline parameters using the Bezier formula</returns>
        public static Vector3 GetPositionAtTimeTwoControlPoints(Vector3 startPoint, Vector3 endPoint,
            Vector3 controlPointOne, Vector3 controlPointTwo, float t)
        {
            Vector3[] lerpsBetweenPointsInSequence = new Vector3[3]
            {
                Vector3.Lerp(startPoint, controlPointOne, t),       // Between point 0 and 1
                Vector3.Lerp(controlPointOne, controlPointTwo, t),  // Between point 1 and 2
                Vector3.Lerp(controlPointTwo, endPoint,t)           // Between point 2 and 3
            };

            return Vector3.Lerp(Vector3.Lerp(lerpsBetweenPointsInSequence[0], lerpsBetweenPointsInSequence[1], t),
                Vector3.Lerp(lerpsBetweenPointsInSequence[1], lerpsBetweenPointsInSequence[2], t), t);
        }
        #endregion

        #region Public Methods
        #region Overrides
        public override float GetTotalLength()
        {
            if (isTwoControlPoint)
            {
                return GetTotalLengthTwoControlPoints(startPoint, endPoint,
                    controlPoints[firstAddtionalPointIndex], controlPoints[secondAddtionalPointIndex], distancePrecision);
            }
            else
            {
                return GetTotalLengthSingleControlPoint(startPoint, endPoint, controlPoints[firstAddtionalPointIndex], distancePrecision);
            }
        }

        public override Vector3 GetEndPoint()
        {
            return endPoint;
        }
        public override Vector3 GetPointAtTime(float t)
        {
            if (isTwoControlPoint)
            {
                return GetPositionAtTimeTwoControlPoints(t);
            }
            else
            {
                return GetPositionAtTimeSingleControlPoint(t);
            }
        }
        public override Vector3 GetStartPoint()
        {
            return startPoint;
        }

        public override void SetEndPoint(Vector3 endPoint)
        {
            this.endPoint = endPoint;
        }
        public override void SetStartPoint(Vector3 startPoint)
        {
            this.startPoint = startPoint;
        }
        #endregion

        /// <summary>
        /// Returns if this Bezier Curve is using two control points or one
        /// </summary>
        /// <returns>If this Bezier Curve is using two control points or one</returns>
        public bool GetIsTwoControlPoint()
        {
            return isTwoControlPoint;
        }

        /// <summary>
        /// Set weather this Bezier Curve should be using two control points or one
        /// </summary>
        /// <param name="isTwoControlPoint">Weather this Bezier Curve should be using two control points or one</param>
        public void SetIsFourPoint(bool isTwoControlPoint)
        {
            // Adjust control point array size accordingly then set the boolean
            if (isTwoControlPoint)
            {
                Vector3[] newControlPointsArray = new Vector3[2];
                for (int i = 0; i < controlPoints.Length; ++i)
                {
                    newControlPointsArray[i] = controlPoints[i];
                }
                this.isTwoControlPoint = isTwoControlPoint;
            }
            else
            {
                Vector3[] newControlPointsArray = new Vector3[1];
                newControlPointsArray[0] = controlPoints[0];
                controlPoints = newControlPointsArray;
                this.isTwoControlPoint = isTwoControlPoint;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 3 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetPositionAtTimeSingleControlPoint(float t)
        {
            return GetPositionAtTimeSingleControlPoint(startPoint, endPoint, controlPoints[firstAddtionalPointIndex], t);
        }

        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetPositionAtTimeTwoControlPoints(float t)
        {
            return GetPositionAtTimeTwoControlPoints(startPoint, endPoint,
                controlPoints[firstAddtionalPointIndex], controlPoints[secondAddtionalPointIndex], t);
        }
        #endregion
    }
}
