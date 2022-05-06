////////////////////////////////////////////////////////////
// File: PlayFabManager.cs
// Author: Jack Peedle
// Date Created: 29/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 29/01/22
// Brief: 
////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using L7Games;
using L7Games.Movement;
using System.IO;
using L7Games.Loading;

namespace L7Games
{
    public class PlayFabManager : MonoBehaviour
    {
        string levelname;

        public ReplaySaveManager replaySaveManager;      
        public TempScoreSystem tempScoreSystem;

        public HUD HUDScript;

        public GameObject rowPrefab;
        public Transform rowsParent;
        public TMP_InputField TMPPlayerName;
        public int time;
        public int KOs;
        public GameObject SubmittedNameImage;
        public GameObject SubmitNameButtonGameObject;

        public bool hasFetchedLeaderboard;

        [SerializeField]
        public static TMP_InputField PlayerName;

        void Start()
        {
            levelname = "Tutorial";

            switch(LoadingData.currentLevel)
            {
                case LEVEL.CITY:
                    levelname = "City";
                    break;
                case LEVEL.OLDTOWN:
                    levelname = "OldTown";
                    break;
            }

            Login();

            hasFetchedLeaderboard = false;
        }

        // Login
        void Login()
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

            Debug.Log("Successful login / account create!");
            string name = null;
            if(result.InfoResultPayload.PlayerProfile != null)
            {
                name = result.InfoResultPayload.PlayerProfile.DisplayName;
            }
        }

        public void GetMainMenuTutorialLeaderboard()
        {
            var TutorialScorerequest = new GetLeaderboardRequest
            {
                StatisticName = "Tutorial_Score",
                StartPosition = 0
            };

            var TutorialTimerequest = new GetLeaderboardRequest
            {
                StatisticName = "Tutorial_Time",
            };

            var TutorialKOsrequest = new GetLeaderboardRequest
            {
                StatisticName = "Tutorial_KOs",
                StartPosition = 0
            };

            PlayFabClientAPI.GetLeaderboard(TutorialScorerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(TutorialTimerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(TutorialKOsrequest, OnLeaderBoardGet, OnError);
        }

        public void GetMainMenuCityLeaderboard() 
        {
            var CityScorerequest = new GetLeaderboardRequest
            {
                StatisticName = "City_Score",
                StartPosition = 0
            };

            var CityTimerequest = new GetLeaderboardRequest
            {
                StatisticName = "City_Time",
            };

            var CityKOsrequest = new GetLeaderboardRequest
            {
                StatisticName = "City_KOs",
                StartPosition = 0
            };

            PlayFabClientAPI.GetLeaderboard(CityScorerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(CityTimerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(CityKOsrequest, OnLeaderBoardGet, OnError);
        }

        public void GetMainMenuOldTownLeaderboard()
        {
            var OldTownScorerequest = new GetLeaderboardRequest
            {
                StatisticName = "OldTown_Score",
                StartPosition = 0
            };

            var OldTownTimerequest = new GetLeaderboardRequest
            {
                StatisticName = "OldTown_Time",
            };

            var OldTownKOsrequest = new GetLeaderboardRequest
            {
                StatisticName = "OldTown_KOs",
                StartPosition = 0
            };

            PlayFabClientAPI.GetLeaderboard(OldTownScorerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(OldTownTimerequest, OnLeaderBoardGet, OnError);
            PlayFabClientAPI.GetLeaderboard(OldTownKOsrequest, OnLeaderBoardGet, OnError);
        }

        public void FinishedLevelTriggered()
        {
            StartCoroutine(Co_WaitToUpdateLeaderBoard());
        }

        private IEnumerator Co_WaitToUpdateLeaderBoard()
        {
            yield return new WaitForSeconds(3f);

            SendLeaderBoards();
            GetLeaderBoard();
        }

        public void SubmitNameButton() 
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = TMPPlayerName.text,
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

            SubmittedNameImage.SetActive(false);
            SubmitNameButtonGameObject.SetActive(false);
        }

        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("Updated Display Name");
        }

        void OnError(PlayFabError error)
        {
            Debug.Log("Error login / Creating Account");
            Debug.Log(error.GenerateErrorReport());
        }

        public void SendLeaderBoards()
        {
            SendScoreLeaderBoard(Mathf.RoundToInt(HUDScript.storedScore), levelname);
            SendTimeLeaderBoard(time, levelname);
            SendKOsLeaderBoard(KOs, levelname);
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

            PlayFabClientAPI.GetLeaderboard(Scorerequest, OnLeaderBoardGet, OnError);

            var Timerequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_Time",
            };

            PlayFabClientAPI.GetLeaderboard(Timerequest, OnLeaderBoardGet, OnError);

            var KOsrequest = new GetLeaderboardRequest
            {
                StatisticName = levelname + "_KOs",
                StartPosition = 0
            };

            PlayFabClientAPI.GetLeaderboard(KOsrequest, OnLeaderBoardGet, OnError);
        }

        void OnLeaderBoardGet(GetLeaderboardResult result)
        {
            // for each row in the leaderboard
            foreach (Transform item in rowsParent)
            {
                // destroy them to be instantiated
                Destroy(item.gameObject);
            }

            foreach (var item in result.Leaderboard) 
            {
                GameObject newGO = Instantiate(rowPrefab, rowsParent);

                Debug.Log("INSTANTIATED ROW PREFAB");

                TMP_Text[] texts = newGO.GetComponentsInChildren<TMP_Text>();
                texts[0].text = (item.Position + 1).ToString();
                texts[1].text = item.DisplayName;
                texts[2].text = item.StatValue.ToString();              
                texts[3].text = time.ToString();
                texts[4].text = KOs.ToString();

                Debug.Log(string.Format("RANK: {0} | Text: {1} | VALUE: {2} | VALUE: {3} | VALUE: {4}", item.Position, item.DisplayName, Mathf.RoundToInt(HUDScript.storedScore), time, KOs)); //item.StatValue
            }          
        }
    }
}
