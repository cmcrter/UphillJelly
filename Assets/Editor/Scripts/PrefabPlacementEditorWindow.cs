using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat
{
    public class PrefabPlacementEditorWindow : EditorWindow
    {
        private bool placementActivated;

        private float heightOffset;

        private GameObject temporaryVisualsationGameObject;

        private LayerMask raycastMask;

        [MenuItem("Window/Prefab Placer")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            PrefabPlacementEditorWindow window = (PrefabPlacementEditorWindow)EditorWindow.GetWindow(typeof(PrefabPlacementEditorWindow));
            window.Show();
        }

        void OnFocus()
        {
            // Remove delegate listener if it has previously
            // been assigned.
            SceneView.duringSceneGui -= this.OnSceneGUI;
            // Add (or re-add) the delegate.
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        void OnDestroy()
        {
            // When the window is destroyed, remove the delegate
            // so that it will no longer do any drawing.
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnGUI()
        {
            GUILayout.Label("Prefab Placer", EditorStyles.boldLabel);
            heightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);
            //raycastMask = EditorGUILayout.MaskField("Placement Layer Mask", )


            placementActivated = GUILayout.Toggle(placementActivated, "Start Placing Prefabs", "Button");
        }

        void OnSceneGUI(SceneView sceneView)
        {
            // Do your drawing here using Handles.

            Vector2 mousePosition = new Vector3(Event.current.mousePosition.x, SceneView.lastActiveSceneView.camera.pixelHeight - Event.current.mousePosition.y);

            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(mousePosition);
            Handles.color = Color.green;

            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit raycastHit2, 100f, LayerMask.a, QueryTriggerInteraction.Ignore))
            {
                Handles.DrawSolidDisc(raycastHit2.point, raycastHit2.normal, 1f);
            }

            Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.left, 100f, 20f);

            HandleUtility.Repaint();
        }
    }
}
