using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    [SerializeField]
    public class Score : MonoBehaviour
    {

        //
        public string playerName;

        //
        public float score;

        //
        public Score(string playerName, float score) {

            //
            this.playerName = playerName;

            //
            this.score = score;

        }

    }
}
