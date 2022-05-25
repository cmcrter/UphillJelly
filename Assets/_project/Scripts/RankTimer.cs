////////////////////////////////////////////////////////////
// File: RankTimer.cs
// Author: Charles Carter
// Date Created: 05/04/22
// Last Edited By: Charles Carter
// Date Last Edited: 19/05/22
// Brief: A temporary script for a game timer and a rank system
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace L7Games
{
    [System.Serializable]
    public class RankBrackets
    {
        public string bracketName;

        public float seconds;
        public float score;
        public int wipeoutThreshold;
    }

    public class RankTimer : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private Timer roundTimer = new Timer(0.1f);

        [SerializeField]
        private List<RankBrackets> LevelBrackets = new List<RankBrackets>();

        public float roundTime
        {
            get
            {
                return roundTimer.current_time * -1f;
            }

            private set
            {
                roundTime = value;
            }
        }

        [SerializeField]
        private TextMeshProUGUI timerText;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            timerText = timerText ?? GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            //Setting essentially an infinite timer since it will only tick away from the duration
            roundTimer = new Timer(0.1f);
        }

        void Update()
        {
            //Tick down in code so timer has no end (not duration needed)
            roundTimer.Tick(-Time.deltaTime);
            UpdateTimerText();
        }

        #endregion

        #region Public Methods

        public void UpdateTimerText()
        {
            if(!timerText)
                return;

            float minutes = Mathf.Floor(roundTimer.current_time / 60);
            float seconds = Mathf.Floor(roundTimer.current_time % 60);
                
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        public void LockTimer()
        {
            roundTimer.isLocked = true;
        }

        public string GetRankRating(float score, float timer, float KOs)
        {
            string rankString = "No Ranking";

            //Making sure there are ranking for this level
            //Easier to harder brackets
            foreach(RankBrackets bracket in LevelBrackets)
            {
                //Checking if this is the correct bracket (score has to be higher, time lower and wipeouts lower)
                if(score >= bracket.score && timer < bracket.seconds && KOs <= bracket.wipeoutThreshold)
                {
                    rankString = bracket.bracketName;
                }
            }

            return rankString;
        }

        public void TurnOffText()
        {
            timerText.enabled = false;
        }

        #endregion
    }
}

