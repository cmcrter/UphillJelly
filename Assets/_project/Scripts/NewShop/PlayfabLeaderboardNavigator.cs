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

    public class PlayfabLeaderboardNavigator : MonoBehaviour
    {
        public PlayFabManager playFabManager;
        public TextMeshProUGUI leaderboardTypeText;
        public int currentLeaderboardInt;

        private void Start() 
        {
            currentLeaderboardInt = 0;
            UpdateLeaderboard();
        }

        public void NextLeaderboardOption() 
        {
            currentLeaderboardInt++;

            if (currentLeaderboardInt >= 3)
            {
                currentLeaderboardInt = 0;
            }

            UpdateLeaderboard();
        }

        public void PreviousLeaderboardOption()
        {
            currentLeaderboardInt--;

            if (currentLeaderboardInt <= -1)
            {
                currentLeaderboardInt = 2;
            }

            UpdateLeaderboard();
        }

        public void UpdateLeaderboard() 
        {
            string levelname = "";

            if (currentLeaderboardInt == 0)
            {
                levelname = "Tutorial";
                leaderboardTypeText.text = "Tutorial Leaderboard";
            }

            if (currentLeaderboardInt == 1)
            {
                levelname = "City";
                leaderboardTypeText.text = "City Leaderboard";
            }

            if (currentLeaderboardInt == 2)
            {
                levelname = "OldTown";
                leaderboardTypeText.text = "Old Town Leaderboard";
            }

            playFabManager.GetCurrentLeaderboard(levelname);
        }

        //Changing this leaderboard's tab to be correct
        public void TabOption(leaderboard_value value)
        {
            foreach(Transform panel in playFabManager.LeaderboardPanels)
            {
                panel.gameObject.SetActive(false);
            }

            playFabManager.LeaderboardPanels[(int)value].gameObject.SetActive(true);
        }
    }
}


