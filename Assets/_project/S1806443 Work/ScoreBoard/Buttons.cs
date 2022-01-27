using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class Buttons : MonoBehaviour{

        //
        public Score score;

        //
        public void SaveLeaderBoard() {

            //
            //Debug.Log(score.score);

            //
            SaveScoreBoard.SaveLeaderBoard(score);

        }

        //
        public void LoadLeaderBoard() {

            //
            SaveScoreBoard.LoadScoreBoard();

        }

    }
}
