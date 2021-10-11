//======================================================================================================================================================================================================================================================================================================
// File:            LineSplineWrapperInspector.cs
// Author:          Matthew Mason
// Date Created:    05/10/2021
// Brief:           The custom inspector the line spline wrapper script
//======================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// The custom inspector the line spline wrapper script
    /// </summary>
    [CustomEditor(typeof(LineSplineWrapper))]
    [CanEditMultipleObjects]
    public class LineSplineWrapperInspector : Editor
    {
        #region Private Variables
        /// <summary>
        /// The line spline wrapper this is inspecting
        /// </summary>
        private LineSplineWrapper lineSplineWrapper;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            lineSplineWrapper = (LineSplineWrapper)target;
        }

        private void OnSceneGUI()
        {
            if (lineSplineWrapper == null)
            {
                lineSplineWrapper = (LineSplineWrapper)target;
            }

            // Check if the new position handles have moved then update the world positions of lines
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(lineSplineWrapper.WorldStartPosition,
                lineSplineWrapper.transform.rotation, target, "Start Position", out Vector3 newWorldStartPosition))
            {
                lineSplineWrapper.SetWorldStartPointAndUpdateLocal(newWorldStartPosition);
            }
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(lineSplineWrapper.WorldEndPosition,
                lineSplineWrapper.transform.rotation, target, "End Position", out Vector3 newWorldEndPosition))
            {
                lineSplineWrapper.SetWorldEndPointAndUpdateLocal(newWorldEndPosition);
            }
        }
        #endregion
    }
}
