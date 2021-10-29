////////////////////////////////////////////////////////////
// File: isOnGrind.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/10/21
// Brief: This is the condition to see if the player starts grinding or not
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using UnityEngine;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Utility.Splines;
using SleepyCat.Input;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isOnGrind : Condition
    {
        #region Variables

        private Rigidbody movementRB;
        private InputHandler inputHandler;

        public SplineWrapper splineCurrentlyGrindingOn;
        public GrindDetails grindDetails;
        [NonSerialized] private GrindedState grindedState = null;

        //Having a few frames of pressed before the button is registered as unpressed
        float coyoteDuration;
        Timer coyoteTimer;
        Coroutine CoyoteCoroutine;

        #endregion

        #region Public Methods

        public isOnGrind()
        {

        }

        public void InitialiseCondition(Rigidbody rb, InputHandler iHandler)
        {
            movementRB = rb;
            inputHandler = iHandler;
        }

        public void SetGrindState(GrindedState state)
        {
            grindedState = state;
        }

        public override bool isConditionTrue()
        {
            return (splineCurrentlyGrindingOn != null) && inputHandler.StartGrindHeld;
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
            if (splineCurrentlyGrindingOn != null && grindedState.isRailDone())
            {
                if (splineCurrentlyGrindingOn.Equals(splineHit) && grindDetails.Equals(grindUsing))
                {
                    grindDetails = null;
                    splineCurrentlyGrindingOn = null;
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}