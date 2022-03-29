using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat
{
    public class PrefabPlacementEditorWindow : EditorWindow
    {
        private const int NumberOfPossibleLayers = 32;

        private static string[] physicLayerNames;

        private bool placementActivated;

        private bool useWorldUp;

        private float heightOffset;

        private Vector3 rotationAngles;

        private GameObject temporaryVisualsationGameObject;

        private LayerMask raycastMask = Physics.AllLayers;

        private GameObject brushObject;

        [MenuItem("Window/Prefab Placer Window")]
        static void Init()
        {
            Debug.Log("Heres");
            // Get existing open window or if none, make a new one:
            PrefabPlacementEditorWindow window = (PrefabPlacementEditorWindow)EditorWindow.GetWindow(typeof(PrefabPlacementEditorWindow));
            window.Show();

            List<string> layerNames = new List<string>(NumberOfPossibleLayers);
            for (int i = 0; i < NumberOfPossibleLayers; ++i)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != null)
                {
                    if (layerName != "")
                    {
                        layerNames.Add(layerName);
                    }
                }
            }
            physicLayerNames = GetPhysicsLayerName();
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
            if (temporaryVisualsationGameObject != null)
            {
                DestroyImmediate(temporaryVisualsationGameObject);
            }
        }

        void OnGUI()
        {
            if (physicLayerNames == null)
            {
                physicLayerNames = GetPhysicsLayerName();
            }

            GUILayout.Label("Prefab Placer", EditorStyles.boldLabel);
            float tempHeightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);
            if (tempHeightOffset != heightOffset)
            {
                heightOffset = tempHeightOffset;
                Undo.RecordObject(this, "Placement Brush height offset changed");
            }
            rotationAngles = EditorGUILayout.Vector3Field("Rotation Offset", rotationAngles);

            raycastMask = EditorGUILayout.MaskField("Placement Layer Mask", raycastMask, physicLayerNames);
            GameObject tempBrushObject = (GameObject)EditorGUILayout.ObjectField("Brush Prefab", brushObject, typeof(GameObject), false);
            if (tempBrushObject != brushObject)
            {
                brushObject = tempBrushObject;
                PlacementStateChanged();
            }
            bool placementActivatedTemp = GUILayout.Toggle(placementActivated, "Start Placing Prefabs", "Button");
            if (placementActivatedTemp != placementActivated)
            {
                placementActivated = placementActivatedTemp;
                PlacementStateChanged();
            }

        }

        void OnSceneGUI(SceneView sceneView)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            // Do your drawing here using Handles.
            if (placementActivated)
            {
                if (Selection.objects.Length > 0)
                {
                    Selection.objects = new Object[0];
                }
                RaycastHit raycastHit2 = new RaycastHit();
                Vector2 mousePosition = new Vector3(Event.current.mousePosition.x, SceneView.lastActiveSceneView.camera.pixelHeight - Event.current.mousePosition.y);

                if (CheckMouseInsideSceneBounds(mousePosition))
                {
                    Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(mousePosition);
                    Handles.color = Color.green;

                    if (Physics.Raycast(ray.origin, ray.direction, out raycastHit2, 100f, raycastMask, QueryTriggerInteraction.Ignore))
                    {
                        if (brushObject != null)
                        {
                            if (temporaryVisualsationGameObject == null)
                            {
                                CreateBrushGhostObject(Vector3.zero, Quaternion.identity);
                            }
                            temporaryVisualsationGameObject.transform.position = raycastHit2.point + raycastHit2.normal * heightOffset;
                            Vector3 upDirection = raycastHit2.normal;
                            if (useWorldUp)
                            {
                                upDirection = Vector3.up;
                            }
                            temporaryVisualsationGameObject.transform.up = upDirection;
                            temporaryVisualsationGameObject.transform.Rotate(Vector3.up, rotationAngles.y);
                            temporaryVisualsationGameObject.transform.Rotate(Vector3.forward, rotationAngles.z);
                            temporaryVisualsationGameObject.transform.Rotate(Vector3.right, rotationAngles.x);
                        }
                        else
                        {
                            if (temporaryVisualsationGameObject != null)
                            {
                                GameObject.DestroyImmediate(temporaryVisualsationGameObject);
                            }
                        }

                        Handles.DrawSolidDisc(raycastHit2.point, raycastHit2.normal, 0.1f);
                    }

                    Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.left, 100f, 20f);


                }
                else
                {
                    if (temporaryVisualsationGameObject != null)
                    {
                        GameObject.DestroyImmediate(temporaryVisualsationGameObject);
                    }
                }
                HandleUtility.Repaint();
                if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                {
                    if (CheckMouseInsideSceneBounds(mousePosition))
                    {
                        GUIUtility.hotControl = 0;
                        if (temporaryVisualsationGameObject != null)
                        {
                            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab((Object)brushObject);
                            newObject.transform.position = raycastHit2.point + raycastHit2.normal * heightOffset;
                            Vector3 upDirection = raycastHit2.normal;
                            if (useWorldUp)
                            {
                                upDirection = Vector3.up;
                            }
                            newObject.transform.up = upDirection;
                            newObject.transform.Rotate(Vector3.up, rotationAngles.y);
                            newObject.transform.Rotate(Vector3.forward, rotationAngles.z);
                            newObject.transform.Rotate(Vector3.right, rotationAngles.x);

                            Undo.RegisterCreatedObjectUndo(newObject, "Placed " + newObject.name + " With Brush");
                        }
                        Event.current.Use();
                    }
                }
                else if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (CheckMouseInsideSceneBounds(mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        Event.current.Use();
                    }
                }
            }
        }

        private static string[] GetPhysicsLayerName()
        {
            List<string> layerNames = new List<string>(NumberOfPossibleLayers);
            for (int i = 0; i < NumberOfPossibleLayers; ++i)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != null)
                {
                    if (layerName != "")
                    {
                        layerNames.Add(layerName);
                    }
                }
            }
            return layerNames.ToArray();
        }

        private void PlacementStateChanged()
        {
            if (brushObject != null && placementActivated)
            {
                if (temporaryVisualsationGameObject == null)
                {
                    CreateBrushGhostObject(Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Vector3 position = temporaryVisualsationGameObject.transform.position;
                    Quaternion rotation = temporaryVisualsationGameObject.transform.rotation;
                    if (temporaryVisualsationGameObject != null)
                    {
                        GameObject.DestroyImmediate(temporaryVisualsationGameObject);
                    }
                }
            }
            else
            {
                if (temporaryVisualsationGameObject != null)
                {
                    GameObject.DestroyImmediate(temporaryVisualsationGameObject);
                }
            }
        }

        private void CreateBrushGhostObject(Vector3 position, Quaternion rotation)
        {
            temporaryVisualsationGameObject = GameObject.Instantiate(brushObject, position, rotation);
            temporaryVisualsationGameObject.name = "TEMP_BRUSH_OBJECT";
            RemoveCollidersFromGhost(temporaryVisualsationGameObject);
        }

        private void RemoveCollidersFromGhost(GameObject brushObject)
        {
            Collider[] colliders = brushObject.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; ++i)
            {
                DestroyImmediate(colliders[i]);
            }
            for (int i = 0; i < brushObject.transform.childCount; ++i)
            {
                RemoveCollidersFromGhost(brushObject.transform.GetChild(i).gameObject);
            }
        }

        private bool CheckMouseInsideSceneBounds(Vector2 mousePositon)
        {
            if (mousePositon.x > 0f && mousePositon.x < SceneView.lastActiveSceneView.camera.pixelWidth)
            {
                if (mousePositon.y > 0f && mousePositon.y < SceneView.lastActiveSceneView.camera.pixelHeight)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
