//===========================================================================================================================================================================================================================================================================
// Name:                HermiteSplineWrapper.cs
// Author:              Matthew Mason
// Date Created:        11-Oct-2021
// Brief:               A spline wrapper used to control a Hermite spline
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A spline wrapper used to control a Hermite spline
    /// </summary>
    public class HermiteSplineWrapper : SplineWrapper
    {
        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The spline that this is containing")]
        private HermiteSpline spline;
        #endregion

        #region Private Variables
        /// <summary>
        /// The total length the spline covers
        /// </summary>
        private float totalWorldLength;

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
        public override Vector3 WorldEndPosition
        {
            get
            {
                return worldEndPoint;
            }
            set
            {
                worldEndPoint = value;
            }
        }
        public override Vector3 WorldStartPosition
        {
            get
            {
                return worldStartPoint;
            }
            set
            {
                worldStartPoint = value;
            }
        }
        #endregion

        public Vector3 EndTangent
        {
            get
            {
                return spline.EndTangent;
            }
            set
            {
                spline.EndTangent = value;
                UpdateWorldPositions();
            }
        }

        public Vector3 StartTangent
        {
            get
            {
                return spline.StartTangent;
            }
            set
            {
                spline.StartTangent = value;
                UpdateWorldPositions();
            }
        }
        #endregion

        #region Unity Methods
        #region Overrides
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateLength();
        }

        protected override void Start()
        {
            base.Start();
            UpdateLength();
        }
        #endregion
        private void OnDrawGizmos()
        {
            UpdateWorldPositions();

            // Drawing start point and tangent 
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldStartPoint, 0.1f);
            Gizmos.DrawCube(worldStartPoint + spline.StartTangent, Vector3.one * 0.1f);
            Gizmos.DrawLine(worldStartPoint, worldStartPoint + spline.StartTangent);

            // Drawing end Point and tangent
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(worldEndPoint, 0.1f);
            Gizmos.DrawCube(worldEndPoint + spline.EndTangent, Vector3.one * 0.1f);
            Gizmos.DrawLine(worldEndPoint, worldEndPoint + spline.EndTangent);

            // Drawing line between all the sampled points to represent the spline
            float tIncrement = 1.0f / spline.DistancePrecision;
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
            return totalWorldLength;
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return HermiteSpline.GetPointAtTime(worldStartPoint, worldEndPoint, spline.StartTangent, spline.EndTangent, t);
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
        public override void UpdateWorldPositions()
        {
            worldStartPoint = transform.TransformPoint(spline.StartPosition);
            worldEndPoint = transform.TransformPoint(spline.EndPosition);
        }
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the total length to be correct based on the current spline
        /// </summary>
        private void UpdateLength()
        {
            totalWorldLength = HermiteSpline.GetTotalLength(worldStartPoint, worldEndPoint, spline.StartTangent,
                spline.EndTangent, spline.DistancePrecision);
        }
        #endregion
    }
}
