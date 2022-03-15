using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;

public class ToCloseToPlayerCondition : Condition
{
    private const int MaxNumberOfPlayers = 4;

    private List<PlayerController> playersWithinRadius;

    public void PlayerEnteredRadius(PlayerController playerPosition)
    {
        if (playersWithinRadius == null)
        {
            playersWithinRadius = new List<PlayerController>(MaxNumberOfPlayers);
        }
        playersWithinRadius.Add(playerPosition);
    }
    public void PlayerExitedRadius(PlayerController playerPosition)
    {
        playersWithinRadius.Add(playerPosition);
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

    public PlayerController[] GetPlayersInRaidus()
    {
        return playersWithinRadius.ToArray();
    }
}
