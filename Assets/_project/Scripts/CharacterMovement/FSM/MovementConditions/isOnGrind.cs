////////////////////////////////////////////////////////////
// File: isOnGrind.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/10/21
// Brief: This is the condition to see if the player starts grinding or not
//////////////////////////////////////////////////////////// 

using System;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Utility.Splines;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isOnGrind : Condition
    {
        #region Variables

        public SplineWrapper splineCurrentlyGrindingOn;

        #endregion

        #region Public Methods

        public override bool isConditionTrue()
        {
            return (splineCurrentlyGrindingOn != null);
        }

        //The players' grind section has touched a grindable thing
        public void playerEnteredGrind(SplineWrapper splineHit)
        {
            if (splineCurrentlyGrindingOn == null)
            {
                splineCurrentlyGrindingOn = splineHit;
            }
        }

        //The players' grind section has left a grindable thing
        public void playerExitedGrind(SplineWrapper splineHit) 
        {
            if (splineCurrentlyGrindingOn.Equals(splineHit))
            {
                splineCurrentlyGrindingOn = null;
            }
        }

        #endregion
    }
}