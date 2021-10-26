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
        }

        public override State returnCurrentState()
        {
            if(groundedCondition.isConditionTrue())
            {
                return parentController.groundedState;
            }
            else if (grindCondition.isConditionTrue()) 
            {
                return parentController.grindingState;
            } 
            else if(wallRideCondition.isConditionTrue()) 
            {
                //return parentController.;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            RaycastHit hit = new RaycastHit();
            parentController.SmoothToGroundRotation(adjustGroundSmoothness, 0.1f, groundedCondition.GroundHit, hit);
        }

        public override void PhysicsTick(float dT)
        {
            //Need some way of making the skateboard feel more stable in the air and just generally nicer
            if(Keyboard.current.leftArrowKey.isPressed)
            {
                //Turn Left
                movementRB.transform.Rotate(new Vector3(0, 5f, 0));
            }

            if(Keyboard.current.rightArrowKey.isPressed)
            {
                //Turn Right
                movementRB.transform.Rotate(new Vector3(0, -5, 0));
            }
        }

        public override void OnStateEnter()
        {
            parentController.playerCamera.FollowRotation = false;
            movementRB.drag = AerialDrag;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            movementRB.angularVelocity = Vector3.zero;

            hasRan = false;
        }
    }
}