////////////////////////////////////////////////////////////////////////////////////////////////////////
// File:            GrindRailPlacementEditorWindow.cs
// Author:          Matthew Mason
// Date Created:    01/10/2021
// Brief:           Editor Window for controlling the use of the Grind Rail Placement Tool 
///////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SleepyCat.Utility.Splines;

namespace SleepyCat.EditorWindows.Splines
{
    /// <summary>
    /// Editor Window for controlling the use of the Grind Rail Placement Tool 
    /// </summary>
    public class SplinePlacementEditorWindow : PlacerWindow
    {
        #region Private Enumerations
        private enum PlacementChoiceState
        {
            NotPlacing,
            PlacingLines,
            PlacingBezierSpline,
            PlacingHermiteSpline,
        }
        #endregion

        #region Private Serialized Fields
        /// <summary>
        /// The current state the window is in for placing the rails
        /// </summary>
        [SerializeField]
        private PlacementChoiceState currentPlacementState;

        /// <summary>
        /// The spline sequence that the user is in the process of creating
        /// </summary>
        [SerializeField]
        private SplineSequence splineSequenceBeingCreated;

        [SerializeField]
        private List<Spline> splinesPlaced;
        #endregion

        #region Unity Methods
        private void OnGUI()
        {
            // If the user is currently not placing anything then they can start placing a sequence, otherwise they must end it
            if (currentPlacementState == PlacementChoiceState.NotPlacing)
            {
                if (GUILayout.Button("Start Spline Sequence"))
                {
                    currentPlacementState = PlacementChoiceState.PlacingLines;
                }
            }
            else
            {
                if (GUILayout.Button("Finish Spline Sequence"))
                {
                    currentPlacementState = PlacementChoiceState.NotPlacing;
                }
            }

            if (currentPlacementState != PlacementChoiceState.NotPlacing)
            {
                // Present the user with spline options
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Toggle(currentPlacementState == PlacementChoiceState.PlacingLines, "Place Line", "Button"))
                {
                    currentPlacementState = PlacementChoiceState.PlacingLines;
                }
                if (GUILayout.Toggle(currentPlacementState == PlacementChoiceState.PlacingBezierSpline, "Place Bezier Spline", "Button"))
                {
                    currentPlacementState = PlacementChoiceState.PlacingBezierSpline;
                }
                if (GUILayout.Toggle(currentPlacementState == PlacementChoiceState.PlacingHermiteSpline, "Place Hermite Spline", "Button"))
                {
                    currentPlacementState = PlacementChoiceState.PlacingHermiteSpline;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        #endregion

        #region Private Static Methods
        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/Spline Placer")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            SplinePlacementEditorWindow window = (SplinePlacementEditorWindow)EditorWindow.GetWindow(typeof(SplinePlacementEditorWindow));
            window.Show();
        }
        #endregion

        #region Private Methods
        protected override void OnPlacementLocationSelected(Vector3 location, Vector3 normal)
        {
            Undo.RecordObject(this, "PlaceSplinePoint");
        }
        #endregion
    }
}



