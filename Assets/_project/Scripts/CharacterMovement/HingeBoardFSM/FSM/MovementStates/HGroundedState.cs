////////////////////////////////////////////////////////////
// File: GroundedState.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 07/01/22
// Brief: The state that the player is in when they're on the ground
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;

namespace L7Games.Movement
{
    [Serializable]
    public class HGroundedState : State
    {
        [NonSerialized] private HisGroundBelow groundedCondition = null;
        [NonSerialized] private HisOnGrind grindedCondition = null;

        private PlayerHingeMovementController parentController;
        private InputHandler inputHandler;
        private Transform playerTransform;
        [SerializeField]
        private Transform testPressDownObject;
        private Rigidbody movementRB;
        private Rigidbody followRB;

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

        public bool bConstantForce = false;
        private bool bCurrentlyApplyingConstant = false;
        [SerializeField]
        private float constantForce;

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
        private bool bPressingDown = false;

        public HGroundedState()
        {
        }

        ~HGroundedState()
        {      
        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB,  Rigidbody playerBackRB, HisGroundBelow groundBelow, HisOnGrind onGrind)
        {
            //Apply variables needed
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;
            followRB = playerBackRB;

            groundedCondition = groundBelow;
            grindedCondition = onGrind;

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;

            bCurrentlyApplyingConstant = bConstantForce;
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
            if(Keyboard.current.escapeKey.isPressed)
            {
                parentController.ResetPlayer();
            }

            if(inputHandler.PushHeldDown && pushCoroutine == null)
            {
                PushBoard();
            }
            else if(inputHandler.BrakeHeldDown)
            {
                ApplyBrakeForce();
            }

            parentController.SmoothToGroundRotation(false, groundAdjustSmoothness, turnSpeed, groundedCondition);
        }

        public override void PhysicsTick(float dT)
        {
            if(bConstantForce)
            {
                movementRB.AddForceAtPosition(playerTransform.forward * constantForce, movementRB.position + movementRB.centerOfMass, ForceMode.Force);
            }

            UpdatePositionAndRotation(dT);
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Grounded");
            
            parentController.playerCamera.FollowRotation = true;
            movementRB.drag = GroundedDrag;
            followRB.drag = GroundedDrag;

            //Making sure they can start a push when landing
            if(inputHandler.PushHeldDown)
            {
                PushBoard();
            }

            if(inputHandler.TurningAxis != 0)
            {
                parentController.StartTurnCoroutine();
            }

            parentController.SmoothToGroundRotation(false, groundAdjustSmoothness, turnSpeed, groundedCondition);

            hasRan = true;
        }

        public override void OnStateExit()
        {
            StopJumpTimerCoroutine();
            StopPushTimerCoroutine();

            UnPressDown();            

            parentController.playerCamera.bMovingBackwards = false;
            hasRan = false;
        }

        #region Private Methods

        private void UpdatePositionAndRotation(float dT)
        {
            if(jumpCoroutine == null) 
            {
                //Depending on the difference of angle in the movement currently and the transform forward of the skateboard, apply more drag the wider the angle (maximum angle being 90 for drag)
                float initialSpeed = movementRB.velocity.magnitude;
                float dotAngle = Vector3.Dot(movementRB.velocity.normalized, playerTransform.forward.normalized);
                float absDotAngle = Mathf.Abs(dotAngle);

                //Moving almost directly backwards
                if(dotAngle < -0.98f)
                {
                    parentController.playerCamera.bMovingBackwards = true;
                    movementRB.transform.forward = -playerTransform.forward;
                    followRB.transform.forward = -playerTransform.forward;
                }
                //Must be moving forwards
                else
                {
                    parentController.playerCamera.bMovingBackwards = false;
                    parentController.AlignWheels();
                }

                movementRB.velocity = initialSpeed * movementRB.transform.forward;

                // 0 means it is perpendicular, 1 means it's perfectly parallel
                if(absDotAngle < 0.99f)
                {
                    movementRB.angularVelocity = Vector3.zero;
                    followRB.angularVelocity = Vector3.zero;

                    //And conserving it if need be
                    if(absDotAngle < 0.3265f)
                    {
                        movementRB.velocity = Vector3.zero;
                        followRB.velocity = Vector3.zero;

                        movementRB.AddForce(playerTransform.forward.normalized, ForceMode.Impulse);
                    }
                }
            }

            //With the hinge, this means that the rb wont just run away without the player
            if(Vector3.Distance(playerTransform.position, movementRB.transform.position) > 0.5f)
            {
                movementRB.transform.position = parentController.boardObject.transform.position + (playerTransform.forward * 0.281f);
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
            if(!bPressingDown)
            {
                movementRB.centerOfMass += new Vector3(0, -0.25f, 0);
                testPressDownObject.position += new Vector3(0, -0.3f, 0);
                bPressingDown = true;
            }
        }

        private void UnPressDown()
        {
            if(bPressingDown)
            {
                movementRB.centerOfMass += new Vector3(0, 0.25f, 0);
                testPressDownObject.position += new Vector3(0, 0.3f, 0);
                bPressingDown = false;
            }
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
            jumpCoroutine = parentController.StartCoroutine(Co_JumpTimer(bPressingDown));
        }

        private void StopJumpTimerCoroutine()
        {
            if(jumpCoroutine != null)
            {
                parentController.StopCoroutine(jumpCoroutine);
            }

            jumpCoroutine = null;
        }

        private IEnumerator Co_JumpTimer(bool isPressingDown)
        {
            //It's technically a new timer on top of the class in use
            jumpTimer = new Timer(jumpTimerDuration);

            //Pressing down makes jumping bigger
            float newjumpSpeed = isPressingDown ? jumpSpeed * 1.5f : jumpSpeed;

            movementRB.AddForce(parentController.transform.up * newjumpSpeed * 1000);
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

                    // Force is effected by the characters angle away from up
                    float angleFromUp = Vector3.Angle(Vector3.up, playerTransform.forward);
                    force *= angleFromUp / 90f;

                    Debug.Log("Push force" + force);

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