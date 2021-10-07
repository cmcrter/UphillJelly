using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    public class HermiteSplineWrapper : SplineWrapper
    {
        [SerializeField]
        private HermiteSpline spline;

        private Vector3 worldStartPoint;

        private Vector3 worldEndPoint;

        /// <summary>
        /// The total length the spline covers
        /// </summary>
        private float totalWorldLength;

        #region Unity Methods
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
            Gizmos.DrawCube(worldEndPoint + spline.endTangent, Vector3.one * 0.1f);
            Gizmos.DrawLine(worldEndPoint, worldEndPoint + spline.endTangent);

            // Drawing line between all the sampled points to represent the spline
            float tIncrement = 1.0f / spline.DistancePrecision;
            for (float f = 0.0f; f < 1f;)
            {
                Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
                Gizmos.DrawLine(GetPointAtTime(f), GetPointAtTime(f += tIncrement));
            }
        }

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

        public override float GetTotalLength()
        {
            return totalWorldLength;
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return HermiteSpline.GetPointAtTime(worldStartPoint, worldEndPoint, spline.StartTangent, spline.endTangent, t);
        }

        /// <summary>
        /// Updates the total length to be correct based on the current spline
        /// </summary>
        private void UpdateLength()
        {
            totalWorldLength = HermiteSpline.GetTotalLength(worldStartPoint, worldEndPoint, spline.StartTangent, 
                spline.endTangent, spline.DistancePrecision);
        }

        public override Vector3 GetWorldStartPoint()
        {
            return worldStartPoint;
        }

        public override Vector3 GetWorldEndPoint()
        {
            return worldEndPoint;
        }

        public override void SetWorldStartPoint(Vector3 startPoint, bool updateLocalPosition)
        {
            worldStartPoint = startPoint;
            if (updateLocalPosition)
            {
                spline.StartPosition = transform.InverseTransformPoint(startPoint);
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

        public override void UpdateWorldPositions()
        {
            SetWorldStartPoint(transform.TransformPoint(spline.StartPosition), false);
            SetWorldEndPoint(transform.TransformPoint(spline.EndPosition), false);
        }
    }
}
