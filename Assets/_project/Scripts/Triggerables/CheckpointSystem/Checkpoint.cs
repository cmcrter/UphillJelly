////////////////////////////////////////////////////////////
// File: Checkpoint.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A triggerable which tells the system when a player has triggered it
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Movement;

namespace SleepyCat.Triggerables.CheckpointSystem
{
    public class Checkpoint : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => CheckpointHit();
        void ITriggerable.UnTrigger(PlayerController player) => CheckpointUnHit();

        #endregion

        #region Unity Methods
        void Start()
        {

        }

        void Update()
        {

        }
        #endregion

        #region Public Methods

        public void CheckpointHit()
        {
            //A player has hit this checkpoint
        }

        public void CheckpointUnHit()
        {
            //A player has left this checkpoint
        }

        #endregion

        #region Private Methods
        #endregion
    }
}