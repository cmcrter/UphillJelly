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
            Vector3 newStartPosition = Handles.PositionHandle(lineSplineWrapper.GetWorldStartPoint(), lineSplineWrapper.transform.rotation);
            Vector3 startHandleDelta = newStartPosition - lineSplineWrapper.GetWorldStartPoint();
            if (startHandleDelta.magnitude > 0)
            {
                lineSplineWrapper.SetWorldStartPoint(newStartPosition, true);
            }
            Vector3 newEndPosition = Handles.PositionHandle(lineSplineWrapper.GetWorldEndPoint(), lineSplineWrapper.transform.rotation);
            Vector3 endHandleDelta = newEndPosition - lineSplineWrapper.GetWorldEndPoint();
            if (endHandleDelta.magnitude > 0)
            {
                lineSplineWrapper.SetWorldEndPoint(newEndPosition, true);
            }
        }
        #endregion
    }
}
