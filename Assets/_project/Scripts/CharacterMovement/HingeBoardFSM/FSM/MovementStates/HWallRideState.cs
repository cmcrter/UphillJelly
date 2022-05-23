////////////////////////////////////////////////////////////
// File: WallRideState.cs
// Author: Charles Carter
// Date Created: 29/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 01/11/21
// Brief: The state the player is in when they're riding along the wall
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;
using L7Games.Tricks;
using Cinemachine;

namespace L7Games.Movement
{
    [Serializable]
    public class HWallRideState : State
    {
        private PlayerHingeMovementController playerMovement;
        private PlayerInput pInput;
        private InputHandler inputManager;
        private Rigidbody fRB;

        [NonSerialized] private HisNextToWallRun nextToWallRun;
        [NonSerialized] private HisGroundBelow nextToGround;

        [SerializeField]
        private float coyoteDuration = 3f;
        [SerializeField]
        private Timer coyoteTimer;
        private Coroutine Co_CoyoteCoroutine;

        [SerializeField]
        private Vector3 wallForward;
        [SerializeField]
        private float rideSpeed = 0.1f;

        bool bJumping;

        [SerializeField]
        LayerMask collisionCheckMask;

        [SerializeField]
        private ScoreableAction wallrideScoreableAction;

        private int currentGrindTrickID;
        private float inputTurn;

        #region Public Methods

        public HWallRideState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody frontRB, HisNextToWallRun wallRun, HisGroundBelow groundBelow)
        {
            playerMovement = controllerParent;
            pInput = controllerParent.input;
            inputManager = controllerParent.inputHandler;
            fRB = frontRB;
            nextToWallRun = wallRun;
            nextToGround = groundBelow;
        }

        public override State returnCurrentState()
        {
            if(nextToGround.isConditionTrue())
            {
                return playerMovement.groundedState;
            }

            if(!nextToWallRun.isConditionTrue())
            {
                return playerMovement.aerialState;
            }

            return this;
        }

        public void RegisterInputs()
        {
            //Register functions
            inputManager.wallRidingJumpUpAction += JumpOffWallRide;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputManager.wallRidingJumpUpAction -= JumpOffWallRide;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            //playerMovement.transform.position = fRB.transform.position;

            //Raycast forward to see if board is hitting something
            //If it is, end the wall ride...
            Debug.DrawLine(playerMovement.transform.position, playerMovement.transform.position + (playerMovement.transform.forward * 0.5f), Color.green);
            Debug.DrawLine(playerMovement.transform.position, playerMovement.transform.position + (playerMovement.transform.forward * 0.5f), Color.green);

            if(Time.frameCount % 5 == 0)
            {
                if(Physics.Raycast(fRB.transform.position, playerMovement.transform.forward, out RaycastHit hit, 1.0f, ~collisionCheckMask, QueryTriggerInteraction.Ignore) || Physics.Raycast(playerMovement.transform.position, playerMovement.transform.forward, out RaycastHit bodyhit, 1.0f, ~collisionCheckMask, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log(hit.collider.name);

                    //Wipeout
                    playerMovement.bWipeOutLocked = false;
                    playerMovement.CallOnWipeout(fRB.velocity);
                    return;
                }
            }
        }

        public override void PhysicsTick(float dT)
        {
            //Pushing the player along the wall
            if(!bJumping)
            {
                fRB.MovePosition(fRB.transform.position + (playerMovement.transform.forward * rideSpeed));
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("WallRiding");

            playerMovement.bWipeOutLocked = true;
            playerMovement.rbCollider.enabled = false;
            playerMovement.boardCollider.enabled = false;

            //Currently only works correctly due to the triggerable collider being a capsule, with a box collider this would cause issues
            wallForward = nextToWallRun.dotProductWithWall > 0 ? nextToWallRun.currentWallRide.transform.right : nextToWallRun.currentWallRide.transform.right * -1;

            if(Vector3.Dot(wallForward, playerMovement.transform.right) > 0)
            {
                playerMovement.characterAnimator.SetBool("wallridingMirror", true);
            }

            playerMovement.characterAnimator.SetBool("wallriding", true);
            playerMovement.bWipeOutLocked = true;

            //Debug.Log(nextToWallRun.dotProductWithWall + " " + wallForward);

            rideSpeed = nextToWallRun.wallSpeed;

            playerMovement.OverrideCamera(playerMovement.wallRideCam, false);
            playerMovement.transform.rotation = Quaternion.LookRotation(wallForward, Vector3.up);

            playerMovement.AlignWheels();
            playerMovement.ResetWheelPos();

            fRB.isKinematic = true;
            fRB.velocity = Vector3.zero;

            Co_CoyoteCoroutine = playerMovement.StartCoroutine(Co_CoyoteTime());
            currentGrindTrickID = playerMovement.trickBuffer.AddScoreableActionInProgress(wallrideScoreableAction);

            hasRan = true;
        }

        public override void OnStateExit()
        {
            inputTurn = inputManager.TurningAxis;

            if(Co_CoyoteCoroutine != null)
            {
                playerMovement.StopCoroutine(Co_CoyoteCoroutine);
                coyoteTimer = null;
            }

            nextToWallRun.StartCooldown();

            playerMovement.camBrain.enabled = true;
            playerMovement.wallRideCam.enabled = false;

            fRB.isKinematic = false;

            bJumping = false;

            playerMovement.characterAnimator.SetBool("wallriding", false);
            playerMovement.characterAnimator.SetBool("wallridingMirror", false);

            playerMovement.rbCollider.enabled = true;
            playerMovement.boardCollider.enabled = true;

            playerMovement.trickBuffer.FinishScorableActionInProgress(currentGrindTrickID);
            JumpingOff();

            hasRan = false;
        }

        public void SetRideSpeed(float newRideSpeed)
        {
            rideSpeed = newRideSpeed;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Coyote time for being on the wall ride
        /// </summary>
        private IEnumerator Co_CoyoteTime()
        {
            coyoteTimer = new Timer(coyoteDuration);

            while(coyoteTimer.isActive)
            {
                coyoteTimer.Tick(Time.deltaTime);
                yield return null;
            }

            //Was on the wall ride too long let gravity take hold
            fRB.velocity = Vector3.zero;
            fRB.isKinematic = false;

            Co_CoyoteCoroutine = null;
        }

        private void JumpingOff()
        {
            playerMovement.bWipeOutLocked = false;
            playerMovement.StartCoroutine(Co_InputDelay());
        }

        private void JumpOffWallRide()
        {
            bJumping = true;
            //Debug.Log("Jumping off wall ride");

            fRB.isKinematic = false;

            playerMovement.StartCoroutine(Co_WallRideJump());
        }

        private IEnumerator Co_WallRideJump()
        {
            yield return new WaitForFixedUpdate();
            float intialMagnitude = fRB.velocity.magnitude;

            playerMovement.ModelRB.velocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            if(!nextToWallRun.currentWallRide)
            {
                fRB.AddForce(playerMovement.transform.up * 350f, ForceMode.Impulse);
            }
            else
            {
                fRB.AddForce((playerMovement.transform.up * 350f) + (nextToWallRun.currentWallRide.transform.forward * 950f), ForceMode.Impulse);
            }

            fRB.AddForce(playerMovement.transform.forward * intialMagnitude * 50f, ForceMode.Impulse);
        }

        private IEnumerator Co_InputDelay()
        {
            inputTurn = inputManager.TurningAxis;
            yield return new WaitForFixedUpdate();
            inputManager.TurningAxis = inputTurn;
            playerMovement.StartAirInfluenctCoroutine();
        }

        #endregion
    }
}
