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
            inputHandler.jumpUpPerformed += JumpOffPressed;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputHandler.jumpUpPerformed -= JumpOffPressed;
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Grinding");

            if(parentController.playerCamera)
            {
                parentController.playerCamera.FollowRotation = false;
            }

            movementRB.transform.position = onGrind.splineCurrentlyGrindingOn.GetClosestPointOnSpline(parentController.transform.position, out timeAlongGrind) + new Vector3(0, 0.41f, 0);
            parentController.transform.position = movementRB.transform.position;

            currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0f, 0.01f);

            parentController.transform.forward = Vector3.Cross(currentSplineDir, Vector3.up);

            //Making sure nothing interferes with the movement
            movementRB.interpolation = RigidbodyInterpolation.Interpolate;
            movementRB.isKinematic = true;
            hasRan = true;
        }

        public override void OnStateExit()
        {
            if (parentController.playerCamera) 
            {
                parentController.playerCamera.FollowRotation = true;
            }

            pos = Vector3.zero;
            currentSplineDir = Vector3.zero;

            movementRB.interpolation = RigidbodyInterpolation.None;
            timeAlongGrind = 0;
            hasRan = false;
        }

        public override void Tick(float dT)
        {
            if(onGrind.grindDetails != null && onGrind.splineCurrentlyGrindingOn)
            {
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
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);

                    grindVisualiser.position = pos;
                    parentController.transform.forward = Vector3.Cross(currentSplineDir, Vector3.up);
                }
                else
                {
                    timeAlongGrind = Mathf.Clamp01(timeAlongGrind + dT * tIncrementPerSecond); // add length to this calculation
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0.90f, 0.01f);
                    parentController.transform.forward = currentSplineDir;
                    JumpOff();
                }
            }
        }

        public override void PhysicsTick(float dT)
        {
            if(timeAlongGrind != 1)
            {
                movementRB.MovePosition(Vector3.Lerp(movementRB.position, pos, 15f * dT));
            }

            parentController.transform.position = movementRB.transform.position;
        }

        public override State returnCurrentState()
        {
            if (!onGrind.isConditionTrue())
            {
                return parentController.aerialState;
            }

            return this;
        }

        public bool isRailDone()
        {
            return timeAlongGrind >= 1f && !movementRB.isKinematic;
        }

        #endregion

        #region Private Methods

        //The auto jump off
        private void JumpOff()
        {
            timeAlongGrind = 1f;
            movementRB.isKinematic = false;

            movementRB.AddForce((parentController.transform.up * onGrind.grindDetails.ExitForce.y) + (parentController.transform.forward * onGrind.grindDetails.ExitForce.z), ForceMode.Impulse);
        }

        //Jump off when player pressed button...
        private void JumpOffPressed()
        {
            if(!hasRan) 
            {
                return;
            }

            timeAlongGrind = 1f;
            movementRB.isKinematic = false;

            parentController.transform.forward = currentSplineDir;

            //essentially the same jump as when grounded
            movementRB.AddForce((parentController.transform.up * jumpSpeed * 1000) + (parentController.transform.forward * jumpSpeed * 1000));
            Mathf.Clamp(movementRB.velocity.y, -99999, 5f);
        }

        #endregion
    }
}
