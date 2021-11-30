////////////////////////////////////////////////////////////
// File: GrindedState.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/11/21
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
        private float timeAlongGrind = 0.001f;
        private float tIncrementPerSecond = 0.01f;

        [Header("Jumping variables")]
        [SerializeField]
        private float jumpCoroutineDuration;
        private Coroutine jumpCoroutine;

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
            inputHandler.grindingJumpUpActionPerformed += ForceRailDone;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputHandler.grindingJumpUpActionPerformed -= ForceRailDone;
        }

        public override void OnStateEnter()
        {
            grindVisualiser.transform.parent = null;

            pInput.SwitchCurrentActionMap("Grinding");
            parentController.playerCamera.FollowRotation = true;

            //Making sure nothing interferes with the movement
            movementRB.transform.position = onGrind.splineCurrentlyGrindingOn.GetClosestPointOnSpline(movementRB.transform.position, out timeAlongGrind) + new Vector3(0, 0.4025f, 0);
            timeAlongGrind = Mathf.Clamp(timeAlongGrind, 0.005f, 1f);

            parentController.transform.position = movementRB.transform.position;

            movementRB.velocity = Vector3.zero;
            movementRB.isKinematic = true;

            if(onGrind.grindDotProduct < -onGrind.angleAllowance)
            {
                bTravelBackwards = true;
            }
            else
            {
                bTravelBackwards = false;
            }

            currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);

            if(bTravelBackwards)
            {
                currentSplineDir *= -1;
            }

            movementRB.transform.forward = currentSplineDir;
            parentController.transform.forward = currentSplineDir;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            grindVisualiser.transform.parent = parentController.transform;

            pos = Vector3.zero;
            currentSplineDir = Vector3.zero;

            //The jumping needs the grind details
            StartJumpCoroutine(onGrind.grindDetails.ExitForce);

            //Let the condition know to reset
            onGrind.playerExitedGrind();

            timeAlongGrind = 0;
            bTravelBackwards = false;
            bForceExit = false;
            hasRan = false;
        }

        public override void Tick(float dT)
        {
            grindVisualiser.position = pos;

            if(onGrind.splineCurrentlyGrindingOn != null) 
            {
                float f = onGrind.splineCurrentlyGrindingOn.GetTotalLength();
                tIncrementPerSecond = onGrind.grindDetails.DuringGrindForce / f;
            }

            if(!bTravelBackwards)
            {
                ForwardMovement();
            }
            else
            {
                BackwardMovement();
            }

            if(!bForceExit)
            {
                movementRB.transform.position = pos;
            }
        }

        public override void PhysicsTick(float dT)
        {
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

        private void ForceRailDone()
        {
            bForceExit = true;
        }

        private void StartJumpCoroutine(Vector3 ExitForce)
        {
            jumpCoroutine = parentController.StartCoroutine(JumpOffPressed(ExitForce));
        }

        //The auto jump off
        //Jump off when player pressed button...
        private IEnumerator JumpOffPressed(Vector3 ExitForce)
        {
            movementRB.velocity = Vector3.zero;
            movementRB.angularVelocity = Vector3.zero;

            movementRB.transform.forward = parentController.transform.forward;
            movementRB.transform.up = parentController.transform.up;

            movementRB.isKinematic = false;
            movementRB.AddForce(((parentController.transform.up * ExitForce.y) + (parentController.transform.forward * ExitForce.z)) * 100, ForceMode.Impulse);
   
            jumpCoroutine = null;
            yield return true;
        }

        private void ForwardMovement()
        {
            if(timeAlongGrind + Time.deltaTime * tIncrementPerSecond < 1f)
            {
                // Clamping it at the max value and min values of a unit interval

                // Check the length of the next increment
                Vector3 nextPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind + Time.deltaTime * tIncrementPerSecond);
                Vector3 currentPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);
                Vector3 velocity = nextPoint - currentPoint;

                // Ideally the distance change should be speed * time.deltaTime
                float desiredDistance = onGrind.grindDetails.DuringGrindForce * Time.deltaTime;
                float currentDistanceChange = velocity.magnitude;

                float desiredChange = desiredDistance / currentDistanceChange;
                timeAlongGrind = Mathf.Clamp01(timeAlongGrind + Time.deltaTime * tIncrementPerSecond * desiredChange); // add length to this calculation

                //Using the calculated time to position everything correctly
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, parentController.ballMovement.radius + 0.0375f, 0);

                if(timeAlongGrind < 0.95f)
                {
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);

                    movementRB.transform.forward = currentSplineDir;
                    parentController.transform.forward = currentSplineDir;
                }
            }
            //if it's at the end
            else if(Vector3.Distance(movementRB.transform.position, onGrind.splineCurrentlyGrindingOn.WorldEndPosition) < 0.7f)
            {
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(1) + new Vector3(0, parentController.ballMovement.radius + 0.0375f, 0);

                currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0.99f, 0.01f);

                movementRB.transform.forward = currentSplineDir;
                parentController.transform.forward = currentSplineDir;

                bForceExit = true;
            }
        }

        private void BackwardMovement()
        {
            if(timeAlongGrind - Time.deltaTime * tIncrementPerSecond > 0f)
            {
                // Clamping it at the max value and min values of a unit interval

                // Check the length of the next increment
                Vector3 nextPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind - Time.deltaTime * tIncrementPerSecond);
                Vector3 currentPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);
                Vector3 velocity = nextPoint - currentPoint;

                // Ideally the distance change should be speed * time.deltaTime
                float desiredDistance = onGrind.grindDetails.DuringGrindForce * Time.deltaTime;
                float currentDistanceChange = velocity.magnitude;

                float desiredChange = desiredDistance / currentDistanceChange;
                timeAlongGrind = Mathf.Clamp01(timeAlongGrind - Time.deltaTime * tIncrementPerSecond * desiredChange); // add length to this calculation

                //Using the calculated time to position everything correctly
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, parentController.ballMovement.radius + 0.0375f, 0);

                if(timeAlongGrind > 0.05f)
                {
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);
                    currentSplineDir *= -1;

                    movementRB.transform.forward = currentSplineDir;
                    parentController.transform.forward = currentSplineDir;
                }
            }
            //if it's at the end
            else if(Vector3.Distance(movementRB.transform.position, onGrind.splineCurrentlyGrindingOn.WorldStartPosition) < 0.7f)
            {
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(0) + new Vector3(0, parentController.ballMovement.radius + 0.0375f, 0);

                currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0.01f, 0.01f);
                currentSplineDir *= -1;

                movementRB.transform.forward = currentSplineDir;
                parentController.transform.forward = currentSplineDir;

                bForceExit = true;
            }
        }

        #endregion
    }
}
