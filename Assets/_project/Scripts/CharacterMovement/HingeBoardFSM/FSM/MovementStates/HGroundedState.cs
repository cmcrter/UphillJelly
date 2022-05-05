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
using FMODUnity;
using FMOD;
using Debug = UnityEngine.Debug;

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

        public Coroutine uncrouchCoroutine
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

        [SerializeField]
        private ScriptableParticles landingDust;

        private FMOD.Studio.EventInstance moveSound;
        [SerializeField]
        private string parameterName;

        private bool cameraBackwards;
        private bool cameraSwitched;

        public HGroundedState()
        {
        }

        ~HGroundedState()
        {      
        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB,  HisGroundBelow groundBelow, HisOnGrind onGrind)
        {
            //Apply variables needed
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;

            groundedCondition = groundBelow;
            grindedCondition = onGrind;

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;

            bCurrentlyApplyingConstant = bConstantForce;
        }

        public void RegisterInputs()
        {
            //Register functions
            inputHandler.groundedJumpUpCancelled += Jump;
            inputHandler.pressDownStarted += PressDown;
            inputHandler.pressDownEnded += Jump;
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
            inputHandler.groundedJumpUpCancelled -= Jump;
            inputHandler.pressDownStarted -= PressDown;
            inputHandler.pressDownEnded -= Jump;

        }

        public override State returnCurrentState()
        {
            //if(grindedCondition.isConditionTrue())
            //{
            //    return parentController.grindingState;
            //}

            if(!groundedCondition.isConditionTrue())
            {
                return parentController.aerialState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            if(inputHandler.PushHeldDown && pushCoroutine == null)
            {
                PushBoard();
            }
            else if(inputHandler.BrakeHeldDown)
            {
                ApplyBrakeForce();
            }

            if(!parentController.playerCamera.bMovingBackwards)
            {
                parentController.AlignWheels();
            }

            parentController.SmoothToGroundRotation(false, groundAdjustSmoothness, turnSpeed, groundedCondition);
        }

        public override void PhysicsTick(float dT)
        {
            UpdatePositionAndRotation(dT);

            if(cameraBackwards)
            {
                if(!cameraSwitched && movementRB.velocity.magnitude > 2f)
                {
                    parentController.OverrideCamera(parentController.backwardsCamera, false);
                    cameraSwitched = true;
                }
            }

            if(!cameraBackwards)
            {
                if(cameraSwitched)
                {
                    parentController.OverrideCamera(parentController.camBrain, false);
                    cameraSwitched = false;
                }
            }

            if(parentController.audioEmitter)
            {
                parentController.audioEmitter.SetParameter("Velocity", movementRB.velocity.magnitude / 15f);

                if(movementRB.velocity.magnitude < 2)
                {
                    parentController.audioEmitter.EventInstance.setVolume(movementRB.velocity.magnitude * 0.5f);
                }
                else
                {
                    moveSound.setVolume(1f);
                }
            }

            if(bConstantForce)
            {
                movementRB.AddForceAtPosition(playerTransform.forward * constantForce, movementRB.position + movementRB.centerOfMass, ForceMode.Force);
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Grounded");
            parentController.characterAnimator.SetBool("grounded", true);

            movementRB.drag = GroundedDrag;
            parentController.ModelRB.drag = GroundedDrag;

            //Making sure they can start a push when landing
            if(inputHandler.PushHeldDown)
            {
                PushBoard();
            }

            parentController.SmoothToGroundRotation(false, groundAdjustSmoothness, turnSpeed, groundedCondition);

            VFXPlayer.instance.PlayVFX(landingDust, parentController.transform.position - new Vector3(0, 0.45f, 0));
            FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerSounds/Land2", parentController.transform.position);

            parentController.audioEmitter.Play();
            parentController.OverrideCamera(parentController.camBrain, false);

            hasRan = true;
        }

        public override void OnStateExit()
        {
            StopJumpTimerCoroutine();
            StopPushTimerCoroutine();

            UnPressDown();

            parentController.characterAnimator.SetBool("grounded", false);
            parentController.audioEmitter.Stop();
            parentController.OverrideCamera(parentController.camBrain, false);

            hasRan = false;
        }

        #region Private Methods

        private void UpdatePositionAndRotation(float dT)
        {
            if(jumpCoroutine == null) 
            {
                //Depending on the difference of angle in the movement currently and the transform forward of the skateboard, apply more drag the wider the angle (maximum angle being 90 for drag)
                Vector3 forward = parentController.transform.forward;

                float dotAngle = Vector3.Dot(movementRB.velocity.normalized, forward);
                float absDotAngle = Mathf.Abs(dotAngle);

                //Moving almost directly backwards
                if(dotAngle < -0.985f)
                {
                    movementRB.velocity = movementRB.velocity.magnitude * -forward;
                    cameraBackwards = true;
                }
                //Must be moving forwards
                else
                {
                    movementRB.velocity = movementRB.velocity.magnitude * forward;
                    cameraBackwards = false;
                }

                // 0 means it is perpendicular, 1 means it's perfectly parallel
                if(absDotAngle < 0.99f)
                {
                    //movementRB.angularVelocity = Vector3.zero;

                    //And conserving it if need be
                    if(absDotAngle < 0.3265f)
                    {
                        movementRB.velocity = Vector3.zero;
                        movementRB.AddForce(playerTransform.forward.normalized * dT, ForceMode.Impulse);
                    }
                }
            }

            //With the hinge, this means that the rb wont just run away without the player
            //if(Vector3.Distance(playerTransform.position, movementRB.transform.position) > 0.65f)
            //{
            //    movementRB.transform.position = parentController.boardObject.transform.position + (playerTransform.forward * 0.281f);
            //}
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
                movementRB.centerOfMass += new Vector3(0, -0.15f, 0);
                bPressingDown = true;

                parentController.characterAnimator.SetFloat("crouchingFloat", 1);
            }
        }

        private void UnPressDown()
        {
            //There has to be a second to wait if there's a jump
            StartUnPressDownCoroutine();
        }

        private void Jump()
        {
            if(jumpCoroutine == null && hasRan)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/PlayerSounds/Jump", parentController.transform.position);
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

        private void StopUnPressDownCoroutine()
        {
            if(uncrouchCoroutine != null)
            {
                parentController.StopCoroutine(uncrouchCoroutine);
            }

            uncrouchCoroutine = null;
        }

        private void StartUnPressDownCoroutine()
        {
            uncrouchCoroutine = parentController.StartCoroutine(Co_UnCrouchTimer());
        }

        private IEnumerator Co_JumpTimer(bool isPressingDown)
        {
            //It's technically a new timer on top of the class in use
            jumpTimer = new Timer(jumpTimerDuration);

            //Pressing down makes jumping bigger
            float newjumpSpeed = /*isPressingDown ? jumpSpeed * 1.25f : */ jumpSpeed;

            yield return new WaitForFixedUpdate();
            jumpTimer.Tick(Time.fixedDeltaTime);

            movementRB.AddForce(parentController.transform.up * newjumpSpeed * 1000f);
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
                    parentController.characterAnimator.SetFloat("pushFloat", Mathf.Clamp01(pushDuringTimer.current_time));

                    //Pushing forward
                    Vector3 force = parentController.transform.forward * forwardSpeed * 250f * Time.fixedDeltaTime;

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

                    //Debug.Log("Push force" + force);

                    //Debug.Log(Time.deltaTime);
                    //Debug.Log(Time.fixedDeltaTime);

                    //Roughly Adjusting fixed update for normal update in testing (roughly)
                    movementRB.AddForce(force * 4f, ForceMode.Impulse);
                }
             
                //Tick each frame
                pushDuringTimer.Tick(Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            pushCoroutine = null;
        }

        //Uncrouching should take a little bit of time to register a second input (for jump boost)
        private IEnumerator Co_UnCrouchTimer()
        {
            for(float t = 0; t < 0.1f; t += Time.deltaTime)
            {
                float newVal = 0 - (1 / 0.1f * t);
                parentController.characterAnimator.SetFloat("crouchingFloat", newVal);
                yield return null;
            }

            if(bPressingDown)
            {
                movementRB.centerOfMass += new Vector3(0, 0.15f, 0);
                bPressingDown = false;
                parentController.characterAnimator.SetFloat("crouchingFloat", -1);
            }
        }

        #endregion
    }
}