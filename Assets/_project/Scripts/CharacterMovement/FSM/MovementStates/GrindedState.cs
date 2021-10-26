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
        [NonSerialized] private isOnGrind onGrind = null;

        [SerializeField]
        Transform grindVisualiser;
        [SerializeField]
        private Vector3 currentPlayerAim = Vector3.zero;
        [SerializeField]
        private Vector3 currentSplineDir;
        [SerializeField]
        private Vector3 pos;

        private float PotentialLengthOfGrind = 1f;
        [SerializeField]
        private float timeAlongGrind;

        [SerializeField]
        float jumpCoroutineDuration;
        Coroutine jumpCoroutine;
        [SerializeField]
        float autoJumpCoroutineDuration;
        Coroutine autoJumpCoroutine;

        #endregion

        #region Public Methods

        public GrindedState()
        {

        }

        public void InitialiseState(PlayerMovementController controllerParent, Rigidbody playerRB, isOnGrind grind)
        {
            parentController = controllerParent;
            movementRB = playerRB;
            onGrind = grind;
            grind.SetGrindState(this);
        }

        public override void OnStateEnter()
        {
            if (parentController.playerCamera)
            {
                parentController.playerCamera.FollowRotation = false;
            }

            movementRB.transform.position = onGrind.splineCurrentlyGrindingOn.GetClosestPointOnSpline(parentController.transform.position, out timeAlongGrind) + new Vector3(0, 0.41f, 0);
            parentController.transform.position = movementRB.transform.position;

            PotentialLengthOfGrind = onGrind.splineCurrentlyGrindingOn.GetTotalLength();
            currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(0f, 0.1f);

            parentController.transform.forward = Vector3.Cross(currentSplineDir, Vector3.up);

            movementRB.isKinematic = true;
            hasRan = true;
        }

        public override void OnStateExit()
        {
            if (parentController.playerCamera) 
            {
                parentController.playerCamera.FollowRotation = true;
            }

            PotentialLengthOfGrind = 1f;
            timeAlongGrind = 0;

            hasRan = false;
        }

        public override void Tick(float dT)
        {
            if(onGrind.grindDetails != null && onGrind.splineCurrentlyGrindingOn)
            {
                timeAlongGrind += (dT * onGrind.grindDetails.DuringGrindForce) / PotentialLengthOfGrind;

                if(timeAlongGrind < 1f)
                {
                    currentSplineDir = onGrind.splineCurrentlyGrindingOn.GetDirection(timeAlongGrind, 0.1f);
                    pos = onGrind.splineCurrentlyGrindingOn.GetPointAtTime(timeAlongGrind);
                    grindVisualiser.position = pos;

                    movementRB.MovePosition(Vector3.Lerp(movementRB.position, pos + new Vector3(0, 0.41f, 0), timeAlongGrind));
                    parentController.transform.forward = Vector3.Cross(currentSplineDir, Vector3.up);
                }
                else
                {
                    parentController.transform.forward = currentSplineDir;
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

        public bool isRailDone()
        {
            return timeAlongGrind > 1f && !movementRB.isKinematic;
        }

        #endregion

        #region Private Methods

        private void JumpOff()
        {
            movementRB.isKinematic = false;
            movementRB.AddForce(((Vector3.up * 1.5f) + parentController.transform.forward) * onGrind.grindDetails.ExitGrindForce, ForceMode.Impulse);
        }

        #endregion
    }
}
