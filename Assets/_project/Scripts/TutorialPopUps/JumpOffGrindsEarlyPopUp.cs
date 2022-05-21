using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Input;
using L7Games.Movement;

public class JumpOffGrindsEarlyPopUp : UiPopUp
{
    private PlayerHingeMovementController playerHinge;

    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        playerHinge = player;
        return player.grindingState.hasRan;
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
                triggeringPlayerInputHandler.grindingJumpUpActionPerformed += TriggeringPlayerInputHandler_grindingJumpUpActionPerformed;
            }
        }
    }

    private void TriggeringPlayerInputHandler_grindingJumpUpActionPerformed()
    {
        //Time.timeScale = 1f;
        //Destroy(gameObject);
        ClosePopUp();
    }

    public override void Initalise(InputHandler inputHandler)
    {
        base.Initalise(inputHandler);
        //Time.timeScale = 0.01f;
        PauseManager.instance.PauseGame();
    }

    private void Update()
    {
    }
}
