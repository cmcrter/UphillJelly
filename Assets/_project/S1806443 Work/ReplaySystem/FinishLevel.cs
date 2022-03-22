////////////////////////////////////////////////////////////
// File: FinishLevel.cs
// Author: Jack Peedle
// Date Created: 22/01/22
// Last Edited By: Jack Peedle
// Date Last Edited: 22/01/22
// Brief:   When the player finishes the level
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class FinishLevel : MonoBehaviour
    {

        //
        public b_Player b_player;

        //
        public PlayFabManager playfabManager;

        //
        public GameObject savePlayerNamePanel;

        //
        public GameObject leaderboardPanel;

        //
        public TempScoreSystem tempScoreSystem;

        //
        public void Start() {

            //
            leaderboardPanel.SetActive(false);

        }

        public void OnTriggerEnter(Collider other) {

            // if the ghost collides with this collectable
            if (other.tag == "FinishPlayer") {

                Debug.Log("COLLIDEDWITHPLAYERFINISH");

                //
                b_player.SaveFinalValues();

                //
                playfabManager.SetScore();

                //
                leaderboardPanel.SetActive(true);

                //
                tempScoreSystem.TimerActive = false;

            }

        }


    }
}
