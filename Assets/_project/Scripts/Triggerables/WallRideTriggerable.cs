////////////////////////////////////////////////////////////
// File: WallRideTriggerable.cs
// Author: Charles Carter
// Date Created: 01/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 01/11/21
// Brief: The player is in the vicinity to attempt a wall ride
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Movement
{
    public class WallRideTriggerable : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        GameObject ITriggerable.ReturnGameObject() => gameObject;
        void ITriggerable.Trigger(PlayerController player) => PlayerEntered();
        void ITriggerable.UnTrigger(PlayerController player) => PlayerLeft();

        #endregion

        #region Variables


        #endregion

        #region Private Methods

        void PlayerEntered()
        {

        }

        void PlayerLeft()
        {

        }

        #endregion
    }

}