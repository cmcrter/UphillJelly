////////////////////////////////////////////////////////////
// File: ScoreBoardData.cs
// Author: Jack Peedle
// Date Created: 22/01/22
// Last Edited By: Jack Peedle
// Date Last Edited: 22/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    // save it in file
    [System.Serializable]
    public class ScoreBoardData
    {

        //
        public List<SleepyCat.ScoreBoardEntryData> scoreBoardScores;
        


        // Contructor to tell the PlayerData where to get the data from
        public ScoreBoardData(ScoreBoardSavedData scoreBoardSavedData) {

            //
            scoreBoardScores = scoreBoardSavedData.highscores;

        }

    }

}
