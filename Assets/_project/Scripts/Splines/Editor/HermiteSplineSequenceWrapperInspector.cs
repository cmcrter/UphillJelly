using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// The custom inspector the Bezier curve spline wrapper script
    /// </summary>
    [CustomEditor(typeof(HermiteSplineSequenceWrapper))]
    [CanEditMultipleObjects]
    public class HermiteSplineSequenceWrapperInspector : Editor
    {
        #region Private Variables
        /// <summary>
        /// The line spline wrapper this is inspecting
        /// </summary>
        private HermiteSplineSequenceWrapper hermiteSplineSequence;
        #endregion

        public override void OnInspectorGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (hermiteSplineSequence == null)
            {
                hermiteSplineSequence = (HermiteSplineSequenceWrapper)target;
            }

            EditorGUILayout.LabelField("Positions And Tangents", EditorStyles.boldLabel);
            int startingListSize = hermiteSplineSequence.NumberOfPositionsAndTangents;
            int newListSize = EditorGUILayout.IntField("Number Of Positions And Tangents", hermiteSplineSequence.NumberOfPositionsAndTangents);
            if (newListSize < 2)
            {
                newListSize = 2;
            }
            if (newListSize > startingListSize)
            {
                while (hermiteSplineSequence.NumberOfPositionsAndTangents < newListSize)
                {
                    hermiteSplineSequence.AddNewPositionAndTangent(Vector3.zero, Vector3.zero);
                }
            }
            else if (newListSize < startingListSize)
            {
                while (hermiteSplineSequence.NumberOfPositionsAndTangents > newListSize)
                {
                    hermiteSplineSequence.RemovePositionAndTangentAtIndex(hermiteSplineSequence.NumberOfPositionsAndTangents - 1);
                }
            }

            for (int i = 0; i < hermiteSplineSequence.NumberOfPositionsAndTangents; ++i)
            {
                EditorGUILayout.LabelField("Position And Tangent " + i.ToString());
                hermiteSplineSequence.SetPositionAtIndex(i, EditorGUILayout.Vector3Field("Position", hermiteSplineSequence.GetPositionAtIndex(i)));
                hermiteSplineSequence.SetTanagentAtIndex(i, EditorGUILayout.Vector3Field("Tangent", hermiteSplineSequence.GetTangentAtIndex(i)));
            }

            //base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (hermiteSplineSequence == null)
            {
                hermiteSplineSequence = (HermiteSplineSequenceWrapper)target;
            }

            // Add change sensitive handles for each of the points and tangents in the target sequence
            for (int i = 0; i < hermiteSplineSequence.NumberOfPositionsAndTangents; ++i)
            {
                if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteSplineSequence.GetPositionAtIndex(i),
                    hermiteSplineSequence.transform.rotation, target, "Hermite Sequence Position", out Vector3 currentPosition))
                {
                    hermiteSplineSequence.SetPositionAtIndexAndUpdateLocal(i, currentPosition);
                }

                if (HandlesHelperFunctions.ChangeSenstivePositionHandle(currentPosition + hermiteSplineSequence.GetTangentAtIndex(i),
                    hermiteSplineSequence.transform.rotation, target, "Hermite Sequence Tangent", out Vector3 newTangent))
                {
                    hermiteSplineSequence.SetTanagentAtIndex(i, newTangent - currentPosition);
                }
            }

            //// Check if the new position handles have moved then update the world positions of spline
            //if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldStartPosition,
            //    hermiteCurveWrapper.transform.rotation, target, "Start Position", out Vector3 newWorldStartPosition))
            //{
            //    hermiteCurveWrapper.SetWorldStartPointAndUpdateLocal(newWorldStartPosition);
            //}
            //if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldEndPosition,
            //    hermiteCurveWrapper.transform.rotation, target, "End Position", out Vector3 newWorldEndPosition))
            //{
            //    hermiteCurveWrapper.SetWorldEndPointAndUpdateLocal(newWorldEndPosition);
            //}

            //// Add the tangent handles so they can define a direction away from the Hermite spline
            //if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldEndPosition + hermiteCurveWrapper.EndTangent,
            //    hermiteCurveWrapper.transform.rotation, target, "End Tangent", out Vector3 newWorldEndTangent))
            //{
            //    hermiteCurveWrapper.EndTangent = newWorldEndTangent - hermiteCurveWrapper.WorldEndPosition;
            //}
            //if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldStartPosition + hermiteCurveWrapper.StartTangent,
            //    hermiteCurveWrapper.transform.rotation, target, "Start Tangent", out Vector3 newWorldStartTangent))
            //{
            //    hermiteCurveWrapper.StartTangent = newWorldStartTangent - hermiteCurveWrapper.WorldStartPosition;
            //}
        }
    }
}
