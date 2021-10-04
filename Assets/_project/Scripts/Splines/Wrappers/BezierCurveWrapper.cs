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
        private BezierCurve spline;

        private Vector3 worldStartPoint;

        private Vector3 worldEndPoint;

        #region Private Variables
        /// <summary>
        /// The additional points needed on top of the start and end point in world space
        /// </summary>
        private Vector3[] worldControlPoints;
        #endregion

        #region Unity Methods
        private void OnDrawGizmos()
        {
            UpdateWorldPositions();

            for (int i = 0; i < spline.controlPoints.Length; ++i)
            {
                Gizmos.DrawWireCube(worldControlPoints[i], new Vector3(1, 1, 1));
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

        public Vector3 GetAdditionalWorldPoint(int index)
        {
            UpdateWorldPositions();
            if (index < worldControlPoints.Length)
            {
                return worldControlPoints[index];
            }
            return Vector3.zero;
        }

        public void SetAdditionalWorldPoint(int index, Vector3 position)
        {
            UpdateWorldPositions();
            if (index < worldControlPoints.Length)
            {
                worldControlPoints[index] = position;
                spline.controlPoints[index] = transform.InverseTransformPoint(worldControlPoints[index]);
            }
        }

        public override void UpdateWorldPositions()
        {
            SetWorldStartPoint(transform.TransformPoint(spline.GetStartPoint()));
            SetWorldEndPoint(transform.TransformPoint(spline.GetEndPoint()));
            worldControlPoints = new Vector3[spline.controlPoints.Length];
            for (int i = 0; i < worldControlPoints.Length; ++i)
            {
                worldControlPoints[i] = transform.TransformPoint(spline.controlPoints[i]);
            }
        }

        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 3 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetThreePointPositionAtTime(float t)
        {
            return BezierCurve.GetThreePointPositionAtTime(worldStartPoint, worldEndPoint, worldControlPoints[BezierCurve.firstAddtionalPointIndex], t);
        }

        /// <summary>
        /// Returns the a point along the spline at a given unit interval assuming it has is working with 4 different points
        /// </summary>
        /// <param name="t">The unit interval</param>
        /// <returns>A point along the spline at a given unit interval</returns>
        private Vector3 GetFourPointPositionAtTime(float t)
        {
            return BezierCurve.GetFourPointPositionAtTime(worldStartPoint, worldEndPoint, 
                worldControlPoints[BezierCurve.firstAddtionalPointIndex], 
                worldControlPoints[BezierCurve.secondAddtionalPointIndex], t);
        }

        public override Vector3 GetPointAtTime(float t)
        {
            if (spline.isFourPoint)
            {
                return GetFourPointPositionAtTime(t);
            }
            else
            {
                return GetThreePointPositionAtTime(t);
            }
        }

        public override float GetTotalLength()
        {
            if (spline.isFourPoint)
            {
                return BezierCurve.GetFourPointTotalLength(worldStartPoint, worldEndPoint,
                worldControlPoints[BezierCurve.firstAddtionalPointIndex],
                worldControlPoints[BezierCurve.secondAddtionalPointIndex], spline.distancePrecision);
            }
            else
            {
                return BezierCurve.GetThreePointTotalLength(worldStartPoint, worldEndPoint, 
                    worldControlPoints[BezierCurve.firstAddtionalPointIndex], spline.distancePrecision);
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

        public override void SetWorldStartPoint(Vector3 startPoint)
        {
            worldStartPoint = startPoint;
            spline.SetStartPoint(transform.InverseTransformPoint(startPoint));
        }

        public override void SetWorldEndPoint(Vector3 endPoint)
        {
            worldEndPoint = endPoint;
            spline.SetEndPoint(transform.InverseTransformPoint(endPoint));
        }
    }
}


