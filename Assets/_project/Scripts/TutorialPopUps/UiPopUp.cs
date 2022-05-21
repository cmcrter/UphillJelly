using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Input;
using L7Games.Movement;

public abstract class UiPopUp : MonoBehaviour
{
    protected bool initalised;

    protected InputHandler triggeringPlayerInputHandler;

    public virtual void Initalise(InputHandler inputHandler)
    {
        initalised = true;
        triggeringPlayerInputHandler = inputHandler;
        gameObject.SetActive(true);
    }

    protected void ClosePopUp()
    {
        PauseManager.instance.UnpauseGame();
        Destroy(gameObject);
    }

    public abstract bool CheckCondition(PlayerHingeMovementController player);
}
