////////////////////////////////////////////////////////////
// File: PlayfabLeaderboardNavigator.cs
// Author: Jack Peedle
// Date Created: 30/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 30/04/22
// Brief: 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace L7Games {

    public class PlayfabLeaderboardNavigator : MonoBehaviour {

        public PlayFabManager playFabManager;

        public TextMeshProUGUI leaderboardTypeText;

        public int currentLeaderboardInt;

        private void Start() {

            currentLeaderboardInt = 0;

        }


        //
        public void NextLeaderboardOption() {

            // 
            currentLeaderboardInt++;

            // 
            if (currentLeaderboardInt >= 3) {

                // 
                currentLeaderboardInt = 0;

            }


            UpdateLeaderboard();

        }


        //
        public void PreviousLeaderboardOption() {

            //
            currentLeaderboardInt--;

            //
            if (currentLeaderboardInt <= -1) {

                //
                currentLeaderboardInt = 2;

            }


            UpdateLeaderboard();

        }

        //
        public void UpdateLeaderboard() {

            if (currentLeaderboardInt == 0) {

                //
                playFabManager.GetMainMenuTutorialLeaderboard();

                leaderboardTypeText.text = "Tutorial Leaderboard";

            }

            if (currentLeaderboardInt == 1) {

                //
                playFabManager.GetMainMenuCityLeaderboard();

                leaderboardTypeText.text = "City Leaderboard";

            }

            if (currentLeaderboardInt == 2) {

                //
                playFabManager.GetMainMenuOldTownLeaderboard();

                leaderboardTypeText.text = "Old Town Leaderboard";

            }

        }


    }

}


