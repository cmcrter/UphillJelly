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
        private WheelCollider backWheel;
        [SerializeField]
        private WheelCollider frontWheel;

        private bool isGrounded = false;

        [SerializeField]
        float forwardWheelTorque = 1.25f;            

        #endregion

        #region Public Fields

        #endregion

        #region Unity Methods

        private void Start()
        {

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

                backWheel.motorTorque += forwardWheelTorque;
                frontWheel.motorTorque += forwardWheelTorque;
            }

            goPlayerModel.transform.position = frontWheel.transform.position + ((backWheel.transform.position - frontWheel.transform.position) / 2);
            goPlayerModel.transform.position = new Vector3(goPlayerModel.transform.position.x, goPlayerModel.transform.position.y + backWheel.radius, goPlayerModel.transform.position.z);
        }

        #endregion
    }
}