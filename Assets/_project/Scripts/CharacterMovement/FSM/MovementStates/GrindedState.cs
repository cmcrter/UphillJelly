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
        Transform grindVisualiser;
        [SerializeField]
        private float grindSpeed = 1;
        [SerializeField]
        private float grindExitForce = 10f;
        [SerializeField]
        private Vector3 currentPlayerAim = Vector3.zero;
        [SerializeField]
        private Vector3 currentSplineDir;
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
            grindVisualiser = state.grindVisualiser;
        }

        public override void OnStateEnter()
        {
            if (parentController.playerCamera)
            {
                parentController.playerCamera.FollowRotation = false;
            }

            movementRB.transform.position = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(0) + new Vector3(0, 0.4f, 0);
            parentController.transform.position = movementRB.transform.position;

            Vector3 dir = onGrind.splineCurrentlyGrindingOn.GetDirection(0, 0.1f);
            dir = new Vector3(0, dir.y, dir.z);
            parentController.transform.rotation = Quaternion.FromToRotation(parentController.transform.rotation.eulerAngles, dir);

            movementRB.isKinematic = true;

            hasRan = true;
        }

        public override void OnStateExit()
        {
            if (parentController.playerCamera) 
            {
                parentController.playerCamera.FollowRotation = true;
            }

            timeAlongGrind = 0;

            hasRan = false;
        }

        public override void Tick(float dT)
        {
            timeAlongGrind += dT * grindSpeed;

            if (onGrind.splineCurrentlyGrindingOn)
            {
                currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.1f);
                Vector3 pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);

                if (timeAlongGrind < 0.99f)
                {
                    movementRB.MovePosition(Vector3.Lerp(movementRB.position, pos + new Vector3(0, 0.4f, 0), timeAlongGrind));
                    grindVisualiser.position = pos;
                }
                else 
                {
                    parentController.transform.rotation = Quaternion.Euler(0, currentSplineDir.y + 90, currentSplineDir.z);
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
