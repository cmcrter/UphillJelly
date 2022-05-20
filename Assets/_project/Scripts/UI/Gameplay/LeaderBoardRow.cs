////////////////////////////////////////////////////////////
// File: LeaderBoardRow.cs
// Author: Charles Carter
// Date Created: 07/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 07/05/22
// Brief: A script to contain the data for a row on the leaderboard
//////////////////////////////////////////////////////////// 

using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace L7Games
{
    public class LeaderBoardRow : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private TextMeshProUGUI[] RowTexts = new TextMeshProUGUI[3];

        #endregion

        #region Public Methods

        public void SetRowTexts(PlayerLeaderboardEntry entry)
        {
            RowTexts[0].text = (entry.Position + 1).ToString();
            RowTexts[1].text = entry.DisplayName;
            RowTexts[2].text = Mathf.Abs(entry.StatValue).ToString();
        }

        #endregion
    }
}