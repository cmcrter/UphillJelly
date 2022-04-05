////////////////////////////////////////////////////////////
// File: RankTimer.cs
// Author: Charles Carter
// Date Created: 05/04/22
// Last Edited By: Charles Carter
// Date Last Edited: 05/04/22
// Brief: A temporary script for a game timer
//////////////////////////////////////////////////////////// 

using TMPro;
using UnityEngine;

namespace L7Games
{
    public class RankTimer : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private Timer roundTimer = new Timer(0.1f);

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

        public bool gameEnded = false;

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
            if(gameEnded)
            {
                return;
            }

            //Tick down in code so timer has no end (not duration needed)
            roundTimer.Tick(-Time.deltaTime);
            UpdateTimerText();
        }

        #endregion

        #region Public Methods

        public void EndTimer()
        {
            gameEnded = true;
        }

        public void UpdateTimerText()
        {
            if(!timerText)
                return;

            float minutes = Mathf.Floor(roundTimer.current_time / 60);
            float seconds = Mathf.Floor(roundTimer.current_time % 60);
                
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        #endregion
    }
}

