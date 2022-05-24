////////////////////////////////////////////////////////////
// File: PlayfabLeaderboardNavigator.cs
// Author: Jack Peedle
// Date Created: 30/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 30/04/22
// Brief: 
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using L7Games.Loading;

namespace L7Games 
{
    [Serializable]
    class PanelData
    {
        public LEVEL levelWithin;
        public Transform levelPanel;
        public leaderboard_value value;
    }

    public class PlayfabLeaderboardNavigator : MonoBehaviour
    {
        public PlayFabManager playFabManager;
        public TextMeshProUGUI leaderboardTypeText;
        public TextMeshProUGUI valueText;

        public int currentLeaderboardInt;
      
        [SerializeField]
        private List<PanelData> MapPanels = new List<PanelData>();

        private void Awake()
        {
            LoadingData.currentLevel = LEVEL.MAINMENU;
            playFabManager.Login();
        }

        private void Start() 
        {
            currentLeaderboardInt = 0;
            UpdatePanels();
        }

        private void UpdatePanels()
        {
            for(int i = 0; i < MapPanels.Count; ++i)
            {
                MapPanels[i].levelPanel.gameObject.SetActive(false);
            }

            //Make the relavant
            leaderboardTypeText.text = MapPanels[currentLeaderboardInt].levelWithin.ToString() + " " + MapPanels[currentLeaderboardInt].value.ToString() + " LEADERBOARD";
            valueText.text = MapPanels[currentLeaderboardInt].value.ToString();
            MapPanels[currentLeaderboardInt].levelPanel.gameObject.SetActive(true);
        }

        public void NextLeaderboardOption() 
        {
            currentLeaderboardInt++;

            if(currentLeaderboardInt == MapPanels.Count)
            {
                currentLeaderboardInt = 0;
            }

            UpdatePanels();
        }

        public void PreviousLeaderboardOption()
        {
            currentLeaderboardInt--;

            if (currentLeaderboardInt <= -1)
            {
                currentLeaderboardInt = 2;
            }

            UpdatePanels();
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