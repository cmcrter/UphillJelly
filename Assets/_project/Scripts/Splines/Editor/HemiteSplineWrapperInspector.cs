using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// The custom inspector the Bezier curve spline wrapper script
    /// </summary>
    [CustomEditor(typeof(HermiteSplineWrapper))]
    [CanEditMultipleObjects]
    public class HemiteSplineWrapperInspector : Editor
    {
        #region Private Variables
        /// <summary>
        /// The line spline wrapper this is inspecting
        /// </summary>
        private HermiteSplineWrapper hermiteCurveWrapper;
        #endregion

        private void OnSceneGUI()
        {
            // Get the target as a BezierCurveWrapper if not already gotten
            if (hermiteCurveWrapper == null)
            {
                hermiteCurveWrapper = (HermiteSplineWrapper)target;
            }

            // Check if the new position handles have moved then update the world positions of spline
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldStartPosition, 
                hermiteCurveWrapper.transform.rotation, target, "Start Position", out Vector3 newWorldStartPosition))
            {
                hermiteCurveWrapper.SetWorldStartPointAndUpdateLocal(newWorldStartPosition);
            }
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldEndPosition,
                hermiteCurveWrapper.transform.rotation, target, "End Position", out Vector3 newWorldEndPosition))
            {
                hermiteCurveWrapper.SetWorldEndPointAndUpdateLocal(newWorldEndPosition);
            }

            // Add the tangent handles so they can define a direction away from the Hermite spline
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldEndPosition + hermiteCurveWrapper.EndTangent,
                hermiteCurveWrapper.transform.rotation, target, "End Tangent", out Vector3 newWorldEndTangent))
            {
                hermiteCurveWrapper.EndTangent = newWorldEndTangent - hermiteCurveWrapper.WorldEndPosition;
            }
            if (HandlesHelperFunctions.ChangeSenstivePositionHandle(hermiteCurveWrapper.WorldStartPosition + hermiteCurveWrapper.StartTangent,
                hermiteCurveWrapper.transform.rotation, target, "Start Tangent", out Vector3 newWorldStartTangent))
            {
                hermiteCurveWrapper.StartTangent = newWorldStartTangent - hermiteCurveWrapper.WorldStartPosition;
            }
        }
    }
}
