////////////////////////////////////////////////////////////
// File: isNextToWallRun.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: The condition to meet for the player to start wall running
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using SleepyCat.Triggerables;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isNextToWallRun : Condition
    {
        #region Variables

        public WallRideTriggerable currentWallRide;
        public float dotProductWithWall;

        private PlayerMovementController parentController;
        private Rigidbody movementRB;
        #endregion

        #region Public Methods

        public void InitialiseCondition(PlayerMovementController movementController, Rigidbody playerRb)
        {
            parentController = movementController;
            movementRB = playerRb;
            dotProductWithWall = 0;
        }

        public override bool isConditionTrue()
        {
            return (currentWallRide != null) && (dotProductWithWall > 0.8f || dotProductWithWall < -0.8f);
        }

        public void CheckWall(WallRideTriggerable wallRide)
        {
            currentWallRide = wallRide;

            //Getting the dot product with the wall to see if it's grindable
            dotProductWithWall = Vector3.Dot(parentController.transform.forward, wallRide.transform.right);
        }

        public void LeftWall(WallRideTriggerable wallRide)
        {
            currentWallRide = null;
            dotProductWithWall = 0f;
        }

        #endregion
    }
}