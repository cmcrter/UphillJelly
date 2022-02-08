using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SleepyCat
{
    public class ScoreBoardUI : MonoBehaviour
    {

        //
        [SerializeField] private TextMeshProUGUI entryNameText = null;

        //
        [SerializeField] private TextMeshProUGUI entryScoreText = null;

        //
        public void initialise(ScoreBoardEntryData scoreBoardEntryData) {

            //
            entryNameText.text = scoreBoardEntryData.entryName;

            //
            entryScoreText.text = scoreBoardEntryData.entryScore.ToString();

        }


    }
}
