////////////////////////////////////////////////////////////
// File: SphereCharacterController.cs
// Author: Charles Carter
// Date Created: 07/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 07/10/21
// Brief: A version of the character controller which is based on sphere physics
//////////////////////////////////////////////////////////// 

using SleepyCat.Utility.StateMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SleepyCat.Movement.Prototypes
{
    public class SphereCharacterController : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private GameObject goPlayerModel;
        [SerializeField]
        private GameObject goForwardAxis;
        [SerializeField]
        private GameObject goBackwardAxis;

        [SerializeField]
        private GameObject goRaycastPoint;
        [SerializeField]
        private Rigidbody rb;
        float initialDrag;
        Vector3 initalPos;
        Quaternion initialRot;

        [SerializeField]
        private float forwardSpeed = 8;
        [SerializeField]
        private float turnSpeed = 4;
        [SerializeField]
        private float backwardSpeed = 3;
        [SerializeField]
        private float jumpSpeed = 50;
        [SerializeField]
        private float SomeMaximumVelocity = 10f;

        [SerializeField]
        private float currentTurnInput = 0f;
        [SerializeField]
        private float additionalGravity = 10f;
        [SerializeField]
        private bool bAddAdditionalGravity = true;

        [SerializeField]
        private bool isGrounded = false;
        //The state machine that determines what the player can do and is doing in the current frame
        [SerializeField]
        private FiniteStateMachine CharacterStateMachine;
        [SerializeField]
        private PlayerCamera playerCamera;
        [SerializeField]
        private SphereCollider ballMovement;

        [SerializeField]
        private LayerMask mask;

        //This is public in case other systems need to know if the player is pushing.
        public Coroutine pushWaitCoroutine { get; private set; }
        public Coroutine pushDuringCoroutine { get; private set; }
        public Coroutine jumpCoroutine { get; private set; }

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

        [SerializeField]
        private bool bShowDriftVal = false;

        #endregion

        #region Public Methods

        //Debugging Options
        public void ResetBoard()
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.drag = initialDrag;

            rb.transform.rotation = initialRot;
            transform.rotation = initialRot;

            rb.transform.position = initalPos;    
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            CharacterStateMachine = new FiniteStateMachine();
        }

        private void Start()
        {
            goPlayerModel.transform.position = new Vector3(ballMovement.transform.position.x, ballMovement.transform.position.y - ballMovement.radius + 0.01f, ballMovement.transform.position.z);
            rb.transform.parent = null;
            initialDrag = rb.drag;

            Jump();

            initalPos = transform.position;
            initialRot = transform.rotation;
        }

        private void Update()
        {
            //CharacterStateMachine.RunMachine(Time.deltaTime);

            //Checking if anything is below it
            if (Physics.Raycast(goRaycastPoint.transform.position, -transform.up, out RaycastHit hit, 0.07f, ~mask, QueryTriggerInteraction.UseGlobal))
            {
                isGrounded = true;

                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(goRaycastPoint.transform.position, goRaycastPoint.transform.position + (-transform.up * 0.07f), Color.magenta);
                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }
            }
            else
            {
                isGrounded = false;
            }

            currentTurnInput = 0;

            if(Keyboard.current.aKey.isPressed)
            {
                if(rb.velocity.magnitude < 2)
                {
                    currentTurnInput = -1f;
                }
                else
                {
                    currentTurnInput -= 0.25f * rb.velocity.magnitude * 0.3f;
                }
            }

            if(Keyboard.current.dKey.isPressed)
            {
                if(rb.velocity.magnitude < 2)
                {
                    currentTurnInput = 1f;
                }
                else
                {
                    currentTurnInput += 0.25f * rb.velocity.magnitude * 0.3f;
                }
            }

            Mathf.Clamp(currentTurnInput, -1, 1);

            if(Keyboard.current.escapeKey.isPressed)
            {
                ResetBoard();
            }

            UpdatePositionAndRotation();
        }

        void FixedUpdate()
        {
            //CharacterStateMachine.RunPhysicsOnMachine(Time.deltaTime);

            if(bAddAdditionalGravity)
            {
                rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
            }

            GroundedMovement();
            AirMovement();
        }

        #endregion

        #region Private Methods

        private void UpdatePositionAndRotation()
        {
            //GameObject's heading
            float headingDeltaAngle = turnSpeed * 1000 * currentTurnInput * Time.deltaTime;
            Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
            Quaternion groundQuat = transform.rotation;

            //Getting the hit of the floor
            if (Physics.Raycast(goRaycastPoint.transform.position, -transform.up, out RaycastHit floorHit, 5f, ~mask, QueryTriggerInteraction.UseGlobal))
            {
                float smoothness = 12f;

                //apply heading rotation
                if (floorHit.normal != Vector3.zero)
                {
                    if (floorHit.distance > 0.25f)
                    {
                        smoothness = 2f;
                    }

                    groundQuat = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, floorHit.normal) * transform.rotation, Time.deltaTime * smoothness);
                }
            }

            transform.rotation = groundQuat;
            transform.rotation = transform.rotation * headingDelta;
            transform.position = rb.transform.position;

            //Depending on the difference of angle in the movement currently and the transform forward of the skateboard, apply more drag the wider the angle (maximum angle being 90 for drag)
            float initialSpeed = rb.velocity.magnitude;
            float dotAngle = Vector3.Dot(rb.velocity.normalized, transform.forward.normalized);
            dotAngle = Mathf.Abs(dotAngle);

            if (bShowDriftVal)
            {
                Debug.Log(dotAngle);
            }

            if (isGrounded && jumpCoroutine == null)
            {
                // 0 means it is perpendicular, 1 means it's perfectly parallel
                if (dotAngle < 0.99f)
                {
                    rb.AddForce(-rb.velocity * (1f + (1.05f - dotAngle)), ForceMode.Impulse);

                    if (dotAngle > 0.35f)
                    {
                        rb.AddForce(initialSpeed * (1f + turnSpeed) * transform.forward, ForceMode.Impulse);
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                        rb.Sleep();
                    }
                }
            }
        }

        private void GroundedMovement()
        {
            if(!isGrounded)
            {
                return;
            }

            if(playerCamera)
            {
                playerCamera.FollowRotation = true;
            }

            if (Keyboard.current.spaceKey.isPressed && !Keyboard.current.sKey.isPressed)
            {
                PushBoard();
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                ApplyBrakeForce();
            }

            if (Keyboard.current.ctrlKey.isPressed && jumpCoroutine == null)
            {
                Jump();
            }
        }

        private void AirMovement()
        {
            if (isGrounded)
            {
                return;
            }

            if(playerCamera)
            {
                playerCamera.FollowRotation = false;
            }

            //Need some way of making the skateboard feel more stable in the air and just generally nicer
            if(Keyboard.current.leftArrowKey.isPressed)
            {
                //Turn Left
                rb.transform.Rotate(new Vector3(0, 4f, 0));
            }

            if(Keyboard.current.rightArrowKey.isPressed)
            {
                //Turn Right
                rb.transform.Rotate(new Vector3(0, -4, 0));
            }
        }

        #region Controls Functions

        private void ApplyBrakeForce()
        {
            //Pushing backward as a constant force
            rb.AddForceAtPosition(-transform.forward * backwardSpeed, rb.position + rb.centerOfMass, ForceMode.Force);
        }

        private void Jump()
        {
            Debug.Log("Jumping");
            StartJumpTimer();
        }

        private void PushBoard()
        {
            if (pushTimer != null && pushTimer.isActive)
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
            pushWaitCoroutine = StartCoroutine(Co_BoardAfterPush());
        }

        private void StopPushTimerCoroutine()
        {
            if (pushWaitCoroutine != null)
            {
                StopCoroutine(pushWaitCoroutine);
            }

            pushWaitCoroutine = null;
        }

        private void StartPushDuringTimerCoroutine()
        {
            StopPushDuringTimerCoroutine();
            pushDuringCoroutine = StartCoroutine(Co_BoardDuringPush());
        }

        private void StopPushDuringTimerCoroutine()
        {
            if (pushDuringCoroutine != null)
            {
                StopCoroutine(pushDuringCoroutine);
            }

            pushDuringCoroutine = null;
        }

        private void StartJumpTimer()
        {
            StopJumpTimerCoroutine();
            jumpCoroutine = StartCoroutine(Co_JumpTimer());
        }

        private void StopJumpTimerCoroutine()
        {
            if(jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }

            jumpCoroutine = null;
        }


        private IEnumerator Co_JumpTimer()
        {
            //It's technically a new timer on top of the class in use
            jumpTimer = new Timer(jumpTimerDuration);

            if (isGrounded)
            {
                //rb.transform.up = Vector3.up;
                rb.AddForce(transform.up.normalized * jumpSpeed * 1000);
                Mathf.Clamp(rb.velocity.y, -99999, 5f);
            }

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
            while (pushTimer.isActive)
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
            while (pushDuringTimer.isActive)
            {
                if (isGrounded)
                {
                    //Pushing forward
                    Vector3 force = transform.forward * forwardSpeed * 1000 * Time.deltaTime;

                    if (rb.velocity.magnitude > 1)
                    {
                        //adjust the force depending on the current speed (15 being the amount that it can maximum be at if it's just pushing)
                        float max = 10f / rb.velocity.magnitude;
                        Mathf.Clamp(max, 0.05f, 1);

                        //Debug.Log(rb.velocity.magnitude + " " + max + " " + (1 - max).ToString());

                        // if the force is above 12, pushing shouldn't add anything
                        force *= max;
                    }
                    else
                    {
                        force *= 2f;
                    }

                    rb.AddForce(force, ForceMode.Impulse);
                }

                //Tick each frame
                pushDuringTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushDuringCoroutine = null;
        }

        #endregion
    }
}