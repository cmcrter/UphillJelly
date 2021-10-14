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

        private bool showPositionsAndTangents;

        private List<bool> activeFoldouts;

        /// <summary>
        /// The SerializedProperty for distance precision the inspected BezierCurveWrapper is using
        /// </summary>
        private SerializedProperty distancePrecision;
        #endregion


        public override void OnInspectorGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (hermiteSplineSequence == null)
            {
                hermiteSplineSequence = (HermiteSplineSequenceWrapper)target;
            }

            if (activeFoldouts == null)
            {
                activeFoldouts = new List<bool>(hermiteSplineSequence.NumberOfPositionsAndTangents);
                for (int i = 0; i < hermiteSplineSequence.NumberOfPositionsAndTangents; ++i)
                {
                    activeFoldouts.Add(false);
                }
            }
            else if (activeFoldouts.Count != hermiteSplineSequence.NumberOfPositionsAndTangents)
            {
                activeFoldouts = new List<bool>(hermiteSplineSequence.NumberOfPositionsAndTangents);
                for (int i = 0; i < hermiteSplineSequence.NumberOfPositionsAndTangents; ++i)
                {
                    activeFoldouts.Add(false);
                }
            }

            distancePrecision = serializedObject.FindProperty("spline").FindPropertyRelative("distancePrecisionPerHermiteSpline");

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
                    activeFoldouts.Add(false);
                }
            }
            else if (newListSize < startingListSize)
            {
                while (hermiteSplineSequence.NumberOfPositionsAndTangents > newListSize)
                {
                    hermiteSplineSequence.RemovePositionAndTangentAtIndex(hermiteSplineSequence.NumberOfPositionsAndTangents - 1);
                    activeFoldouts.RemoveAt(hermiteSplineSequence.NumberOfPositionsAndTangents - 1);
                }
            }
            if (showPositionsAndTangents = EditorGUILayout.Foldout(showPositionsAndTangents, "Positions And Tangents"))
            {
                ++EditorGUI.indentLevel;

                for (int i = 0; i < hermiteSplineSequence.NumberOfPositionsAndTangents; ++i)
                {
                    if (activeFoldouts[i] = EditorGUILayout.Foldout(activeFoldouts[i], "Position And Tangent " + i.ToString()))
                    {
                        ++EditorGUI.indentLevel;
                        hermiteSplineSequence.SetLocalPositionAtIndex(i, EditorGUILayout.Vector3Field("Position", hermiteSplineSequence.GetLocalPositionAtIndex(i)));
                        hermiteSplineSequence.SetTanagentAtIndex(i, EditorGUILayout.Vector3Field("Tangent", hermiteSplineSequence.GetTangentAtIndex(i)));
                        --EditorGUI.indentLevel;
                    }
                }
                --EditorGUI.indentLevel;
            }

            EditorGUILayout.Space();

            // Everything else section
            EditorGUILayout.PropertyField(distancePrecision);



            serializedObject.ApplyModifiedProperties();
            hermiteSplineSequence.UpdateWorldPositions();
            SceneView.RepaintAll();
        }

        private void OnEnable()
        {
            // Getting the object and its properties from the editor
            hermiteSplineSequence = (HermiteSplineSequenceWrapper)target;
            distancePrecision = serializedObject.FindProperty("spline").FindPropertyRelative("distancePrecision");
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
