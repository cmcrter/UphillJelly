//==========================================================================================================================================================================================================================================================================================================
// File:            SplinePlaneMeshGenWindow.cs
// Author:          Matthew Mason
// Date Created:    14/10/2021
// Brief:           An editor window used to generate meshes from a spline
//==========================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SleepyCat.Utility.Splines;

/// <summary>
/// An editor window used to  generate a mesh from a spline
/// </summary>
public class SplineMeshGenWindow : EditorWindow
{
    // TODO: Finish cleaning, Add angle checking to tube, add spline sequence mesh gen support

    #region Private Constants
    /// <summary>
    /// The smallest number of the segment the generated mesh can be divided into
    /// </summary>
    private const int minimumSegmentCount = 1;
    /// <summary>
    /// The smallest number of side a tube mesh can have
    /// </summary>
    private const int minimumTubeSidesPerSegment = 3;
    #endregion

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
    /// If the mesh draw should be a tube, otherwise it is a plane
    /// </summary>
    private bool drawingTube;

    /// <summary>
    /// How much of an angle change is needed before a new segment of the mesh can be made
    /// </summary>
    private float segmentRequiredAngleChange;
    /// <summary>
    /// The amount the width curve is multiplied by across it length
    /// </summary>
    private float widthMultiplier;

    /// <summary>
    /// The number of segments the mesh should be made up of
    /// </summary>
    private int segmentCount;
    /// <summary>
    /// The number of side that the tube will be given per segment
    /// </summary>
    private int tubeSidesPerSegment;

    /// <summary>
    /// The spline wrapper to use as the spline for the mesh
    /// </summary>
    private SplineWrapper splineGeneratedFrom;

    /// <summary>
    /// The direction of the last point check along the spline
    /// </summary>
    private Vector3 previousDirection;
    /// <summary>
    /// The left point in the segment devision made
    /// </summary>
    private Vector3 previousLeftPoint;
    /// <summary>
    /// The right point in the segment devision made
    /// </summary>
    private Vector3 previousRightPoint;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        widthCurve = new AnimationCurve();

        // Set default values for settings (mostly arbitrary values so the user has something to look at rather needing to set everything up first)
        widthMultiplier = 1f;
        segmentCount = 10;
        tubeSidesPerSegment = 8;
        // Setting animation curve to straight line at 1, meaning the mesh has an even width throughout
        widthCurve.AddKey(0f, 1f);
        widthCurve.AddKey(1f, 1f);
    }
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnGUI()
    {
        // Show the mesh settings
        // Title
        GUILayout.Label("Mesh Settings", EditorStyles.boldLabel);
        // Mesh type toggle buttons
        EditorGUILayout.BeginHorizontal();
        drawingTube = GUILayout.Toggle(drawingTube, "Tube", "Button");
        drawingTube = !GUILayout.Toggle(!drawingTube, "Plane", "Button");
        EditorGUILayout.EndHorizontal();
        // Spline used
        splineGeneratedFrom = (SplineWrapper)EditorGUILayout.ObjectField("Spline To Use", splineGeneratedFrom, typeof(SplineWrapper), true);
        // Segment count
        segmentCount = EditorGUILayout.IntField("Segment Count", segmentCount);
        if (segmentCount < 1)
        {
            segmentCount = 1;
        }
        // If it is a tube then a field for the number of segments
        if (drawingTube)
        {
            tubeSidesPerSegment = EditorGUILayout.IntField("Tube Side Count", tubeSidesPerSegment);
            if (tubeSidesPerSegment < 3)
            {
                tubeSidesPerSegment = 3;
            }
        }

        // Width Settings
        widthMultiplier = EditorGUILayout.FloatField("Width Multiplier", widthMultiplier);
        widthCurve = EditorGUILayout.CurveField("WidthCurve", widthCurve);

        // Optimization setting
        segmentRequiredAngleChange = EditorGUILayout.FloatField("Segment Require Angle Change", segmentRequiredAngleChange);
        EditorGUILayout.Space();

        // Showing the user a button to generate the mesh and it functionality
        if (GUILayout.Button("Generate Mesh"))
        {
            if (splineGeneratedFrom.gameObject.transform.localScale != Vector3.one)
            {
                Debug.LogWarning("Newly generated spline mesh will not be affected by the local scale of base spline");
            }

            if (drawingTube)
            {
                Mesh newMesh = GenerateTubeMesh();
                MeshSaverEditor.SaveMesh(newMesh, "NewTubeSplineMesh", false, true);
            }
            else
            {
                Mesh newMesh = GeneratePlaneMesh();
                MeshSaverEditor.SaveMesh(newMesh, "NewSplinePlaneMesh", false, true);
            }

        }
    }
    #endregion

    #region Private Static Methods
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Spline Plane Mesh Gen Window")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        SplineMeshGenWindow window = (SplineMeshGenWindow)EditorWindow.GetWindow(typeof(SplineMeshGenWindow));
        window.Show();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Generates a plane mesh based on the spline and the settings defined in the window
    /// </summary>
    /// <returns>A plane mesh based on the spline and the settings defined in the window</returns>
    private Mesh GeneratePlaneMesh()
    {
        previousRightPoint = Vector3.negativeInfinity;
        previousLeftPoint = Vector3.negativeInfinity;

        Mesh mesh = new Mesh();
        // The number of triangles should be equal to twice the number of generated segments (then times 3 for each point in the triangle, for the list)
        List<int> triangles = new List<int>(segmentCount * 2 * 3);
        // The number of vertexes should be equal to twice the number of generated segments + 2 for the start vertexes (then times 3 for each point in the triangle, for the list)
        List<Vector3> verts = new List<Vector3>(segmentCount * 2 + 2);

        int currentIndiceIndex = 0;
        SegmentDevisionLine newSegment;

        Vector3 direction = splineGeneratedFrom.GetDirection(0.0f, 0.01f);
        previousDirection = direction;
        Vector3 pointOnLine = splineGeneratedFrom.GetLocalPointAtTime(0.0f);

        // Set up the previous right and left points so each segment can be drawn
        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        direction = GetRightFromForwardVector(direction);
        previousRightPoint = pointOnLine + direction * widthCurve.Evaluate(0.0f) * widthMultiplier;
        previousLeftPoint = pointOnLine + direction * widthCurve.Evaluate(0.0f) * -widthMultiplier;
        verts.Add(previousRightPoint);
        verts.Add(previousLeftPoint);

        // Iterate over the spline and create the vertexes and triangles for each point in the spline
        float stepIncrement = 1f / segmentCount;
        float f = stepIncrement;
        for (; f < 1.0f; f += stepIncrement)
        {
            direction = splineGeneratedFrom.GetDirection(f, 0.01f);
            if (Vector3.Angle(direction, previousDirection) > segmentRequiredAngleChange)
            {
                previousDirection = direction;
                pointOnLine = splineGeneratedFrom.GetLocalPointAtTime(f);
                newSegment = CreateSegmentDevision(pointOnLine, direction, f);
                AddSegmentVertsAndTrianglesFromSegmentToLists(verts, triangles, newSegment, currentIndiceIndex, out currentIndiceIndex);
            }
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
    /// Generates a tube mesh based on the spline and the settings defined in the window
    /// </summary>
    /// <returns>A tube mesh based on the spline and the settings defined in the window</returns>
    private Mesh GenerateTubeMesh()
    {
        Mesh mesh = new Mesh();
        // number of segment * number of circle segments * triangles per quad + number of circle segments * triangles per quad = number of triangles
        List<int> triangles = new List<int>((segmentCount * tubeSidesPerSegment * 2 + tubeSidesPerSegment * 2) * 3);
        // number of circle segments * (number of segments + One extra for start) + the front and back middle segments
        List<Vector3> verts = new List<Vector3>(tubeSidesPerSegment * (segmentCount + 1) + 2);

        // Set up for stepping through the spline
        float stepIncrement = 1f / segmentCount;
        float f;
        Vector3 pointOnLine = splineGeneratedFrom.GetLocalPointAtTime(0.0f);
        Vector3 direction = splineGeneratedFrom.GetDirection(0.0f, stepIncrement);
        Vector3 rightDirection = Vector3.right;
        Vector3 upDirection = Vector3.up;
        Vector3.OrthoNormalize(ref direction, ref rightDirection, ref upDirection);

        // Setting up the first circle along the tube
        verts.Add(pointOnLine);
        verts.AddRange(GetCircleOfPointsAroundPoint(splineGeneratedFrom.LocalStartPosition, direction, rightDirection, 0.0f));
        AddTubeStartCapTriangles(triangles, tubeSidesPerSegment, 0);
        CreateTubeSegmentTrianglesFromStartingCircle(triangles, tubeSidesPerSegment, 1); // The starting index will be 1 at this point because the point in the center in the start cap will be 0 then the first vertex on the first circle of the tube will be 1

        int currentIndex = tubeSidesPerSegment + 1;

        // Add each of the circle segments
        for (f = stepIncrement; f < 1.0f; f += stepIncrement)
        {
            direction = splineGeneratedFrom.GetDirection(f, stepIncrement);
            Vector3.OrthoNormalize(ref direction, ref rightDirection, ref upDirection);

            verts.AddRange(GetCircleOfPointsAroundPoint(splineGeneratedFrom.GetLocalPointAtTime(f), direction, rightDirection, f));
            CreateTubeSegmentTrianglesFromStartingCircle(triangles, tubeSidesPerSegment, currentIndex);

            currentIndex += tubeSidesPerSegment;
        }

        // Add end cap
        f = f - stepIncrement;
        Vector3 linePoint = splineGeneratedFrom.GetLocalPointAtTime(f);
        direction = (splineGeneratedFrom.LocalEndPosition - linePoint).normalized;
        Vector3.OrthoNormalize(ref direction, ref rightDirection, ref upDirection);
        verts.AddRange(GetCircleOfPointsAroundPoint(splineGeneratedFrom.LocalEndPosition, direction, rightDirection, 1f));
        verts.Add(splineGeneratedFrom.LocalEndPosition);
        AddTubeSplineEndCapTriangles(triangles, tubeSidesPerSegment, verts.Count - 1);

        // Set the vertexes and meshes
        mesh.SetVertices(verts.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        return mesh;
    }

    /// <summary>
    /// Build the triangles for the end cap of the tube by connecting corners to each other and the middle
    /// </summary>
    /// <param name="trianglesToAddTo">The list if triangles to add the indexes to</param>
    /// <param name="numberOfTubeSides">The number of side for each segment of the tube</param>
    /// <param name="endIndex">The index of the vertex in the center of the end cap, should generally be the final vertex of the mesh</param>
    private void AddTubeSplineEndCapTriangles(List<int> trianglesToAddTo, int numberOfTubeSides, int endIndex)
    {
        for(int i = 0; i < numberOfTubeSides - 1; ++i)
        {
            // Middle vertex of the cap
            trianglesToAddTo.Add(endIndex);
            // Currently Iterated corner of the end cap
            trianglesToAddTo.Add(endIndex - numberOfTubeSides + i);
            // The corner of the end cap that would be iterated next
            trianglesToAddTo.Add((endIndex - numberOfTubeSides) + 1 + i);
        }

        // Do the final triangle outside of the loop since it connect the last and start corners
        // Middle vertex of the cap
        trianglesToAddTo.Add(endIndex);
        // Highest index corner of the cap
        trianglesToAddTo.Add(endIndex - 1);
        // lowest index corner of the cap
        trianglesToAddTo.Add(endIndex - numberOfTubeSides);
    }
    /// <summary>
    /// Build the triangles for the end cap of the tube by connecting corners to each other and the middle
    /// </summary>
    /// <param name="trianglesToAddTo">The list if triangles to add the indexes to</param>
    /// <param name="numberOfTubeSides">The number of sides for each segment of the tube</param>
    /// <param name="startingIndex">The index of the vertex in the center if the start cap (should generally be 0 unless it is being part way into the mesh)</param>
    private void AddTubeStartCapTriangles(List<int> trianglesToAddTo, int numberOfTubeSides, int startingIndex = 0)
    {
        for (int i = 0; i < numberOfTubeSides - 1; ++i)
        {
            // The corner of the start cap that would be iterated next
            trianglesToAddTo.Add(startingIndex + i + 2);
            // The corner of the start cap currently iterated
            trianglesToAddTo.Add(startingIndex + i + 1);
            // The middle vertex of the cap
            trianglesToAddTo.Add(startingIndex);
        }
        // Do the final triangle outside of the loop since it connect the last and start corners
        // The lowest index corner of the cap
        trianglesToAddTo.Add(startingIndex + 1);
        // The highest index corner of the cap
        trianglesToAddTo.Add(startingIndex + numberOfTubeSides);
        // The middle vertex of the cap
        trianglesToAddTo.Add(startingIndex);
    }
    /// <summary>
    /// Creates the triangles that make up one segment of the tube mesh
    /// Loop over each side of the current segment (The startingIndexOfStartCircle should indicate what segment -
    /// - it is on as it will be index of the vertex that starts off a segment)
    /// </summary>
    /// <param name="trianglesToAddTo">The list of triangles to add the new ones to</param>
    /// <param name="numberOfTubeSides">The number of sides for each segment of the tube</param>
    /// <param name="startingIndexOfStartCircle">The index of the first vertex on the circle for the start of the segment</param>
    private void CreateTubeSegmentTrianglesFromStartingCircle(List<int> trianglesToAddTo, int numberOfTubeSides, int startingIndexOfStartCircle)
    {
        for (int i = 0; i < numberOfTubeSides - 1; ++i)
        {
            // The vertex of the end circle that is iterated over next
            trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides + 1 + i);
            // The vertex of the end circle that is currently iterated over
            trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides + i);
            // The vertex of the start circle that is currently iterated over
            trianglesToAddTo.Add(startingIndexOfStartCircle + i);

            // The vertex of the start circle that is iterated over next
            trianglesToAddTo.Add(startingIndexOfStartCircle + 1 + i);
            // The vertex of the end circle that is currently iterated over 
            trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides + 1 + i);
            // The vertex of the start circle that is currently iterated over
            trianglesToAddTo.Add(startingIndexOfStartCircle + i);
        }

        // The corner of the end circle with the lowest index
        trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides);
        trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides + numberOfTubeSides - 1);
        trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides - 1);

        trianglesToAddTo.Add(startingIndexOfStartCircle);
        trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides);
        trianglesToAddTo.Add(startingIndexOfStartCircle + numberOfTubeSides - 1);
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

        direction = GetRightFromForwardVector(direction);

        Vector3 rightPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * widthMultiplier;
        Vector3 leftPoint = pointOnSpline + direction * widthCurve.Evaluate(t) * -widthMultiplier;

        newSegment.right = rightPoint;
        newSegment.left = leftPoint;

        previousRightPoint = rightPoint;
        previousLeftPoint = leftPoint;

        return newSegment;
    }

    private Vector2 RotateVector2(Vector2 vectorToRotate, float angle)
    {
        // rotate vector(x1, y1) counterclockwise by the given angle
        float rads = angle * Mathf.Deg2Rad;

        float newX = vectorToRotate.x * Mathf.Cos(rads) - vectorToRotate.y * Mathf.Sin(rads);
        float newY = vectorToRotate.x * Mathf.Sin(rads) + vectorToRotate.y * Mathf.Cos(rads);

        return new Vector3(newX, newY);
    }

    /// <summary>
    /// Returns a vector 90 degrees clockwise along the X and Z axis to a given vector
    /// </summary>
    /// <param name="forwardVector">The vector to find a vector to the right of</param>
    /// <returns>a vector 90 degrees clockwise along the X and Z axis to a given vector</returns>
    private Vector3 GetRightFromForwardVector(Vector3 forwardVector)
    {
        //// rotate vector(x1, y1) counterclockwise by the given angle
        //Vector2 newAngle = RotateVector2(new Vector2(forwardVector.x, forwardVector.z), 90f);

        //return new Vector3(newAngle.x, forwardVector.y, newAngle.y);
        // transformed vectors right and left:
        return new Vector3(forwardVector.z, forwardVector.y, -forwardVector.x);
    }

    private Vector3 GetUpFromForwardVector(Vector3 forwardVector)
    {
        // rotate vector(x1, y1) counterclockwise by the given angle
        Vector2 newAngle = RotateVector2(new Vector2(forwardVector.y, forwardVector.z), 90f);

        return new Vector3(forwardVector.x, newAngle.x , newAngle.y);
    }

    private Vector3[] GetCircleOfPointsAroundPoint(Vector3 pointOnSpline, Vector3 direction, Vector3 rightDirection, float t)
    {
        int index = 0;

        float rotationAnglePerSegment = 360f / tubeSidesPerSegment;
        Vector3[] newPoints = new Vector3[tubeSidesPerSegment];
         
        for (float currentAngle = 0f; Mathf.Ceil(currentAngle) < 360f; currentAngle += rotationAnglePerSegment)
        {
            // Get direction of rotation to next point
            Vector3 newDirection = Quaternion.AngleAxis(currentAngle, direction) * rightDirection;
            newPoints[index++] = pointOnSpline + newDirection * widthCurve.Evaluate(t) * widthMultiplier;
        }

        return newPoints;
    }



    /// <summary>
    /// Adds vertexes and triangles to provided lists based on a new segment division, the current index for the triangles that has been reached, updating and outing the index for the triangles afterwards
    /// </summary>
    /// <param name="vertsListToAddTo">The list of vertexes to add to</param>
    /// <param name="trianglesListToAddTo">The list of triangles to add</param>
    /// <param name="segment">The segment to based the new vertexes and triangles off of</param>
    /// <param name="currentIndiceIndex">the current index for the current right vertexes of SegmentDevisionLine in the in the vertexes array</param>
    /// <param name="newCurrentIndiceIndex">output for the updating of currentIndiceIndex</param>
    private void AddSegmentVertsAndTrianglesFromSegmentToLists(List<Vector3> vertsListToAddTo, List<int> trianglesListToAddTo, SegmentDevisionLine segment, int currentIndiceIndex, out int newCurrentIndiceIndex)
    {
        // Add the segment points to the vertexes
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
        direction = GetRightFromForwardVector(direction);
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

    private void DrawTubeCircleEnd(Vector3[] points, Vector3 splinePoint)
    {
        Handles.DrawLine(splinePoint, points[0]);
        for (int i = 1; i < points.Length; ++i)
        {
            Handles.color = Color.white;
            Handles.DrawLine(points[i-1], points[i]);
            Handles.DrawLine(splinePoint, points[i]);
        }
        Handles.DrawLine(points[points.Length - 1], points[0]);
    }

    private void DrawTubeCircleMiddle(Vector3[] points)
    {
        for (int i = 1; i < points.Length; ++i)
        {
            Handles.color = Color.white;
            Handles.DrawLine(points[i - 1], points[i]);
        }
        Handles.DrawLine(points[points.Length - 1], points[0]);
    }
    /// <summary>
    /// Called during the scene view to draw the spline mesh outline onto the view
    /// </summary>
    /// <param name="sceneView">The scene view that is being called during</param>
    private void OnSceneGUI(SceneView sceneView)
    {
        // Draw the mesh wire-frame that would be generated
        if (splineGeneratedFrom != null)
        {
            if (drawingTube)
            {
                float stepIncrement = 1f / segmentCount;
                float f;
                Vector3[] previousCirclePoints = null;
                Vector3[] points;

                Vector3 direction = Vector3.forward;
                Vector3 rightDirection = Vector3.right;
                Vector3 upDirection = Vector3.up;

                for (f = 0.0f; f < 1.0f; f += stepIncrement)
                {
                    direction = splineGeneratedFrom.GetDirection(f, stepIncrement);
                    Vector3.OrthoNormalize(ref direction, ref rightDirection, ref upDirection);

                    points = GetCircleOfPointsAroundPoint(splineGeneratedFrom.GetPointAtTime(f), direction, rightDirection, f);
                    DrawTubeCircleMiddle(points);
                    if (previousCirclePoints != null)
                    {
                        for (int i = 0; i < points.Length; ++i)
                        {
                            Handles.DrawLine(previousCirclePoints[i], points[i]);
                        }
                    }
                    previousCirclePoints = points;
                    if (f == 0.0f)
                    {
                        DrawTubeCircleEnd(points, splineGeneratedFrom.WorldStartPosition);
                    }
                    Handles.color = Color.blue;
                    Handles.DrawLine(splineGeneratedFrom.GetPointAtTime(f), splineGeneratedFrom.GetPointAtTime(f) + direction);
                    Handles.color = Color.red;
                    Handles.DrawLine(splineGeneratedFrom.GetPointAtTime(f), splineGeneratedFrom.GetPointAtTime(f) + rightDirection);
                    Handles.color = Color.green;
                    Handles.DrawLine(splineGeneratedFrom.GetPointAtTime(f), splineGeneratedFrom.GetPointAtTime(f) + upDirection);
                }

                f = f - stepIncrement;
                Vector3 linePoint = splineGeneratedFrom.GetPointAtTime(f);
                direction = (splineGeneratedFrom.WorldEndPosition - linePoint).normalized;
                Vector3.OrthoNormalize(ref direction, ref rightDirection, ref upDirection);

                points = GetCircleOfPointsAroundPoint(splineGeneratedFrom.WorldEndPosition, direction, rightDirection, 1f);
                DrawTubeCircleEnd(points, splineGeneratedFrom.WorldEndPosition);
                for (int i = 0; i < points.Length; ++i)                {
                    Handles.DrawLine(previousCirclePoints[i], points[i]);
                }
            }
            else
            {
                // Set up previous left and right view to a junk value so the they do not get used until they are properly assigned
                previousRightPoint = Vector3.negativeInfinity;
                previousLeftPoint = Vector3.negativeInfinity;
                float stepIncrement = 1f / segmentCount;
                previousDirection = -splineGeneratedFrom.GetDirection(0.0f, stepIncrement);
                float f;
                for (f = 0.0f; f < 1.0f; f += stepIncrement)
                {
                    Vector3 direction = splineGeneratedFrom.GetDirection(f, stepIncrement);
                    Debug.Log(Vector3.Angle(direction, previousDirection));
                    if (Vector3.Angle(direction, previousDirection) > segmentRequiredAngleChange)
                    {
                        previousDirection = direction;
                        DrawLineSegmentsHandles(splineGeneratedFrom.GetPointAtTime(f), splineGeneratedFrom.GetDirection(f, stepIncrement), f);
                    }
                }

                f = f - stepIncrement;
                Vector3 linePoint = splineGeneratedFrom.GetPointAtTime(f);
                DrawLineSegmentsHandles(splineGeneratedFrom.WorldEndPosition, (splineGeneratedFrom.WorldEndPosition - linePoint).normalized, 1.0f);
            }
        }
    }
    #endregion
}
