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

namespace SleepyCat.Movement
{
    public class CharacterController : MonoBehaviour
    {
        #region Variables

        [Header("Central Necessary Variables")]
        //This is the rig
        [SerializeField]
        private GameObject goPlayerModel;
        [SerializeField]
        private GameObject goForcePoint;

        //This is the sphere we'll use to keep track of momentum, speed and weight

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
        private float fMaxSkateboardSpeed = 500f;
        [SerializeField]
        private float fTurnSpeed = 150f;

        [Header("Debug Inspector Values")]
        [SerializeField]
        private bool isGrounded = false;

        //This is public in case other systems need to know if the player is pushing.
        public Coroutine pushWaitCoroutine { get; private set; }
        public Coroutine pushDuringCoroutine { get; private set; }

        [SerializeField]
        private Timer pushTimer;
        [SerializeField]
        private Timer pushDuringTimer;

        #endregion

        #region Public Fields

        #endregion

        #region Unity Methods

        private void Start()
        {
            rb.centerOfMass += new Vector3(0, -0.035f, 0);
            goForcePoint.transform.position = rb.position + rb.centerOfMass;

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
                Debug.DrawRay(goPlayerModel.transform.position, -goPlayerModel.transform.up, Color.blue, 0.05f);
                Debug.DrawRay(rb.position + rb.centerOfMass, goForcePoint.transform.forward, Color.cyan, 0.05f);

                if (Keyboard.current.escapeKey.isPressed)
                {
                    ResetBoard();
                }             
            }

            //Checking if anything is below it
            if (Physics.Raycast(goPlayerModel.transform.position, -goPlayerModel.transform.up, 1.0f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
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

            if (Keyboard.current.spaceKey.isPressed)
            {
                PushBoard();
            }

            if (Keyboard.current.aKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {

                }
                else
                {
                    //Turn Left based on speed
                    rb.AddRelativeTorque(Vector3.up * -fTurnSpeed * ( rb.velocity.magnitude / 100f ) * Time.deltaTime, ForceMode.Acceleration);
                }
            }

            if (Keyboard.current.dKey.isPressed)
            {
                if (rb.velocity.magnitude < 2)
                {

                }
                else
                {
                    //Turn Right based on speed
                    rb.AddRelativeTorque(Vector3.up * fTurnSpeed * ( rb.velocity.magnitude / 100f ) * Time.deltaTime, ForceMode.Acceleration);
                }
            }
        }

        private void AirMovement()
        {
            if (isGrounded)
            {
                return;
            }
        }

        private void PushBoard()
        {
            if (pushTimer != null && pushTimer.isActive)
            {
                return;
            }

            //Pushing forward and down (so it's a diagonal direction)

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


        //Debugging Options
        private void ResetBoard()
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            
            goPlayerModel.transform.rotation = Quaternion.identity;
            goPlayerModel.transform.position = new Vector3(0, 0.2f, 0);
        }

        #endregion
    }
}