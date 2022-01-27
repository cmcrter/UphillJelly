////////////////////////////////////////////////////////////
// File: ScoreData.cs
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
    // save it in file
    public class ScoreData
    {

        //
        [SerializeField]
        public List<Score> scores;

        //
        public ScoreData() {

            //
            scores = new List<Score>();

            Debug.Log(scores.ToArray());

        }
        
        
    }


}
