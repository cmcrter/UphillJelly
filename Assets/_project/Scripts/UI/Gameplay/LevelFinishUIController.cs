////////////////////////////////////////////////////////////
// File: LevelFinishUIController.cs
// Author: Charles Carter
// Date Created: 19/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 19/05/22
// Brief: A controller for the UI at the end of a level
//////////////////////////////////////////////////////////// 

using L7Games.Movement;
using TMPro;
using UnityEngine;

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

        #endregion

        #region Unity Methods

        

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
            bracketToPassThrough.wipeoutThreshold = (player.KOCount - 1);

            //Triggering this since it cannot run without being logged in anyway
            leaderboardManager.FinishedLevelTriggered(bracketToPassThrough);

            //Updating general panel text's
            rankText.text = ratingString;
            scoreText.text = HUDScript.storedScore.ToString();
            timerText.text = ((int)Timer.roundTime * -1).ToString() + "s";
            wipeoutText.text = (player.KOCount - 1).ToString();
        }

        public void LoginPlayfab()
        {
            leaderboardManager.Login();
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

        #endregion
    }
}