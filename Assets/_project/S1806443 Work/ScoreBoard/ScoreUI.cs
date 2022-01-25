using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        void Start() {

            //
            scoreManager.AddScore(new Score("Test", 10));

            //
            scoreManager.AddScore(new Score("Test2", 20));

            //
            scoreManager.AddScore(new Score("Test3", 30));

            //
            scoreManager.AddScore(new Score("Test4", 40));

            //
            scoreManager.AddScore(new Score("Test5", 50));

            //
            scoreManager.AddScore(new Score("Test6", 60));

            //
            scoreManager.AddScore(new Score("Test7", 70));

            //
            scoreManager.AddScore(new Score("Test8", 80));

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

            }
            
        }

    }
}
