//================================================================================================================================================================================================================================================================================================================================================================
// File:                CheckpointManager.cs
// Author:              Matthew Mason
// Date Created:        02/11/2021
// Last Edited By:      Matthew Mason
// Date Last Edited:    02/11/2021
// Brief:               A script in charge of managing the checkpoints and the player entry of them
//================================================================================================================================================================================================================================================================================================================================================================

using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;

using UnityEngine.InputSystem;

namespace L7Games.Triggerables.CheckpointSystem
{
    /// <summary>
    /// A script in charge of managing the checkpoints and the player entry of them
    /// </summary>
    public class CheckpointManager : MonoBehaviour
    {
        #region Private Serialized Fields
        [SerializeField] 
        [Tooltip("The checkpoints this is managing")]
        private Checkpoint[] managedCheckpoints;

        [SerializeField]
        [Tooltip("The checkpoint the player will spawn at if no other one is found")]
        private Checkpoint defaultCheckpoint;
        #endregion

        #region Private Variables
        /// <summary>
        /// The checkpoints that the each player should re spawn at
        /// </summary>
        private Dictionary<PlayerController, Checkpoint> playersCurrentCheckpoints;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            playersCurrentCheckpoints = new Dictionary<PlayerController, Checkpoint>();
        }

        private void OnEnable()
        {
            for (int i = 0; i < managedCheckpoints.Length; ++i)
            {
                managedCheckpoints[i].PlayerEntered += RegisterPlayersCurrentCheckpoint;
            }
        }

        private void Start()
        {
            if (defaultCheckpoint != null)
            {
                if (playersCurrentCheckpoints == null)
                {
                    RegisterAllPlayers();
                }
                foreach (PlayerController player in FindObjectsOfType<PlayerController>())
                {
                    playersCurrentCheckpoints.Add(player, defaultCheckpoint);
                }
            }
            else
            {
                Debug.LogError("defaultCheckpoint not assigned to checkpoint manager, cannot assign", this);
            }

        }

        void Update()
        {
            if (Keyboard.current.commaKey.wasPressedThisFrame)
            {
                foreach (PlayerController playerController in playersCurrentCheckpoints.Keys)
                {
                    MovePlayerToTheirLastCheckPoint(playerController);
                }

            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Called to move a given the player back to their last checkpoint
        /// </summary>
        /// <param name="playerToMove">The player that should be moved to their last checkpoint</param>
        public void MovePlayerToTheirLastCheckPoint(PlayerController playerToMove)
        {
            // Get the transform to move the player to
            if (playersCurrentCheckpoints.TryGetValue(playerToMove, out Checkpoint playerLastCheckpoint))
            {
                Transform spawnTransform = playerLastCheckpoint.SpawnTransform;
                playerToMove.ResetPlayer(spawnTransform);
            }
            else
            {
                Transform spawnTransform = defaultCheckpoint.SpawnTransform;
                playerToMove.ResetPlayer(spawnTransform);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets a given player's last entered checkpoint to be the given one
        /// </summary>
        /// <param name="playerEntering">The player that just enter the checkpoint</param>
        /// <param name="checkpointEntered">The checkpoint just being entered</param>
        private void RegisterPlayersCurrentCheckpoint(PlayerController playerEntering, Checkpoint checkpointEntered)
        {
            playersCurrentCheckpoints[playerEntering] = checkpointEntered;
        }

        private void RegisterAllPlayers()
        {
            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                playersCurrentCheckpoints.Add(player, defaultCheckpoint);
            }
        }
        #endregion
    }
}


