//================================================================================================================================================================================================================================================================================================================================================
// File: KillBox.cs
// Author: Matthew Mason, Charles Carter
// Date Created: 02/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 01/04/22
// Brief: A triggerable that makes the player enter the fail state when they enter the trigger (either falling off the board to just quickly teleporting back)
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;
using L7Games.Triggerables.CheckpointSystem;

namespace L7Games.Triggerables
{
    /// <summary>
    /// A triggerable that makes the player enter the fail state when they enter the trigger (either falling off the board to just quickly teleporting back)
    /// </summary>
    public class KillBox : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts
        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => KillPlayer(player);
        void ITriggerable.UnTrigger(PlayerController player) { }
        #endregion

        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The checkpoint manager used to control the reseting of the player")]
        private CheckpointManager checkpointManager;

        [SerializeField]
        private bool wipeoutOnHit = false;

        #endregion

        #region Private Methods
        /// <summary>
        /// Makes the player enter the fail stat
        /// </summary>
        /// <param name="player">The player controller that entered the kill box</param>
        private void KillPlayer(PlayerController player)
        {
            if(wipeoutOnHit)
            {
                player.CallOnWipeout(Vector3.zero);
            }
            else
            {
                checkpointManager.MovePlayerToTheirLastCheckPoint(player);
            }
        }
        #endregion
    }
}



