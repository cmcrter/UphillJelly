////////////////////////////////////////////////////////////
// File: CharacterController.cs
// Author: Charles Carter
// Date Created: 28/09/21
// Last Edited By: Charles Carter
// Date Last Edited: 30/09/21
// Brief: A quick script to register the inputs for the character
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    public class CharacterController : MonoBehaviour
    {
        #region Variables

        [Header("Central Necessary Variables")]
        //This is the rig
        [SerializeField]
        private GameObject goPlayerModel;
        //This is the center of mass
        [SerializeField]
        private GameObject goForcePoint;
        [SerializeField]
        private GameObject goFrontAxis;

        //This is the wheel collider we'll use to keep track of momentum, speed and weight
        [SerializeField]
        private WheelCollider backWheelRight;
        [SerializeField]
        private WheelCollider frontWheelRight;
        [SerializeField]
        private WheelCollider backWheelLeft;
        [SerializeField]
        private WheelCollider frontWheelLeft;

        [SerializeField]
        private Rigidbody rb;

        [Header("Customization Values")]
        [SerializeField]
        private float fPushWaitAmount = 0.5f;
        [SerializeField]
        private float pushForce = 10000f;
        [SerializeField]
        private float brakeForce = 1000f;
        [SerializeField]
        private float fMaxSkateboardSpeed = 500f;
        [SerializeField]
        private float fTurnSpeed = 150f;

        [Header("Debug Inspector Values")]
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

            goPlayerModel.transform.rotation = Quaternion.identity;
            goPlayerModel.transform.position = new Vector3(0, 0.2f, 0);
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            CharacterStateMachine = new FiniteStateMachine();
        }

        private void Start()
        {
            rb.centerOfMass = goForcePoint.transform.position;

            //GameObject goCOM = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //goCOM.transform.parent = goPlayerModel.transform;
            //goCOM.transform.position = rb.centerOfMass;
            //goCOM.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            //Otherwise the wheels go to sleep
            backWheelLeft.motorTorque = 0.00001f;
            backWheelRight.motorTorque = 0.00001f;
            frontWheelLeft.motorTorque = 0.00001f;
            frontWheelRight.motorTorque = 0.00001f;
        }

        private void Update() 
        {
            //Any debugging stuff needed
            if (Debug.isDebugBuild)
            {
                Debug.DrawRay(goPlayerModel.transform.position, -goPlayerModel.transform.up, Color.blue);
                Debug.DrawRay(rb.position + rb.centerOfMass, goForcePoint.transform.forward, Color.cyan);
                Debug.DrawLine(goFrontAxis.transform.position, goFrontAxis.transform.position + ( goFrontAxis.transform.forward.normalized * 0.1f), Color.green);

                if (Keyboard.current.escapeKey.isPressed)
                {
                    ResetBoard();
                }             
            }

            //Checking if anything is below it
            if (Physics.Raycast(goPlayerModel.transform.position, -goPlayerModel.transform.up, out RaycastHit hit, 1.0f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        private void FixedUpdate()
        {
            GroundMovement();
            AirMovement();
        }

        #endregion

        #region Private Methods

        private void GroundMovement()
        {
            if (!isGrounded)
            {
                return;
            }

            if (playerCamera)
            {
                playerCamera.FollowRotation = true;
            }

            RaycastHit hit;
            if (Physics.Raycast(goFrontAxis.transform.position, goFrontAxis.transform.forward, out hit, 0.1f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
            {

                //Angling for ramp to be smoother
                //if (Vector3.Angle(hit.normal, Vector3.up) > 5 && Vector3.Angle(hit.normal, Vector3.up) < 50)
                //{
                //    Quaternion rot = Quaternion.FromToRotation(goPlayerModel.transform.up, hit.normal) * Quaternion.Euler(goPlayerModel.transform.rotation.x, goPlayerModel.transform.rotation.y, goPlayerModel.transform.rotation.z);
                //    goPlayerModel.transform.rotation = Quaternion.Lerp(goPlayerModel.transform.rotation, rot, Time.deltaTime * 10f);
                //}

                //if (Mathf.Abs(Vector3.Angle(hit.normal, goPlayerModel.transform.up)) < 40)
                //{
                //    if (Debug.isDebugBuild)
                //    {
                //        Debug.Log("Adjusting for Ramp");
                //    }

                Quaternion fromRotation = goPlayerModel.transform.rotation;
                Quaternion toRotation = Quaternion.FromToRotation(goPlayerModel.transform.up, hit.normal) * goPlayerModel.transform.rotation;
                goPlayerModel.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, Time.deltaTime * 5f);

                //    //Applying a bit extra of a force for the player
                //    //rb.AddForceAtPosition(goForcePoint.transform.forward * ( pushForce * 0.1f ) * Time.deltaTime, rb.position + rb.centerOfMass, ForceMode.Impulse);
                //}
            }

            if (Keyboard.current.spaceKey.isPressed && !Keyboard.current.sKey.isPressed)
            {
                PushBoard();
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                ApplyBrakeForce();
            }

            if (Keyboard.current.aKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {
                    goPlayerModel.transform.Rotate(new Vector3(0, -2, 0));
                }
                else
                {
                    //Turn Left based on speed
                    rb.AddRelativeTorque(Vector3.up * -fTurnSpeed * /*( rb.velocity.magnitude / fMaxSkateboardSpeed ) * */ Time.deltaTime, ForceMode.Acceleration);
                }
            }

            if (Keyboard.current.dKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {
                    goPlayerModel.transform.Rotate(new Vector3(0, 2, 0));
                }
                else
                {
                    //Turn Right based on speed
                    rb.AddRelativeTorque(Vector3.up * fTurnSpeed * /*( rb.velocity.magnitude / fMaxSkateboardSpeed ) * */ Time.deltaTime, ForceMode.Acceleration);
                }
            }
        }

        private void AirMovement()
        {
            if (isGrounded)
            {
                return;
            }

            rb.angularVelocity = Vector3.zero;
            
            if (playerCamera)
            {
                playerCamera.FollowRotation = false;
            }

            //Need some way of making the skateboard feel more stable in the air and just generally nicer

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                //if (Debug.isDebugBuild)
                //{
                //    Debug.Log("Player spinning left");
                //}

                //Turn Left based on speed
                goPlayerModel.transform.Rotate(new Vector3(0, 4f, 0));
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                //if (Debug.isDebugBuild)
                //{
                //    Debug.Log("Player spinning right");
                //}

                //Turn Right based on speed
                //rb.AddRelativeTorque(Vector3.up * (fTurnSpeed * 0.005f) * Time.deltaTime, ForceMode.Impulse);
                goPlayerModel.transform.Rotate(new Vector3(0, -4f, 0));
            }
        }

        private void ApplyBrakeForce()
        {
            //Pushing backward as a constant force
            rb.AddForceAtPosition(-goForcePoint.transform.forward * brakeForce * Time.deltaTime, rb.position + rb.centerOfMass, ForceMode.Force);
        }

        private void PushBoard()
        {
            if (pushTimer != null && pushTimer.isActive)
            {
                return;
            }

            //Pushing forward
            rb.AddForceAtPosition(goForcePoint.transform.forward * pushForce * Time.deltaTime, rb.position + rb.centerOfMass, ForceMode.Acceleration);

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
            fTurnSpeed *= 0.25f;

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
            fTurnSpeed *= 4;
        }

        #endregion
    }
}