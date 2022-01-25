using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    [SerializeField]
    public class ScoreData
    {

        //
        public List<Score> scores;

        //
        public ScoreData() {

            //
            scores = new List<Score>();

            Debug.Log(scores.ToArray());

        }


    }
}
