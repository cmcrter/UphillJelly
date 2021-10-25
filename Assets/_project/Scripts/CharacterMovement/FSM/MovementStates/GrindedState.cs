////////////////////////////////////////////////////////////
// File: GrindedState.cs
// Author: Charles Carter
// Date Created: 25/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 25/10/21
// Brief: The player state for when the player is grinding
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement 
{
    [Serializable]
    public class GrindedState : State
    {
        #region Variables

        private PlayerMovementController parentController;
        private Rigidbody movementRB;
        private isOnGrind onGrind;

        [SerializeField]
        private float grindSpeed = 1;
        private float timeAlongGrind;

        #endregion

        #region Public Methods

        public GrindedState(PlayerMovementController controllerParent, Rigidbody playerRB, isOnGrind grind, GrindedState state)
        {
            parentController = controllerParent;
            movementRB = playerRB;
            onGrind = grind;

            grindSpeed = state.grindSpeed;
        }

        public override void OnStateEnter()
        {
            movementRB.velocity = Vector3.zero;
            movementRB.useGravity = false;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            timeAlongGrind = 0;
            movementRB.useGravity = true;

            hasRan = false;
        }

        public override void Tick(float dT)
        {
            timeAlongGrind += dT * grindSpeed;

            if (timeAlongGrind >= 1)
            {
                JumpOff();
            }
        }

        public override void PhysicsTick(float dT)
        {
            movementRB.MovePosition(onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, 0.5f, 0));
            parentController.transform.position = movementRB.transform.position;
        }

        public override State returnCurrentState()
        {
            if (!onGrind.isConditionTrue())
            {
                return parentController.aerialState;
            }

            return this;
        }

        #endregion

        #region Private Methods

        private void JumpOff()
        {
            movementRB.AddForce((Vector3.up + parentController.transform.forward) * 100, ForceMode.Impulse);
        }

        #endregion
    }
}
