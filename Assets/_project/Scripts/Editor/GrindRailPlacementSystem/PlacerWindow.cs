////////////////////////////////////////////////////////////////////////////////////////////////////////
// File:            PlacerWindow.cs
// Author:          Matthew Mason
// Date Created:    12/10/2021
// Brief:           An abstract parent for window used to place objects
///////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// An abstract parent for window used to place objects
/// </summary>
public abstract class PlacerWindow : EditorWindow
{
    [SerializeField]
    [Tooltip("The amount the placement is raised from the position by the normal")]
    protected float raisedHeight;

    // Window has been selected
    protected virtual void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    protected virtual bool RaycastPlacementLocation(out RaycastHit hitInfo)
    {
        Event guiEvent = Event.current;

        // Get where the user is pointing in the scene
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition), out hitInfo))
        {
            return true;
        }
        return false;
    }    

    protected virtual void DrawPlacementIndicator(RaycastHit raycastHit)
    {
        // Draw the indicator at the spot plus it height addition
        Handles.color = Color.red;

        Handles.DrawWireDisc(raycastHit.point, raycastHit.normal, 0.1f);
        Handles.DrawWireDisc(raycastHit.point, Vector3.Cross(raycastHit.normal, Vector3.right), 0.1f);
        Handles.DrawWireDisc(raycastHit.point, Vector3.Cross(raycastHit.normal, Vector3.forward), 0.1f);

        HandleUtility.Repaint();
    }

    protected virtual void OnSceneGUI(SceneView sceneView)
    {
        Event guiEvent = Event.current;

        // Get where the user is pointing in the scene
        if (RaycastPlacementLocation(out RaycastHit raycastHit))
        {
            DrawPlacementIndicator(raycastHit);

            // Check if the user has click to place the 
            if (guiEvent.type == EventType.MouseDown)
            {
                if (guiEvent.button == 0)
                {
                    OnPlacementLocationSelected(raycastHit.point + raycastHit.normal * raisedHeight, raycastHit.normal);
                }
            }
        }
    }

    protected abstract void OnPlacementLocationSelected(Vector3 location, Vector3 normal);
}
