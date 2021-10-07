//=============================================================================================================================================================================================================
//  Name:               BezierCurve.cs
//  Author:             Matthew Mason
//  Date Created:       25-Mar-2020
//  Date Last Modified: 07-Oct-2021
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
    [System.Serializable]
    public class HermiteSplineSequence : Spline
    {
        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The number of points that are sampled along each of the splines")]
        public float distancePrecisionPerHermiteSpline;

        [SerializeField]
        [Tooltip("Each of the positions for the start and end of splines in the sequence")]
        private List<Vector3> positions;
        [SerializeField]
        [Tooltip("Each of the tangents for the start and end of splines in the sequence")]
        private List<Vector3> tangents;
        #endregion

        #region Private Variables
        /// <summary>
        /// The total approximate length of the combined splines
        /// </summary>
        private float totalLength;

        /// <summary>
        /// The length of each hermit spline in the sequence
        /// </summary>
        private List<float> lengths;
        #endregion

        #region Public Properties
        #region Overrides
        public override Vector3 EndPosition
        {
            get
            {
                if (positions == null)
                {
                    positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
                }
                while (positions.Count < 2) // A count of 2 should mean that it has a start and endpoint
                {
                    positions.Add(Vector3.zero);
                }
                return positions[1];
            }
            set
            {
                if (positions == null)
                {
                    positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
                }
                while (positions.Count < 2) // A count of 2 should mean that it has a start and endpoint
                {
                    positions.Add(Vector3.zero);
                }
                positions[1] = value;
                UpdateLengths();
            }
        }
        public override Vector3 StartPosition
        {
            get
            {
                if (positions == null)
                {
                    positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
                }
                while (positions.Count < 2) // A count of 2 should mean that it has a start and endpoint
                {
                    positions.Add(Vector3.zero);
                }
                return positions[0];
            }
            set
            {
                if (positions == null)
                {
                    positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
                }
                while (positions.Count < 2) // A count of 2 should mean that it has a start and endpoint
                {
                    positions.Add(Vector3.zero);
                }
                positions[0] = value;
                UpdateLengths();
            }
        }
        #endregion

        /// <summary>
        /// The number of points and tangents pairs that currently exist in this spline sequence
        /// </summary>
        public int NumberOfPositionsAndTangents
        {
            get
            {
                return positions.Count;
            }
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Returns a point at the given unit interval "t" that is that far along all the splines in a defined Hermite spline sequence, taking into account the length of each of the splines and weighting them accordingly
        /// </summary>
        /// <param name="points">The point that make up the Hermite spline start and end points, must be equal to the number of tangents</param>
        /// <param name="tangents">The tangents for the points of the hermit splines, must be equal to the number of points</param>
        /// <param name="lengths">The lengths of each of splines in the sequence (pre-calculated before from the method in order to save time)</param>
        /// <param name="t">The unit interval for how far up the spline to sample the points</param>
        /// <returns>A point at the given unit interval "t" that is that far along all the splines in a defined Hermite spline sequence, taking into account the length of each of the splines and weighting them accordingly</returns>
        public static Vector3 GetLengthBasedPointAtTime(List<Vector3> points, List<Vector3> tangents, List<float> lengths, float t)
        {
            if (points == null)
            {
                Debug.LogError("Points list give in GetLengthBasedPointAtTime was null, returning Vector3.zero");
                return Vector3.zero;
            }
            if (tangents == null)
            {
                Debug.LogError("tangents list give in GetLengthBasedPointAtTime was null, returning Vector3.zero");
                return Vector3.zero;
            }
            if (points.Count != tangents.Count)
            {
                Debug.LogError("The number of point was not equal to the number of tangents in GetLengthBasedPointAtTime function, returning Vector3.zero");
                return Vector3.zero;
            }

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
                    return HermiteSpline.GetPointAtTime(points[index],
                        tangents[index], points[index + 1],
                        tangents[index + 1], targetLength / sectionDistance);
                }
                // Otherwise remove of the length of the section from the target length
                else
                {
                    targetLength -= sectionDistance;
                }
            }
            // Return the end point of the last spline if the target length was not within any of the splines
            if (points.Count > 1)
            {
                return points[points.Count - 1];
            }
            else
            {
                Debug.LogError("SplineSequence contained no points when GetLengthBasedPoint was called");
                return Vector3.zero;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public HermiteSplineSequence()
        {
            positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            tangents = new List<Vector3>() { Vector3.zero, Vector3.zero };
        }
        #region Overrides
        public override float GetTotalLength()
        {
            return totalLength;
        }

        /// <summary>
        /// Returns a point at the given unit interval "t" that is that far along all the splines in this Hermite spline sequence, taking into account the length of each of the splines and weighting them accordingly
        /// </summary>
        /// <param name="t">The unit interval for how far up the spline to sample the points</param>
        /// <returns>A point at the given unit interval "t" that is that far along all the splines in a defined Hermite spline sequence, taking into account the length of each of the splines and weighting them accordingly</returns>
        public override Vector3 GetPointAtTime(float t)
        {
            return HermiteSplineSequence.GetLengthBasedPointAtTime(positions, tangents, lengths, t);
        }
        #endregion
        /// <summary>
        /// Returns the length between to different start and end positions
        /// </summary>
        /// <param name="startIndex">The index of the spline positions to start getting the length from</param>
        /// <param name="endIndex">The index of the spline positions to stop getting the length from</param>
        /// <returns>The length between to different start and end positions</returns>
        public float GetLengthBetweenPositions(int startIndex, int endIndex)
        {
            float totalLength = 0f;
            for (int i = startIndex; i < endIndex; ++i)
            {
                totalLength += lengths[startIndex];
            }
            return totalLength;
        }

        /// <summary>
        /// Return the position at a given index
        /// </summary>
        /// <param name="index">The index to get the position from</param>
        /// <returns>the position at a given index</returns>
        public Vector3 GetPositionAtIndex(int index)
        {
            if (positions == null)
            {
                positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            while (positions.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                positions.Add(Vector3.zero);
            }
            if (index < positions.Count)
            {
                if (index > -1)
                {
                    return positions[index];
                }
                Debug.LogError("Index given to GetPointAtIndex method of HermiteSplineSequnce was a negative value, returning Vector3.zero");
                return Vector3.zero;
            }
            Debug.LogError("Index given to GetPointAtIndex method of HermiteSplineSequnce was greater than positions count value, returning Vector3.zero");
            return Vector3.zero;
        }
        /// <summary>
        /// Returns the tangent at a given index
        /// </summary>
        /// <param name="index">The index to get the tangent from</param>
        /// <returns>the tangent at a given index</returns>
        public Vector3 GetTangentAtIndex(int index)
        {
            if (tangents == null)
            {
                tangents = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            while (tangents.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                tangents.Add(Vector3.zero);
            }
            if (index < tangents.Count)
            {
                if (index > -1)
                {
                    return tangents[index];
                }
                Debug.LogError("Index given to GetTangentAtIndex method of HermiteSplineSequnce was a negative value, returning Vector3.zero");
                return Vector3.zero;
            }
            Debug.LogError("Index given to GetTangentAtIndex method of HermiteSplineSequnce was greater than tangents count value, returning Vector3.zero");
            return Vector3.zero;
        }

        /// <summary>
        /// Adds a new positions and tangent to the end of the spline sequence
        /// </summary>
        /// <param name="position">The value of the new position to add</param>
        /// <param name="tangent">The value of the new tangent to add</param>
        public void AddNewPositionAndTangent(Vector3 position, Vector3 tangent)
        {
            if (positions == null)
            {
                positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (tangents == null)
            {
                tangents = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }

            positions.Add(position);
            tangents.Add(tangent);

            UpdateLengths();
        }
        /// <summary>
        /// Removes a position and tangent
        /// </summary>
        /// <param name="indexToRemoveAt">The index that the Position and Tangent in question should be removed at</param>
        public void RemovePositionAndTangentAtIndex(int indexToRemoveAt)
        {
            if (tangents == null)
            {
                tangents = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (indexToRemoveAt < NumberOfPositionsAndTangents)
            {
                if (indexToRemoveAt > 2)
                {
                    positions.RemoveAt(indexToRemoveAt);
                    tangents.RemoveAt(indexToRemoveAt);
                }
                if (NumberOfPositionsAndTangents < 2)
                {
                    // Make sure there is always a start and end point
                    AddNewPositionAndTangent(Vector3.zero, Vector3.zero);
                }
                Debug.LogError("Index given to RemovePositionAndTangentAtIndex method of HermiteSplineSequnce was a negative value");
            }
            Debug.LogError("Index given to RemovePositionAndTangentAtIndex method of HermiteSplineSequnce was greater than points and tangent count value");
            UpdateLengths();
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline position
        /// </summary>
        /// <param name="index">The index to change the position of</param>
        /// <param name="newPosition">The value of the new position</param>
        public void SetPositionAtIndex(int index, Vector3 newPosition)
        {
            if (positions == null)
            {
                positions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (index < positions.Count)
            {
                if (index > -1)
                {
                    positions[index] = newPosition;
                }
                Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was a negative value");
            }
            Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was greater than points count value");
            UpdateLengths();
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline tangent
        /// </summary>
        /// <param name="index">The index to change the tangent of</param>
        /// <param name="newPosition">The value of the new tangent</param>
        public void SetTanagentAtIndex(int index, Vector3 newTangent)
        {
            if (tangents == null)
            {
                tangents = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (index < tangents.Count)
            {
                if (index > -1)
                {
                    tangents[index] = newTangent;
                }
                Debug.LogError("Index given to SetTanagentAtIndex method of HermiteSplineSequnce was a negative value");
            }
            Debug.LogError("Index given to SetTanagentAtIndex method of HermiteSplineSequnce was greater than points count value");
            UpdateLengths();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the size of all the lengths in the spline sequence
        /// </summary>
        private void UpdateLengths()
        {
            totalLength = 0f;
            lengths = new List<float>(positions.Count - 1);
            for (int i = 0; i < positions.Count - 1; ++i)
            {
                lengths.Add(HermiteSpline.GetTotalLength(positions[i], positions[i + 1], tangents[i],
                tangents[i + 1], distancePrecisionPerHermiteSpline));
                totalLength += lengths[lengths.Count];
            }
        }
        #endregion
    }
}


