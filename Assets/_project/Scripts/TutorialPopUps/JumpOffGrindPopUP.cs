using L7Games.Input;
using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOffGrindPopUP : UiPopUp
{
    private PlayerHingeMovementController playerHinge;

    public event System.Action popUpFinished;

    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        playerHinge = player;
        return player.grindingState == player.playerStateMachine.currentState;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHinge == null)
        {
            playerHinge = FindObjectOfType<PlayerHingeMovementController>();
        }
        if (playerHinge.playerStateMachine.currentState != playerHinge.grindingState)
        {
            playerHinge.inputHandler.enabled = true;
            if (popUpFinished != null)
            {
                popUpFinished();
            }
            ClosePopUp();
        }
    }

    public override void Initalise(InputHandler inputHandler)
    {
        base.Initalise(inputHandler);
    }
}
