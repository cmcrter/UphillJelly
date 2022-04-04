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
using L7Games.Utility.StateMachine;
using L7Games.Input;
using System.Collections;
using System.Collections.Generic;

namespace L7Games.Movement 
{
    [Serializable]
    public class HGrindedState : State
    {
        #region Variables

        private PlayerHingeMovementController parentController;
        private Rigidbody movementRB;
        private Rigidbody modelRB;

        [NonSerialized] private HisOnGrind onGrind = null;
        private PlayerInput pInput;
        private InputHandler inputHandler;

        [Header("Spline Following Variables")]
        [SerializeField]
        private Transform grindVisualiser;
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

        //This is simpler and more effective than using the one-shot vfx system
        [SerializeField]
        private GameObject grindVFXObject;

        [SerializeField]
        private TrickBuffer trickBuffer;
        [SerializeField]
        private ScoreableAction grindScoreableAction;

        private int currentGrindTrickID;
        private FMOD.Studio.EventInstance GS;

        #endregion

        #region Public Methods

        public HGrindedState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB, Rigidbody bodyRB, HisOnGrind grind)
        {
            parentController = controllerParent;
            movementRB = playerRB;
            modelRB = bodyRB;

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
            parentController.characterAnimator.SetBool("grinding", true);
            //parentController.bWipeOutLocked = true;
            parentController.camBrain.Follow = parentController.transform;

            //Making sure nothing interferes with the movement
            Vector3 closestPoint = onGrind.splineCurrentlyGrindingOn.GetClosestPointOnSpline(movementRB.transform.position, out timeAlongGrind) + new Vector3(0, 0.25f, 0);
            //Debug.Log(closestPoint);

            movementRB.transform.position = closestPoint;
            timeAlongGrind = Mathf.Clamp(timeAlongGrind, 0.0075f, 0.9925f);

            movementRB.velocity = Vector3.zero;
            movementRB.useGravity = false;
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

            parentController.transform.forward = currentSplineDir;

            grindVFXObject.SetActive(true);

            currentGrindTrickID = trickBuffer.AddScoreableActionInProgress(grindScoreableAction);
            GS = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/GrindRail2");
            GS.start();

            hasRan = true;
        }

        public override void OnStateExit()
        {
            grindVisualiser.transform.parent = parentController.transform;
            grindVisualiser.transform.localPosition = Vector3.zero;

            pos = Vector3.zero;
            currentSplineDir = Vector3.zero;

            if(onGrind.grindDetails)
            {
                //The jumping needs the grind details
                StartJumpCoroutine(onGrind.grindDetails.ExitForce);
            }

            //Let the condition know to reset
            onGrind.playerExitedGrind();

            parentController.StartAirInfluenctCoroutine();
            parentController.characterAnimator.SetBool("grinding", false);
            parentController.ResetCameraView();

            grindVFXObject.SetActive(false);

            GS.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            GS.release();

            timeAlongGrind = 0;
            bTravelBackwards = false;
            bForceExit = false;
            parentController.bWipeOutLocked = false;

            trickBuffer.FinishScorableActionInProgress(currentGrindTrickID);

            hasRan = false;
        }

        public override void Tick(float dT)
        {
            if(onGrind.splineCurrentlyGrindingOn != null) 
            {
                tIncrementPerSecond = onGrind.grindDetails.DuringGrindForce / onGrind.splineCurrentlyGrindingOn.GetTotalLength();
            }

            if(!bTravelBackwards)
            {
                ForwardMovement();
            }
            else
            {
                BackwardMovement();
            }

            grindVisualiser.position = pos;
            parentController.transform.rotation = Quaternion.LookRotation(currentSplineDir, parentController.transform.up);
        }

        public override void PhysicsTick(float dT)
        {
            if(!bForceExit)
            {
                movementRB.MovePosition(pos);
                movementRB.MoveRotation(Quaternion.LookRotation(currentSplineDir, parentController.transform.up));
            }
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

            movementRB.interpolation = RigidbodyInterpolation.None;
            modelRB.interpolation = RigidbodyInterpolation.None;

            yield return new WaitForFixedUpdate();
            movementRB.useGravity = true;
            movementRB.isKinematic = false;
            movementRB.detectCollisions = true;
            movementRB.AddForce(((parentController.transform.up * ExitForce.y) + (parentController.transform.forward * ExitForce.z)) * 100, ForceMode.Impulse);
   
            jumpCoroutine = null;
            yield return true;
        }

        private void ForwardMovement()
        {
            if(timeAlongGrind + Time.deltaTime * tIncrementPerSecond < 1f)
            {
                // Check the length of the next increment
                Vector3 nextPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind + Time.deltaTime * tIncrementPerSecond);
                Vector3 currentPoint = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);
                Vector3 velocity = nextPoint - currentPoint;

                // Ideally the distance change should be speed * time.deltaTime
                float desiredDistance = onGrind.grindDetails.DuringGrindForce * Time.deltaTime;
                float currentDistanceChange = velocity.magnitude;

                float desiredChange = desiredDistance / currentDistanceChange;
                timeAlongGrind = Mathf.Clamp01(timeAlongGrind + Time.deltaTime * tIncrementPerSecond * desiredChange); // add length to this calculation

                Vector3 newPos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, 0.45f + 0.0375f, 0);

                //Using the calculated time to position everything correctly
                pos = newPos;

                if(timeAlongGrind < 0.95f)
                {
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);
                }
            }
            //if it's at the end
            else if(Vector3.Distance(movementRB.transform.position, onGrind.splineCurrentlyGrindingOn.WorldEndPosition) < 0.7f)
            {
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(1) + new Vector3(0, 0.45f + 0.0375f, 0);

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

                Vector3 newPos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, 0.45f + 0.0375f, 0);

                //Using the calculated time to position everything correctly
                pos = newPos;

                if(timeAlongGrind > 0.05f)
                {
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.01f);
                    currentSplineDir *= -1;
                }
            }
            //if it's at the end
            else if(Vector3.Distance(movementRB.transform.position, onGrind.splineCurrentlyGrindingOn.WorldStartPosition) < 0.7f)
            {
                pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(0) + new Vector3(0, 0.45f + 0.0375f, 0);

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
