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
        [SerializeField]
        private float grindExitForce = 10f;
        [SerializeField]
        private Vector3 currentPlayerAim = Vector3.zero;
        [SerializeField]
        private float timeAlongGrind;

        #endregion

        #region Public Methods

        public GrindedState(PlayerMovementController controllerParent, Rigidbody playerRB, isOnGrind grind, GrindedState state)
        {
            parentController = controllerParent;
            movementRB = playerRB;
            onGrind = grind;

            grindSpeed = state.grindSpeed;
            grindExitForce = state.grindExitForce;
        }

        public override void OnStateEnter()
        {
            movementRB.isKinematic = true;
            parentController.bAddAdditionalGravity = false;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            timeAlongGrind = 0;
            movementRB.isKinematic = false;
            parentController.bAddAdditionalGravity = true;

            hasRan = false;
        }

        public override void Tick(float dT)
        {
            timeAlongGrind += dT * grindSpeed;

            if (onGrind.splineCurrentlyGrindingOn)
            {
                Vector3 dir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.1f);

                if (timeAlongGrind < 0.99f)
                {
                    movementRB.MovePosition(onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind) + new Vector3(0, 0.5f, 0));
                }
                else 
                {
                    JumpOff();
                }
            }
        }

        public override void PhysicsTick(float dT)
        {
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
            movementRB.isKinematic = false;
            movementRB.AddForce(((Vector3.up * 1.5f) + parentController.transform.forward) * grindExitForce, ForceMode.Impulse);
        }

        #endregion
    }
}
