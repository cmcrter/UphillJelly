//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       04/10/2021
//  Brief:              A MonoBehaviour for wrapping up and controlling a Bezier Curve 
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Utility.Splines
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

        #region Public Properties
        #region Overrides
        public override Vector3 LocalEndPosition
        {
            get
            {
                return spline.EndPosition;
            }
            set
            {
                spline.EndPosition = value;
                UpdateWorldPositions();
            }
        }
        public override Vector3 LocalStartPosition
        {
            get
            {
                return spline.StartPosition;
            }
            set
            {
                spline.StartPosition = value;
                UpdateWorldPositions();
            }
        }
        public override Vector3 WorldEndPosition
        {
            get
            {
                return worldEndPoint;
            }
        }
        public override Vector3 WorldStartPosition
        {
            get
            {
                return worldStartPoint;
            }
        }
        #endregion

        public bool IsUsingTwoControlPoints
        {
            get
            {
                return spline.GetIsTwoControlPoint();
            }
            set
            {
                spline.SetIsTwoControlPoint(value);
            }
        }

        public float DistancePrecision
        {
            get
            {
                return spline.distancePrecision;
            }
            set
            {
                spline.distancePrecision = value;
            }
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (spline == null)
            {
                spline = new BezierCurve();
            }
            if (worldControlPoints == null)
            {
                if (IsUsingTwoControlPoints)
                {
                    worldControlPoints = new Vector3[2];
                }
                else
                {
                    worldControlPoints = new Vector3[1];
                }
            }
        }

        private void OnDrawGizmos()
        {
            UpdateWorldPositions();

            Gizmos.DrawSphere(worldStartPoint, 0.1f);
            Gizmos.DrawSphere(worldEndPoint, 0.1f);


            for (int i = 0; i < spline.NumberOfControlPoints ; ++i)
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
                if (worldControlPoints == null)
                {
                    UpdateWorldPositions();
                }
                return Spline.GetTotalLengthOfSpline(new BezierCurve(WorldStartPosition, worldEndPoint, worldControlPoints[BezierCurve.firstControlPointIndex],
                    worldControlPoints[BezierCurve.secondControlPointIndex], DistancePrecision), DistancePrecision);
            }
            else
            {
                if (worldControlPoints == null)
                {
                    UpdateWorldPositions();
                }
                return Spline.GetTotalLengthOfSpline(new BezierCurve(WorldStartPosition, worldEndPoint, worldControlPoints[BezierCurve.firstControlPointIndex],
                    DistancePrecision), DistancePrecision);
            }
        }

        public override Vector3 GetLocalPointAtTime(float t)
        {
            return spline.GetPointAtTime(t);
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

        public override void SetWorldEndPointAndUpdateLocal(Vector3 endPoint)
        {
            worldEndPoint = endPoint;
            spline.EndPosition = transform.InverseTransformPoint(endPoint);
        }

        public override void SetWorldStartPointAndUpdateLocal(Vector3 startPoint)
        {
            worldStartPoint = startPoint;
            spline.StartPosition = transform.InverseTransformPoint(startPoint);
        }
        public override void SetWorldEndPointWithoutLocal(Vector3 endPoint)
        {
            worldEndPoint = endPoint;
        }
        public override void SetWorldStartPointWithoutLocal(Vector3 startPoint)
        {
            worldStartPoint = startPoint;
        }
        public override void UpdateWorldPositions()
        {
            if (spline == null)
            {
                spline = new BezierCurve();
            }
            worldStartPoint = transform.TransformPoint(spline.StartPosition);
            worldEndPoint = transform.TransformPoint(spline.EndPosition);

            worldControlPoints = new Vector3[spline.NumberOfControlPoints];
            for (int i = 0; i < worldControlPoints.Length; ++i)
            {
                worldControlPoints[i] = transform.TransformPoint(spline.GetControlPointAt(i));
            }
        }
        #endregion

        public bool SetLocalControlPointValues(int index, Vector3 newValue)
        {
            if (index < spline.NumberOfControlPoints)
            {
                spline.SetControlPointAt(index, newValue);
                UpdateWorldPositions();
                return true;
            }
            return false;
        }
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
                spline.SetControlPointAt(index, transform.InverseTransformPoint(worldControlPoints[index]));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Attempts to output a control point at a given index
        /// </summary>
        /// <param name="index">The index to get the control point at (should be 0 if its is a single control point and 0-1 if it is two control point</param>
        /// <returns>True if a control point at the index was found, false if not</returns>
        public bool TryGetLocalControlPoint(int index, out Vector3 controlPoint)
        {
            if (index < spline.NumberOfControlPoints)
            {
                controlPoint = spline.GetControlPointAt(index);
                return true;
            }
            controlPoint = spline.FirstControlPoint;
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
            controlPoint = worldControlPoints[0];
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
                worldControlPoints[BezierCurve.firstControlPointIndex],
                worldControlPoints[BezierCurve.secondControlPointIndex], t);
        }
        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 3 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetThreePointPositionAtTime(float t)
        {
            return BezierCurve.GetPositionAtTimeSingleControlPoint(worldStartPoint, worldEndPoint, worldControlPoints[BezierCurve.firstControlPointIndex], t);
        }
        #endregion
    }
}


