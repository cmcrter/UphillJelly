//======================================================================================================================================================================================================================================================================================================
// File:            HandlesExtraFunctions.cs
// Author:          Matthew Mason
// Date Created:    11/10/2021
// Brief:           Static class containing functions used for quick functionality with the Editor Handles
//======================================================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat.Utility
{
    /// <summary>
    /// Static class containing functions used for quick functionality with the Editor Handles
    /// </summary>
    public static class HandlesHelperFunctions
    {
        /// <summary>
        /// Used to create a handle that will return true if it was moved and create and undo entry if it was
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="rotation"></param>
        /// <param name="changedObject"></param>
        /// <param name="handleDescription"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public static bool ChangeSenstivePositionHandle(Vector3 startPosition, Quaternion rotation, Object changedObject, string handleDescription, out Vector3 newPosition)
        {
            // Check if the new position handles have moved then update the world positions of spline
            newPosition = Handles.PositionHandle(startPosition, rotation);
            Vector3 startHandleDelta = newPosition - startPosition;
            if (startHandleDelta.magnitude > 0)
            {
                Undo.RecordObject(changedObject, changedObject.name + handleDescription + " handles moved");
                return true;
            }
            return false;
        }
    }
}
