using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;
using L7Games.Input;
using L7Games.Loading;

public class TutoiralPopUpManager : MonoBehaviour
{


    [Header("Triggers")]
    [SerializeField]
    private PopUpTrigger xTrickTrigger;

    [Header("PopUps")]
    [SerializeField]
    private RespawnUiPopUp respawnPopUp;

    [SerializeField]
    private XToTrickUiPopUp xToTrickUiPop;

    [SerializeField]
    private JumpOffGrindPopUP jumpOffGrindPop;

    [SerializeField]
    private JumpOffGrindsEarlyPopUp jumpOffGrindsEarlyPopUp;

    [SerializeField]
    private WipingOutPopUp wipingOutPopUp;

    [Header("Misc")]
    [SerializeField]
    private PlayerHingeMovementController playerHinge;

    [SerializeField]
    private InputHandler playerInputHandler;
    [SerializeField]
    private bool tutorialDoneOverride;

    private Coroutine respawnWaitCoroutine;

    private void Start()
    {
        if (LoadingData.player.doneTutorial && tutorialDoneOverride)
        {
            CleanUpPopUps();
            return;
        }

        if (playerHinge == null)
        {
            // Get the player from the scene
            playerHinge = FindObjectOfType<PlayerHingeMovementController>();
            playerInputHandler = playerHinge.GetComponent<InputHandler>();
        }
        else
        {
            playerInputHandler = playerHinge.GetComponent<InputHandler>();
        }
        if (jumpOffGrindPop != null)
        {
            StartCoroutine(WaitForGrindTillEndPopUpTrigger());
        }
        else
        {
            StartCoroutine(WaitForGrindTillGrindJump());
        }

        if (wipingOutPopUp != null)
        {
            wipingOutPopUp.Initalise(playerInputHandler);
        }
    }

    private void OnEnable()
    {
        if (playerHinge == null)
        {
            // Get the player from the scene
            playerHinge = FindObjectOfType<PlayerHingeMovementController>();
        }
        playerHinge.onWipeout += PlayerHinge_onWipeout;

        if (xTrickTrigger != null)
        {
            xTrickTrigger.triggerHit += XTrickTrigger_triggerHit;
        }    

        if (jumpOffGrindPop != null)
        {
            jumpOffGrindPop.popUpFinished += JumpOffGrindPop_popUpFinished;
        }
    }

    private void JumpOffGrindPop_popUpFinished()
    {
        StartCoroutine(WaitForGrindTillGrindJump());
    }

    private void PlayerHinge_onWipeout(Vector3 obj)
    {
        if (respawnPopUp != null)
        {
            respawnWaitCoroutine = StartCoroutine(WaitThenShowRespawnPopUp());
            playerInputHandler.wipeoutResetStarted += PlayerInputHandler_wipeoutResetStarted;
            playerHinge.onWipeout -= PlayerHinge_onWipeout;
        }
    }

    private void PlayerInputHandler_wipeoutResetStarted()
    {
        StopCoroutine(respawnWaitCoroutine);
        playerInputHandler.wipeoutResetStarted -= PlayerInputHandler_wipeoutResetStarted;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitThenShowRespawnPopUp()
    {
        Debug.Log("Wait then show re spawn");
        yield return new WaitForSeconds(3f);
        if (respawnPopUp.CheckCondition(playerHinge))
        {
            Debug.Log("Respawns pop up Initalise");
            respawnPopUp.Initalise(playerInputHandler);
        }
    }

    private void XTrickTrigger_triggerHit(PlayerController player)
    {
        xToTrickUiPop.Initalise(playerInputHandler);
    }

    private IEnumerator WaitForGrindTillEndPopUpTrigger()
    {
        yield return new WaitUntil(isPlayerGrinding);
        // Disable the player input

        playerInputHandler.enabled = false;
        jumpOffGrindPop.Initalise(playerInputHandler);
    }

    private IEnumerator WaitForGrindTillGrindJump()
    {
        yield return new WaitUntil(isPlayerGrinding);
        //yield return new WaitForSeconds(0.2f);
        // Disable the player input
        if (jumpOffGrindsEarlyPopUp.CheckCondition(playerHinge))
        {
            jumpOffGrindsEarlyPopUp.Initalise(playerInputHandler);
        }

    }

    private bool isPlayerGrinding()
    {
        return playerHinge.grindingState.hasRan;
    }

    private void CleanUpPopUps()
    {
        Destroy(xTrickTrigger.gameObject);
        Destroy(respawnPopUp.gameObject);
        Destroy(xToTrickUiPop.gameObject);
        Destroy(jumpOffGrindPop.gameObject);
        Destroy(jumpOffGrindsEarlyPopUp.gameObject);
        Destroy(wipingOutPopUp.gameObject);
        Destroy(gameObject);
    }

    private void CheckIfTutorialsCompleted()
    {
        if (respawnPopUp != null)
        {
            return;
        }
        if (xToTrickUiPop != null)
        {
            return;
        }
        if (jumpOffGrindPop != null)
        {
            return;
        }
        if (jumpOffGrindsEarlyPopUp != null)
        {
            return;
        }
        if (wipingOutPopUp != null)
        {
            return;
        }
        CleanUpPopUps();
    }
}
