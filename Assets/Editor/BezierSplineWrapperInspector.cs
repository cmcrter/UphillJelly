//======================================================================================================================================================================================================================================================================================================
// File:            BezierSplineWrapperInspector.cs
// Author:          Matthew Mason
// Date Created:    05/10/2021
// Brief:           The custom inspector for the bezier curve spline wrapper
//======================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L7Games.Utility.Splines
{
    /// <summary>
    /// The custom inspector the Bezier curve spline wrapper script
    /// </summary>
    [CustomEditor(typeof(BezierCurveWrapper))]
    [CanEditMultipleObjects]
    public class BezierSplineWrapperInspector : Editor
    {
        #region Private Variables
        /// <summary>
        /// The line spline wrapper this is inspecting
        /// </summary>
        private BezierCurveWrapper bezierCurveWrapper;

        /// <summary>
        /// The SerializedProperty for distance precision the inspected BezierCurveWrapper is using
        /// </summary>
        private SerializedProperty distancePrecision;
        /// <summary>
        /// The SerializedProperty for the endPoint the inspected BezierCurveWrapper is using
        /// </summary>
        private SerializedProperty endPoint;
        /// <summary>
        /// The SerializedProperty for the startPoint the inspected BezierCurveWrapper is using
        /// </summary>
        private SerializedProperty startPoint;
        #endregion

        #region Unity Methods
        #region Overrides
        public override void OnInspectorGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (bezierCurveWrapper == null)
            {
                bezierCurveWrapper = (BezierCurveWrapper)target;
            }

            // Control point section
            EditorGUILayout.LabelField("Control Points", EditorStyles.boldLabel);
            bezierCurveWrapper.IsUsingTwoControlPoints = EditorGUILayout.Toggle("IsUsingTwoControlPoints", bezierCurveWrapper.IsUsingTwoControlPoints);
            // Make a control point field for each control point
            int numberOfControlPoints = 1;
            if (bezierCurveWrapper.IsUsingTwoControlPoints)
            {
                numberOfControlPoints = 2;
            }
            for (int i = 0; i < numberOfControlPoints; ++i)
            {
                if (bezierCurveWrapper.TryGetLocalControlPoint(i, out Vector3 controlPointValue))
                {
                    bezierCurveWrapper.SetLocalControlPointValues(i, EditorGUILayout.Vector3Field("Control Point " + i.ToString(), controlPointValue));
                }
            }
            EditorGUILayout.Space();

            // Positions section
            EditorGUILayout.LabelField("Positions", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(endPoint);
            EditorGUILayout.PropertyField(startPoint);
            EditorGUILayout.Space();

            // Everything else section
            EditorGUILayout.PropertyField(distancePrecision);
            EditorGUILayout.LabelField("Length: " + bezierCurveWrapper.GetTotalLength());

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
        private void OnEnable()
        {
            // Getting the object and its properties from the editor
            bezierCurveWrapper = (BezierCurveWrapper)target;
            distancePrecision = serializedObject.FindProperty("spline").FindPropertyRelative("distancePrecision");
            endPoint = serializedObject.FindProperty("spline").FindPropertyRelative("endPoint");
            startPoint = serializedObject.FindProperty("spline").FindPropertyRelative("startPoint");
        }

        private void OnSceneGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (bezierCurveWrapper == null)
            {
                bezierCurveWrapper = (BezierCurveWrapper)target;
            }

            // Check if the new position handles have moved then update the world positions of spline
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(bezierCurveWrapper.WorldStartPosition,
                bezierCurveWrapper.transform.rotation, target, "Start Position", out Vector3 newWorldStartPosition))
            {
                bezierCurveWrapper.SetWorldStartPointAndUpdateLocal(newWorldStartPosition);
            }
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(bezierCurveWrapper.WorldEndPosition,
                bezierCurveWrapper.transform.rotation, target, "End Position", out Vector3 newWorldEndPosition))
            {
                bezierCurveWrapper.SetWorldEndPointAndUpdateLocal(newWorldEndPosition);
            }

            // Make handles for the extra points, only updating them if they are moved
            int numberOfControlPoints = 1;
            if (bezierCurveWrapper.IsUsingTwoControlPoints)
            {
                numberOfControlPoints = 2;
            }
            for (int i = 0; i < numberOfControlPoints; ++i)
            {
                if (bezierCurveWrapper.TryGetWorldControlPoint(i, out Vector3 controlPositionStart))
                {
                    if (HandlesHelperFunctions.ChangeSenstivePositionHandle(controlPositionStart, bezierCurveWrapper.transform.rotation,
                        target, "Control Point", out Vector3 controlPointPosition))
                    {
                        bezierCurveWrapper.SetWorldControlPoint(i, controlPointPosition);
                    }
                }
            }
        }
        #endregion
    }
}
