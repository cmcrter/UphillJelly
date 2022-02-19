//==================================================================================================================================================================================================================================================================================================================
//  Name:               SplineTrafficCustomInspector.cs
//  Author:             Matthew Mason
//  Date Created:       11/01/2022 
//  Date Last Modified: 11/01/2022
//  Brief:              An editor window for the spline traffic script component
//==================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L7Games.CustomInspectors
{
    [CustomEditor(typeof(SplineTraffic))]
    public class SplineTrafficCustomInspector : Editor
    {
        #region Private Variables
        /// <summary>
        /// If the inspector should current be showing the prefab array
        /// </summary>
        private bool showPrefabsArray;

        /// <summary>
        /// Property for if the traffic should be following along the x
        /// </summary>
        private SerializedProperty followX;
        /// <summary>
        /// Property for if the object should follow the y position of the spline
        /// </summary>
        private SerializedProperty followY;
        /// <summary>
        /// Property for if the object should follow the z position of the spline
        /// </summary>
        private SerializedProperty followZ;
        /// <summary>
        /// The prefabs that can be spawned to move along the splines
        /// </summary>
        private SerializedProperty spawnablePrefabs;
        /// <summary>
        /// How fast the object will move along the spline
        /// </summary>
        private SerializedProperty speed;
        /// <summary>
        /// The spline wrapper to follow along 
        /// </summary>
        private SerializedProperty splineInUse;
        /// <summary>
        /// The amount of time between spawning traffic objects
        /// </summary>
        private SerializedProperty timeBetweenSpawns;
        /// <summary>
        /// The amount of time between spawning traffic objects
        /// </summary>
        private SerializedProperty preSpawnObjects;

        /// <summary>
        /// The component this is inspecting
        /// </summary>
        private SplineTraffic splineTrafficComponent;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            if (splineTrafficComponent == null)
            {
                splineTrafficComponent = (SplineTraffic)serializedObject.targetObject;
            }

            speed = serializedObject.FindProperty("speed");
            followX = serializedObject.FindProperty("followX");
            followY = serializedObject.FindProperty("followY");
            followZ = serializedObject.FindProperty("followZ");
            timeBetweenSpawns = serializedObject.FindProperty("timeBetweenSpawns");
            splineInUse = serializedObject.FindProperty("splineInUse");
            spawnablePrefabs = serializedObject.FindProperty("spawnablePrefabs");
            preSpawnObjects = serializedObject.FindProperty("preSpawnObjects");
        }

        public override void OnInspectorGUI()
        {
            if (splineTrafficComponent == null)
            {
                splineTrafficComponent = (SplineTraffic)serializedObject.targetObject;
            }
            serializedObject.Update();

            // Serialized Property Fields
            EditorGUILayout.PropertyField(followX);
            EditorGUILayout.PropertyField(followY);
            EditorGUILayout.PropertyField(followZ);

            EditorGUILayout.PropertyField(preSpawnObjects);

            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(timeBetweenSpawns);

            EditorGUILayout.PropertyField(splineInUse);

            EditorGUILayout.Space();

            // Count field for the prefab array
            int newCount = EditorGUILayout.IntField("Spawnable Prefabs Count", splineTrafficComponent.SpawnablePrefabsCount);
            if (newCount != splineTrafficComponent.SpawnablePrefabsCount)
            {
                splineTrafficComponent.AdjustSpawnablePrefabsCount(newCount);
            }
            // Add up the total weights of all the spawnable prefabs for later
            float totalWeight = 0f;
            for (int i = 0; i < splineTrafficComponent.SpawnablePrefabsCount; ++i)
            {
                totalWeight += splineTrafficComponent.GetPrefabWeightAtIndex(i);
            }

            // Only show all element if the user wants to see the,
            if (showPrefabsArray = EditorGUILayout.Foldout(showPrefabsArray, "Spawnable Prefabs"))
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < spawnablePrefabs.arraySize; ++i)
                {
                    // Show the array element
                    SerializedProperty property = spawnablePrefabs.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(property);
                    ++EditorGUI.indentLevel;
                    // Show its chance of spawning
                    EditorGUILayout.LabelField("Spawn Chance: " + (splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight * 100f) + "%".ToString(), GUILayout.Width(150f));
                    --EditorGUI.indentLevel;
                }
                --EditorGUI.indentLevel;
            }

            if (GUILayout.Button("Normalize weights"))
            {
                for (int i = 0; i < splineTrafficComponent.SpawnablePrefabsCount; ++i)
                {
                    splineTrafficComponent.SetPrefabWeightAtIndex(i, splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}


