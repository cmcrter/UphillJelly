////////////////////////////////////////////////////////////
// File: InputHandler.cs
// Author: Charles Carter, Matthew Mason
// Date Created: 05/10/21
// Last Edited By: Matthew Mason
// Date Last Edited: 05/10/21
// Brief: A script which handles the inputs passed from the input system
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;

namespace SleepyCat.Input
{
    public class InputHandler : MonoBehaviour
    {
        #region Public Fields

        #endregion

        #region Private Serialized Fields
        private bool eventsBound = false;
        private bool pushHeldDown = false;
        private bool pressDownHeldDown = false;

        [SerializeField]
        private PlayerInput playerInput;
        #endregion

        #region Public Delegates 
        #endregion

        #region Public Events
        #region Press Down Action
        /// <summary>
        /// Called when the player stops pressing down on the skateboard action
        /// </summary>
        public event System.Action pressDownEnded;
        /// <summary>
        /// Called when the player starts the pressing down on the skateboard action
        /// </summary>
        public event System.Action pressDownStarted;
        /// <summary>
        /// Called every frame the player is holding the pressing down on the skateboard action
        /// </summary>
        public event System.Action pressDownUpdate;
        #endregion

        #region Push Action
        /// <summary>
        /// Called when the player stops pushing
        /// </summary>
        public event System.Action pushEnded;
        /// <summary>
        /// Called when the player starts the pushing action
        /// </summary>
        public event System.Action pushStarted;
        /// <summary>
        /// Called every frame the player is holding the push action
        /// </summary>
        public event System.Action pushUpdate;
        #endregion

        public event System.Action jumpUpPerformed;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (!eventsBound)
            {
                eventsBound = true;
                playerInput.actions["Push"].canceled        += PushAction_Canceled;
                playerInput.actions["Push"].performed       += PushAction_Performed;

                playerInput.actions["PressDown"].canceled   += PressDownAction_Canceled;
                playerInput.actions["PressDown"].performed  += PressDownAction_Performed;

                playerInput.actions["JumpUp"].performed     += JumpUpAction_Performed;

                playerInput.actions["Balance"].performed    += BalanceAction_Performed;
            }

            if (pushUpdate != null)
            {
                pushUpdate();
            }
        }

        private void BalanceAction_Performed(InputAction.CallbackContext callbackContext)
        {
            Debug.Log(callbackContext.ReadValue<float>()); // Todo: Test with controller
        }


        #endregion

        #region Private Methods
        #region Press Down Action
        private void PressDownAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (!pressDownHeldDown)
            {
                pressDownHeldDown = true;
                if (pushStarted != null)
                {
                    pushStarted();
                }
            }
        }

        private void PressDownAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            if (pushHeldDown)
            {
                pushHeldDown = false;
                if (pushEnded != null)
                {
                    pushEnded();
                }
            }
        }
        #endregion

        #region Push Action
        private void PushAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (!pushHeldDown)
            {
                pushHeldDown = true;
                if (pushStarted != null)
                {
                    pushStarted();
                }
            }
        }

        private void PushAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            if (pushHeldDown)
            {
                pushHeldDown = false;
                if (pushEnded != null)
                {
                    pushEnded();
                }
            }
        }
        #endregion

        private void JumpUpAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (jumpUpPerformed != null)
            {
                jumpUpPerformed();
            }
        }
        #endregion
    }
}

