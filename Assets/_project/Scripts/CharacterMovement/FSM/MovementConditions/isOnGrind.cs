////////////////////////////////////////////////////////////
// File: isOnGrind.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/10/21
// Brief: This is the condition to see if the player starts grinding or not
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Utility.Splines;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isOnGrind : Condition
    {
        #region Variables

        private Rigidbody movementRB;
        public SplineWrapper splineCurrentlyGrindingOn;
        public GrindDetails grindDetails;

        #endregion

        #region Public Methods

        public isOnGrind(Rigidbody rb)
        {
            movementRB = rb;
        }

        public override bool isConditionTrue()
        {
            return (splineCurrentlyGrindingOn != null);
        }

        //The players' grind section has touched a grindable thing
        public void playerEnteredGrind(SplineWrapper splineHit, GrindDetails grindUsing)
        {
            if (splineCurrentlyGrindingOn == null)
            {
                grindDetails = grindUsing;
                splineCurrentlyGrindingOn = splineHit;
            }
        }

        //The players' grind section has left a grindable thing
        public void playerExitedGrind(SplineWrapper splineHit, GrindDetails grindUsing) 
        {
            if (splineCurrentlyGrindingOn)
            {
                if (splineCurrentlyGrindingOn.Equals(splineHit))
                {
                    grindDetails = null;
                    splineCurrentlyGrindingOn = null;
                }
            }
        }

        #endregion
    }
}