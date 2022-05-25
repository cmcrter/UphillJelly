using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;

public class ToCloseToPlayerCondition : Condition
{
    private const int MaxNumberOfPlayers = 4;
    
    private List<PlayerHingeMovementController> playersWithinRadius;

    public void PlayerEnteredRadius(PlayerHingeMovementController playerPosition)
    {
        if (playersWithinRadius == null)
        {
            playersWithinRadius = new List<PlayerHingeMovementController>(MaxNumberOfPlayers);
        }
        playersWithinRadius.Add(playerPosition);
    }
    public void PlayerExitedRadius(PlayerHingeMovementController playerPosition)
    {
        if(Debug.isDebugBuild)
        {
            Debug.Log("Removing Player");
        }

        playersWithinRadius.Remove(playerPosition);
    }

    public override bool isConditionTrue()
    {
        if (playersWithinRadius == null)
        {
            return false;
        }
        if (playersWithinRadius.Count > 0)
        {
            return true;
        }
        return false;
    }

    public PlayerHingeMovementController[] GetPlayersInRaidus()
    {
        return playersWithinRadius.ToArray();
    }
}
