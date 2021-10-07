using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    public class HermiteSplineSequenceWrapper : SplineWrapper
    {
        public HermiteSplineSequence spline;

        private List<Vector3> worldPoints;
        private List<float> worldLengths;

        /// <summary>
        /// If the end of the last spline links to the start of the first spline
        /// </summary>
        [SerializeField]
        private bool isCircuit;

        private void OnDrawGizmos()
        {
            UpdateWorldPositions();

            for (int i = 0; i < worldPoints.Count; ++i)
            {
                Gizmos.DrawSphere(worldPoints[i], 0.1f);
                Gizmos.DrawCube(worldPoints[i] + spline.GetTangentAtIndex(i), Vector3.one * 0.1f);
                Gizmos.DrawLine(worldPoints[i], worldPoints[i] + spline.GetTangentAtIndex(i));
            }

            for (int i = 0; i < worldLengths.Count; ++i)
            {
                // Drawing line between all the sampled points to represent the spline
                float tIncrement = 1.0f / spline.distancePrecisionPerHermiteSpline;
                for (float f = 0.0f; f < 1f;)
                {
                    Vector3 lineStart = HermiteSpline.GetPointAtTime(worldPoints[i], worldPoints[i + 1], 
                        spline.GetTangentAtIndex(i), spline.GetTangentAtIndex(i + 1), f);

                    Vector3 lineEnd = HermiteSpline.GetPointAtTime(worldPoints[i], worldPoints[i + 1],
                        spline.GetTangentAtIndex(i), spline.GetTangentAtIndex(i + 1), f += tIncrement);

                    Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
                    Gizmos.DrawLine(lineStart, lineEnd);
                }
            }
        }

        public override Vector3 GetPointAtTime(float t)
        {
            List<Vector3> tangents =new List<Vector3>(spline.NumberOfPositionsAndTangents);
            for (int i = 0; i < spline.NumberOfPositionsAndTangents; ++i)
            {
                tangents.Add(spline.GetTangentAtIndex(i));
            }
            return HermiteSplineSequence.GetLengthBasedPointAtTime(worldPoints, tangents, worldLengths, t);
        }


        public override float GetTotalLength()
        {
            float totalLength = 0f;
            if (worldLengths != null)
            {
                for (int i = 0; i < worldLengths.Count; ++i)
                {
                    totalLength += worldLengths[i];
                }
            }
            return totalLength;
        }

        private void UpdateLengths()
        {
            worldLengths = new List<float>(worldPoints.Count - 1);
            for (int i = 0; i < worldPoints.Count - 1; ++i)
            {
                worldLengths.Add(GetLengthOfSingleSpline(i));
            }
        }

        private float GetLengthOfSingleSpline(int startPointIndex)
        {
            return HermiteSpline.GetTotalLength(worldPoints[startPointIndex],
                worldPoints[startPointIndex + 1], spline.GetTangentAtIndex(startPointIndex),
                spline.GetTangentAtIndex(startPointIndex + 1), spline.distancePrecisionPerHermiteSpline);
        }

        private Vector3 GetPositionAtTimeBetweenPoints(int startPointIndex, float t)
        {
            return HermiteSpline.GetPointAtTime(worldPoints[startPointIndex],
                spline.GetTangentAtIndex(startPointIndex), worldPoints[startPointIndex + 1],
                spline.GetTangentAtIndex(startPointIndex + 1), t);
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateLengths();
        }

        public override Vector3 GetWorldStartPoint()
        {
            if (worldPoints.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPoints = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            return worldPoints[0];
        }

        public override Vector3 GetWorldEndPoint()
        {
            if (worldPoints.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPoints = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            return worldPoints[worldPoints.Count - 1];
        }

        public override void SetWorldStartPoint(Vector3 startPoint, bool updateLocalPosition)
        {
            if (worldPoints.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPoints = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            worldPoints[0] = startPoint;
            if (updateLocalPosition)
            {
                spline.StartPosition = startPoint;
            }
            UpdateLengths();
        }

        public override void SetWorldEndPoint(Vector3 endPoint, bool updateLocalPosition)
        {
            if (worldPoints.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPoints = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            worldPoints[worldPoints.Count - 1] = endPoint;
            if (updateLocalPosition)
            {
                spline.EndPosition = endPoint;
            }
            UpdateLengths();
        }

        public override void UpdateWorldPositions()
        {
            worldPoints = new List<Vector3>(spline.NumberOfPositionsAndTangents);
            for (int i = 0; i < spline.NumberOfPositionsAndTangents; ++i)
            {
                worldPoints.Add(transform.TransformPoint(spline.GetPositionAtIndex(i)));
            }
            UpdateLengths();
        }
    }
}
