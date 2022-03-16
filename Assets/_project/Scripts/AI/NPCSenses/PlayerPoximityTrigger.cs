//===================================================================================================================================================================================================================================================================
//  Name:               PlayerProximityTrigger
//  Author:             Matthew Mason
//  Date Created:       15/03/2022
//  Date Last Modified: 15/03/2022
//  Brief:              A component used to detect players entering a trigger
//===================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;

public class PlayerPoximityTrigger : MonoBehaviour
{
    public event System.Action<PlayerHingeMovementController> playerEnteredTrigger;

    public event System.Action<PlayerHingeMovementController> playerExitedTrigger;

    public List<PlayerHingeMovementController> playersEntered;

    public List<KeyValuePair<PlayerHingeMovementController, Collider>> playerCollidersEntered;

    private void Start()
    {
        playersEntered = new List<PlayerHingeMovementController>();
        playerCollidersEntered = new List<KeyValuePair<PlayerHingeMovementController, Collider>>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHingeMovementController playerController = other.transform.root.GetComponentInChildren<PlayerHingeMovementController>();
        if (playerController != null)
        {
            // If the collider is a valid collider
            if (!playersEntered.Contains(playerController))
            {
                if (playerController != null)
                {
                    // Check if this is a new PlayerController
                    if (!CheckControllerAlreadyEntered(playerController))
                    {
                        playersEntered.Add(playerController);
                        if (playerEnteredTrigger != null)
                        {
                            playerEnteredTrigger(playerController);
                        }
                    }

                }
            }
            playerCollidersEntered.Add(new KeyValuePair<PlayerHingeMovementController, Collider>(playerController, other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHingeMovementController playerController = other.transform.root.GetComponentInChildren<PlayerHingeMovementController>();
        if (playerController != null)
        {
            // Try and get the found pair index from the collider
            if (TryGetPairEnteredFromCollider(other, out int foundIndex))
            {
                // Remove the found pair
                playerCollidersEntered.RemoveAt(foundIndex);
                // If the controller no longer exists in the pairs then the player has left the trigger
                if (!CheckControllerAlreadyEntered(playerController))
                {
                    if (playerExitedTrigger != null)
                    {
                        playerExitedTrigger(playerController);
                    }
                    playersEntered.Remove(playerController);
                }
            }
        }
    }

    private bool TryGetPairEnteredFromCollider(Collider colliderToCheckFor, out int foundControllColliderPairIndex)
    {
        foundControllColliderPairIndex = -1;
        for (int i = 0; i < playerCollidersEntered.Count; ++i)
        {
            if (playerCollidersEntered[i].Value == colliderToCheckFor)
            {
                foundControllColliderPairIndex = i;
                return true;
            }
        }
        return false;
    }

    private bool CheckControllerAlreadyEntered(PlayerHingeMovementController controllerToCheckFor)
    {
        for (int i = 0; i < playerCollidersEntered.Count; ++i)
        {
            if (playerCollidersEntered[i].Key == controllerToCheckFor)
            {
                return true;
            }
        }
        return false;
    }
}
