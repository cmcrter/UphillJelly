////////////////////////////////////////////////////////////
// File: ScoreBoard.cs
// Author: Jack Peedle
// Date Created: 22/01/22
// Last Edited By: Jack Peedle
// Date Last Edited: 22/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.ScoreBoard
{
    public class ScoreBoard : MonoBehaviour
    {

        //
        [SerializeField] private int maxScoreboardEntries = 10;

        //
        [SerializeField] private Transform highScoreHolderTransform = null;

        //
        [SerializeField] private GameObject scoreBoardEntryObject = null;

        [Header("Test")]
        [SerializeField] ScoreBoardEntryData testEntryData;

    }
}
