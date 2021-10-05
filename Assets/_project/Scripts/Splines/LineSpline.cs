//===========================================================================================================================================================================================================================================================================
// Name:                LineSpline.cs
// Author:              Matthew Mason
// Date Created:        25-Mar-2020
// Date Last Modified:  05-Oct-2021
// Brief:               A spline that is a line between 2 points
//============================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.Utility.Splines
{
    /// <summary>
    /// A spline that is a line between 2 points
    /// </summary>
    [System.Serializable]
    public class LineSpline : Spline
    {
        #region Public Variables
        [Tooltip("The point at which at which the line spline ends, would be the position at t 1")]
        public Vector3 endPoint;
        [Tooltip("The Point at which the line spline starts, would be the position at t 0")]
        public Vector3 startPoint;
        #endregion

        #region Public Methods
        #region Overrides
        public override float GetTotalLength()
        {
            return Vector3.Distance(startPoint, endPoint);
        }

        public override Vector3 GetEndPoint()
        {
            return endPoint;
        }
        public override Vector3 GetPointAtTime(float t)
        {
            return Vector3.Lerp(startPoint, endPoint, t);
        }
        public override Vector3 GetStartPoint()
        {
            return startPoint;
        }

        public override void SetEndPoint(Vector3 endPoint)
        {
            this.endPoint = endPoint;
        }
        public override void SetStartPoint(Vector3 startPoint)
        {
            this.startPoint = startPoint;
        }
        #endregion
        #endregion
    }
}


