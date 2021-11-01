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

        //Having a few frames of leaving a grind before another one is registered
        [SerializeField]
        private float coyoteDuration = 0.2f;
        [SerializeField]
        private Timer coyoteTimer;

        Coroutine CoyoteCoroutine;

        bool ButtonPressed = false;

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
            return splineCurrentlyGrindingOn && ButtonPressed;
        }

        //The players' grind section has touched a grindable thing
        public void playerEnteredGrind(SplineWrapper splineHit, GrindDetails grindUsing)
        {
            if (splineCurrentlyGrindingOn == null && !grindedState.hasRan)
            {
                grindDetails = grindUsing;
                splineCurrentlyGrindingOn = splineHit;

                inputHandler.StartCoroutine(Co_WaitForButtonPress());
            }
        }

        public void playerExitedGrind()
        {
            CoyoteCoroutine = inputHandler.StartCoroutine(Co_CoyoteTimer());

            ButtonPressed = false;
            grindDetails = null;
            splineCurrentlyGrindingOn = null;

        }

        //The player left a grind area
        public void PlayerLeftGrindArea(SplineWrapper splineHit)
        {
            if(grindedState.hasRan)
            {
                return;
            }

            if(splineCurrentlyGrindingOn == splineHit)
            {
                splineCurrentlyGrindingOn = null;
                grindDetails = null;
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_WaitForButtonPress()
        {
            while(splineCurrentlyGrindingOn)
            {
                if(inputHandler.StartGrindHeld && CoyoteCoroutine == null)
                {
                    ButtonPressed = true;
                    inputHandler.StopCoroutine(Co_WaitForButtonPress());
                }

                yield return null;
            }
        }

        private IEnumerator Co_CoyoteTimer()
        {
            //It's technically a new timer on top of the class in use
            coyoteTimer = new Timer(coyoteDuration);

            //Whilst it has time left
            while(coyoteTimer.isActive)
            {
                //Tick each frame
                coyoteTimer.Tick(Time.deltaTime);
                yield return null;
            }

            CoyoteCoroutine = null;
        }

        #endregion
    }
}