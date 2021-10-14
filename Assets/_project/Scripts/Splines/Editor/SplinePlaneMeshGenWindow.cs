using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SleepyCat.Utility.Splines;


public class SplinePlaneMeshGenWindow : EditorWindow
{
    private SplineWrapper splineGeneratedFrom;

    private Vector3 previousRightPoint;
    private Vector3 previousLeftPoint;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Spline Plane Mesh Gen Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SplinePlaneMeshGenWindow window = (SplinePlaneMeshGenWindow)EditorWindow.GetWindow(typeof(SplinePlaneMeshGenWindow));
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        splineGeneratedFrom = (SplineWrapper)EditorGUILayout.ObjectField("Spline To Use", splineGeneratedFrom, typeof(SplineWrapper), true);
        if (splineGeneratedFrom != null)//GUILayout.Button("Generate Mesh"))
        {
            //Mesh mesh = new Mesh();
            //mesh.vertices = new Vector3[20];
            //mesh.SetIndices(new int[60], MeshTopology.Triangles, 0);
            for (float f = 0.0f; f <= 1.0f; f += 0.1f)
            {
                // Get the direction
                Vector3 direction = splineGeneratedFrom.GetDirection(f, f + 0.1f );

                // Get the left and right directions
                //rotate vector(x1, y1) counterclockwise by the given angle
                float angle = 90f * Mathf.Deg2Rad;

                float newX = direction.x * Mathf.Cos(angle) - direction.z * Mathf.Sin(angle);
                float newZ = direction.x * Mathf.Sin(angle) + direction.z * Mathf.Cos(angle);


                direction = new Vector3(newX, direction.y, newZ);
            }
        }

        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
         previousRightPoint = Vector3.negativeInfinity;
         previousLeftPoint = Vector3.negativeInfinity; 

        if (splineGeneratedFrom != null)//GUILayout.Button("Generate Mesh"))
        {
            float stepIncrement = 0.01f;
            //Mesh mesh = new Mesh();
            //mesh.vertices = new Vector3[20];
            //mesh.SetIndices(new int[60], MeshTopology.Triangles, 0);
            for (float f = 0.0f; f <= 1.0f; f += stepIncrement)
            {
                Vector3 direction;

                direction = splineGeneratedFrom.GetDirection(f, f + 0.00000001f);

                direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

                // Get the direction
                //if (f + stepIncrement < 1f)
                //{
                //    direction = splineGeneratedFrom.GetDirection(f, f + 0.00000001f);
                //}
                //else
                //{
                //    direction = splineGeneratedFrom.GetDirection(f, 1f - f);
                //}


                // Get the left and right directions
                //rotate vector(x1, y1) counterclockwise by the given angle
                float angle = 90f * Mathf.Deg2Rad;

                float newX = direction.x * Mathf.Cos(angle) - direction.z * Mathf.Sin(angle);
                float newZ = direction.x * Mathf.Sin(angle) + direction.z * Mathf.Cos(angle);


                direction = new Vector3(newX, direction.y, newZ);
                Vector3 linePoint = splineGeneratedFrom.GetPointAtTime(f);
                Vector3 rightPoint = splineGeneratedFrom.GetPointAtTime(f) + direction * 5f;
                Vector3 leftPoint = splineGeneratedFrom.GetPointAtTime(f) + direction * -5f;
                Handles.DrawLine(linePoint, rightPoint);
                Handles.DrawLine(linePoint, leftPoint);

                if (previousRightPoint != Vector3.negativeInfinity)
                {
                    Handles.DrawLine(previousRightPoint, rightPoint);
                    Handles.DrawLine(previousLeftPoint, leftPoint);
                }
                previousRightPoint = rightPoint;
                previousLeftPoint = leftPoint;



                HandleUtility.Repaint();


            }
        }
    }
}
