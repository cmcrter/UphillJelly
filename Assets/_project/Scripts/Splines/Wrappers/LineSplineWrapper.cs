//===========================================================================================================================================================================================================================================================================
// Name:                LineSplineWrapper.cs
// Author:              Matthew Mason
// Date Created:        04-10-2021
// Brief:               SplineWrapper that controls and contains a line spline
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// SplineWrapper that controls and contains a line spline
    /// </summary>
    public class LineSplineWrapper : SplineWrapper
    {
        #region Private Serialized Fields 
        [SerializeField] [Tooltip("The line spline that is script is containing")]
        private LineSpline spline;
        #endregion

        /// <summary>
        /// The start point for the spline relative to the world
        /// </summary>
        private Vector3 worldStartPoint;

        /// <summary>
        /// The end point for the spline relative to the world
        /// </summary>
        private Vector3 worldEndPoint;

        public override Vector3 GetPointAtTime(float t)
        {
            return Vector3.Lerp(worldStartPoint, worldEndPoint, t);
        }

        public override float GetTotalLength()
        {
            return Vector3.Distance(worldStartPoint, worldEndPoint);
        }

        public override Vector3 GetWorldEndPoint()
        {
            return worldEndPoint;
        }

        public override Vector3 GetWorldStartPoint()
        {
            return worldStartPoint;
        }

        public void OnDrawGizmos()
        {
            // Drawing the 2 points and a line between them
            UpdateWorldPositions();
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldStartPoint, 0.1f);
            Gizmos.DrawSphere(worldEndPoint, 0.1f);
            Gizmos.DrawLine(worldStartPoint, worldEndPoint);
        }

        public override void SetWorldEndPoint(Vector3 endPoint)
        {
            worldEndPoint = endPoint;
            spline.SetEndPoint(transform.InverseTransformPoint(endPoint));
        }

        public override void SetWorldStartPoint(Vector3 startPoint)
        {
            worldStartPoint = startPoint;
            spline.SetStartPoint(transform.InverseTransformPoint(startPoint));
        }

        public override void UpdateWorldPositions()
        {
            SetWorldStartPoint(transform.TransformPoint(spline.GetStartPoint()));
            SetWorldEndPoint(transform.TransformPoint(spline.GetEndPoint()));
        }
    }
}
