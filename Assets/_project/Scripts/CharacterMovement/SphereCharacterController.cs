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

            rb.transform.position = new Vector3(0, 0.2f, 0);
        }

        #endregion

        #region Unity Methods

        void Start()
        {
            rb.transform.parent = null;
        }

        private void Update()
        {
            //Checking if anything is below it
            if (Physics.Raycast(goRaycastPoint.transform.position, Vector3.down, out RaycastHit hit, 0.25f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
            {
                isGrounded = true;

                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(transform.position, transform.position + ( -transform.up * 1f ), Color.blue);
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

            //GameObject's heading
            float headingDeltaAngle = turnSpeed * 1000 * currentTurnInput * Time.deltaTime;
            Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
            //align with surface normal
            if (hit.transform != null)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.FromToRotation(transform.up, hit.normal.normalized) * transform.rotation), Time.deltaTime * 10f);
            }

            //apply heading rotation
            transform.rotation = transform.rotation * headingDelta;
            transform.position = rb.transform.position;
        }

        void FixedUpdate()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
                GroundedMovement();
            }

        }

        #endregion

        #region Private Methods



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
                    currentTurnInput = -0.5f;
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
                    currentTurnInput = 0.5f;
                }
            }

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