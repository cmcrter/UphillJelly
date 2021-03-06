////////////////////////////////////////////////////////////
// File: PlayFabManager.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/01/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/06/22
// Brief: A script to manage retrieving and sending data for the leaderboards.
////////////////////////////////////////////////////////////

using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using L7Games.Loading;
using UnityEngine.UI;
using System.Collections;

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

        //private List<CompiledLeaderboardRow> allresults = new List<CompiledLeaderboardRow>();
        //private List<PlayerLeaderboardEntry> allEntires = new List<PlayerLeaderboardEntry>();

        private string levelname;

        private leaderboard_value currentLeadboardToPopulate = leaderboard_value.SCORE;
        public List<Transform> LeaderboardPanels;
        public List<Transform> CurrentScorePanels;
        public List<Transform> LeaderboardRowObjects;

        public List<Scrollbar> scrollBars = new List<Scrollbar>();

        [SerializeField]
        private int maxLeaderboardRows = 50;

        [SerializeField]
        private List<LeaderboardData> valuesFilledIn = new List<LeaderboardData>();
        public List<PlayerLeaderboardEntry> scoresEntries = new List<PlayerLeaderboardEntry>();
        public List<PlayerLeaderboardEntry> timerEntries = new List<PlayerLeaderboardEntry>();
        public List<PlayerLeaderboardEntry> wipeouteEntries = new List<PlayerLeaderboardEntry>();

        string name;

        [Header("Overrides")]
        [SerializeField]
        private bool overrides;
        [SerializeField]
        private string overrideName;

        private bool loggedIn= false;

        void Start()
        {
            levelname = LoadingData.getLevelString(LoadingData.currentLevel);

            if(LoadingData.currentLevel != LEVEL.MAINMENU)
            {
                Login();
            }
        }

        private void OnEnable()
        {
            //Login();
        }

        private void SetName()
        {
            if(LoadingData.player != null)
            {
                name = LoadingData.player.profileName != null ? LoadingData.player.profileName : overrideName;
            }
            else
            {
                name = "Dev";
            }

            if(overrides)
            {
                name = overrideName;
            }
        }

        public void Login()
        {
            SetName();

            // Login request
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = SystemInfo.deviceUniqueIdentifier + name, 
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };

            // Logging with ID, request and a success and error function
            PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        }

        void OnSuccess(LoginResult result)
        {
            loggedIn = true;

            if(Debug.isDebugBuild)
            {
                Debug.Log("Successful login / account create!" + " " + result.PlayFabId);
            }

            //The map is finished
            if(LoadingData.currentLevel == LEVEL.MAINMENU)
            {
                //Get all leaderboards and put it in relative sections
                GetAllLeaderboards();
            }

            UpdateUserTitleDisplayNameRequest UpdateRequest = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name,
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(UpdateRequest, OnDisplayNameUpdate, OnError);
        }

        //This is specifically for the main menu
        public void GetAllLeaderboards()
        {
            for(int j = (int)LEVEL.TUTORIAL; j <= (int)LEVEL.OLDTOWN; ++j)
            {
                for(int i = 0; i < (int)leaderboard_value.COUNT; ++i)
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
                        default:
                            if(Debug.isDebugBuild)
                            {
                                Debug.Log("Value Not Recognised: " + i, this);
                            }
                            break;
                    }

                    GetLeaderboardRequest request = new GetLeaderboardRequest
                    {
                        StatisticName = thisLevelName + thisFeature,
                        StartPosition = 0,
                        MaxResultsCount = 50
                    };

                    //Debug.Log("Getting values for: " + ((LEVEL)j).ToString() + " on value: " + ((leaderboard_value)i).ToString());

                    LeaderboardData data = new LeaderboardData((LEVEL)j, (leaderboard_value)i);
                    PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError, data);
                }             
            }
        }

        public void FinishedLevelTriggered(RankBrackets sessionBracket)
        {
            if(loggedIn)
            {
                SendLeaderBoards(sessionBracket);
                GetLeaderBoard();
            }
        }

        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            if(Debug.isDebugBuild)
            {
                Debug.Log("Updated Display Name" + " " + result.DisplayName);
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
                StartPosition = 0,
                MaxResultsCount = 50
            };

            LeaderboardData data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.SCORE);
            PlayFabClientAPI.GetLeaderboard(Scorerequest, OnLeaderBoardGet, OnError, data);

            var Timerequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_Time",
                StartPosition = 0,
                MaxResultsCount = 50
            };

            data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.TIME);
            PlayFabClientAPI.GetLeaderboard(Timerequest, OnLeaderBoardGet, OnError, data);

            var KOsrequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_KOs",
                StartPosition = 0,
                MaxResultsCount = 50
            };

            data = new LeaderboardData(LoadingData.currentLevel, leaderboard_value.WIPEOUTs);
            PlayFabClientAPI.GetLeaderboard(KOsrequest, OnLeaderBoardGet, OnError, data);

            //Putting all the leaderboards together cohesively (back when it would be 1 leaderboard)
            //CompileLeadboards();
        }

        void OnLeaderBoardGet(GetLeaderboardResult result)
        {
            LeaderboardData data = (LeaderboardData)result.CustomData;

            for(int i = 0; i < valuesFilledIn.Count; ++i)
            {
                if(valuesFilledIn[i].Level == data.Level && valuesFilledIn[i].valueToRetrieve == data.valueToRetrieve)
                {
                    return;
                }
            }

            ClearEntryList(data.valueToRetrieve);

            //Time and Wipeouts should be shown with lowest being the highest ranked
            if(data.valueToRetrieve == leaderboard_value.TIME || data.valueToRetrieve == leaderboard_value.WIPEOUTs) 
            {
                result.Leaderboard = ReverseListIncludingRank(result.Leaderboard);
            }

            int resultCount = 0;

            Debug.Log(result.Leaderboard.Count + " entries that could be displayed");

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
                    resultCount++;
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

                //Logging the entries to the correct lists
                AddEntryToList(data.valueToRetrieve, entry);
            }

            if(LoadingData.currentLevel != LEVEL.MAINMENU)
            {
                StartCoroutine(Co_ScrollPosCorrection((int)data.valueToRetrieve, resultCount));
            }

            valuesFilledIn.Add(data);
        }
       
        public void SwitchPanel(int newValue)
        {
            for(int i = 0; i < LeaderboardPanels.Count; ++i)
            {
                LeaderboardPanels[i].gameObject.SetActive(false);
                CurrentScorePanels[i].gameObject.SetActive(false);
            }

            if(scrollBars[newValue] != null)
            {
                scrollBars[newValue].value = 1f;
            }

            LeaderboardPanels[newValue].gameObject.SetActive(true);
            CurrentScorePanels[newValue].gameObject.SetActive(true);

            if(Debug.isDebugBuild)
            {
                Debug.Log("Switching panel to: " + (leaderboard_value)newValue);
            }
        }

        public static string GetPredictedPosition(float Val, List<PlayerLeaderboardEntry> entries, bool descending)
        {
            int currentPos = 0;

            foreach(PlayerLeaderboardEntry entry in entries)
            {
                if(!descending)
                {
                    if(Val >= entry.StatValue)
                    {
                        currentPos++;
                        return currentPos.ToString();
                    }
                }
                else
                {
                    if(Val <= entry.StatValue)
                    {
                        currentPos++;
                        return currentPos.ToString();
                    }
                }

                currentPos++;
            }

            return (entries.Count + 1).ToString();
        }

        private void ClearEntryList(leaderboard_value valueToRetrieve)
        {
            //Logging the entries to the correct lists
            switch(valueToRetrieve)
            {
                case leaderboard_value.SCORE:
                    scoresEntries.Clear();
                    break;
                case leaderboard_value.TIME:
                    timerEntries.Clear();
                    break;
                case leaderboard_value.WIPEOUTs:
                    wipeouteEntries.Clear();
                    break;
            }
        }

        private void AddEntryToList(leaderboard_value valueToRetrieve, PlayerLeaderboardEntry entry)
        {
            switch(valueToRetrieve)
            {
                case leaderboard_value.SCORE:
                    scoresEntries.Add(entry);
                    break;
                case leaderboard_value.TIME:
                    timerEntries.Add(entry);
                    break;
                case leaderboard_value.WIPEOUTs:
                    wipeouteEntries.Add(entry);
                    break;
            }
        }

        private static List<PlayerLeaderboardEntry> ReverseListIncludingRank(List<PlayerLeaderboardEntry> givenList)
        {
            givenList.Reverse();

            for(int i = 0; i < givenList.Count; ++i)
            {
                givenList[i].Position = i;
            }

            return givenList;
        }

        private IEnumerator Co_ScrollPosCorrection(int index, int AmountofItems)
        {
            yield return null;

            if(index < scrollBars.Count)
            {
                if(scrollBars[index] != null)
                {
                    scrollBars[index].size = 1f / AmountofItems;
                    scrollBars[index].value = 1f;
                }
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
