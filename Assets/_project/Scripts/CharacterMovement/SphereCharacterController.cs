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
using System.Collections.Generic;
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

        [SerializeField]
        private float fPushWaitAmount = 0.5f;
        [SerializeField]
        private float forwardSpeed = 8;
        [SerializeField]
        private float turnSpeed = 4;
        [SerializeField]
        private float backwardSpeed = 3;

        [SerializeField]
        private float currentTurnInput = 0f;
        [SerializeField]
        private float additionalGravity = 10f;

        [SerializeField]
        private bool isGrounded = false;
        //The state machine that determines what the player can do and is doing in the current frame
        [SerializeField]
        FiniteStateMachine CharacterStateMachine;
        [SerializeField]
        PlayerCamera playerCamera;
        [SerializeField]
        SphereCollider ballMovement;

        [SerializeField]
        LayerMask mask;

        //This is public in case other systems need to know if the player is pushing.
        public Coroutine pushWaitCoroutine { get; private set; }
        public Coroutine pushDuringCoroutine { get; private set; }

        [SerializeField]
        private Timer pushTimer;
        [SerializeField]
        private Timer pushDuringTimer;

        #endregion

        #region Public Methods

        //Debugging Options
        public void ResetBoard()
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            rb.transform.rotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;

            rb.transform.position = new Vector3(0, ballMovement.radius + 0.1f, 0);          
        }

        #endregion

        #region Unity Methods

        void Start()
        {
            goPlayerModel.transform.position = new Vector3(0, ballMovement.transform.position.y - ballMovement.radius, 0);
            rb.transform.parent = null;
        }

        private void Update()
        {
            //Checking if anything is below it
            if (Physics.Raycast(goRaycastPoint.transform.position, -transform.up, out RaycastHit hit, 0.25f, ~mask, QueryTriggerInteraction.UseGlobal))
            {
                isGrounded = true;

                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }
            }
            else
            {
                isGrounded = false;
            }

            if (Keyboard.current.escapeKey.isPressed)
            {
                ResetBoard();
            }

            UpdatePositionAndRotation(hit.normal);
        }

        void FixedUpdate()
        {
            rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);

            if (isGrounded)
            {
                GroundedMovement();
            }
        }

        #endregion

        #region Private Methods

        private void UpdatePositionAndRotation(Vector3 GroundRotation)
        {
            //GameObject's heading
            float headingDeltaAngle = turnSpeed * 1000 * currentTurnInput * Time.deltaTime;
            Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
            Quaternion groundQuat = transform.rotation;

            //apply heading rotation
            if (GroundRotation != Vector3.zero)
            {
                groundQuat = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, GroundRotation) * transform.rotation, Time.deltaTime * 12f);
            }

            transform.rotation = groundQuat;
            transform.rotation = transform.rotation * headingDelta;
            transform.position = rb.transform.position;

            Vector3 rbVel = new Vector3(0, rb.velocity.y, 0);
            Vector3 TiltAndRoll =  new Vector3(0, transform.rotation.eulerAngles.y, 0);

            float driftAmount = Vector3.Angle(rbVel, TiltAndRoll);
            float initialDrag = rb.drag;
            if (driftAmount > 5f && currentTurnInput == 0 && (180 - driftAmount) > 2f || currentTurnInput != 0 && driftAmount > 90f && (180 - driftAmount) > 2f)
            {
                Debug.Log("Drift King: " + rb.velocity.normalized + "Model wants to go: " + transform.rotation.eulerAngles.normalized + " creating a difference of: " + driftAmount);
                rb.drag = 10f;
                rb.AddForceAtPosition(-rb.velocity.normalized * backwardSpeed * 1000 * Time.deltaTime, rb.position + rb.centerOfMass, ForceMode.Force);
                rb.drag = initialDrag;
            }
        }


        private void GroundedMovement()
        {
            currentTurnInput = 0;

            if (Keyboard.current.aKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {
                    currentTurnInput = -1f;
                }
                else
                {
                    currentTurnInput -= 0.25f * rb.velocity.magnitude * 0.25f;
                }
            }

            if (Keyboard.current.dKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {
                    currentTurnInput = 1f;
                }
                else
                {
                    currentTurnInput += 0.25f * rb.velocity.magnitude * 0.25f;
                }
            }

            Mathf.Clamp(currentTurnInput, -1, 1);

            if (Keyboard.current.spaceKey.isPressed && !Keyboard.current.sKey.isPressed)
            {
                PushBoard();
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                ApplyBrakeForce();
            }
        }

        private void ApplyBrakeForce()
        {
            //Pushing backward as a constant force
            rb.AddForceAtPosition(-transform.forward * backwardSpeed * 1000 * Time.deltaTime, rb.position + rb.centerOfMass, ForceMode.Force);
        }

        private void PushBoard()
        {
            if (pushTimer != null && pushTimer.isActive)
            {
                return;
            }

            //Pushing forward
            rb.AddForce(transform.forward * forwardSpeed * 1000 * Time.deltaTime, ForceMode.Acceleration);

            StartPushTimerCoroutine();
            StartPushDuringTimerCoroutine();
        }

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

        //Running the timer
        private IEnumerator Co_BoardAfterPush()
        {
            //It's technically a new timer on top of the class in use
            pushTimer = new Timer(fPushWaitAmount);

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
            turnSpeed *= 0.25f;

            //It's technically a new timer on top of the class in use
            pushDuringTimer = new Timer(0.25f);

            //Whilst it has time left
            while (pushDuringTimer.isActive)
            {
                //Tick each frame
                pushDuringTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushDuringCoroutine = null;
            turnSpeed *= 4;
        }

        #endregion
    }
}