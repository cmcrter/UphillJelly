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
        private float fMaxSkateboardSpeed = 50000f;
        [SerializeField]
        private float fTurnSpeed = 100f;

        [Header("Debug Inspector Values")]
        [SerializeField]
        private bool isGrounded = false;

        //This is public in case other systems need to know if the player is pushing.
        public Coroutine pushCoroutine { get; private set; }        

        [SerializeField]
        private Timer pushTimer;

        #endregion

        #region Public Fields

        #endregion

        #region Unity Methods

        private void Start()
        {
            //Otherwise the wheels go to sleep
            backWheelLeft.motorTorque = 0.00001f;
            backWheelRight.motorTorque = 0.00001f;
            frontWheelLeft.motorTorque = 0.00001f;
            frontWheelRight.motorTorque = 0.00001f;
        }

        private void Update() 
        {
            //Checking if anything is below it
            if (Physics.Raycast(goPlayerModel.transform.position, Vector3.down, 1.0f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
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

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                PushBoard();
            }

            float turn = fTurnSpeed * Time.deltaTime * (rb.velocity.magnitude / fMaxSkateboardSpeed);

            if (Keyboard.current.aKey.isPressed)
            {
                turn *= -1;

                Vector3 force = transform.right * turn;
                rb.AddForceAtPosition(force, goPlayerModel.transform.position);

                //Turn Left
                //goPlayerModel.transform.Rotate(new Vector3(0, 2, 0));
            }

            if (Keyboard.current.dKey.isPressed)
            {
                Vector3 force = transform.right * turn;
                rb.AddForceAtPosition(force, goPlayerModel.transform.position);

                //Turn Right
                //goPlayerModel.transform.Rotate(new Vector3(0, -2, 0));
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

            StartPushTimerCoroutine();
        }


        private void StartPushTimerCoroutine()
        {
            StopPushTimerCoroutine();
            pushCoroutine = StartCoroutine(Co_BoardPush());
        }

        private void StopPushTimerCoroutine()
        {
            if (pushCoroutine != null)
            {
                StopCoroutine(pushCoroutine);
            }

            pushCoroutine = null;
        }

        //Running the timer
        private IEnumerator Co_BoardPush()
        {
            rb.AddRelativeForce(goPlayerModel.transform.forward * pushForce * Time.deltaTime, ForceMode.Acceleration);

            //It's technically a new timer on top of the class in use
            pushTimer = new Timer(fPushWaitAmount);

            //Whilst it has time left
            while (pushTimer.isActive)
            {
                //Tick each frame
                pushTimer.Tick(Time.deltaTime);
                yield return null;
            }

            pushCoroutine = null;
        }

        #endregion
    }
}