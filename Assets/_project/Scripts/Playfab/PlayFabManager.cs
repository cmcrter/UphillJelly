////////////////////////////////////////////////////////////
// File: PlayFabManager.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/01/22
// Last Edited By: Charles Carter
// Date Last Edited: 07/05/22
// Brief: A script to manage retrieving and sending data for the leaderboards.
////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using L7Games.Loading;
using L7Games.Movement;

namespace L7Games
{
    public enum leaderboard_value
    {
        SCORE = 0,
        TIME = 1,
        WIPEOUTs = 2,
        COUNT
    }

    //public class CompiledLeaderboardRow
    //{
    //    public int compiled_rank;
    //    public string compiled_sessionID;
    //    public string compiled_name;
    //    public int compiled_score;
    //    public int compiled_time;
    //    public int compiled_KOs;

    //    public CompiledLeaderboardRow(string sessionID, string name,  int score, int time, int KOs)
    //    {
    //        compiled_sessionID = sessionID;
    //        compiled_name = name;
    //        compiled_score = score;
    //        compiled_time = time;
    //        compiled_KOs = KOs;
    //    }
    //}

    //A data container for the 
    [System.Serializable]
    public class LeaderboardData
    {
        public LEVEL Level;
        public leaderboard_value valueToRetrieve;

        public LeaderboardData(LEVEL thisLevel, leaderboard_value thisvalueToRetrieve)
        {
            Level = thisLevel;
            valueToRetrieve = thisvalueToRetrieve;
        }
    }

    public class PlayFabManager : MonoBehaviour
    {
        [Header("Necessary Variables")]
        public ReplaySaveManager replaySaveManager;

        [Header("UI Values")]
        public GameObject rowPrefab;
        public Transform rowsParent;
        //public TMP_InputField TMPPlayerName;
        //public int KOs;
        //public GameObject SubmittedNameImage;
        //public GameObject SubmitNameButtonGameObject;

        //private List<CompiledLeaderboardRow> allresults = new List<CompiledLeaderboardRow>();
        //private List<PlayerLeaderboardEntry> allEntires = new List<PlayerLeaderboardEntry>();
        private string levelname;

        private leaderboard_value currentLeadboardToPopulate = leaderboard_value.SCORE;
        public List<Transform> LeaderboardPanels;
        public List<Transform> LeaderboardRowObjects;

        [SerializeField]
        int maxLeaderboardRows = 10;

        [SerializeField]
        private List<LeaderboardData> valuesFilledIn = new List<LeaderboardData>();

        string name;

        [Header("Overrides")]
        [SerializeField]
        private bool overrides;
        [SerializeField]
        private string overrideName;

        void Start()
        {
            levelname = LoadingData.getLevelString(LoadingData.currentLevel);

            Login();
        }

        private void OnEnable()
        {
            //Login();
        }

        public void Login()
        {
            // Login request
            var request = new LoginWithCustomIDRequest
            {
                // make a new account with a custom ID
                CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };

            // Loging with ID, request and a success and error function
            PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        }

        void OnSuccess(LoginResult result)
        {
            if(Debug.isDebugBuild)
            {
                Debug.Log("Successful login / account create!");
            }

            string name = null;
            if(result.InfoResultPayload.PlayerProfile != null)
            {
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
            }

            SubmitNameButton();
    
            //The map is finished
            if(LoadingData.currentLevel == LEVEL.MAINMENU)
            {
                //Get all leaderboards and put it in relative sections
                GetAllLeaderboards();
            }
        }

        //This is specifically for the main menu
        public void GetAllLeaderboards()
        {
            for(int j = (int)LEVEL.TUTORIAL; j < (int)LEVEL.COUNT; ++j)
            {
                for(int i = 0; i < 3; ++i)
                {
                    string thisFeature = "";
                    string thisLevelName = LoadingData.getLevelString((LEVEL)j);

                    switch(i)
                    {
                        case (int)leaderboard_value.SCORE:
                            thisFeature = "_Score";
                            break;
                        case (int)leaderboard_value.TIME:
                            thisFeature = "_Time";
                            break;
                        case (int)leaderboard_value.WIPEOUTs:
                            thisFeature = "_KOs";
                            break;
                    }

                    GetLeaderboardRequest request = new GetLeaderboardRequest
                    {
                        StatisticName = thisLevelName + thisFeature,
                        StartPosition = 0
                    };

                    //Debug.Log("Getting values for: " + ((LEVEL)j).ToString() + " on value: " + ((leaderboard_value)i).ToString());

                    LeaderboardData data = new LeaderboardData((LEVEL)j, (leaderboard_value)i);
                    PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError, data);
                }             
            }
        }

        public void FinishedLevelTriggered(RankBrackets sessionBracket)
        {
            SendLeaderBoards(sessionBracket);
            GetLeaderBoard();
        }

        public void SubmitNameButton() 
        {
            if(LoadingData.player != null)
            {
                name = LoadingData.player.profileName != null ? LoadingData.player.profileName : overrideName;

                if(overrides)
                {
                    name = overrideName;
                }
                else if(LoadingData.player.profileName == null)
                {
                    name = "Dev";
                }

            }
            else
            {
                name = "Dev";
            }

            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name,
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        }

        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            if(Debug.isDebugBuild)
            {
                Debug.Log("Updated Display Name");
            }
        }

        void OnError(PlayFabError error)
        {
            if(Debug.isDebugBuild)
            {
                Debug.Log("Error login / Creating Account");
                Debug.Log(error.GenerateErrorReport());
            }
        }

        public void SendLeaderBoards(RankBrackets bracket)
        {
            SendScoreLeaderBoard((int)bracket.score, levelname);
            SendTimeLeaderBoard((int)bracket.seconds, levelname);

            //The threshold is being used to store the count in this
            SendKOsLeaderBoard(bracket.wipeoutThreshold, levelname);
        }

        public void SendScoreLeaderBoard(int score, string levelname)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate 
                    {
                        StatisticName = levelname + "_Score",
                        Value = score,
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
        }

        public void SendTimeLeaderBoard(int time, string levelname) 
        {
            var request = new UpdatePlayerStatisticsRequest 
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = levelname + "_Time",
                        Value = time,
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
        }
       
        public void SendKOsLeaderBoard(int KOs, string levelname)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = levelname + "_KOs",
                        Value = KOs,
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
        }

        void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Successful leaderboard sent");
        }

        public void GetLeaderBoard()
        {
            var Scorerequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_Score",                
                StartPosition = 0
            };

            LeaderboardData data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.SCORE);
            PlayFabClientAPI.GetLeaderboard(Scorerequest, OnLeaderBoardGet, OnError, data);

            var Timerequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_Time",
                StartPosition = 0
            };

            data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.TIME);
            PlayFabClientAPI.GetLeaderboard(Timerequest, OnLeaderBoardGet, OnError, data);

            var KOsrequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_KOs",
                StartPosition = 0
            };

            data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.WIPEOUTs);
            PlayFabClientAPI.GetLeaderboard(KOsrequest, OnLeaderBoardGet, OnError, data);

            //Putting all the leaderboards together cohesively (back when it would be 1 leaderboard)
            //CompileLeadboards();
        }

        void OnLeaderBoardGet(GetLeaderboardResult result)
        {
            LeaderboardData data = (LeaderboardData)result.CustomData;
            int resultCount = 0;

            for(int i = 0; i < valuesFilledIn.Count; ++i)
            {
                if(valuesFilledIn[i].Level == data.Level && valuesFilledIn[i].valueToRetrieve == data.valueToRetrieve)
                {
                    return;
                }
            }

            //Populating this leaderboard
            foreach(PlayerLeaderboardEntry entry in result.Leaderboard)
            {
                GameObject newGO;

                if(LoadingData.currentLevel != LEVEL.MAINMENU)
                {
                    if(resultCount == maxLeaderboardRows || data.Level != LoadingData.currentLevel)
                    {
                        continue;
                    }

                    newGO = Instantiate(rowPrefab, LeaderboardRowObjects[(int)data.valueToRetrieve]);
                }
                else
                {
                    int mapStartVal = 0;
                    switch(data.Level)
                    {
                        case LEVEL.CITY:
                            mapStartVal = 3;
                            break;
                        case LEVEL.OLDTOWN:
                            mapStartVal = 6;
                            break;
                    }

                    int transformIndex = mapStartVal + (int)data.valueToRetrieve;

                    //Debug.Log(transformIndex, this);

                    newGO = Instantiate(rowPrefab, LeaderboardRowObjects[transformIndex]);
                }

                if(newGO.TryGetComponent(out LeaderBoardRow row))
                {
                    row.SetRowTexts(entry);
                }

                resultCount++;
            }

            valuesFilledIn.Add(data);
        }
       
        public void SwitchPanel(int newValue)
        {
            for(int i = 0; i < LeaderboardPanels.Count; ++i)
            {
                LeaderboardPanels[i].gameObject.SetActive(false);
            }

            LeaderboardPanels[newValue].gameObject.SetActive(true);

            if(Debug.isDebugBuild)
            {
                Debug.Log("Switching panel to: " + (leaderboard_value)newValue);
            }
        }

        //        private List<CompiledLeaderboardRow> CreateCompiledPosition()
        //        {
        //            List<CompiledLeaderboardRow> CompiledPositions = new List<CompiledLeaderboardRow>();
        //
        //            foreach(PlayerLeaderboardEntry entry in allEntires)
        //            {
        //                //Need a way to remove duplicate runs, so each row has all the stats.
        //            }

        //            return CompiledPositions;
        //        }

        //        private void CompileLeadboards()
        //        {
        //            //Each this should have a session ID attached, using that you can build a good leaderboard
        //            foreach(CompiledLeaderboardRow item in allresults)
        //            {
        //                GameObject newGO = Instantiate(rowPrefab, rowsParent);

        //                if(newGO.TryGetComponent(out LeaderBoardRow row))
        //                {
        //                    row.SetRowTexts(item, KOs);
        //                }

        //                Debug.Log(string.Format("RANK: {0} | Text: {1} | VALUE: {2} | VALUE: {3} | VALUE: {4}", item.compiled_rank.ToString(), item.compiled_name.ToString()));
        //            }
        //        }
    }
}
