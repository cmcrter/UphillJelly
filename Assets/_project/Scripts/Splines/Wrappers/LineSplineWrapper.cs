//===========================================================================================================================================================================================================================================================================
// Name:                LineSplineWrapper.cs
// Author:              Matthew Mason
// Date Created:        05-10-2021
// Brief:               SplineWrapper that controls and contains a line spline
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Utility.Splines
{
    /// <summary>
    /// SplineWrapper that controls and contains a line spline
    /// </summary>
    public class LineSplineWrapper : SplineWrapper
    {
        #region Private Serialized Fields 
        [SerializeField] 
        [Tooltip("The line spline that this script is containing")]
        private LineSpline spline;
        #endregion

        #region Private Variables
        /// <summary>
        /// The end point for the spline relative to the world
        /// </summary>
        private Vector3 worldEndPoint;
        /// <summary>
        /// The start point for the spline relative to the world
        /// </summary>
        private Vector3 worldStartPoint;
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
        #endregion

        #region Unity Methods
        public void OnDrawGizmos()
        {
            // Drawing the 2 points and a line between them
            UpdateWorldPositions();
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldStartPoint, 0.1f);
            Gizmos.DrawSphere(worldEndPoint, 0.1f);
            Gizmos.DrawLine(worldStartPoint, worldEndPoint);
        }
        #endregion

        #region Public Methods
        #region Overrides
        public override float GetTotalLength()
        {
            return Vector3.Distance(worldStartPoint, worldEndPoint);
        }

        public override Vector3 GetLocalPointAtTime(float t)
        {
            return spline.GetPointAtTime(t);
        }
        public override Vector3 GetPointAtTime(float t)
        {
            return Vector3.Lerp(worldStartPoint, worldEndPoint, t);
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
                spline = new LineSpline();
            }
            worldStartPoint = transform.TransformPoint(spline.StartPosition);
            worldEndPoint = transform.TransformPoint(spline.EndPosition);
        }
        public override Vector3 GetDirection(float t)
        {
            return worldEndPoint - worldStartPoint;
        }

        public override Vector3 GetDirection(float t, float stepDistance)
        {
            return worldEndPoint - worldStartPoint;
        }
        #endregion
        #endregion
    }
}