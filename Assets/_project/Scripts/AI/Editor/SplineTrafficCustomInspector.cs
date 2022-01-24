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
        private SerializedProperty upwardsOffset;

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
            upwardsOffset = serializedObject.FindProperty("upwardsOffset");
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
            EditorGUILayout.PropertyField(upwardsOffset);
            int newCount = EditorGUILayout.IntField(splineTrafficComponent.PrefabsAndWeightsCount);
            if (newCount != splineTrafficComponent.PrefabsAndWeightsCount)
            {
                splineTrafficComponent.AdjustPrefabsAndWeightsCount(newCount);
            }
            float totalWeight = 0f;
            for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
            {
                totalWeight += splineTrafficComponent.GetPrefabWeightAtIndex(i);
            }
            if (showPrefabsArray = EditorGUILayout.Foldout(showPrefabsArray, "Prefabs"))
            {
                if (totalWeight == 0f && splineTrafficComponent.PrefabsAndWeightsCount > 0)
                {
                    splineTrafficComponent.SetPrefabWeightAtIndex(0, 1f);
                }

                for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Prefab " + i.ToString(), GUILayout.MaxWidth(75f));
                    GameObject newGameObject = (GameObject)EditorGUILayout.ObjectField(splineTrafficComponent.GetPrefabAtIndex(i), typeof(GameObject), false);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Weight", GUILayout.MaxWidth(75f));
                    float newWeight = EditorGUILayout.FloatField(splineTrafficComponent.GetPrefabWeightAtIndex(i));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("Spawn Chance: " + (splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight * 100f).ToString(), GUILayout.Width(125f));
                    EditorGUILayout.EndHorizontal();

                    if (newGameObject != splineTrafficComponent.GetPrefabAtIndex(i))
                    {
                        splineTrafficComponent.SetPrefabAtIndex(i, newGameObject);
                    }
                    if (newWeight != splineTrafficComponent.GetPrefabWeightAtIndex(i))
                    {
                        splineTrafficComponent.SetPrefabWeightAtIndex(i, newWeight);
                    }
                }
            }
            if (GUILayout.Button("Adjust Total Weight To 1"))
            {
                for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
                {
                    splineTrafficComponent.SetPrefabWeightAtIndex(i, splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}


