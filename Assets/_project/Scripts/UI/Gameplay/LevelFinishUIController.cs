////////////////////////////////////////////////////////////
// File: LevelFinishUIController.cs
// Author: Charles Carter
// Date Created: 19/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 19/05/22
// Brief: A controller for the UI at the end of a level
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using L7Games.Loading;
using L7Games.Movement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace L7Games
{
    public class LevelFinishUIController : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private PlayFabManager leaderboardManager;

        [Header("This Session")]
        public HUD HUDScript;
        public RankTimer Timer;
        public PlayerController player;
        public EventSystem eventSystem;
        public GameObject nextButton;

        //Using this class to pass through to the leaderboards
        private RankBrackets bracketToPassThrough;

        [Header("Panel/Game Management")]
        [SerializeField]
        private GameObject leaderboardPanel;
        [SerializeField]
        private GameObject mainPanel;

        [Header("Main Panel UI")]
        //Text to show you how well you did in the level
        [SerializeField]
        private TextMeshProUGUI topText;
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI rankText;
        [SerializeField]
        private TextMeshProUGUI timerText;
        [SerializeField]
        private TextMeshProUGUI wipeoutText;

        //Reusing main menu level select displayers
        [Header("Level Select Variables")]
        [SerializeField]
        private List<MainMenuLevelDisplay> levelDisplayers = new List<MainMenuLevelDisplay>();

        [SerializeField]
        private Button restartLevelButton;

        [Header("Leaderboard Panel UI")]
        //Text to show you how well you did in the level
        [SerializeField]
        private TextMeshProUGUI scoreNameText;
        [SerializeField]
        private TextMeshProUGUI timeNameText;
        [SerializeField]
        private TextMeshProUGUI wipeoutsNameText;

        [SerializeField]
        private TextMeshProUGUI currentScoreText;
        [SerializeField]
        private TextMeshProUGUI currentRankText;
        [SerializeField]
        private TextMeshProUGUI currentTimerText;
        [SerializeField]
        private TextMeshProUGUI currentWipeoutText;

        //Predicted spot in the leaderboards
        [SerializeField]
        private TextMeshProUGUI predictedScoreText;
        [SerializeField]
        private TextMeshProUGUI predictedTimerText;
        [SerializeField]
        private TextMeshProUGUI predictedWipeoutsText;

        #endregion

        #region Unity Methods

        private void Start()
        {
            AdjustLevelSelectScroller();
        }

        private void OnEnable()
        {
            // Setting up controller support for levels now its active
            List<UnityEngine.UI.Toggle> levelSelectionToggles = new List<UnityEngine.UI.Toggle>();
            for (int i = 0; i < levelDisplayers.Count; ++i)
            {
                levelSelectionToggles.Add(levelDisplayers[i].ThisToggle);
            }

            LevelSelectControllerSupport.Setup(levelSelectionToggles, restartLevelButton, restartLevelButton);
        }

        #endregion

        #region Public Methods

        public void PopulateInformation()
        {
            //This is the set time
            Timer.LockTimer();

            string ratingString = Timer.GetRankRating(HUDScript.storedScore, Timer.roundTime, player.KOCount);

            bracketToPassThrough = new RankBrackets();
            bracketToPassThrough.bracketName = ratingString;
            bracketToPassThrough.score = HUDScript.storedScore;
            bracketToPassThrough.seconds = (Timer.roundTime * -1);
            //The end trigger always wipes you out
            bracketToPassThrough.wipeoutThreshold = player.KOCount;

            //Triggering this since it cannot run without being logged in anyway
            leaderboardManager.FinishedLevelTriggered(bracketToPassThrough);

            //Updating Leaderboard Visual
            scoreNameText.text = timeNameText.text = wipeoutsNameText.text = LoadingData.player.profileName;
            currentRankText.text = ratingString;
            currentScoreText.text = Mathf.FloorToInt(HUDScript.storedScore).ToString();
            currentTimerText.text = ((int)Timer.roundTime * -1).ToString() + "s";
            currentWipeoutText.text = player.KOCount.ToString();

            predictedScoreText.text = PlayFabManager.GetPredictedPosition(HUDScript.storedScore, leaderboardManager.scoresEntries, false);
            predictedTimerText.text = PlayFabManager.GetPredictedPosition(Timer.roundTime * -1, leaderboardManager.timerEntries, true);
            predictedWipeoutsText.text = PlayFabManager.GetPredictedPosition(player.KOCount, leaderboardManager.wipeouteEntries, true);

            //Updating general panel text's
            rankText.text = ratingString;
            scoreText.text = Mathf.FloorToInt(HUDScript.storedScore).ToString();
            timerText.text = ((int)Timer.roundTime * -1).ToString() + "s";
            wipeoutText.text = player.KOCount.ToString();

            if(LoadingData.player != null)
            {
                LoadingData.player.iCurrency += (int)(HUDScript.storedScore * 0.1f);
            }

            eventSystem.SetSelectedGameObject(nextButton);

        }



        public void SwitchToLeaderboard()
        {
            leaderboardPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        public void SwitchToMainPanel()
        {
            mainPanel.SetActive(true);
            leaderboardPanel.SetActive(false);
        }

        private void AdjustLevelSelectScroller()
        {
            if(LevelManager.ConfirmedLevels != null)
            {
                List<LEVEL> usedLevels = new List<LEVEL>();

                //Main Menu is in 0th slot
                for(int i = 0; i < levelDisplayers.Count; ++i)
                {
                    for(int j = 1; j < LevelManager.ConfirmedLevels.Length; ++j)
                    {
                        if(i < LevelManager.ConfirmedLevels.Length - 1)
                        {
                            if(LoadingData.currentLevel != LevelManager.ConfirmedLevels[j].levelType && !usedLevels.Contains(LevelManager.ConfirmedLevels[j].levelType))
                            {
                                levelDisplayers[i].SetLevel(LevelManager.ConfirmedLevels[j]);
                                usedLevels.Add(LevelManager.ConfirmedLevels[j].levelType);
                                break;
                            }
                        }
                        else
                        {
                            levelDisplayers[i].gameObject.SetActive(false);
                        }
                    }
                }
            }



        }

        #endregion
    }
}