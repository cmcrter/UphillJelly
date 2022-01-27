////////////////////////////////////////////////////////////
// File: ScoreUI.cs
// Author: Jack Peedle
// Date Created: 20/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 27/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SleepyCat
{
    public class ScoreUI : MonoBehaviour
    {

        //
        public RowUI rowUI;

        //
        public ScoreManager scoreManager;

        //
        public TMP_InputField playerInputName;

        //
        public TempScoreSystem tempScoreSystem;

        //
        public int playerScore;

        //
        void Start() {

            Debug.Log("AddedStartingScores");

            //
            var scores = scoreManager.GetHighScores().ToArray();

            //
            for (int i = 0; i < scores.Length; i++) {

                //
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>();

                //
                row.rank.text = (i + 1).ToString();

                //
                row.playerName.text = scores[i].playerName;

                //
                row.score.text = scores[i].score.ToString();

                //
                row.timeCompleted.text = scores[i].timeCompleted.ToString();

                //
                row.KOs.text = scores[i].KOs.ToString();

                //
                //row.score.text = scores[i].score.ToString();

            }
            
        }

        //
        public void UpdateLeaderBoard() {


            //
            rowUI.playerName.text = playerInputName.text;

            //
            playerScore = tempScoreSystem.PlayerScore;

            //  
            scoreManager.AddScore(new Score(playerInputName.text, 10, 100, 6));


            Debug.Log("AddedStartingScores");

            //
            var scores = scoreManager.GetHighScores().ToArray();

            //
            for (int i = 0; i < scores.Length; i++) {

                //
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>();

                //
                row.rank.text = (i + 1).ToString();

                //
                row.playerName.text = scores[i].playerName;

                //
                row.score.text = scores[i].score.ToString();

                //
                row.timeCompleted.text = scores[i].timeCompleted.ToString();

                //
                row.KOs.text = scores[i].KOs.ToString();

            }

        }

    }
}
