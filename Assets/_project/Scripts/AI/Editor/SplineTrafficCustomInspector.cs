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
        /// If property for if the traffic should be following along the x
        /// </summary>
        private SerializedProperty followX;
        private SerializedProperty followY;
        private SerializedProperty followZ;
        private SerializedProperty timeBetweenSpawns;
        private SerializedProperty splineInUse;
        private SerializedProperty spawnablePrefabs;

        SerializedProperty speed;

        SplineTraffic splineTrafficComponent;
        #endregion

        void OnEnable()
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
        }

        public override void OnInspectorGUI()
        {
            if (splineTrafficComponent == null)
            {
                splineTrafficComponent = (SplineTraffic)serializedObject.targetObject;
            }
            serializedObject.Update();
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(followX);
            EditorGUILayout.PropertyField(followY);
            EditorGUILayout.PropertyField(followZ);
            EditorGUILayout.PropertyField(timeBetweenSpawns);
            EditorGUILayout.PropertyField(splineInUse);

            EditorGUILayout.Space();

            int newCount = EditorGUILayout.IntField("Spawnable Prefabs Count", splineTrafficComponent.SpawnablePrefabsCount);
            if (newCount != splineTrafficComponent.SpawnablePrefabsCount)
            {
                splineTrafficComponent.AdjustPrefabsAndWeightsCount(newCount);
            }
            float totalWeight = 0f;
            for (int i = 0; i < splineTrafficComponent.SpawnablePrefabsCount; ++i)
            {
                totalWeight += splineTrafficComponent.GetPrefabWeightAtIndex(i);
            }



            if (showPrefabsArray = EditorGUILayout.Foldout(showPrefabsArray, "Spawnable Prefabs"))
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < spawnablePrefabs.arraySize; ++i)
                {
                    SerializedProperty property = spawnablePrefabs.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(property);
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.LabelField("Spawn Chance: " + (splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight * 100f) + "%".ToString(), GUILayout.Width(150f));
                    --EditorGUI.indentLevel;
                }
                --EditorGUI.indentLevel;
            }
            if (GUILayout.Button("Adjust Total Weight To 1"))
            {
                for (int i = 0; i < splineTrafficComponent.SpawnablePrefabsCount; ++i)
                {
                    splineTrafficComponent.SetPrefabWeightAtIndex(i, splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}


