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

namespace L7Games.Movement
{
    [Serializable]
    public class HWallRideState : State
    {
        private PlayerHingeMovementController playerMovement;
        private PlayerInput pInput;
        private Rigidbody rb;
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

        #region Public Methods

        public HWallRideState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB, HisNextToWallRun wallRun, HisGroundBelow groundBelow)
        {
            playerMovement = controllerParent;
            pInput = controllerParent.input;
            rb = playerRB;
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

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            //Pushing the player along the wall
            rb.MovePosition(rb.transform.position + (wallForward * rideSpeed));
        }

        public override void PhysicsTick(float dT)
        {
            playerMovement.transform.position = rb.transform.position;
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("WallRiding");

            rb.isKinematic = true;
            rb.velocity = Vector3.zero;

            playerMovement.ResetWheelPos();
            playerMovement.AlignWheels();

            //Currently only works correctly due to the triggerable collider being a capsule, with a box collider this would cause issues
            wallForward = nextToWallRun.currentWallRide.transform.right;
            if(nextToWallRun.dotProductWithWall < 0)
            {
                wallForward *= -1;
            }

            playerMovement.transform.forward = wallForward;
            
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

            rb.isKinematic = false;
            hasRan = false;
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_CoyoteTime()
        {
            coyoteTimer = new Timer(coyoteDuration);

            while(coyoteTimer.isActive)
            {
                coyoteTimer.Tick(Time.deltaTime);
                yield return null;
            }

            rb.isKinematic = false;
            Co_CoyoteCoroutine = null;
        }

        #endregion
    }
}
