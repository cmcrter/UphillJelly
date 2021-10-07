//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       04/10/2021
//  Brief:              A MonoBehaviour for wrapping up and controlling a Bezier Curve 
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// MonoBehaviour for wrapping up and controlling a Bezier Curve 
    /// </summary>
    public class BezierCurveWrapper : SplineWrapper
    {
        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The Bezier Curve spline this controlling")]
        private BezierCurve spline;
        #endregion

        #region Private Variables
        /// <summary>
        /// The point at which at which the line spline ends in world space, would be the position at t 1
        /// </summary>
        private Vector3 worldEndPoint;
        /// <summary>
        /// The Point at which the line spline starts in world space, would be the position at t 0
        /// </summary>
        private Vector3 worldStartPoint;

        /// <summary>
        /// The additional points needed on top of the start and end point in world space
        /// </summary>
        private Vector3[] worldControlPoints;
        #endregion

        #region Unity Methods
        private void OnDrawGizmos()
        {
            UpdateWorldPositions();

            Gizmos.DrawSphere(worldStartPoint, 0.1f);
            Gizmos.DrawSphere(worldEndPoint, 0.1f);

            for (int i = 0; i < spline.controlPoints.Length; ++i)
            {
                Gizmos.DrawWireCube(worldControlPoints[i], new Vector3(0.1f, 0.1f, 0.1f));
            }

            if (spline.GetIsTwoControlPoint())
            {
                Gizmos.DrawLine(worldStartPoint, worldControlPoints[0]);
                Gizmos.DrawLine(worldEndPoint, worldControlPoints[1]);
            }
            else
            {
                Gizmos.DrawLine(worldStartPoint, worldControlPoints[0]);
                Gizmos.DrawLine(worldEndPoint, worldControlPoints[0]);
            }

            // Drawing line between all the sampled points to represent the spline
            float tIncrement = 1.0f / spline.distancePrecision;
            for (float f = 0.0f; f < 1f;)
            {
                Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
                Gizmos.DrawLine(GetPointAtTime(f), GetPointAtTime(f += tIncrement));
            }
        }
        #endregion

        #region Public Methods
        #region Overrides
        public override float GetTotalLength()
        {
            if (spline.GetIsTwoControlPoint())
            {
                return BezierCurve.GetTotalLengthTwoControlPoints(worldStartPoint, worldEndPoint,
                worldControlPoints[BezierCurve.firstAddtionalPointIndex],
                worldControlPoints[BezierCurve.secondAddtionalPointIndex], spline.distancePrecision);
            }
            else
            {
                return BezierCurve.GetTotalLengthSingleControlPoint(worldStartPoint, worldEndPoint,
                    worldControlPoints[BezierCurve.firstAddtionalPointIndex], spline.distancePrecision);
            }
        }

        public override Vector3 GetPointAtTime(float t)
        {
            if (spline.GetIsTwoControlPoint())
            {
                return GetFourPointPositionAtTime(t);
            }
            else
            {
                return GetThreePointPositionAtTime(t);
            }
        }
        public override Vector3 GetWorldStartPoint()
        {
            return worldStartPoint;
        }

        public override Vector3 GetWorldEndPoint()
        {
            return worldEndPoint;
        }

        public override void UpdateWorldPositions()
        {
            worldStartPoint = transform.TransformPoint(spline.StartPosition);
            worldEndPoint = transform.TransformPoint(spline.EndPosition);

            if (spline.GetIsTwoControlPoint())
            {
                Vector3[] newControlPointsArray = new Vector3[2];
                if (worldControlPoints != null)
                {
                    for (int i = 0; i < worldControlPoints.Length; ++i)
                    {
                        newControlPointsArray[i] = worldControlPoints[i];
                    }
                }
            }
            else
            {
                if (worldControlPoints != null)
                {
                    Vector3[] newControlPointsArray = new Vector3[1];
                    newControlPointsArray[0] = worldControlPoints[0];
                    worldControlPoints = newControlPointsArray;
                }
            }

            worldControlPoints = new Vector3[spline.controlPoints.Length];
            for (int i = 0; i < worldControlPoints.Length; ++i)
            {
                worldControlPoints[i] = transform.TransformPoint(spline.controlPoints[i]);
            }
        }
        public override void SetWorldEndPoint(Vector3 endPoint, bool updateLocalPosition)
        {
            worldEndPoint = endPoint;
            if (updateLocalPosition)
            {
                spline.EndPosition = transform.InverseTransformPoint(endPoint);
            }
        }
        public override void SetWorldStartPoint(Vector3 startPoint, bool updateLocalPosition)
        {
            worldStartPoint = startPoint;
            if (updateLocalPosition)
            {
                spline.StartPosition = transform.InverseTransformPoint(startPoint);
            }
        }
        #endregion

        /// <summary>
        /// Sets a world control position at given index
        /// </summary>
        /// <param name="index">The index of the control point to change the value of</param>
        /// <param name="position">The new position of the control point</param>
        /// <returns>True if an control point as changed, false if the control point at index didn't exist to change</returns>
        public bool SetWorldControlPoint(int index, Vector3 position)
        {
            UpdateWorldPositions();
            if (index < worldControlPoints.Length)
            {
                worldControlPoints[index] = position;
                spline.controlPoints[index] = transform.InverseTransformPoint(worldControlPoints[index]);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to output a control point at a given index
        /// </summary>
        /// <param name="index">The index to get the control point at (should be 0 if its is a single control point and 0-1 if it is two control point</param>
        /// <returns>True if a control point at the index was found, false if not</returns>
        public bool TryGetWorldControlPoint(int index, out Vector3 controlPoint)
        {
            UpdateWorldPositions();
            if (index < worldControlPoints.Length)
            {
                controlPoint = worldControlPoints[index];
                return true;
            }
            controlPoint = Vector3.zero;
            return false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetFourPointPositionAtTime(float t)
        {
            return BezierCurve.GetPositionAtTimeTwoControlPoints(worldStartPoint, worldEndPoint,
                worldControlPoints[BezierCurve.firstAddtionalPointIndex],
                worldControlPoints[BezierCurve.secondAddtionalPointIndex], t);
        }
        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 3 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetThreePointPositionAtTime(float t)
        {
            return BezierCurve.GetPositionAtTimeSingleControlPoint(worldStartPoint, worldEndPoint, worldControlPoints[BezierCurve.firstAddtionalPointIndex], t);
        }
        #endregion
    }
}


