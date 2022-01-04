////////////////////////////////////////////////////////////
// File: Checkpoint.cs
// Author: Charles Carter, Matthew Mason
// Date Created: 04/10/21
// Last Edited By: Matthew Mason
// Date Last Edited: 02/11/21
// Brief: A triggerable which tells the system when a player has triggered it
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;

namespace L7Games.Triggerables.CheckpointSystem
{
    public class Checkpoint : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts
        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => CheckpointHit(player);
        void ITriggerable.UnTrigger(PlayerController player) => CheckpointUnHit(player);
        #endregion

        #region Private Serialized Fields
        [SerializeField] [Tooltip("The transform the player spawns at from this checkpoint")]
        private Transform spawnTransform;
        #endregion

        #region Private Variables
        /// <summary>
        /// A list of all the players that entered this checkpoint previously
        /// </summary>
        private List<PlayerController> playersEnteredPreviously;
        #endregion

        #region Public Properties
        /// <summary>
        /// The transform the player spawns at from this checkpoint
        /// </summary>
        public Transform SpawnTransform
        {
            get
            {
                if (spawnTransform == null)
                {
                    #if DEVELOPMENT_BUILD || UNITY_EDITOR
                    Debug.LogWarning("Spawn Transform was null when referenced in");
                    #endif
                    return transform;
                }

                return spawnTransform;
            }
        }
        #endregion

        #region Public Delegates
        /// <summary>
        /// Delegate for events caused by the player entering a checkpoint
        /// </summary>
        /// <param name="playerEntering">The player that entered the checkpoint</param>
        /// <param name="checkpointEntered">The player that exited the checkpoint</param>
        public delegate void PlayerEnteringCheckpointDelegate(PlayerController playerEntering, Checkpoint checkpointEntered);
        #endregion

        #region Public Events
        /// <summary>
        /// Event called when the player enters this checkpoint
        /// </summary>
        public event PlayerEnteringCheckpointDelegate PlayerEntered;
        #endregion

        #region Unity Methods
        void Awake()
        {
            playersEnteredPreviously = new List<PlayerController>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Called when a player hits the checkpoint trigger
        /// </summary>
        /// <param name="player">The player that hit the trigger</param>
        public void CheckpointHit(PlayerController player)
        {
            if (!playersEnteredPreviously.Contains(player))
            {
                playersEnteredPreviously.Add(player);
                // A new player has hit this checkpoint
                if (PlayerEntered != null)
                {
                    PlayerEntered(player, this);
                }
            }
        }

        /// <summary>
        /// Called when the player leaves the trigger
        /// </summary>
        /// <param name="player">The player that left the trigger</param>
        public void CheckpointUnHit(PlayerController player)
        {
            // A player has left this checkpoint
        }
        #endregion
    }
}