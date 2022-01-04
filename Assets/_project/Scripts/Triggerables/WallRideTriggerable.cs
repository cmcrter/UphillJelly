////////////////////////////////////////////////////////////
// File: WallRideTriggerable.cs
// Author: Charles Carter
// Date Created: 01/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 01/11/21
// Brief: The player is in the vicinity to attempt a wall ride
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Movement;

namespace L7Games.Triggerables
{
    public class WallRideTriggerable : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        GameObject ITriggerable.ReturnGameObject() => gameObject;
        void ITriggerable.Trigger(PlayerController player) => PlayerEntered(player);
        void ITriggerable.UnTrigger(PlayerController player) => PlayerLeft(player);

        #endregion

        #region Variables

        [SerializeField]
        private Transform wallRideWall;
        [SerializeField]
        private float CoyoteTime;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            wallRideWall = wallRideWall ?? transform;
        }

        #endregion

        #region Private Methods

        //Letting the player know that they entered a wall run space
        void PlayerEntered(PlayerController player)
        {
            player.AddWallRide(this);
        }

        //Letting the player know that they exited a wall run space
        void PlayerLeft(PlayerController player)
        {
            player.RemoveWallRide(this);
        }

        #endregion
    }

}