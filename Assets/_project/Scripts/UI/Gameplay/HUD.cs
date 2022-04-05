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
    #region Variables

    //The General texts visible on the HUD
    [SerializeField]
    private TextMeshProUGUI TrickText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    //For wiping out or finishing a combo, showing an image
    [SerializeField]
    private Image SoundImage;

    [SerializeField]
    private TrickBuffer trickBuffer;

    [SerializeField]
    private Color comboSuccessColor;

    [SerializeField]
    private Color comboFailedColor;

    [SerializeField]
    private Color trickTextBaseColor;

    [SerializeField]
    private float trickTextFadeOutDuration;

    [SerializeField]
    private PlayerController playerControllingHud;

    private ScoreableAction actionInProgress;

    private ScoreableAction lastScoreableAction;

    private int currentTrickCount;


    private Timer trickComboTextClearTimer;

    private Coroutine trickTextFadeOut;

    private string comboString;

    private float storedScore;
    #endregion



    #region Unity Methods
    void Start()
    {
	    // TODO: This will only work in single player, this should be one of the first things to fixed if local multiplayer is implemented
        if (trickBuffer == null)
        {
            trickBuffer = FindObjectOfType<TrickBuffer>();
        }
    }
 
    void Update()
    {
        ScoreText.text = storedScore.ToString();
    }

    private void OnEnable()
    {
        trickBuffer.ActionStarted += TrickBuffer_ActionStarted;
        trickBuffer.ActionCompleted += TrickBuffer_ActionCompleted;

        trickBuffer.ComboCompleted += TrickBuffer_ComboCompleted;
        trickBuffer.ComboBroken += TrickBuffer_ComboBroken;

        MoneyCollectable.MoneyPickedUp += MoneyCollectable_MoneyPickedUp;
    }

    private void OnDisable()
    {
        trickBuffer.ActionStarted -= TrickBuffer_ActionStarted;
        trickBuffer.ActionCompleted -= TrickBuffer_ActionCompleted;

        trickBuffer.ComboCompleted -= TrickBuffer_ComboCompleted;
        trickBuffer.ComboBroken -= TrickBuffer_ComboBroken;

        MoneyCollectable.MoneyPickedUp -= MoneyCollectable_MoneyPickedUp;
    }

    private void MoneyCollectable_MoneyPickedUp(L7Games.Movement.PlayerController playerPickingUpMoney)
    {
        if (playerPickingUpMoney == playerControllingHud)
        {
            storedScore += 10f;
        }
    }

    private void TrickBuffer_ComboBroken()
    {
        comboString = "";
        trickTextFadeOut = StartCoroutine(FadeOutTrickText(Color.red, trickTextFadeOutDuration));
        lastScoreableAction = null;
        actionInProgress = null;
    }

    private void TrickBuffer_ComboCompleted()
    {
        comboString = "";
        trickTextFadeOut = StartCoroutine(FadeOutTrickText(Color.green, trickTextFadeOutDuration));
        lastScoreableAction = null;
        actionInProgress = null;
        storedScore += trickBuffer.GetScoreFromCurrentCombo();
    }

    private void TrickBuffer_ActionCompleted(ScoreableAction completedAction)
    {
        // Add action to the current text
        // If the action is the first one
        if (actionInProgress == null && lastScoreableAction == null)
        {
            StopCoroutine(trickTextFadeOut);
            TrickText.color = trickTextBaseColor;
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

        TrickText.text = comboScoreText + '\n' + comboString;
    }

    private void TrickBuffer_ActionStarted(ScoreableAction obj)
    {
        //throw new System.NotImplementedException();
    }
    #endregion

    #region Public Methods

    public void AddTrickToText(string trickName)
    {
        
    }

    public void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void HUDReset()
    {
        // TODO: this should clear everything on character being reset

        // Knock 20% off the score
        storedScore *= 0.8f;
    }
    #endregion

    #region Private Methods
    private IEnumerator FadeOutTrickText(Color colorWhileFading, float fadeDuration)
    {
        trickComboTextClearTimer = new Timer(fadeDuration);
        while (trickComboTextClearTimer.isActive)
        {
            trickComboTextClearTimer.Tick(Time.deltaTime);
            TrickText.color = new Color(colorWhileFading.r, colorWhileFading.g, colorWhileFading.b, Mathf.Clamp01(trickComboTextClearTimer.current_time / fadeDuration));
            yield return null;
        }
        TrickText.text = "";
    }
    #endregion
}
