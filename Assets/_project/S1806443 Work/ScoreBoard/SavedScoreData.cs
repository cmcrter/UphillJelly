using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    // save it in file
    [System.Serializable]
    public class SavedScoreData{

        //
        //public List<string> savedPlayerName;

        //
        public int savedScore;

        //
        //public int savedTimeCompleted;

        //
        //public int savedKOs;

        //
        //public List<Score> savedLeaderboardScores;

        //
        public SavedScoreData(Score score) {

            savedScore = score.score;

            Debug.Log(savedScore);

            //
            //savedPlayerName = scoreData.scores.

            //savedLeaderboardScores = new List<ScoreData>();

            //
            //savedLeaderboardScores = scoreData.scores;

            //Debug.Log(savedLeaderboardScores.ToArray());

            //
            //savedPlayerName = scoreData.playerName.ToString();

            //
            //savedScore = scoreData.score;

            //
            //savedTimeCompleted = scoreData.timeCompleted;

            //
            //savedKOs = scoreData.KOs;

        }

    }
}
