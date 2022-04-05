////////////////////////////////////////////////////////////
// File: HUD.cs
// Author: Charles Carter, Matthew Mason
// Date Created: 15/02/22
// Last Edited By: Matthew Mason
// Date Last Edited: 05/04/22
// Brief: A script to manage the HUD of a player
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using L7Games.Tricks;
using L7Games;
using L7Games.Movement;
using L7Games.Triggerables.Collectables;
using System.Collections;

public class HUD : MonoBehaviour
{
    #region Private Serialized Fields
    [SerializeField]
    [Tooltip("The colour the trickComboText will turn when a combo is failed")]
    private Color comboFailedColor;
    [SerializeField]
    [Tooltip("The colour the trickComboText will turn when a combo is completed")]
    private Color comboSuccessColor;
    [SerializeField]
    [Tooltip("The colour the trickComboText will turn when a combo is failed")]
    private Color trickTextBaseColor;

    [SerializeField]
    [Tooltip("How long it takes for the trick text to fade out once it is finished (Success or fail)")]
    private float trickTextFadeOutDuration;

    //For wiping out or finishing a combo, showing an image
    [SerializeField]
    [Tooltip("For wiping out or finishing a combo, showing an image")]
    private Image SoundImage;

    [SerializeField]
    [Tooltip("The player that the HUD is getting information from")]
    private PlayerHingeMovementController controllingPlayer;

    [SerializeField]
    [Tooltip("The text displaying the overall score")]
    private TextMeshProUGUI ScoreText;
    //The General texts visible on the HUD
    [SerializeField] 
    [Tooltip("The text displaying the current trick combo")]
    private TextMeshProUGUI trickComboText;


    [SerializeField] 
    [Tooltip("The trick buffer to get the current combo from")]
    private TrickBuffer trickBuffer;
    #endregion

    #region Private Variables
    /// <summary>
    /// The coroutine controlling the fading of the text
    /// </summary>
    private Coroutine trickTextFadeOut;

    /// <summary>
    /// The current total score for the player (this is place holder and should be gotten from its own score system)
    /// </summary>
    private float storedScore;

    /// <summary>
    /// The amount of the times the last trick has been the last completed trick in sequence
    /// </summary>
    private int currentTrickCount;

    /// <summary>
    /// The action that is currently in progress in the trick buffer
    /// </summary>
    private ScoreableAction actionInProgress;
    /// <summary>
    /// The last action completed received from the trick buffer
    /// </summary>
    private ScoreableAction lastScoreableAction;

    /// <summary>
    /// The text made up of all the combo pieces the player has done in the current combo
    /// </summary>
    private string comboString;

    /// <summary>
    /// The timer counting down the time till the ended combo text should have faded/
    /// </summary>
    private Timer trickComboTextFadeTimer;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // TODO: This will only work in single player, this should be one of the first things to fixed if local multiplayer is implemented
        if (trickBuffer == null)
        {
            trickBuffer = FindObjectOfType<TrickBuffer>();
        }
        if (controllingPlayer == null)
        {
            controllingPlayer = FindObjectOfType<PlayerHingeMovementController>();
        }
    }

    private void OnDisable()
    {
        // Unbind from events
        if (trickBuffer != null)
        {
            trickBuffer.ActionStarted -= TrickBuffer_ActionStarted;
            trickBuffer.ActionCompleted -= TrickBuffer_ActionCompleted;

            trickBuffer.ComboCompleted -= TrickBuffer_ComboCompleted;
            trickBuffer.ComboBroken -= TrickBuffer_ComboBroken;
        }
        if (controllingPlayer != null)
        {
            controllingPlayer.onRespawn -= PlayerControllingHud_onRespawn;
        }
        MoneyCollectable.MoneyPickedUp -= MoneyCollectable_MoneyPickedUp;
    }

    private void OnEnable()
    {
        // Binding to events
        if (trickBuffer != null)
        {
            trickBuffer.ActionStarted += TrickBuffer_ActionStarted;
            trickBuffer.ActionCompleted += TrickBuffer_ActionCompleted;

            trickBuffer.ComboCompleted += TrickBuffer_ComboCompleted;
            trickBuffer.ComboBroken += TrickBuffer_ComboBroken;
        }
        if (controllingPlayer != null)
        {
            controllingPlayer.onRespawn += PlayerControllingHud_onRespawn;
        }
        MoneyCollectable.MoneyPickedUp += MoneyCollectable_MoneyPickedUp;
    }

    void Update()
    {
        // This will be changed when score has its own system
        ScoreText.text = Mathf.Round(storedScore).ToString();
    }
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods
    private IEnumerator FadeOutTrickText(Color colorWhileFading, float fadeDuration)
    {
        trickComboTextFadeTimer = new Timer(fadeDuration);
        while (trickComboTextFadeTimer.isActive)
        {
            trickComboTextFadeTimer.Tick(Time.deltaTime);
            trickComboText.color = new Color(colorWhileFading.r, colorWhileFading.g, colorWhileFading.b, Mathf.Clamp01(trickComboTextFadeTimer.current_time / fadeDuration));
            yield return null;
        }
        trickComboText.text = "";
    }

    /// <summary>
    /// Called when the player has reset to reset elements of the HUD
    /// </summary>
    private void HUDReset()
    {
        // TODO: this should clear everything on character being reset

        // Knock 20% off the score (Temporary, this script should not be controlling score)
        storedScore *= 0.8f;
    }
    /// <summary>
    /// Called when money is picked up to increase score (Temporary, this script should not be controlling score)
    /// </summary>
    /// <param name="playerPickingUpMoney">The player that picked up the money that triggered this event</param>
    private void MoneyCollectable_MoneyPickedUp(L7Games.Movement.PlayerController playerPickingUpMoney)
    {
        if (playerPickingUpMoney == controllingPlayer)
        {
            storedScore += 10f;
        }
    }
    /// <summary>
    /// Called when the player that this HUD is displaying information for respawns
    /// </summary>
    private void PlayerControllingHud_onRespawn()
    {
        HUDReset();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="completedAction"></param>
    private void TrickBuffer_ActionCompleted(ScoreableAction completedAction)
    {
        // Add action to the current text
        // If the action is the first one
        if (actionInProgress == null && lastScoreableAction == null)
        {
            StopCoroutine(trickTextFadeOut);
            trickComboText.color = trickTextBaseColor;
            comboString = completedAction.trickName;
            currentTrickCount = 1;
            lastScoreableAction = completedAction;
        }
        else
        {
            // Check if the last trick performed was the same as the new one
            if (completedAction == lastScoreableAction || completedAction == lastScoreableAction)
            {
                ++currentTrickCount;
                if (currentTrickCount > 2)
                {
                    comboString = comboString.Remove(comboString.Length - 4);
                }
                comboString += " X " + currentTrickCount.ToString();

            }
            else
            {
                // otherwise its a new trick in the combo
                comboString += " + " + completedAction.trickName;
                currentTrickCount = 1;
                lastScoreableAction = completedAction;
            }
        }

        // Figure Out the score Text
        string comboScoreText = trickBuffer.GetScoreFromCurrentCombo().ToString() + " X " + trickBuffer.ComboMultiplier;

        trickComboText.text = comboScoreText + '\n' + comboString;
    }
    private void TrickBuffer_ActionStarted(ScoreableAction obj)
    {
        //throw new System.NotImplementedException();
    }
    private void TrickBuffer_ComboBroken()
    {
        comboString = "";
        trickTextFadeOut = StartCoroutine(FadeOutTrickText(comboFailedColor, trickTextFadeOutDuration));
        lastScoreableAction = null;
        actionInProgress = null;
    }
    private void TrickBuffer_ComboCompleted()
    {
        comboString = "";
        trickTextFadeOut = StartCoroutine(FadeOutTrickText(comboSuccessColor, trickTextFadeOutDuration));
        lastScoreableAction = null;
        actionInProgress = null;
        storedScore += trickBuffer.GetScoreFromCurrentCombo();
    }

    #endregion
}
