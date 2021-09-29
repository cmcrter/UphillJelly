////////////////////////////////////////////////////////////
// File: CharacterController.cs
// Author: Charles Carter
// Date Created: 28/09/21
// Last Edited By: 
// Date Last Edited:
// Brief: A quick script to register the inputs for the character
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;

namespace SleepyCat.Movement
{
    public class CharacterController : MonoBehaviour
    {
        #region Variables

        //This is the rig
        [SerializeField]
        GameObject goPlayerModel;
        //This is the sphere we'll use to keep track of momentum, speed and weight
        [SerializeField]
        private WheelCollider backWheelRight;
        [SerializeField]
        private WheelCollider frontWheelRight;
        [SerializeField]
        private WheelCollider backWheelLeft;
        [SerializeField]
        private WheelCollider frontWheelLeft;     

        private bool isGrounded = false;

        [SerializeField]
        float forwardWheelTorque = 1.25f;

        [SerializeField]
        Rigidbody rb;

        #endregion

        #region Public Fields

        #endregion

        #region Unity Methods

        private void Start()
        {
            backWheelLeft.motorTorque = 0.00001f;
            backWheelRight.motorTorque = 0.00001f;
            frontWheelRight.motorTorque = 0.00001f;
            frontWheelRight.motorTorque = 0.00001f;
        }

        private void Update() 
        {
            if (Physics.Raycast(goPlayerModel.transform.position, Vector3.down, 1.0f, ~gameObject.layer, QueryTriggerInteraction.UseGlobal))
            {
                isGrounded = true;
            }
        }

        private void FixedUpdate()
        {
            GroundMovement();
        }

        #endregion

        #region Private Methods

        private void GroundMovement()
        {
            if (Keyboard.current.spaceKey.isPressed && isGrounded)
            {
                Debug.Log("Forward Key Pressed");

                rb.AddRelativeForce(goPlayerModel.transform.forward * 5000 * Time.deltaTime, ForceMode.Impulse);
            }

            if (Keyboard.current.aKey.isPressed && isGrounded)
            {
                //Turn Left
            }
        }

        #endregion
    }
}