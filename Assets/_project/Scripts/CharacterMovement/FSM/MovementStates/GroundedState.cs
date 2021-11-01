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
        [NonSerialized] private isOnGrind grindedCondition = null;

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
        private float groundAdjustSmoothness = 16;

        //This is public in case other systems need to know if the player is pushing or jumping or recently unpressing down.
        public Coroutine pushCoroutine
        {
            get; private set;
        }

        public Coroutine jumpCoroutine
        {
            get; private set;
        }

        [Header("Coroutine management")]
        [SerializeField]
        private float pushTimerDuration = 0.65f;
        [SerializeField]
        private float pushForceDuration = 0.35f;
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

        public void InitialiseState(PlayerMovementController controllerParent, Rigidbody playerRB, isGroundBelow groundBelow, isOnGrind onGrind)
        {
            //Apply variables needed
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;

            groundedCondition = groundBelow;
            grindedCondition = onGrind;

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;
        }

        public void RegisterInputs()
        {
            //Register functions
            inputHandler.groundedJumpUpPerformed += Jump;
            inputHandler.pressDownStarted += PressDown;
            inputHandler.pressDownEnded += UnPressDown;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputHandler.groundedJumpUpPerformed -= Jump;
            inputHandler.pressDownStarted -= PressDown;
            inputHandler.pressDownEnded -= UnPressDown;

        }

        public override State returnCurrentState()
        {
            if(grindedCondition.isConditionTrue())
            {
                return parentController.grindingState;
            }

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

            if(inputHandler.PushHeldDown && pushCoroutine == null)
            {
                PushBoard();
            }
            else if(inputHandler.BrakeHeldDown)
            {
                ApplyBrakeForce();
            }
        }

        public override void PhysicsTick(float dT)
        {

        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Grounded");
            
            parentController.playerCamera.FollowRotation = true;
            movementRB.drag = GroundedDrag;

            //Making sure they can start a push when landing
            if(inputHandler.PushHeldDown)
            {
                PushBoard();
            }

            hasRan = true;
        }

        public override void OnStateExit()
        {
            StopJumpTimerCoroutine();
            StopPushTimerCoroutine();

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

        private void PressDown()
        {
            movementRB.centerOfMass += new Vector3(0, -0.25f, 0);
        }

        private void UnPressDown()
        {
            movementRB.centerOfMass += new Vector3(0, 0.25f, 0);
        }

        private void Jump()
        {
            if(jumpCoroutine == null && hasRan)
            {
                Debug.Log("Jumping");
                StartJumpTimer();
            }
        }

        private void PushBoard()
        {
            if(pushCoroutine != null && pushDuringTimer.isActive)
            {
                return;
            }

            StartPushTimerCoroutine();
        }

        #endregion

        /// <summary>
        /// Push Coroutine Management
        /// </summary>
        private void StartPushTimerCoroutine()
        {
            pushCoroutine = parentController.StartCoroutine(Co_BoardPush());
        }

        private void StopPushTimerCoroutine()
        {
            if(pushCoroutine != null)
            {
                parentController.StopCoroutine(pushCoroutine);
            }

            pushCoroutine = null;
        }
        
        private void StartJumpTimer()
        {
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

            movementRB.AddForce(parentController.transform.up * jumpSpeed * 1000);
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

        //Running the during timer
        private IEnumerator Co_BoardPush()
        {
            //It's technically a new timer on top of the class in use
            pushDuringTimer = new Timer(pushTimerDuration);

            //Whilst it has time left
            while(pushDuringTimer.isActive && hasRan)
            {
                if(pushDuringTimer.current_time <= pushForceDuration)
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
                }
             
                //Tick each frame
                pushDuringTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushCoroutine = null;
        }

        #endregion
    }
}