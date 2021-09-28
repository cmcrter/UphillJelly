
using UnityEngine;
using UnityEngine.UI;

namespace ChadLads.Scoreboards
{
    public class ScoreboardEntryUI : MonoBehaviour
    {
        [SerializeField]
        private Text entryNameText = null;
        [SerializeField]
        private Text entrySpeedText = null;
        [SerializeField]
        private Text entryCringeText = null;
        [SerializeField]
        private Text entryRadicalText = null;


        public void Initialize(ScoreboardEntryData scoreboardEntryData)
        {
            entryNameText.text = scoreboardEntryData.entryName;
            entrySpeedText.text = scoreboardEntryData.entrySpeed;
            entryCringeText.text = scoreboardEntryData.entryCringe.ToString();
            entryRadicalText.text = scoreboardEntryData.entryRadical.ToString();

        }
    }

}
