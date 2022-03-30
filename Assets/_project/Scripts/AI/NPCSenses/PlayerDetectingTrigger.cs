//===================================================================================================================================================================================================================================================================
//  Name:               PlayerDetectingTrigger
//  Author:             Matthew Mason
//  Date Created:       15/03/2022
//  Date Last Modified: 15/03/2022
//  Brief:              A component used to detect players entering a trigger
//===================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;
using L7Games;

/// <summary>
/// A Simple Triggerable used to detect a player entering the trigger and send up events for them entering and leaving and 
/// </summary>
public class PlayerDetectingTrigger : MonoBehaviour, ITriggerable
{
    /// <summary>
    /// Called when the player enters the trigger
    /// </summary>
    public event System.Action<PlayerController> playerEnteredTrigger;

    /// <summary>
    /// Called when the player exits the trigger
    /// </summary>
    public event System.Action<PlayerController> playerExitedTrigger;

    public GameObject ReturnGameObject()
    {
        return transform.gameObject;
    }

    public void Trigger(PlayerController player)
    {
        if (playerEnteredTrigger != null)
        {
            playerEnteredTrigger(player);
        }
    }

    public void UnTrigger(PlayerController player)
    {
        if (playerExitedTrigger != null)
        {
            playerExitedTrigger(player);
        }
    }
}
