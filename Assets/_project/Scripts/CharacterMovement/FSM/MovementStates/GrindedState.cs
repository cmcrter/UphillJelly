////////////////////////////////////////////////////////////
// File: GrindedState.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/10/21
// Brief: The player state for when the player is grinding
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Input;
using System.Collections;

namespace SleepyCat.Movement 
{
    [Serializable]
    public class GrindedState : State
    {
        #region Variables

        private PlayerMovementController parentController;
        private Rigidbody movementRB;
        [NonSerialized] private isOnGrind onGrind = null;
        private PlayerInput pInput;
        private InputHandler inputHandler;

        [Header("Spline Following Variables")]
        [SerializeField]
        private Transform grindVisualiser;
        [SerializeField]
        private Vector3 currentPlayerAim = Vector3.zero;
        [SerializeField]
        private Vector3 currentSplineDir;
        [SerializeField]
        private Vector3 pos;

        [SerializeField]
        private float timeAlongGrind;
        private float tIncrementPerSecond = 0.01f;

        [Header("Jumping variables")]
        [SerializeField]
        private float jumpCoroutineDuration;
        private Coroutine jumpCoroutine;
        [SerializeField]
        private float autoJumpCoroutineDuration;
        private Coroutine autoJumpCoroutine;
        [SerializeField]
        private float jumpSpeed = 50;

        private bool bTravelBackwards = false;
        private bool bForceExit = false;
        Vector3 jumpDir;

        #endregion

        #region Public Methods

        public GrindedState()
        {

        }

        public void InitialiseState(PlayerMovementController controllerParent, Rigidbody playerRB, isOnGrind grind)
        {
            parentController = controllerParent;
            movementRB = playerRB;
            onGrind = grind;
            grind.SetGrindState(this);

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;
        }

        public void RegisterInputs()
        {
            //Register functions
            inputHandler.grindingJumpUpActionPerformed += JumpOffPressed;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputHandler.grindingJumpUpActionPerformed -= JumpOffPressed;
        }

        public override void OnStateEnter()
        {
            grindVisualiser.transform.parent = null;

            pInput.SwitchCurrentActionMap("Grinding");
            parentController.playerCamera.FollowRotation = true;

            //if(parentController.playerCamera)
            //{
            //    parentController.playerCamera.FollowRotation = false;
            //}

            //Making sure nothing interferes with the movement
            movementRB.position = onGrind.splineCurrentlyGrindingOn.GetClosestPointOnSpline(movementRB.transform.position, out timeAlongGrind) + new Vector3(0, 0.401f, 0);
            parentController.transform.position = movementRB.transform.position;

            movementRB.isKinematic = true;

            currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);
            parentController.transform.forward = currentSplineDir;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            grindVisualiser.transform.parent = parentController.transform;

            pos = Vector3.zero;
            currentSplineDir = Vector3.zero;

            //Let the condition know to reset
            onGrind.playerExitedGrind();

            timeAlongGrind = 0;
            bForceExit = false;
            hasRan = false;
        }

        public override void Tick(float dT)
        {
            if(onGrind.grindDetails != null && onGrind.splineCurrentlyGrindingOn && hasRan)
            {
                grindVisualiser.position = pos;

                if(timeAlongGrind + dT * tIncrementPerSecond < 1f)
                {
                    // Clamping it at the max value and min values of a unit interval

                    // Check the length of the next increment
                    Vector3 nextPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind + dT * tIncrementPerSecond);
                    Vector3 currentPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);
                    Vector3 velocity = nextPoint - currentPoint;

                    // Ideally the distance change should be speed * time.deltaTime
                    float desiredDistance = onGrind.grindDetails.DuringGrindForce * dT;
                    float currentDistanceChange = velocity.magnitude;

                    float desiredChange = desiredDistance / currentDistanceChange;
                    timeAlongGrind = Mathf.Clamp01(timeAlongGrind + dT * tIncrementPerSecond * desiredChange); // add length to this calculation

                    //Using the calculated time to position everything correctly
                    pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, parentController.ballMovement.radius + 0.01f, 0);

                    if(timeAlongGrind < 0.95f)
                    {
                        currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);
                        parentController.transform.forward = currentSplineDir;
                    }
                }             
                //if it's at the end
                else if(Vector3.Distance(parentController.transform.position, onGrind.splineCurrentlyGrindingOn.WorldEndPosition) < 0.7f)
                {
                    pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(1) + new Vector3(0, parentController.ballMovement.radius + 0.01f, 0);

                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0.99f, 0.01f);
                    parentController.transform.forward = currentSplineDir;

                    JumpOffPressed();
                }
            }
        }

        public override void PhysicsTick(float dT)
        {
            if(!bForceExit)
            {
                movementRB.MovePosition(Vector3.Lerp(movementRB.position, pos, dT * 10f));
            }

            parentController.transform.position = movementRB.transform.position;
        }

        public override State returnCurrentState()
        {
            //This is a condition specific to this state
            if (isRailDone())
            {
                return parentController.aerialState;
            }

            return this;
        }

        public bool isRailDone()
        {
            return bForceExit;
        }

        #endregion

        #region Private Methods

        //The auto jump off
        //Jump off when player pressed button...
        private void JumpOffPressed()
        {
            movementRB.interpolation = RigidbodyInterpolation.None;
            movementRB.isKinematic = false;
            movementRB.AddForce(((parentController.transform.up.normalized * onGrind.grindDetails.ExitForce.y) + (parentController.transform.forward.normalized * onGrind.grindDetails.ExitForce.z)) * 100f, ForceMode.Impulse);

            bForceExit = true;
        }

        #endregion
    }
}
