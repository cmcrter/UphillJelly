using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Input;

public class RespawnUiPopUp : UiPopUp
{
    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        Debug.Log("IsWipedout: " + player.IsWipedOut);
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

    public override void Initalise(InputHandler inputHandler)
    {
        base.Initalise(inputHandler);
        PauseManager.instance.PauseGame();
    }

    private void TriggeringPlayerInputHandler_wipeoutResetStarted()
    {
        if (triggeringPlayerInputHandler != null)
        {
            triggeringPlayerInputHandler.wipeoutResetStarted -= TriggeringPlayerInputHandler_wipeoutResetStarted;
        }
        Debug.Log("Unpause wipeout Reset");
        ClosePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
