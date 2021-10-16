//==========================================================================================================================================================================================================================================================================================================
// File:            SplinePlaneMeshGenWindow.cs
// Author:          Matthew Mason
// Date Created:    14/10/2021
// Brief:           An editor window used to  generate a mesh from a spline
//==========================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SleepyCat.Utility.Splines;

/// <summary>
/// An editor window used to  generate a mesh from a spline
/// </summary>
public class SplinePlaneMeshGenWindow : EditorWindow
{
    #region Private Structures
    /// <summary>
    /// The line that breaks up the spline into segment so it can be made into quads
    /// </summary>
    private struct SegmentDevisionLine
    {
        public Vector3 right;
        public Vector3 left;
    }
    #endregion

    #region Private Variables
    /// <summary>
    /// The animation curve that defines the width of the mesh along the spline
    /// </summary>
    private AnimationCurve widthCurve;

    /// <summary>
    /// The amount the width curve is multiplied by across it length
    /// </summary>
    private float widthMultiplier;

    /// <summary>
    /// The number of segments the mesh should be made up of
    /// </summary>
    private int segmentCount;

    /// <summary>
    /// The spline wrapper to use as the spline for the mesh
    /// </summary>
    private SplineWrapper splineGeneratedFrom;

    /// <summary>
    /// The right point in the segment devision made
    /// </summary>
    private Vector3 previousRightPoint;
    /// <summary>
    /// The left point in the segment devision made
    /// </summary>
    private Vector3 previousLeftPoint;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        widthCurve = new AnimationCurve();
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
        // Show the mesh settings (Spline field, Segment count, Width multiplier and Width curve)
        GUILayout.Label("Mesh Settings", EditorStyles.boldLabel);
        splineGeneratedFrom = (SplineWrapper)EditorGUILayout.ObjectField("Spline To Use", splineGeneratedFrom, typeof(SplineWrapper), true);
        segmentCount = EditorGUILayout.IntField("Segment Count", segmentCount);
        if (segmentCount < 1)
        {
            segmentCount = 1;
        }
        widthMultiplier = EditorGUILayout.FloatField("Width Multiplier", widthMultiplier);
        widthCurve = EditorGUILayout.CurveField("WidthCurve", widthCurve);
        EditorGUILayout.Space();

        // Show the user a button to generate the mesh
        if (GUILayout.Button("Generate Mesh"))
        {
            if (splineGeneratedFrom.gameObject.transform.localScale != Vector3.one)
            {
                Debug.LogWarning("Newly generated spline mesh will not be affected by the local scale of base spline");
            }

            Mesh newMesh = GenerateMesh();
            MeshSaverEditor.SaveMesh(newMesh, "NewSplineMesh", false, true);
        }
    }
    #endregion

    #region Private Static Methods
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Spline Plane Mesh Gen Window")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        SplinePlaneMeshGenWindow window = (SplinePlaneMeshGenWindow)EditorWindow.GetWindow(typeof(SplinePlaneMeshGenWindow));
        window.Show();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Generates a mesh based on the spline and the settings defined in the window
    /// </summary>
    /// <returns>A mesh based on the spline and the settings defined in the window</returns>
    private Mesh GenerateMesh()
    {
        previousRightPoint = Vector3.negativeInfinity;
        previousLeftPoint = Vector3.negativeInfinity;

        Mesh mesh = new Mesh();
        // The number of triangles should be equal to twice the number of generated segments (then times 3 for each point in the triangle, for the list)
        List<int> triangles = new List<int>(segmentCount * 2 * 3);
        // The number of vertices should be equal to twice the number of generated segments + 2 for the start vertices (then times 3 for each point in the triangle, for the list)
        List<Vector3> verts = new List<Vector3>(segmentCount * 2 + 2);

        int currentIndiceIndex = 0;
        SegmentDevisionLine newSegment;

        Vector3 direction = splineGeneratedFrom.GetDirection(0.0f, 0.01f);
        Vector3 pointOnLine = splineGeneratedFrom.GetLocalPointAtTime(0.0f);

        // Set up the previous right and left points so each segment can be drawn
        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        direction = GetRightFromVector(direction);
        previousRightPoint = pointOnLine + direction * widthCurve.Evaluate(0.0f) * widthMultiplier;
        previousLeftPoint = pointOnLine + direction * widthCurve.Evaluate(0.0f) * -widthMultiplier;
        verts.Add(previousRightPoint);
        verts.Add(previousLeftPoint);

        // Iterate over the spline and create the vertices and triangles for each point in the spline
        float stepIncrement = 1f / segmentCount;
        float f = stepIncrement;
        for (; f < 1.0f; f += stepIncrement)
        {
            direction = splineGeneratedFrom.GetDirection(f, 0.01f);
            pointOnLine = splineGeneratedFrom.GetLocalPointAtTime(f);
            newSegment = CreateSegmentDevision(pointOnLine, direction, f);
            AddSegmentVertsAndTrianglesFromSegmentToLists(verts, triangles, newSegment, currentIndiceIndex, out currentIndiceIndex);
        }

        // Connect up the last made point to the end of the spline
        f -= stepIncrement;
        Vector3 linePoint = splineGeneratedFrom.GetLocalPointAtTime(f);
        newSegment = CreateSegmentDevision(splineGeneratedFrom.LocalEndPosition, (splineGeneratedFrom.LocalEndPosition - linePoint).normalized, 1.0f);
        AddSegmentVertsAndTrianglesFromSegmentToLists(verts, triangles, newSegment, currentIndiceIndex, out currentIndiceIndex);

        mesh.SetVertices(verts.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        return mesh;
    }

    /// <summary>
    /// Builds a segment devision line based on a point on the spline and the direction of the spline as of that point
    /// </summary>
    /// <param name="pointOnSpline">The point on the spline to end the segment at</param>
    /// <param name="direction">The direction the spline was moving as of reach the point on the spline</param>
    /// <param name="t">The time value for how far up the spline the point on the spline is</param>
    /// <returns>A segment devision line based on a point on the spline and the direction of the spline as of that point</returns>
    private SegmentDevisionLine CreateSegmentDevision(Vector3 pointOnSpline, Vector3 direction, float t)
    {
        SegmentDevisionLine newSegment = new SegmentDevisionLine();

        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        direction = GetRightFromVector(direction);

        Vector3 rightPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * widthMultiplier;
        Vector3 leftPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * -widthMultiplier;

        newSegment.right = rightPoint;
        newSegment.left = leftPoint;

        previousRightPoint = rightPoint;
        previousLeftPoint = leftPoint;

        return newSegment;
    }

    /// <summary>
    /// Returns a vector 90 degrees clockwise along the X and Z axis to a given vector
    /// </summary>
    /// <param name="forwardVector">The vector to find a vector to the right of</param>
    /// <returns>a vector 90 degrees clockwise along the X and Z axis to a given vector</returns>
    private Vector3 GetRightFromVector(Vector3 forwardVector)
    {
        // rotate vector(x1, y1) counterclockwise by the given angle
        float angle = 90f * Mathf.Deg2Rad;

        float newX = forwardVector.x * Mathf.Cos(angle) - forwardVector.z * Mathf.Sin(angle);
        float newZ = forwardVector.x * Mathf.Sin(angle) + forwardVector.z * Mathf.Cos(angle);

        return new Vector3(newX, forwardVector.y, newZ);
    }

    /// <summary>
    /// Adds vertices and triangles to provided lists based on a new segment division, the current indice index for the triangles that has been reached, updating and outing the indice index for the triangles afterwards
    /// </summary>
    /// <param name="vertsListToAddTo">The list of vertices to add to</param>
    /// <param name="trianglesListToAddTo">The list of triangles to add</param>
    /// <param name="segment">The segment to based the new vertice and triangles off of</param>
    /// <param name="currentIndiceIndex">the current index for the current right vertice of SegmentDevisionLine in the in the vertice array</param>
    /// <param name="newCurrentIndiceIndex">output for the updating of currentIndiceIndex</param>
    private void AddSegmentVertsAndTrianglesFromSegmentToLists(List<Vector3> vertsListToAddTo, List<int> trianglesListToAddTo, SegmentDevisionLine segment, int currentIndiceIndex, out int newCurrentIndiceIndex)
    {
        // Add the segment points to the vertices
        vertsListToAddTo.Add(segment.right);
        vertsListToAddTo.Add(segment.left);
        // Build the triangles from these points
        // 1--3
        // | /|
        // |/ |
        // 0--2
        // (Right side from this perspective is the top of the segment)
        // 0-1-3 triangle
        trianglesListToAddTo.Add(currentIndiceIndex + 3);
        trianglesListToAddTo.Add(currentIndiceIndex + 1);
        trianglesListToAddTo.Add(currentIndiceIndex);
        // 0-3-2 triangle
        trianglesListToAddTo.Add(currentIndiceIndex + 2);
        trianglesListToAddTo.Add(currentIndiceIndex + 3);
        trianglesListToAddTo.Add(currentIndiceIndex);

        newCurrentIndiceIndex = currentIndiceIndex + 2;
    }

    /// <summary>
    /// Draw the lines for a segment (connecting the new points to the previous ones) using the Handles class
    /// </summary>
    /// <param name="pointOnSpline">The point on the spline to end the segment at</param>
    /// <param name="direction">The direction the spline was moving as of reach the point on the spline</param>
    /// <param name="t">The time value for how far up the spline the point on the spline is</param>
    private void DrawLineSegmentsHandles(Vector3 pointOnSpline, Vector3 direction, float t)
    {
        // Draw the directional vector
        Handles.color = Color.green;
        Handles.DrawLine(pointOnSpline, pointOnSpline + direction);

        // Project the direction on a level plane to eliminate roll rotation
        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        // Get the left and right directions and draw them
        direction = GetRightFromVector(direction);
        Vector3 rightPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * widthMultiplier;
        Vector3 leftPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * -widthMultiplier;
        Handles.color = Color.white;
        Handles.DrawLine(pointOnSpline, rightPoint);
        Handles.DrawLine(pointOnSpline, leftPoint);

        // Connect lines to the previousRightPoint
        if (previousRightPoint != Vector3.negativeInfinity)
        {
            Handles.DrawLine(previousRightPoint, rightPoint);
            Handles.DrawLine(previousLeftPoint, leftPoint);
            Handles.DrawLine(previousRightPoint, leftPoint);
        }
        previousRightPoint = rightPoint;
        previousLeftPoint = leftPoint;

        HandleUtility.Repaint();
    }
    /// <summary>
    /// Called during the scene view to draw the spline mesh outline onto the view
    /// </summary>
    /// <param name="sceneView">The scene view that is being called during</param>
    private void OnSceneGUI(SceneView sceneView)
    {
        // Set up previous left and right view to a junk value so the they do not get used until they are properly assigned
        previousRightPoint = Vector3.negativeInfinity;
        previousLeftPoint = Vector3.negativeInfinity;

        // Draw the mesh wire-frame that would be generated
        if (splineGeneratedFrom != null)
        {
            float stepIncrement = 1f / segmentCount; 
            float f;
            for (f = 0.0f; f < 1.0f; f += stepIncrement)
            {
                DrawLineSegmentsHandles(splineGeneratedFrom.GetPointAtTime(f), splineGeneratedFrom.GetDirection(f, stepIncrement), f);
            }

            f = f - stepIncrement;
            Vector3 linePoint = splineGeneratedFrom.GetPointAtTime(f);
            DrawLineSegmentsHandles(splineGeneratedFrom.WorldEndPosition, (splineGeneratedFrom.WorldEndPosition - linePoint).normalized, 1.0f);
        }
    }
    #endregion
}
