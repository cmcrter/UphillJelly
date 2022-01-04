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
    public class HAerialState : State
    {
        [NonSerialized] private HisGroundBelow groundedCondition = null;
        [NonSerialized] private HisNextToWallRun wallRideCondition = null;
        [NonSerialized] private HisOnGrind grindCondition = null;

        private PlayerHingeMovementController parentController;
        private Rigidbody movementRB;
        private Rigidbody followRB;
        private PlayerInput pInput;
        private InputHandler inputHandler;

        private Transform playerTransform;

        [SerializeField]
        public float AerialDrag = 0.05f;
        [SerializeField]
        private float adjustGroundSmoothness = 4f;

        public HAerialState()
        {

        }

        public void InitialiseState(PlayerHingeMovementController controllerParent, Rigidbody playerRB, Rigidbody backWheelRB, HisGroundBelow groundBelow, HisNextToWallRun wallRunning, HisOnGrind grinding)
        {
            parentController = controllerParent;
            playerTransform = controllerParent.transform;
            movementRB = playerRB;
            followRB = backWheelRB;

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
            parentController.AlignWheels();
            parentController.SmoothToGroundRotation(true, adjustGroundSmoothness, 1f, groundedCondition);
        }

        public override void PhysicsTick(float dT)
        {
            //Giving a minimum distance before the turning is effective
            if(Vector3.Distance(groundedCondition.BackGroundHit.point, followRB.transform.position) > 5f && Vector3.Distance(groundedCondition.FrontGroundHit.point, movementRB.transform.position) > 5f)
            {
                //Need some way of making the skateboard feel more stable in the air and just generally nicer
                if(inputHandler.TurningAxis < 0)
                {
                    //Turn Left
                    parentController.transform.Rotate(new Vector3(0, 1f, 0));
                }

                if(inputHandler.TurningAxis > 0)
                {
                    //Turn Right
                    parentController.transform.Rotate(new Vector3(0, -1f, 0));
                }
            }
        }

        public override void OnStateEnter()
        {
            pInput.SwitchCurrentActionMap("Aerial");

            parentController.ResetWheelPos();
            parentController.AlignWheels();

            parentController.playerCamera.FollowRotation = false;

            movementRB.drag = AerialDrag;
            followRB.drag = AerialDrag;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            hasRan = false;
        }
    }
}