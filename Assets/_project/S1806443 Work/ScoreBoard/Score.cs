////////////////////////////////////////////////////////////
// File: Score.cs
// Author: Jack Peedle
// Date Created: 20/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 27/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    [SerializeField]
    public class Score : MonoBehaviour
    {

        //
        [SerializeField]
        public string playerName;

        //
        [SerializeField]
        public int score;

        //
        [SerializeField]
        public int timeCompleted;

        //
        [SerializeField]
        public int KOs;

        //
        public Score(string playerName, int score, int timeCompleted, int KOs) {


            //
            this.playerName = playerName;

            //
            this.score = score;

            //
            this.timeCompleted = timeCompleted;

            //
            this.KOs = KOs;

        }

    }
}
