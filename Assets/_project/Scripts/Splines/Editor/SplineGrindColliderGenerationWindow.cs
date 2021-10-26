//==========================================================================================================================================================================================================================================================================================================
// File:            SplineColliderGenerationWindow.cs
// Author:          Matthew Mason
// Date Created:    25/10/2021
// Brief:           A editor window used to generate collider along a spline
//==========================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SleepyCat.Utility.Splines;
using SleepyCat.Movement;

public class SplineGrindColliderGenerationWindow : EditorWindow
{
    #region Private Constants
    /// <summary>
    /// The smallest number of the segment the generated mesh can be divided into
    /// </summary>
    private const int minimumNumberOfColliders = 1;
    #endregion

    #region Private Variables
    #region Window Fields
    /// <summary>
    /// If the generated colliders should marked as triggers 
    /// </summary>
    private bool areTriggers;

    /// <summary>
    /// The how being the generated capsules radius should be
    /// </summary>
    private float capsuleRadius = 0.1f;
    /// <summary>
    /// The amount the direction must change but in order for a new collider to be make
    /// </summary>
    private float segmentRequiredAngleChange = 0.1f;

    /// <summary>
    /// The number of colliders that will make up the splines colliders 
    /// </summary>
    private int numberOfColliders = 10;

    /// <summary>
    /// The PhysicMaterial to assign to the colliders
    /// </summary>
    private PhysicMaterial physicMaterialToUse;

    /// <summary>
    /// The spline wrapper to use as the spline for the mesh
    /// </summary>
    private SplineWrapper splineGeneratedFrom;
    #endregion

    /// <summary>
    /// The colliders that have been created by generation from this window
    /// </summary>
    private List<CapsuleCollider> createdColliders;

    /// <summary>
    /// The direction the spline was going on the last step
    /// </summary>
    private Vector3 previousDirection;
    #endregion

    #region Private Static Methods
    // Add menu named "Collider Generation Window" to the Window menu
    [MenuItem("Window/Spline Generation/Collider Generation Window")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        SplineGrindColliderGenerationWindow window = (SplineGrindColliderGenerationWindow)EditorWindow.GetWindow(typeof(SplineGrindColliderGenerationWindow));
        window.Show();
    }

    #endregion

    #region Unity Methods
    private void OnGUI()
    {
        // Spline Settings - Will Take Effect On Generation
        EditorGUILayout.LabelField("Spline Setting - Will Take Effect On Generation", EditorStyles.boldLabel);
        GUISplineGeneratedFromField();
        GUINumberOfCollidersField();
        segmentRequiredAngleChange = EditorGUILayout.FloatField("Segment Require Angle Change", segmentRequiredAngleChange);
        EditorGUILayout.Space();

        // Collider - Will Update Colliders
        EditorGUILayout.LabelField("Collider - Will Update Colliders", EditorStyles.boldLabel);
        areTriggers = EditorGUILayout.Toggle("Set As Triggers", areTriggers);
        physicMaterialToUse = (PhysicMaterial)EditorGUILayout.ObjectField("Physic Material To Use", physicMaterialToUse, typeof(PhysicMaterial), true);
        capsuleRadius = EditorGUILayout.FloatField("Capsules Radius", capsuleRadius);
        EditorGUILayout.Space();

        // Generated Collider Dependant functionality
        string buttonText = "Generate Colliders";
        if (createdColliders != null)
        {
            if (createdColliders.Count > 0)
            {
                // Remove colliders that have been deleted
                for (int i = createdColliders.Count - 1; i > -1 && createdColliders.Count > 0; --i)
                {
                    if (createdColliders[i] == null)
                    {
                        createdColliders.RemoveAt(i);
                    }
                }

                // Updated any generated colliders 
                if (createdColliders.Count > 0)
                {
                    buttonText = "Regenerate Colliders";
                    for (int i = 0; i < createdColliders.Count; ++i)
                    {
                        createdColliders[i].isTrigger = areTriggers;
                        createdColliders[i].height = (createdColliders[i].height - createdColliders[i].radius) + capsuleRadius;
                        createdColliders[i].radius = capsuleRadius;
                        createdColliders[i].material = physicMaterialToUse;
                    }
                }
            }
        }

        // Show the generated/Regenerate colliders button
        if (GUILayout.Button(buttonText))
        {
            GenerateColliders();
            if (!splineGeneratedFrom.gameObject.TryGetComponent<GrindDetails>(out GrindDetails grindDetails))
            {
                splineGeneratedFrom.gameObject.AddComponent<GrindDetails>();
            }
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Generates a single game object and its collider component for one step along the spline
    /// </summary>
    /// <param name="lastPointOnCurve">The point on the curve that was the current in the last step</param>
    /// <param name="currentPointOnCurve">The point along the spline the iteration has currently reached</param>
    /// <returns>The newly created capsule collider attached to the new GameObject</returns>
    private CapsuleCollider GenerateColliderObject(Vector3 lastPointOnCurve, Vector3 currentPointOnCurve)
    {
        Vector3 direction = (lastPointOnCurve - currentPointOnCurve).normalized;
        GameObject go_NewColliderObject = new GameObject("CapsuleCollider");
        go_NewColliderObject.transform.parent = splineGeneratedFrom.transform;
        go_NewColliderObject.transform.position = lastPointOnCurve + (currentPointOnCurve - lastPointOnCurve) * 0.5f; // Set the capsule to half way between the 2 point
        go_NewColliderObject.transform.up = direction;
        CapsuleCollider collider = go_NewColliderObject.AddComponent<CapsuleCollider>();
        collider.radius = capsuleRadius;
        collider.height = Vector3.Distance(lastPointOnCurve, currentPointOnCurve) + collider.radius;
        collider.isTrigger = areTriggers;
        collider.material = physicMaterialToUse;
        return collider;
    }

    /// <summary>
    /// Generates the colliders along the set spline based on the settings
    /// </summary>
    private void GenerateColliders()
    {
        if (createdColliders != null)
        {
            if (createdColliders.Count > 0)
            {
                for (int i = 0; i < createdColliders.Count; ++i)
                {
                    GameObject.DestroyImmediate(createdColliders[i].gameObject);
                }
            }
        }
        createdColliders = new List<CapsuleCollider>();
        // Set up for stepping through the spline
        float stepIncrement = 1f / numberOfColliders;
        float f;
        
        Vector3 lastPointOnCurve = splineGeneratedFrom.GetPointAtTime(0.0f);
        Vector3 direction = splineGeneratedFrom.GetDirection(0.0f, stepIncrement);
        previousDirection = direction;


        // Add each of the circle segments
        for (f = stepIncrement; f < 1.0f; f += stepIncrement)
        {
            direction = splineGeneratedFrom.GetDirection(f, stepIncrement);
            if (Vector3.Angle(direction, previousDirection) > segmentRequiredAngleChange)
            {
                previousDirection = direction;
                Vector3 pointOnCurve = splineGeneratedFrom.GetPointAtTime(f);
                createdColliders.Add((GenerateColliderObject(lastPointOnCurve, pointOnCurve)));
                lastPointOnCurve = pointOnCurve;
            }
        }
        // Add end capsule
        createdColliders.Add(GenerateColliderObject(lastPointOnCurve, splineGeneratedFrom.WorldEndPosition));
    }
    /// <summary>
    /// Draw the SplineGeneratedFrom Field into the window GUI and handles its value changes
    /// </summary>
    private void GUISplineGeneratedFromField()
    {
        SplineWrapper previousSpline = splineGeneratedFrom;
        splineGeneratedFrom = (SplineWrapper)EditorGUILayout.ObjectField("Spline To Use", splineGeneratedFrom, typeof(SplineWrapper), true);
        // If a new spline is set, check for its colliders
        if (splineGeneratedFrom != previousSpline)
        {
            createdColliders = new List<CapsuleCollider>();
            if (splineGeneratedFrom != null)
            {
                createdColliders.AddRange(splineGeneratedFrom.GetComponentsInChildren<CapsuleCollider>());
                if (createdColliders.Count > 0)
                {
                    areTriggers = createdColliders[0].isTrigger;
                    capsuleRadius = createdColliders[0].radius;
                    physicMaterialToUse = createdColliders[0].material;
                }
            }
        }
    }
    /// <summary>
    /// Draws the NumberOfColliders Field into the window GUI and handles its value changes
    /// </summary>
    private void GUINumberOfCollidersField()
    {
        numberOfColliders = EditorGUILayout.IntField("Number Of Colliders", numberOfColliders);
        if (numberOfColliders < minimumNumberOfColliders)
        {
            numberOfColliders = minimumNumberOfColliders;
        }
    }
    #endregion
}
