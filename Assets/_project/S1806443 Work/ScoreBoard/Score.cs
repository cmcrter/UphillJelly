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
