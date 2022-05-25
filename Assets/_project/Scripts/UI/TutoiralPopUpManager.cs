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
    [Tooltip("The Trigger used triggering the jump off grind early tutorial pop up.")]
    private PopUpTrigger grindEarlyJumpOffTrigger;

    [SerializeField]
    [Tooltip("The Trigger used triggering the jump off grind at the end tutorial pop up")]
    private PopUpTrigger grindEndJumpOffTrigger;

    [Header("PopUps")]
    [SerializeField]
    private RespawnUiPopUp respawnPopUp;

    [SerializeField]
    private XToTrickUiPopUp xToTrickUiPop;

    [SerializeField]
    private JumpOffGrindPopUP jumpOffGrindEndPopUp;

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

    private bool endJumpTriggerPassed;
    private bool earlyJumpTriggerPassed;

    private bool endJumpHasPriority;

    private bool endJumpWaitingforGrind;
    private bool earlyJumpWaitingforGrind;

    private bool endJumpCompleted;

    private void Start()
    {
        if (LoadingData.player != null)
        {
            if (LoadingData.player.doneTutorial)
            {
                CleanUpPopUps();
                return;
            }
        }
        if (tutorialDoneOverride)
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

        // Set up end and early grind jump coroutines 
        // Setting up trigger passing
        if (grindEndJumpOffTrigger != null)
        {
            endJumpTriggerPassed = false;
            grindEndJumpOffTrigger.triggerHit += GrindEndJumpOffTrigger_triggerHit;
        }
        else
        {
            endJumpTriggerPassed = true;
        }
        if (grindEarlyJumpOffTrigger != null)
        {
            earlyJumpTriggerPassed = false;
            grindEarlyJumpOffTrigger.triggerHit += GrindEarlyJumpOffTrigger_triggerHit;
        }
        else
        {
            earlyJumpTriggerPassed = true;
        }

        // Sorting out if end jump has priority
        if (grindEndJumpOffTrigger == null && grindEarlyJumpOffTrigger != null)
        {
            endJumpHasPriority = false;
        }
        else
        {
            endJumpHasPriority = true;
        }
        // Setting up waiting for grind bools
        endJumpWaitingforGrind = false;
        earlyJumpWaitingforGrind = false;
        // Starting the main Coroutines
        if (jumpOffGrindEndPopUp != null)
        {
            endJumpCompleted = false;
            StartCoroutine(EndJumpPopUpCoroutine());
        }
        else
        {
            endJumpCompleted = true;
        }
        if (jumpOffGrindsEarlyPopUp != null)
        {
            StartCoroutine(EarlyJumpPopUpCoroutine());
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

        if (jumpOffGrindEndPopUp != null)
        {
            jumpOffGrindEndPopUp.popUpFinished += JumpOffGrindPop_popUpFinished;
        }
        if (jumpOffGrindsEarlyPopUp != null)
        {
            jumpOffGrindsEarlyPopUp.popUpFinished += JumpOffGrindsEarlyPopUp_popUpFinished;
        }
    }

    private void Update()
    {
        CheckIfTutorialsCompleted();
    }

    private void GrindEndJumpOffTrigger_triggerHit(PlayerController obj)
    {
        endJumpTriggerPassed = true;
        endJumpWaitingforGrind = true;
    }

    private void GrindEarlyJumpOffTrigger_triggerHit(PlayerController obj)
    {
        earlyJumpTriggerPassed = true;
        earlyJumpWaitingforGrind = true;
    }

    private void JumpOffGrindPop_popUpFinished()
    {
        endJumpCompleted = true;
        endJumpWaitingforGrind = false;
    }

    private void JumpOffGrindsEarlyPopUp_popUpFinished()
    {
        earlyJumpWaitingforGrind = false;
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
        Destroy(respawnPopUp.gameObject);
        CheckIfTutorialsCompleted();
        playerInputHandler.wipeoutResetStarted -= PlayerInputHandler_wipeoutResetStarted;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitThenShowRespawnPopUp()
    {
        yield return new WaitForSeconds(3f);
        playerInputHandler.wipeoutResetStarted -= PlayerInputHandler_wipeoutResetStarted;
        respawnPopUp.Initalise(playerInputHandler);
    }

    private void XTrickTrigger_triggerHit(PlayerController player)
    {
        xToTrickUiPop.Initalise(playerInputHandler);
    }

    private IEnumerator EndJumpPopUpCoroutine()
    {
        yield return new WaitUntil(isEndJumpTriggerPassed);
        if (endJumpHasPriority)
        {
            yield return new WaitUntil(isPlayerGrinding);
        }
        else
        {
            // Keep waiting for a grind until no long waiting for 
            do
            {
                yield return new WaitUntil(isPlayerGrinding);
            } while (earlyJumpWaitingforGrind || !isPlayerGrinding());
        }

        // Once the player is grinding and it is end jumps turn
        // Disable the player input
        playerInputHandler.enabled = false;
        // Start the pop-up
        jumpOffGrindEndPopUp.Initalise(playerInputHandler);
    }

    private IEnumerator EarlyJumpPopUpCoroutine()
    {
        // If neither early nor end jump has a collider this must wait for end jump
        if (grindEarlyJumpOffTrigger == null && grindEndJumpOffTrigger == null)
        {
            yield return new WaitUntil(IsEndJumpCompleted);
            yield return new WaitUntil(isPlayerGrinding);
        }
        else
        {
            yield return new WaitUntil(IsEarlyJumpTriggerPassed);
            if (endJumpHasPriority)
            {
                // Keep waiting for a grind until no long waiting for 
                do
                {
                    yield return new WaitUntil(isPlayerGrinding);
                } while (endJumpWaitingforGrind || !jumpOffGrindsEarlyPopUp.CheckCondition(playerHinge));
            }
            else
            {
                yield return new WaitUntil(isPlayerGrinding);
            }
        }

        if (jumpOffGrindsEarlyPopUp.CheckCondition(playerHinge))
        {
            jumpOffGrindsEarlyPopUp.Initalise(playerInputHandler);
            playerInputHandler.disableWipeoutInput = true;
        }
    }

    private bool isEndJumpTriggerPassed()
    {
        return endJumpTriggerPassed;
    }
    private bool IsEarlyJumpTriggerPassed()
    {
        return earlyJumpTriggerPassed;
    }
    private bool IsEarlyJumpWaitingForGrind()
    {
        return earlyJumpWaitingforGrind;
    }
    private bool IsEndJumpCompleted()
    {
        return endJumpCompleted;
    }

    private bool isPlayerGrinding()
    {
        return playerHinge.grindingState.hasRan;
    }

    private void CleanUpPopUps()
    {
        if (respawnPopUp != null)
        {
            Destroy(respawnPopUp.gameObject);
        }
        if (xToTrickUiPop != null)
        {
            Destroy(xTrickTrigger.gameObject);
        }
        if (jumpOffGrindEndPopUp != null)
        {
            Destroy(jumpOffGrindEndPopUp.gameObject);
        }
        if (jumpOffGrindsEarlyPopUp != null)
        {
            Destroy(jumpOffGrindsEarlyPopUp.gameObject);
        }
        if (wipingOutPopUp != null)
        {
            Destroy(wipingOutPopUp.gameObject);
        }
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
        if (jumpOffGrindEndPopUp != null)
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
        if (LoadingData.player != null)
        {
            LoadingData.player.doneTutorial = true;
        }
        CleanUpPopUps();
    }
}
