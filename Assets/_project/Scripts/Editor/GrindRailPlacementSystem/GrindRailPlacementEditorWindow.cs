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

/// <summary>
/// Editor Window for controlling the use of the Grind Rail Placement Tool 
/// </summary>
public class GrindRailPlacementEditorWindow : EditorWindow
{
    #region Private Enumerations
    private enum PlacementState
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
    private PlacementState currentPlacementState;

    /// <summary>
    /// The spline sequence that the user is in the process of creating
    /// </summary>
    [SerializeField]
    private SplineSequence splineSequenceBeingCreated;
    #endregion

    #region Unity Methods
    private void OnGUI()
    {
        // If the user is currently not placing anything then they can start placing a sequence, otherwise they must end it
        if (currentPlacementState == PlacementState.NotPlacing)
        {
            if (GUILayout.Button("Start Spline Sequence"))
            {
                currentPlacementState = PlacementState.PlacingLines; 
            }
        }
        else
        {
            if (GUILayout.Button("Finish Spline Sequence"))
            {
                currentPlacementState = PlacementState.NotPlacing;
            }
        }

        if (currentPlacementState != PlacementState.NotPlacing)
        {
            // Present the user with spline options
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentPlacementState == PlacementState.PlacingLines, "Place Line" , "Button"))
            {
                currentPlacementState = PlacementState.PlacingLines;
            }
            if (GUILayout.Toggle(currentPlacementState == PlacementState.PlacingBezierSpline, "Place Bezier Spline",  "Button"))
            {
                currentPlacementState = PlacementState.PlacingBezierSpline;
            }
            if (GUILayout.Toggle(currentPlacementState == PlacementState.PlacingHermiteSpline, "Place Hermite Spline",  "Button"))
            {
                currentPlacementState = PlacementState.PlacingHermiteSpline;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    // Window has been selected
    private void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }
    #endregion

    #region Private Static Methods
    // Add menu named "My Window" to the Window menu
    [MenuItem("Grind Rail/Placer")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        GrindRailPlacementEditorWindow window = (GrindRailPlacementEditorWindow)EditorWindow.GetWindow(typeof(GrindRailPlacementEditorWindow));
        window.Show();
    }
    #endregion

    #region Private Methods
    private void OnSceneGUI(SceneView sceneView)
    {
        if (currentPlacementState != PlacementState.NotPlacing)
        {
            Event guiEvent = Event.current;

            // Get where the user is pointing in the scene
            if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition), out RaycastHit raycastHit))
            {
                // Draw the indicator at the spot plus it height addition
                Handles.color = Color.red;

                Handles.DrawWireDisc(raycastHit.point, raycastHit.normal, 0.1f);
                Handles.DrawWireDisc(raycastHit.point, Vector3.Cross(raycastHit.normal, Vector3.right), 0.1f);
                Handles.DrawWireDisc(raycastHit.point, Vector3.Cross(raycastHit.normal, Vector3.forward), 0.1f);

                HandleUtility.Repaint();

                // Check if the user has click to place the 
                if (guiEvent.type == EventType.MouseDown)
                {
                    if (guiEvent.button == 0)
                    {
                        Undo.RecordObject(this, "PlaceSplinePoint");
                    }
                }
            }
        }
    }
    #endregion
}
