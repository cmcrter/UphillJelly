using L7Games.Input;
using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XToTrickUiPopUp : UiPopUp
{
    private PlayerHingeMovementController hingePlayer;
    [SerializeField]
    private GameObject grahpicsChildren;

    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        return true;
    }

    public override void Initalise(InputHandler inputHandler)
    {
        initalised = true;
        triggeringPlayerInputHandler = inputHandler;
        if (hingePlayer == null)
        {
            hingePlayer = inputHandler.GetComponent<PlayerHingeMovementController>();
        }
        gameObject.SetActive(true);
        grahpicsChildren.SetActive(false);
        StartCoroutine(WaitUntilPlayerIsAtJumpApex());
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
                triggeringPlayerInputHandler.trickPressed += TriggeringPlayerInputHandler_trickPressed;
            }
        }
    }

    private void TriggeringPlayerInputHandler_trickPressed()
    {
        ClosePopUp();
        triggeringPlayerInputHandler.trickPressed -= TriggeringPlayerInputHandler_trickPressed;
        triggeringPlayerInputHandler.disableWipeoutInput = false;
    }

    private IEnumerator WaitUntilPlayerIsAtJumpApex()
    {
        yield return new WaitUntil(IsAtJumpApex);
        grahpicsChildren.SetActive(true);
        PauseManager.instance.PauseGame();
        triggeringPlayerInputHandler.disableWipeoutInput = true;
    }

    private bool IsAtJumpApex()
    {
        return hingePlayer.fRB.velocity.y < 0f && !hingePlayer.groundBelow.isConditionTrue();
    }
}
