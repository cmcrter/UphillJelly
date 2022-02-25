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
        private Rigidbody bRB;

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

        [SerializeField]
        CinemachineVirtualCamera wallRideCam;
        bool bJumping;

        #region Public Methods

        public HWallRideState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody frontRB, Rigidbody backRB, HisNextToWallRun wallRun, HisGroundBelow groundBelow)
        {
            playerMovement = controllerParent;
            pInput = controllerParent.input;
            inputManager = controllerParent.inputHandler;
            fRB = frontRB;
            bRB = backRB;
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
        }

        public override void PhysicsTick(float dT)
        {
            //Pushing the player along the wall
            if(!bJumping)
            {
                fRB.MovePosition(fRB.transform.position + (wallForward * rideSpeed));
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("WallRiding");
            playerMovement.characterAnimator.SetBool("wallriding", true);
            playerMovement.bWipeOutLocked = true;

            //Currently only works correctly due to the triggerable collider being a capsule, with a box collider this would cause issues
            wallForward = nextToWallRun.dotProductWithWall > 0 ? nextToWallRun.currentWallRide.transform.right : nextToWallRun.currentWallRide.transform.right * -1;
            Debug.Log(nextToWallRun.dotProductWithWall + " " + wallForward);

            rideSpeed = nextToWallRun.wallSpeed;

            playerMovement.camBrain.enabled = false;
            wallRideCam.enabled = true;

            playerMovement.transform.rotation = Quaternion.LookRotation(wallForward, Vector3.up);
            playerMovement.AlignWheels();
            playerMovement.ResetWheelPos();           

            fRB.isKinematic = true;
            fRB.velocity = Vector3.zero;

            bRB.isKinematic = true;
            bRB.velocity = Vector3.zero;

            Co_CoyoteCoroutine = playerMovement.StartCoroutine(Co_CoyoteTime());

            hasRan = true;
        }

        public override void OnStateExit()
        {
            if(Co_CoyoteCoroutine != null)
            {
                playerMovement.StopCoroutine(Co_CoyoteCoroutine);
                coyoteTimer = null;
            }

            nextToWallRun.StartCooldown();

            playerMovement.camBrain.enabled = true;
            wallRideCam.enabled = false;

            fRB.isKinematic = false;
            bRB.isKinematic = false;
            bJumping = false;

            playerMovement.characterAnimator.SetBool("wallriding", false);
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

            fRB.isKinematic = false;
            bRB.isKinematic = false;

            Co_CoyoteCoroutine = null;
        }

        private void JumpOffWallRide()
        {
            bJumping = true;
            //Debug.Log("Jumping off wall ride");

            fRB.isKinematic = false;
            bRB.isKinematic = false;

            fRB.AddForce((playerMovement.transform.up * 450f) + (nextToWallRun.currentWallRide.transform.forward * 950), ForceMode.Impulse);
            playerMovement.StartAirInfluenctCoroutine();
            playerMovement.StartCoroutine(WipeOutCooldown());
        }

        private IEnumerator WipeOutCooldown()
        {
            yield return new WaitForSeconds(1.2f);
            playerMovement.bWipeOutLocked = false;
        }

        #endregion
    }
}
