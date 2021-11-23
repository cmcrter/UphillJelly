////////////////////////////////////////////////////////////
// File: AerialState.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: The state the character is in when they're in the air
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Input;

namespace SleepyCat.Movement
{
    [Serializable]
    public class AerialState : State
    {
        [NonSerialized] private isGroundBelow groundedCondition = null;
        [NonSerialized] private isNextToWallRun wallRideCondition = null;
        [NonSerialized] private isOnGrind grindCondition = null;

        private PlayerMovementController parentController;
        private Rigidbody movementRB;
        private PlayerInput pInput;
        private InputHandler inputHandler;

        [SerializeField]
        public float AerialDrag = 0.05f;
        [SerializeField]
        private float adjustGroundSmoothness = 2f;

        public AerialState()
        {

        }

        public void InitialiseState(PlayerMovementController controllerParent, Rigidbody playerRB, isGroundBelow groundBelow, isNextToWallRun wallRunning, isOnGrind grinding)
        {
            parentController = controllerParent;
            movementRB = playerRB;

            grindCondition = grinding;
            groundedCondition = groundBelow;
            wallRideCondition = wallRunning;

            pInput = controllerParent.input;
            inputHandler = controllerParent.inputHandler;
        }

        public void RegisterInputs()
        {
            //Register functions
        }

        public void UnRegisterInputs()
        {
            //Unregister functions
        }

        public override State returnCurrentState()
        {
            if(groundedCondition.isConditionTrue())
            {
                return parentController.groundedState;
            }

            if (grindCondition.isConditionTrue()) 
            {
                return parentController.grindingState;
            } 

            if(wallRideCondition.isConditionTrue()) 
            {
                return parentController.wallRideState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            parentController.SmoothToGroundRotation(true, adjustGroundSmoothness, 0f, groundedCondition);
        }

        public override void PhysicsTick(float dT)
        {
            //Need some way of making the skateboard feel more stable in the air and just generally nicer
            if(inputHandler.TurningAxis < 0)
            {
                //Turn Left
                movementRB.transform.Rotate(new Vector3(0, 5f, 0));
            }

            if(inputHandler.TurningAxis > 0)
            {
                //Turn Right
                movementRB.transform.Rotate(new Vector3(0, -5, 0));
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Aerial");

            parentController.playerCamera.FollowRotation = false;
            movementRB.drag = AerialDrag;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            hasRan = false;
        }
    }
}