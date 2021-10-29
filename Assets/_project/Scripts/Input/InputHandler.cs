////////////////////////////////////////////////////////////
// File: InputHandler.cs
// Author: Charles Carter, Matthew Mason
// Date Created: 05/10/21
// Last Edited By: Matthew Mason
// Date Last Edited: 28/10/21
// Brief: A script which handles the inputs passed from the input system
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;

namespace SleepyCat.Input
{
    /// <summary>
    /// A script which handles the inputs passed from the input system
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        #region Private Serialized Fields
        [SerializeField] [Tooltip("The PlayerInput to get the action events from")]
        private PlayerInput playerInput;
        #endregion

        #region Publicly Accessible Properties
        /// <summary>
        /// If the brake button is currently being held down
        /// </summary>
        public bool BrakeHeldDown { get; private set; } = false;
        /// <summary>
        /// If the events have been to the player inputs yet
        /// </summary>
        public bool MethodsBoundToPlayerEvents { get; private set; } = false;
        /// <summary>
        /// If the press down button is currently being held down
        /// </summary>
        public bool PressDownHeldDown { get; private set; } = false;
        /// <summary>
        /// If push button is currently being held down
        /// </summary>
        public bool PushHeldDown { get; private set; } = false;
        /// <summary>
        /// If the start grind key is held down
        /// </summary>
        public bool StartGrindHeld { get; private set; } = false;

        /// <summary>
        /// The axis value for the balance actions
        /// </summary>
        public float BalanceAxis { get; private set; } = 0f;
        /// <summary>
        /// The axis value for the turning actions
        /// </summary>
        public float TurningAxis { get; private set; } = 0f;
        #endregion

        #region Public Delegates
        /// <summary>
        /// Delegate for events that take in a value between -1 to 1 as a 1d axis value
        /// </summary>
        /// <param name="axisValue">a value between -1 to 1 as a 1d axis value</param>
        public delegate void OneDimensionalAxisAction(float axisValue);
        #endregion

        #region Public Events
        #region Break Action
        /// <summary>
        /// Called when the player stops pressing down on the skateboard action
        /// </summary>
        public event System.Action brakeEnded;
        /// <summary>
        /// Called when the player starts the pressing down on the skateboard action
        /// </summary>
        public event System.Action brakeStarted;
        /// <summary>
        /// Called every frame the player is holding the pressing down on the skateboard action
        /// </summary>
        public event System.Action breakUpdate;
        #endregion

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

        #region Start Grinding Action
        /// <summary>
        /// Called when the start grind input action has been ended
        /// </summary>
        public event System.Action startGrindEnded;
        /// <summary>
        /// Called when the start grind input action has been start
        /// </summary>
        public event System.Action startGrindStarted;
        /// <summary>
        /// Called when the start grind input action has been start
        /// </summary>
        public event System.Action startGrindUpdate;
        #endregion

        /// <summary>
        /// Called when the grinding jump action from the 
        /// </summary>
        public event System.Action grindingJumpUpActionPerformed;
        /// <summary>
        /// Called when the grounded jump input action has been performed
        /// </summary>
        public event System.Action groundedJumpUpPerformed;
        /// <summary>
        /// Called when the wall riding jump input action has been performed
        /// </summary>
        public event System.Action wallRidingJumpUpAction;


        /// <summary>
        /// Called every frame with the current value of the balance action axis
        /// </summary>
        public event OneDimensionalAxisAction balanceUpdate;
        /// <summary>
        /// Called every frame with the current value of the turning action axis
        /// </summary>
        public event OneDimensionalAxisAction turningUpdated;
        #endregion

        #region Unity Methods
        private void Update()
        {
            // To the PlayerInput events if not done so already
            if (!MethodsBoundToPlayerEvents)
            {
                // All actions need unique names even across all action maps
                MethodsBoundToPlayerEvents = true;

                BindGrindingActions();
                BindGroundedActions();
                BindAerialActions();
                BindWallRidingAction();
            }

            // Buttons held down events
            if (BrakeHeldDown)
            {
                if (breakUpdate != null)
                {
                    breakUpdate();
                }
            }
            if (PushHeldDown)
            {
                if (pushUpdate != null)
                {
                    pushUpdate();
                }
            }
            if (PressDownHeldDown)
            {
                if (pressDownUpdate != null)
                {
                    pressDownUpdate();
                }
            }

            // Axis Update events
            if (balanceUpdate != null)
            {
                balanceUpdate(BalanceAxis);
            }
            if (turningUpdated != null)
            {
                turningUpdated(TurningAxis);
            }
        }
        private void OnDisable()
        {
            UnbindAerialActions();
            UnbindGrindingActions();
            UnbindGroundedAction();
            UnbindWallRidingAction();
        }
        #endregion

        #region Private Methods
        #region Balance Action
        /// <summary>
        ///  Called when the player cancels the balance action to cancel out the axis value
        /// </summary>
        /// <param name="callbackContext">Input action call back context</param>
        private void BalanceAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            BalanceAxis = 0.0f;
        }

        /// <summary>
        ///  Called when the player performed the balance action to set the axis value
        /// </summary>
        /// <param name="callbackContext">Input action call back context</param>
        private void BalanceAction_Performed(InputAction.CallbackContext callbackContext)
        {
            BalanceAxis = callbackContext.ReadValue<float>();
        }
        #endregion

        #region Jumping Actions
        /// <summary>
        /// Called when the player performs the grinding jump action
        /// </summary>
        /// <param name="callbackContext">Grinding Jump Action's CallbackContext</param>
        private void GrindingJumpUpAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (grindingJumpUpActionPerformed != null)
            {
                grindingJumpUpActionPerformed();
            }
        }
        /// <summary>
        /// Called when the player performs the grounded jump action
        /// </summary>
        /// <param name="callbackContext">Jump Action's CallbackContext</param>
        private void GroundedJumpUpAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (groundedJumpUpPerformed != null)
            {
                groundedJumpUpPerformed();
            }
        }
        /// <summary>
        /// Called when the player performs the wall riding jump action
        /// </summary>
        /// <param name="callbackContext">Jump Action's CallbackContext</param>
        private void WallRidingJumpUpAction_Performed(InputAction.CallbackContext callbackContext)
        {
            if (wallRidingJumpUpAction != null)
            {
                wallRidingJumpUpAction();
            }
        }
        #endregion

        #region Press Down Action
        /// <summary>
        /// Called when the pressed down input action is performed
        /// </summary>
        /// <param name="callbackContext">Input action callback context</param>
        private void PressDownAction_Performed(InputAction.CallbackContext callbackContext)
        {
            PressDownHeldDown = true;
            if (pressDownStarted != null)
            {
                pressDownStarted();
            }
        }
        /// <summary>
        /// Called when the pressed down input action is performed
        /// </summary>
        /// <param name="callbackContext">Input action callback context</param>
        private void PressDownAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            PressDownHeldDown = false;
            if (pressDownEnded != null)
            {
                pressDownEnded();
            }
        }
        #endregion

        #region Push Action
        /// <summary>
        /// Called when the push action is performed
        /// </summary>
        /// <param name="callbackContext">The push action's CallbackContext</param>
        private void PushAction_Performed(InputAction.CallbackContext callbackContext)
        {
            PushHeldDown = true;
            if (pushStarted != null)
            {
                pushStarted();
            }
        }

        /// <summary>
        /// Called when the push action is performed
        /// </summary>
        /// <param name="callbackContext">The push action's CallbackContext</param>
        private void PushAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            PushHeldDown = false;
            if (pushEnded != null)
            {
                pushEnded();
            }
        }
        #endregion

        #region Brake Action
        /// <summary>
        /// Called when the brake action is canceled
        /// </summary>
        /// <param name="callbackContext">The brake action's CallbackContext</param>
        private void BrakeAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            BrakeHeldDown = false;
            if (brakeEnded != null)
            {
                brakeEnded();
            }
        }

        /// <summary>
        /// Called when the brake action is performed
        /// </summary>
        /// <param name="callbackContext">The brake action's CallbackContext</param>
        private void BrakeAction_Peformed(InputAction.CallbackContext callbackContext)
        {
            BrakeHeldDown = true;
            if (brakeStarted != null)
            {
                brakeStarted();
            }
        }
        #endregion

        #region Start Grind Action
        /// <summary>
        /// Called when the player performs the start grind action
        /// </summary>
        /// <param name="callbackContext">start grind action's CallbackContext</param>
        private void StartGrindAction_Performed(InputAction.CallbackContext callbackContext)
        {
            StartGrindHeld = true;
            if (startGrindStarted != null)
            {
                startGrindStarted();
            }
        }


        private void StartGrindAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            StartGrindHeld = false;
            if (startGrindEnded != null)
            {
                startGrindEnded();
            }
        }
        #endregion

        #region Turning Action
        /// <summary>
        /// Called when the player cancels the turn action to cancel out the turning axis value
        /// </summary>
        /// <param name="callbackContext">The turning action's CallbackContext</param>
        private void TurningAction_Canceled(InputAction.CallbackContext callbackContext)
        {
            TurningAxis = 0.0f;
        }

        /// <summary>
        /// Called when the player performs the turning action
        /// </summary>
        /// <param name="callbackContext">The turning action's CallbackContext</param>
        private void TurningAction_Peformed(InputAction.CallbackContext callbackContext)
        {
            TurningAxis = callbackContext.ReadValue<float>();
        }
        #endregion

        /// <summary>
        /// Bind to all the events to the aerial actions
        /// </summary>
        private void BindAerialActions()
        {
            playerInput.actions["Aerial_Turning"].performed += TurningAction_Peformed;
            playerInput.actions["Aerial_Turning"].canceled  += TurningAction_Canceled;

            playerInput.actions["Aerial_StartGrind"].performed += StartGrindAction_Performed;
            playerInput.actions["Aerial_StartGrind"].canceled += StartGrindAction_Canceled;
        }
        /// <summary>
        /// Bind to all the events to the grinding actions
        /// </summary>
        private void BindGrindingActions()
        {
            playerInput.actions["Grinding_Turning"].performed   += TurningAction_Peformed;
            playerInput.actions["Grinding_Turning"].canceled    += TurningAction_Canceled;

            playerInput.actions["Grinding_JumpUp"].performed += GrindingJumpUpAction_Performed;
        }
        /// <summary>
        /// Bind to all the events to the grounded actions
        /// </summary>
        private void BindGroundedActions()
        {
            playerInput.actions["Grounded_Push"].canceled   += PushAction_Canceled;
            playerInput.actions["Grounded_Push"].performed += PushAction_Performed;

            playerInput.actions["Grounded_PressDown"].canceled += PressDownAction_Canceled;
            playerInput.actions["Grounded_PressDown"].performed += PressDownAction_Performed;

            playerInput.actions["Grounded_JumpUp"].performed += GroundedJumpUpAction_Performed;

            playerInput.actions["Grounded_Balance"].performed += BalanceAction_Performed;
            playerInput.actions["Grounded_Balance"].canceled += BalanceAction_Canceled;

            playerInput.actions["Grounded_Brake"].performed += BrakeAction_Peformed;
            playerInput.actions["Grounded_Brake"].canceled += BrakeAction_Canceled;

            playerInput.actions["Grounded_Turning"].performed += TurningAction_Peformed;
            playerInput.actions["Grounded_Turning"].canceled += TurningAction_Canceled;

            playerInput.actions["Grounded_StartGrind"].performed += StartGrindAction_Performed;
        }
        /// <summary>
        /// Bind to all the events to the wall riding actions
        /// </summary>
        private void BindWallRidingAction()
        {
            playerInput.actions["WallRiding_JumpUp"].performed += WallRidingJumpUpAction_Performed;

            playerInput.actions["WallRiding_Turning"].performed += TurningAction_Peformed;
            playerInput.actions["WallRiding_Turning"].canceled += TurningAction_Canceled;
        }
        /// <summary>
        /// Unbind to all the events to the aerial actions
        /// </summary>
        private void UnbindAerialActions()
        {
            playerInput.actions["Aerial_Turning"].performed -= TurningAction_Peformed;
            playerInput.actions["Aerial_Turning"].canceled -= TurningAction_Canceled;

            playerInput.actions["Aerial_StartGrind"].performed -= StartGrindAction_Performed;
            playerInput.actions["Aerial_StartGrind"].canceled -= StartGrindAction_Canceled;
        }
        /// <summary>
        /// Unbind to all the events to the grinding actions
        /// </summary>
        private void UnbindGrindingActions()
        {
            playerInput.actions["Grinding_Turning"].performed -= TurningAction_Peformed;
            playerInput.actions["Grinding_Turning"].canceled -= TurningAction_Canceled;

            playerInput.actions["Grinding_JumpUp"].performed -= GrindingJumpUpAction_Performed;
        }
        /// <summary>
        /// Unbind to all the events to the grounded actions
        /// </summary>
        private void UnbindGroundedAction()
        {
            playerInput.actions["Grounded_Push"].canceled           -= PushAction_Canceled;
            playerInput.actions["Grounded_Push"].performed          -= PushAction_Performed;
                                                                    
            playerInput.actions["Grounded_PressDown"].canceled      -= PressDownAction_Canceled;
            playerInput.actions["Grounded_PressDown"].performed     -= PressDownAction_Performed;
                                                                    
            playerInput.actions["Grounded_JumpUp"].performed        -= GroundedJumpUpAction_Performed;
                                                                    
            playerInput.actions["Grounded_Balance"].performed       -= BalanceAction_Performed;
            playerInput.actions["Grounded_Balance"].canceled        -= BalanceAction_Canceled;
                                                                    
            playerInput.actions["Grounded_Brake"].performed         -= BrakeAction_Peformed;
            playerInput.actions["Grounded_Brake"].canceled          -= BrakeAction_Canceled;
                                                                    
            playerInput.actions["Grounded_Turning"].performed       -= TurningAction_Peformed;
            playerInput.actions["Grounded_Turning"].canceled        -= TurningAction_Canceled;
                                                                    
            playerInput.actions["Grounded_StartGrind"].performed    -= StartGrindAction_Performed;
        }
        /// <summary>
        /// Unbind to all the events to the wall riding actions
        /// </summary>
        private void UnbindWallRidingAction()
        {
            playerInput.actions["WallRiding_JumpUp"].performed -= WallRidingJumpUpAction_Performed;

            playerInput.actions["WallRiding_Turning"].performed -= TurningAction_Peformed;
            playerInput.actions["WallRiding_Turning"].canceled -= TurningAction_Canceled;
        }
        #endregion
    }
}

