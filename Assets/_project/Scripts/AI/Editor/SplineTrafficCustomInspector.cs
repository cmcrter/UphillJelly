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

[CustomEditor(typeof(SplineTraffic))]
public class SplineTrafficCustomInspector : Editor
{
    private bool showPrefabsArray;

    SerializedProperty speed;

    SplineTraffic splineTrafficComponent;

    void OnEnable()
    {
        if (splineTrafficComponent == null)
        {
            splineTrafficComponent = (SplineTraffic)serializedObject.targetObject;
        }
        speed = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        if (splineTrafficComponent == null)
        {
            splineTrafficComponent = (SplineTraffic)serializedObject.targetObject;
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(speed);
        int newCount = EditorGUILayout.IntField(splineTrafficComponent.PrefabsAndWeightsCount);
        if (newCount != splineTrafficComponent.PrefabsAndWeightsCount)
        {
            splineTrafficComponent.AdjustPrefabsAndWeightsCount(newCount);
        }
        if (showPrefabsArray = EditorGUILayout.Foldout(showPrefabsArray, "Prefabs"))
        {
            float totalWeight = 0f;
            for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
            {
                totalWeight += splineTrafficComponent.GetPrefabWeightAtIndex(i);
            }

            if (totalWeight != 1f)
            {
                for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
                {
                    splineTrafficComponent.SetPrefabWeightAtIndex(i, splineTrafficComponent.GetPrefabWeightAtIndex(i) / totalWeight);
                }
            }
            for (int i = 0; i < splineTrafficComponent.PrefabsAndWeightsCount; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Prefab " + i.ToString(), GUILayout.MaxWidth(75f));
                GameObject newGameObject = (GameObject)EditorGUILayout.ObjectField(splineTrafficComponent.GetPrefabAtIndex(i), typeof(GameObject), false, GUILayout.MaxWidth(75f));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Weight");
                float newWeight = EditorGUILayout.FloatField(splineTrafficComponent.GetPrefabWeightAtIndex(i), GUILayout.MinWidth(100f));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("Spawn Chance: " + (splineTrafficComponent.GetPrefabWeightAtIndex(i) * 100f).ToString());
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
        if (GUILayout.Button("Adjust weight "))
        {

        }

        serializedObject.ApplyModifiedProperties();
    }
}
