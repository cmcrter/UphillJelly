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
        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The point at which at which the line spline ends, would be the position at t 1")]
        private Vector3 endPoint;

        [SerializeField]
        [Tooltip("The Point at which the line spline starts, would be the position at t 0")]
        private Vector3 startPoint;
        #endregion

        #region Public Properties
        #region Overrides
        public override Vector3 EndPosition
        {
            get
            {
                return endPoint;
            }
            set
            {
                this.endPoint = value;
            }
        }
        public override Vector3 StartPosition
        {
            get
            {
                return startPoint;
            }
            set
            {
                this.startPoint = value;
            }
        }
        #endregion
        #endregion

        #region Public Methods
        #region Overrides
        public override float GetTotalLength()
        {
            return Vector3.Distance(startPoint, endPoint);
        }

        public override Vector3 GetPointAtTime(float t)
        {
            return Vector3.Lerp(startPoint, endPoint, t);
        }
        #endregion
        #endregion
    }
}


