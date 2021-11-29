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
        ///  The default number of samples that are used when drawing or calculating the length of the spline
        /// </summary>
        public const float defaultDistancePrecision = 20f;

        /// <summary>
        /// The index in the additional points array the first additional point would be
        /// </summary>
        public const int firstControlPointIndex = 0;
        /// <summary>
        /// The index in the additional points array the second additional point would be
        /// </summary>
        public const int secondControlPointIndex = 1;
        /// <summary>
        /// The max number of possible control points accessible
        /// </summary>
        public const int maxNumberOfControlPoints = 2;
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
        private Vector3[] controlPoints;
        [SerializeField]
        [Tooltip("The point at which the spline ends")]
        private Vector3 endPoint;
        [SerializeField]
        [Tooltip("The point at which the spline starts")]
        private Vector3 startPoint;
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
            }
        }
        #endregion

        /// <summary>
        /// The number of control points the curve is currently using
        /// </summary>
        public int NumberOfControlPoints
        {
            get
            {
                return isTwoControlPoint ? 2 : 1;
            }
        }
        /// <summary>
        /// The first control point of the bezier curve 
        /// </summary>
        public Vector3 FirstControlPoint
        {
            get
            {
                if (controlPoints == null)
                {
                    SetUpControlPoints();
                }
                return controlPoints[firstControlPointIndex];
            }
            set
            {
                if (controlPoints == null)
                {
                    SetUpControlPoints();
                }
                controlPoints[firstControlPointIndex] = value;
            }
        }
        /// <summary>
        /// The second control point of the bezier curve 
        /// </summary>
        public Vector3 SecondControlPoint
        {
            get
            {
                if (!isTwoControlPoint)
                {
                    Debug.LogWarning("Second control point should not be accessed on a bezier curve marked as one control point ");
                    SetIsTwoControlPoint(true);
                }
                if (controlPoints == null)
                {
                    SetUpControlPoints();
                }
                return controlPoints[secondControlPointIndex];
            }
            set
            {
                if (!isTwoControlPoint)
                {
                    Debug.LogWarning("Second control point should not be accessed on a bezier curve marked as one control point ");
                    SetIsTwoControlPoint(true);
                }
                if (controlPoints == null)
                {
                    SetUpControlPoints();
                }
                controlPoints[secondControlPointIndex] = value;
            }
        }
        #endregion

        #region Public Static Methods
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

        //public Vector3 GetVelocity(float t)
        //{
        //    return GetFirstDerivative(Vector3 p0, points[1], points[2], t)) -
        //        transform.position;
        //}
        #endregion

        #region Public Methods
        public BezierCurve()
        {
            startPoint = Vector3.zero;
            endPoint = Vector3.zero;
            isTwoControlPoint = false;
            controlPoints = new Vector3[1] { Vector3.zero };
            distancePrecision = defaultDistancePrecision;
        }

        public BezierCurve(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, float distancePrecision = defaultDistancePrecision)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            isTwoControlPoint = false;
            controlPoints = new Vector3[1] { controlPoint };
            this.distancePrecision = distancePrecision;
        }

        public BezierCurve(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint0, Vector3 controlPoint1, float distancePrecision = defaultDistancePrecision)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            isTwoControlPoint = false;
            controlPoints = new Vector3[2] { controlPoint0, controlPoint1 };
            this.distancePrecision = distancePrecision;
        }

        #region Overrides
        public override float GetTotalLength()
        {
            return Spline.GetTotalLengthOfSpline(this, distancePrecision);
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
        #endregion

        /// <summary>
        /// Returns if this Bezier Curve is using two control points or one
        /// </summary>
        /// <returns>If this Bezier Curve is using two control points or one</returns>
        public bool GetIsTwoControlPoint()
        {

            return isTwoControlPoint;
        }

        public void SetControlPointAt(int index, Vector3 newValue)
        {
            if (index == firstControlPointIndex)
            {
                FirstControlPoint = newValue;
                return;
            }
            else if (index == secondControlPointIndex)
            {
                SecondControlPoint = newValue;
                return;
            }
            Debug.LogError("SetControlPointAt was given an invalid index");
        }

        public Vector3 GetControlPointAt(int index)
        {
            if (index == firstControlPointIndex)
            {
                return FirstControlPoint;
            }
            else if (index == secondControlPointIndex)
            {
                return SecondControlPoint;
            }
            Debug.LogError("GetControlPointAt was given an invalid index");
            return Vector3.negativeInfinity;
        }

        /// <summary>
        /// Set weather this Bezier Curve should be using two control points or one
        /// </summary>
        /// <param name="isTwoControlPoint">Weather this Bezier Curve should be using two control points or one</param>
        public void SetIsTwoControlPoint(bool isTwoControlPoint)
        {
            // Adjust control point array size accordingly then set the boolean
            if (isTwoControlPoint)
            {
                Vector3[] newControlPointsArray = new Vector3[2];
                for (int i = 0; i < controlPoints.Length; ++i)
                {
                    newControlPointsArray[i] = controlPoints[i];
                }
                controlPoints = newControlPointsArray;
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
            return GetPositionAtTimeSingleControlPoint(startPoint, endPoint, controlPoints[firstControlPointIndex], t);
        }

        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetPositionAtTimeTwoControlPoints(float t)
        {
            return GetPositionAtTimeTwoControlPoints(startPoint, endPoint,
                controlPoints[firstControlPointIndex], controlPoints[secondControlPointIndex], t);
        }

        private void SetUpControlPoints()
        {
            if (controlPoints == null)
            {
                if (isTwoControlPoint)
                {
                    controlPoints = new Vector3[2];
                }
                else
                {
                    controlPoints = new Vector3[1];
                }
            }
        }
        #endregion
    }
}
