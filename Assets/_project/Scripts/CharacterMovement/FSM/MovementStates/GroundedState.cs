////////////////////////////////////////////////////////////
// File: GroundedState.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: The state that the player is in when they're on the ground
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Input;

namespace SleepyCat.Movement
{
    [Serializable]
    public class GroundedState : State
    {
        [NonSerialized] private isGroundBelow groundedCondition = null;

        private PlayerMovementController parentController;
        private InputHandler inputHandler;
        private Transform playerTransform;
        private Rigidbody movementRB;
        private PlayerInput pInput;

        [SerializeField]
        public float GroundedDrag = 0.05f;
        [SerializeField]
        private float forwardSpeed = 8;
        [SerializeField]
        private float turnSpeed = 4;
        [SerializeField]
        private float backwardSpeed = 3;
        [SerializeField]
        private float jumpSpeed = 50;

        [SerializeField]
        private float groundAdjustSmoothness = 12;

        //This is public in case other systems need to know if the player is pushing.
        public Coroutine pushWaitCoroutine
        {
            get; private set;
        }
        public Coroutine pushDuringCoroutine
        {
            get; private set;
        }
        public Coroutine jumpCoroutine
        {
            get; private set;
        }

        [SerializeField]
        private float pushCooldownTimerDuration = 0.65f;
        [SerializeField]
        private Timer pushTimer;
        [SerializeField]
        private float pushDuringTimerDuration = 0.35f;
        [SerializeField]
        private Timer pushDuringTimer;

        [SerializeField]
        private float jumpTimerDuration = 0.35f;
        [SerializeField]
        private Timer jumpTimer;

        public GroundedState()
        {
        }

        ~GroundedState()
        {      
        }

        public void InitialiseState(PlayerMovementController controllerParent, Rigidbody playerRB, isGroundBelow groundBelow)
        {
            //Apply variables needed
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;

            groundedCondition = groundBelow;
            pInput = controllerParent.input;

            //Register Input functions

        }

        public void DeInitialiseState()
        {
            //Unregister functions

        }

        public override State returnCurrentState()
        {
            if(!groundedCondition.isConditionTrue())
            {
                return parentController.aerialState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            UpdatePositionAndRotation(dT);

            if(Keyboard.current.escapeKey.isPressed)
            {
                parentController.ResetBoard();
            }
        }

        public override void PhysicsTick(float dT)
        {
            if(Keyboard.current.spaceKey.isPressed && !Keyboard.current.sKey.isPressed)
            {
                PushBoard();
            }
            else if(Keyboard.current.sKey.isPressed)
            {
                ApplyBrakeForce();
            }

            if(Keyboard.current.ctrlKey.isPressed)
            {
                Jump();
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Grounded");
            
            parentController.playerCamera.FollowRotation = true;
            movementRB.drag = GroundedDrag;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            hasRan = false;
        }

        #region Private Methods

        private void UpdatePositionAndRotation(float dT)
        {
            parentController.SmoothToGroundRotation(groundAdjustSmoothness, turnSpeed, groundedCondition.GroundHit, groundedCondition.FrontGroundHit);

            if(jumpCoroutine == null) 
            {
                //Depending on the difference of angle in the movement currently and the transform forward of the skateboard, apply more drag the wider the angle (maximum angle being 90 for drag)
                float initialSpeed = movementRB.velocity.magnitude;
                float dotAngle = Vector3.Dot(movementRB.velocity.normalized, playerTransform.forward.normalized);
                dotAngle = Mathf.Abs(dotAngle);

                // 0 means it is perpendicular, 1 means it's perfectly parallel
                if(dotAngle < 0.99f)
                {
                    movementRB.AddForce(-movementRB.velocity * (1f + (1.05f - dotAngle)), ForceMode.Impulse);

                    if(dotAngle > 0.35f)
                    {
                        movementRB.AddForce(initialSpeed * (1f + 0.01f) * playerTransform.forward, ForceMode.Impulse);
                    }
                    else
                    {
                        movementRB.velocity = Vector3.zero;
                        movementRB.Sleep();
                        movementRB.AddForce(playerTransform.forward.normalized, ForceMode.Impulse);
                    }
                }
            }
        }

        #region Controls Functions

        private void ApplyBrakeForce()
        {
            //Pushing backward as a constant force
            movementRB.AddForceAtPosition(-playerTransform.forward * backwardSpeed, movementRB.position + movementRB.centerOfMass, ForceMode.Force);
        }

        private void Jump()
        {
            Debug.Log("Jumping");
            StartJumpTimer();
        }

        private void PushBoard()
        {
            if(pushTimer != null && pushTimer.isActive)
            {
                return;
            }

            StartPushTimerCoroutine();
            StartPushDuringTimerCoroutine();
        }

        #endregion

        /// <summary>
        /// Push Coroutine Management
        /// </summary>
        private void StartPushTimerCoroutine()
        {
            StopPushTimerCoroutine();
            pushWaitCoroutine = parentController.StartCoroutine(Co_BoardAfterPush());
        }

        private void StopPushTimerCoroutine()
        {
            if(pushWaitCoroutine != null)
            {
                parentController.StopCoroutine(pushWaitCoroutine);
            }

            pushWaitCoroutine = null;
        }

        private void StartPushDuringTimerCoroutine()
        {
            StopPushDuringTimerCoroutine();
            pushDuringCoroutine = parentController.StartCoroutine(Co_BoardDuringPush());
        }

        private void StopPushDuringTimerCoroutine()
        {
            if(pushDuringCoroutine != null)
            {
                parentController.StopCoroutine(pushDuringCoroutine);
            }

            pushDuringCoroutine = null;
        }

        private void StartJumpTimer()
        {
            StopJumpTimerCoroutine();
            jumpCoroutine = parentController.StartCoroutine(Co_JumpTimer());
        }

        private void StopJumpTimerCoroutine()
        {
            if(jumpCoroutine != null)
            {
                parentController.StopCoroutine(jumpCoroutine);
            }

            jumpCoroutine = null;
        }


        private IEnumerator Co_JumpTimer()
        {
            //It's technically a new timer on top of the class in use
            jumpTimer = new Timer(jumpTimerDuration);

            movementRB.AddForce(Vector3.up * jumpSpeed * 1000);
            Mathf.Clamp(movementRB.velocity.y, -99999, 5f);

            //Whilst it has time left
            while(jumpTimer.isActive)
            {
                //Tick each frame
                jumpTimer.Tick(Time.deltaTime);
                yield return null;
            }

            jumpCoroutine = null;
        }

        //Running the timer
        private IEnumerator Co_BoardAfterPush()
        {
            //It's technically a new timer on top of the class in use
            pushTimer = new Timer(pushCooldownTimerDuration);

            //Whilst it has time left
            while(pushTimer.isActive)
            {
                //Tick each frame
                pushTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushWaitCoroutine = null;
        }

        //Running the during timer
        private IEnumerator Co_BoardDuringPush()
        {
            //It's technically a new timer on top of the class in use
            pushDuringTimer = new Timer(pushDuringTimerDuration);

            //Whilst it has time left
            while(pushDuringTimer.isActive && hasRan)
            {
                //Pushing forward
                Vector3 force = playerTransform.forward * forwardSpeed * 1000 * Time.deltaTime;

                if(movementRB.velocity.magnitude > 1)
                {
                    //adjust the force depending on the current speed (15 being the amount that it can maximum be at if it's just pushing)
                    float max = 10f / movementRB.velocity.magnitude;
                    Mathf.Clamp(max, 0.05f, 1);

                    //Debug.Log(rb.velocity.magnitude + " " + max + " " + (1 - max).ToString());

                    // if the force is above 12, pushing shouldn't add anything
                    force *= max;
                }
                else
                {
                    force *= 2f;
                }

                movementRB.AddForce(force, ForceMode.Impulse);

                //Tick each frame
                pushDuringTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushDuringCoroutine = null;
        }

        #endregion
    }
}