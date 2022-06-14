////////////////////////////////////////////////////////////
// File: PlayfabLeaderboardNavigator.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 30/04/22
// Last Edited By: Charles Carter
// Date Last Edited: 23/05/22
// Brief: A script to change panels in the main menu for the leaderboards
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using L7Games.Loading;

namespace L7Games 
{
    //A class to contain the data which each panel represents
    [Serializable]
    class PanelData
    {
        public LEVEL levelWithin;
        public Transform levelPanel;
        public leaderboard_value value;
    }

    public class PlayfabLeaderboardNavigator : MonoBehaviour
    {
        #region Variables

        public PlayFabManager playFabManager;
        public TextMeshProUGUI leaderboardTypeText;
        public TextMeshProUGUI valueText;

        public int currentLeaderboardInt;
      
        [SerializeField]
        private List<PanelData> MapPanels = new List<PanelData>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            LoadingData.currentLevel = LEVEL.MAINMENU;

            //Unneccessary but making sure there's a login
            //playFabManager.Login();
        }

        private void Start() 
        {
            currentLeaderboardInt = 0;
            UpdatePanels();
        }

        #endregion

        #region Private Methods

        private void UpdatePanels()
        {
            //Going through the panels and shutting any on panels off just in case
            for(int i = 0; i < MapPanels.Count; ++i)
            {
                MapPanels[i].levelPanel.gameObject.SetActive(false);
            }

            //Make the relavant
            leaderboardTypeText.text = MapPanels[currentLeaderboardInt].levelWithin.ToString() + " " + MapPanels[currentLeaderboardInt].value.ToString() + " LEADERBOARD";
            valueText.text = MapPanels[currentLeaderboardInt].value.ToString();
            MapPanels[currentLeaderboardInt].levelPanel.gameObject.SetActive(true);
        }

        #endregion

        #region Public Methods

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

        #endregion
    }
}