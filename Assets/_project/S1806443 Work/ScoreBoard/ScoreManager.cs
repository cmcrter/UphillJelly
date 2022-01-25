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
        [SerializeField]
        public List<Score> VisualisedScores;

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
        [ContextMenu("Add Entry")]
        public void AddScore(SleepyCat.Score score) {

            //
            sd.scores.Add(score);

        }

        //
        [ContextMenu("Add Test Entry")]
        public void AddScoreTest(SleepyCat.Score testscore) {

            //
            sd.scores.Add(testscore);

        }


    }
}
