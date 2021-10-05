//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       25-Mar-2020
//  Date Last Modified: 21-Apr-2020
//  Brief:              A child of the spline component for showing make a linked set of Hermite splines
//=============================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A child of the spline component for showing make a linked set of Hermite splines
    /// </summary>
    public class HermiteSplineSequence : Spline
    {
        public float distancePrecisionPerHermiteSpline;

        private float totalLength;

        public List<KeyValuePair<Vector3, Vector3>> pointsAndTangents;
        private List<float> lengths;

        public static Vector3 GetLengthBasedPointAtTime(List<KeyValuePair<Vector3, Vector3>> pointsAndTangents, List<float> lengths, float t)
        {
            // Ensuring t is a unit interval
            t = Mathf.Clamp01(t);

            float totalLength = 0f;
            if (lengths != null)
            {
                for (int i = 0; i < lengths.Count; ++i)
                {
                    totalLength += lengths[i];
                }
            }

            // Using t to find the length along the spline sequence to aim for
            float targetLength = totalLength * t;

            for (int index = 0; index < lengths.Count; ++index)
            {
                float sectionDistance = lengths[index];
                // Checking if the target length shorter than this spline
                if (targetLength - sectionDistance < 0f)
                {
                    return HermiteSpline.GetPointAtTime(pointsAndTangents[index].Key,
                        pointsAndTangents[index].Value, pointsAndTangents[index + 1].Key,
                        pointsAndTangents[index + 1].Value, targetLength / sectionDistance);
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            // Return the end point of the last spline if the target length was not within any of the splines
            if (pointsAndTangents.Count > 1)
            {
                return pointsAndTangents[pointsAndTangents.Count - 1].Key;
            }
            else
            {
                Debug.LogError("SplineSequence contained no points when GetLengthBasedPoint was called");
                return Vector3.zero;
            }
        }

        public override float GetTotalLength()
        {
            float totalLength = 0f;
            if (lengths != null)
            {
                for (int i = 0; i < lengths.Count; ++i)
                {
                    totalLength += lengths[i];
                }
            }
            return totalLength;
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return HermiteSplineSequence.GetLengthBasedPointAtTime(pointsAndTangents, lengths, t);
        }

        private float GetLengthOfSingleSpline(int startPointIndex)
        {
            return HermiteSpline.GetTotalLength(pointsAndTangents[startPointIndex].Key,
                pointsAndTangents[startPointIndex + 1].Key, pointsAndTangents[startPointIndex].Value,
                pointsAndTangents[startPointIndex + 1].Value, distancePrecisionPerHermiteSpline);
        }

        private void UpdateLengths()
        {
            lengths = new List<float>(pointsAndTangents.Count - 1);
            for (int i = 0; i < pointsAndTangents.Count - 1; ++i)
            {
                lengths.Add(GetLengthOfSingleSpline(i));
            }
        }

        public override Vector3 GetStartPoint()
        {
            if (pointsAndTangents.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                pointsAndTangents = new List<KeyValuePair<Vector3, Vector3>>(2)
            {
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero),
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero)
            };
            }
            return pointsAndTangents[0].Key;
        }

        public override Vector3 GetEndPoint()
        {
            if (pointsAndTangents.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                pointsAndTangents = new List<KeyValuePair<Vector3, Vector3>>(2)
            {
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero),
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero)
            };
            }
            return pointsAndTangents[1].Key;
        }

        public override void SetStartPoint(Vector3 startPoint)
        {
            if (pointsAndTangents.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                pointsAndTangents = new List<KeyValuePair<Vector3, Vector3>>(2)
            {
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero),
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero)
            };
            }
            pointsAndTangents[0] = new KeyValuePair<Vector3, Vector3>(startPoint, pointsAndTangents[0].Value);
        }

        public override void SetEndPoint(Vector3 endPoint)
        {
            if (pointsAndTangents.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                pointsAndTangents = new List<KeyValuePair<Vector3, Vector3>>(2)
            {
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero),
                new KeyValuePair<Vector3, Vector3>(Vector3.zero, Vector3.zero)
            };
            }
            pointsAndTangents[1] = new KeyValuePair<Vector3, Vector3>(endPoint, pointsAndTangents[0].Value);
        }
    }
}


