//===========================================================================================================================================================================================================================================================================
// Name:                HermiteSplineSequenceWrapper.cs
// Author:              Matthew Mason
// Date Created:        11-Oct-2021
// Brief:               A spline wrapper used to control a HermiteSplineSequence
//============================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A spline wrapper used to control a HermiteSplineSequence
    /// </summary>
    public class HermiteSplineSequenceWrapper : SplineWrapper
    {
        #region Private Serialized Fields
        [SerializeField] [Tooltip("The HermiteSplineSequence this is controlling")]
        private HermiteSplineSequence spline;
        #endregion

        #region Private Variables
        /// <summary>
        /// The length of the each of the spline sin the sequence
        /// </summary>
        private List<float> worldLengths;

        /// <summary>
        /// The world points for all the connected points of the spline sequence
        /// </summary>
        private List<Vector3> worldPositions;
        #endregion

        #region Public Properties
        #region Overrides
        public override Vector3 WorldEndPosition
        {
            get
            {
                if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
                {
                    worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
                }
                return worldPositions[worldPositions.Count - 1];
            }
            set
            {
                if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
                {
                    worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
                }
                worldPositions[worldPositions.Count - 1] = value;
                UpdateLengths();
            }
        }
        public override Vector3 WorldStartPosition
        {
            get
            {
                if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
                {
                    worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
                }
                return worldPositions[0];
            }
            set
            {
                if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
                {
                    worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
                }
                worldPositions[0] = value;
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
                return worldPositions.Count;
            }
        }
        #endregion

        #region Unity Methods
        #region Overrides
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateLengths();
        }
        #endregion
        private void OnDrawGizmos()
        {
            // Draw world points and tangents
            for (int i = 0; i < worldPositions.Count; ++i)
            {
                Gizmos.DrawSphere(worldPositions[i], 0.1f);
                Gizmos.DrawCube(worldPositions[i] + spline.GetTangentAtIndex(i), Vector3.one * 0.1f);
                Gizmos.DrawLine(worldPositions[i], worldPositions[i] + spline.GetTangentAtIndex(i));
            }

            // Draw spline but stepping over them and drawing line between them
            for (int i = 0; i < worldLengths.Count; ++i)
            {
                // Drawing line between all the sampled points to represent the spline
                float tIncrement = 1.0f / spline.distancePrecisionPerHermiteSpline;
                for (float f = 0.0f; f < 1f;)
                {
                    Vector3 lineStart = HermiteSpline.GetPointAtTime(worldPositions[i], worldPositions[i + 1], 
                        spline.GetTangentAtIndex(i), spline.GetTangentAtIndex(i + 1), f);

                    Vector3 lineEnd = HermiteSpline.GetPointAtTime(worldPositions[i], worldPositions[i + 1],
                        spline.GetTangentAtIndex(i), spline.GetTangentAtIndex(i + 1), f += tIncrement);

                    Gizmos.color = Color.Lerp(Color.blue, Color.red, f);
                    Gizmos.DrawLine(lineStart, lineEnd);
                }
            }
        }
        #endregion

        #region Public Methods
        #region Overrides
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

        public override Vector3 GetPointAtTime(float t)
        {
            List<Vector3> tangents =new List<Vector3>(spline.NumberOfPositionsAndTangents);
            for (int i = 0; i < spline.NumberOfPositionsAndTangents; ++i)
            {
                tangents.Add(spline.GetTangentAtIndex(i));
            }
            return HermiteSplineSequence.GetLengthBasedPointAtTime(worldPositions, tangents, worldLengths, t);
        }

        public override void SetWorldEndPointAndUpdateLocal(Vector3 endPoint)
        {
            if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            worldPositions[worldPositions.Count - 1] = endPoint;
            spline.EndPosition = endPoint;
            UpdateLengths();
        }
        public override void SetWorldStartPointAndUpdateLocal(Vector3 startPoint)
        {
            if (worldPositions.Count < 2) // a count of 2 should mean that it has a start and endpoint
            {
                worldPositions = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            }
            worldPositions[0] = startPoint;
            spline.StartPosition = startPoint;
            UpdateLengths();
        }
        public override void UpdateWorldPositions()
        {
            worldPositions = new List<Vector3>(spline.NumberOfPositionsAndTangents);
            for (int i = 0; i < spline.NumberOfPositionsAndTangents; ++i)
            {
                worldPositions.Add(transform.TransformPoint(spline.GetPositionAtIndex(i)));
            }
            UpdateLengths();
        }

        #endregion
        /// <summary>
        /// Returns a position of spline sequence at the given index local to the game object
        /// </summary>
        /// <param name="index">The index to get the position from</param>
        /// <returns>A position of spline sequence at the given index local to the game object</returns>
        public Vector3 GetLocalPositionAtIndex(int index)
        {
            return spline.GetPositionAtIndex(index);
        }
        /// <summary>
        /// Return the position at a given index
        /// </summary>
        /// <param name="index">The index to get the position from</param>
        /// <returns>the position at a given index</returns>
        public Vector3 GetPositionAtIndex(int index)
        {
            if (worldPositions == null)
            {
                worldPositions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            while (worldPositions.Count < 2) // A count of 2 should mean that it has a start and endpoint
            {
                worldPositions.Add(Vector3.zero);
            }
            if (index < worldPositions.Count)
            {
                if (index > -1)
                {
                    return worldPositions[index];
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
            return spline.GetTangentAtIndex(index);
        }

        /// <summary>
        /// Adds a new positions and tangent to the end of the spline sequence
        /// </summary>
        /// <param name="worldPosition">The value of the new position to add</param>
        /// <param name="tangent">The value of the new tangent to add</param>
        public void AddNewPositionAndTangent(Vector3 worldPosition, Vector3 tangent)
        {
            if (worldPositions == null)
            {
                worldPositions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }

            worldPositions.Add(worldPosition);
            spline.AddNewPositionAndTangent(transform.InverseTransformPoint(worldPosition), tangent);

            UpdateLengths();
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline position locally to the gameObject
        /// </summary>
        /// <param name="index">The index to change the position of</param>
        /// <param name="newPosition">The value of the new position</param>
        public void SetLocalPositionAtIndex(int index, Vector3 newPosition)
        {
            spline.SetPositionAtIndex(index, newPosition);
            UpdateWorldPositions();
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline position
        /// </summary>
        /// <param name="index">The index to change the position of</param>
        /// <param name="newPosition">The value of the new position</param>
        public void SetPositionAtIndex(int index, Vector3 newPosition)
        {
            if (worldPositions == null)
            {
                worldPositions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (index < worldPositions.Count)
            {
                if (index > -1)
                {
                    worldPositions[index] = newPosition;
                }
                else
                {
                    Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was a negative value");
                }
            }
            else
            {
                Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was greater than points count value");
            }
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline position, updating the local position of the contained Hermite spline at the same time
        /// </summary>
        /// <param name="index">The index to change the position of</param>
        /// <param name="newPosition">The value of the new position</param>
        public void SetPositionAtIndexAndUpdateLocal(int index, Vector3 newPosition)
        {
            if (worldPositions == null)
            {
                worldPositions = new List<Vector3>() { Vector3.zero, Vector3.zero };
            }
            if (index < worldPositions.Count)
            {
                if (index > -1)
                {
                    worldPositions[index] = newPosition;
                }
                else
                {
                    Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was a negative value");
                }
            }
            else
            {
                Debug.LogError("Index given to SetPositionAtIndex method of HermiteSplineSequnce was greater than points count value");
            }
            spline.SetPositionAtIndex(index,transform.InverseTransformPoint(worldPositions[index]));
        }
        /// <summary>
        /// Sets the value of a given Hermite Spline tangent
        /// </summary>
        /// <param name="index">The index to change the tangent of</param>
        /// <param name="newTangent">The value of the new tangent</param>
        public void SetTanagentAtIndex(int index, Vector3 newTangent)
        {
            spline.SetTanagentAtIndex(index, newTangent);
        }
        /// <summary>
        /// Removes a position and tangent
        /// </summary>
        /// <param name="indexToRemoveAt">The index that the Position and Tangent in question should be removed at</param>
        public void RemovePositionAndTangentAtIndex(int indexToRemoveAt)
        {
            if (indexToRemoveAt < NumberOfPositionsAndTangents)
            {
                if (indexToRemoveAt > -1)
                {
                    worldPositions.RemoveAt(indexToRemoveAt);
                    spline.RemovePositionAndTangentAtIndex(indexToRemoveAt);
                }
                else
                {
                    Debug.LogError("Index given to RemovePositionAndTangentAtIndex method of HermiteSplineSequnce was a negative value");
                    return;
                }

                if (NumberOfPositionsAndTangents < 2)
                {
                    // Make sure there is always a start and end point
                    worldPositions.Add(Vector3.zero);
                    UpdateLengths();
                    return;
                }
                UpdateLengths();
                return;
            }
            Debug.LogError("Index given to RemovePositionAndTangentAtIndex method of HermiteSplineSequnce was greater than points and tangent count value");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the length of the spline between the point at the starting index and point at the next index
        /// </summary>
        /// <param name="startPointIndex">The index to get the point to start finding the index at</param>
        /// <returns>The length of the spline between the point at the starting index and next one</returns>
        private float GetLengthOfSingleSpline(int startPointIndex)
        {
            return HermiteSpline.GetTotalLength(worldPositions[startPointIndex],
                worldPositions[startPointIndex + 1], spline.GetTangentAtIndex(startPointIndex),
                spline.GetTangentAtIndex(startPointIndex + 1), spline.distancePrecisionPerHermiteSpline);
        }

        /// <summary>
        /// Update the lengths of the splines in the sequence
        /// </summary>
        private void UpdateLengths()
        {
            worldLengths = new List<float>(worldPositions.Count - 1);
            for (int i = 0; i < worldPositions.Count - 1; ++i)
            {
                worldLengths.Add(GetLengthOfSingleSpline(i));
            }
        }
        #endregion
    }
}
