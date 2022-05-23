using L7Games.Input;
using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipingOutPopUp : UiPopUp
{
    private PlayerHingeMovementController playerHinge;

    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        playerHinge = player;
        return player.IsWipedOut;
    }

    private void OnEnable()
    {
        if (!initalised)
        {
            Debug.Log("StartJumpPopUp was not initialised before being enabled");
        }
        else
        {
            if (triggeringPlayerInputHandler != null)
            {
                triggeringPlayerInputHandler.wipeoutResetStarted += TriggeringPlayerInputHandler_wipeoutResetStarted;
            }
        }
    }

    private void TriggeringPlayerInputHandler_wipeoutResetStarted()
    {
        if (triggeringPlayerInputHandler != null)
        {
            triggeringPlayerInputHandler.wipeoutResetStarted -= TriggeringPlayerInputHandler_wipeoutResetStarted;
        }
        ClosePopUp();
    }

    public override void Initalise(InputHandler inputHandler)
    {
        base.Initalise(inputHandler);
        PauseManager.instance.PauseGame();
        if (playerHinge == null)
        {
            playerHinge = FindObjectOfType<PlayerHingeMovementController>();
        }
        playerHinge.ignoreNextWipeoutOnWipeoutCount = true;
    }
}
