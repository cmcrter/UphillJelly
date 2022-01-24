using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SleepyCat
{
    public class ScoreManager : MonoBehaviour
    {

        //
        private ScoreData sd;

        //
        private void Awake() {

            //
            sd = new ScoreData();

        }

        //
        public IEnumerable<Score> GetHighScores() {

            //
            return sd.scores.OrderByDescending(x => x.score);

        }

        //
        //
        [ContextMenu("Add Test Entry")]
        public void AddScore(Score score) {

            //
            sd.scores.Add(score);

        }


    }
}
