////////////////////////////////////////////////////////////
// File: PlayFabManager.cs
// Author: Jack Peedle
// Date Created: 29/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 29/01/22
// Brief: 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

namespace SleepyCat
{
    public class PlayFabManager : MonoBehaviour
    {

        //
        public ReplaySaveManager replaySaveManager;

        //
        public TempScoreSystem tempScoreSystem;

        //
        public GameObject rowPrefab;

        //
        public Transform rowsParent;

        //
        public TMP_InputField TMPPlayerName;

        //
        public int score;

        //
        public int time;

        //
        public int KOs;

        //
        public GameObject SubmittedNameImage;

        //
        public GameObject SubmitNameButtonGameObject;


        //
        void Start() {

            //
            Login();

        }

        // Login
        void Login() {

            // Login request
            var request = new LoginWithCustomIDRequest {

                // make a new account with a custom ID
                CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true,

                //
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {

                    //
                    GetPlayerProfile = true

                }
                //

            };

            // Loging with ID, request and a success and error function
            PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);

        }

        //
        void OnSuccess(LoginResult result) {

            //
            Debug.Log("Successful login / account create!");

            //
            string name = null;

            //
            if (result.InfoResultPayload.PlayerProfile != null)
                //
                name = result.InfoResultPayload.PlayerProfile.DisplayName;

        }

        //
        public void SubmitNameButton() {

            //
            var request = new UpdateUserTitleDisplayNameRequest {

                //
                DisplayName = TMPPlayerName.text,

            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

            //
            SubmittedNameImage.SetActive(false);

            //
            SubmitNameButtonGameObject.SetActive(false);

        }

        //
        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result) {

            //
            Debug.Log("Updated Display Name");

        }

        //
        void OnError(PlayFabError error) {

            //
            Debug.Log("Error login / Creating Account");

            //
            Debug.Log(error.GenerateErrorReport());

        }


        //
        public void SetScore() {

            //
            score = tempScoreSystem.PlayerScore;

            //
            time = Mathf.RoundToInt(tempScoreSystem.PlayerTime);

        }

        //
        public void SendLeaderBoards() {

            if (replaySaveManager.isMapTutorial) {

                //
                SendTutorialScoreLeaderBoard(score);

                //
                SendTutorialTimeLeaderBoard(time);

                //
                SendTutorialKOsLeaderBoard(KOs);

            }

            if (replaySaveManager.isMapCity) {

                //
                SendCityScoreLeaderBoard(score);

                //
                SendCityTimeLeaderBoard(time);

                //
                SendCityKOsLeaderBoard(KOs);

            }



        }


        //
        public void SendTutorialScoreLeaderBoard(int score) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "Tutorial_Score",

                        //
                        Value = score,

                    }

                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        //
        public void SendCityScoreLeaderBoard(int score) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "City_Score",

                        //
                        Value = score,

                    }

                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        //
        public void SendTutorialTimeLeaderBoard(int time) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "Tutorial_Time",

                        //
                        Value = time,

                    }


                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        //
        public void SendCityTimeLeaderBoard(int time) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "City_Time",

                        //
                        Value = time,

                    }


                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        //
        public void SendTutorialKOsLeaderBoard(int KOs) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "Tutorial_KOs",

                        //
                        Value = KOs,

                    }


                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        //
        public void SendCityKOsLeaderBoard(int KOs) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "City_KOs",

                        //
                        Value = KOs,

                    }


                }


            };

            //
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }


        //
        void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result) {

            //
            Debug.Log("Successful leaderboard sent");

        }

        //
        public void GetLeaderBoard() {

            if (replaySaveManager.isMapTutorial) {

                //
                var TutorialScorerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "Tutorial_Score",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                var TutorialTimerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "Tutorial_Time",
                    //StartPosition = 0 //,
                    //MaxResultsCount = 3,

                };

                //
                var TutorialKOsrequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "Tutorial_KOs",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(TutorialScorerequest, OnLeaderBoardGet, OnError);

                //
                PlayFabClientAPI.GetLeaderboard(TutorialTimerequest, OnLeaderBoardGet, OnError);

                //
                PlayFabClientAPI.GetLeaderboard(TutorialKOsrequest, OnLeaderBoardGet, OnError);

            }

            if (replaySaveManager.isMapCity) {

                //
                var CityScorerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "City_Score",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                var CityTimerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "City_Time",
                    //StartPosition = 0 //,
                    //MaxResultsCount = 3,

                };

                //
                var CityKOsrequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "City_KOs",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(CityScorerequest, OnLeaderBoardGet, OnError);

                //
                PlayFabClientAPI.GetLeaderboard(CityTimerequest, OnLeaderBoardGet, OnError);

                //
                PlayFabClientAPI.GetLeaderboard(CityKOsrequest, OnLeaderBoardGet, OnError);

            }

        }

        //
        void OnLeaderBoardGet(GetLeaderboardResult result) {

            Debug.Log("TTTTTTTTTTTTTTTTTTTTT");

            // for each row in the leaderboard
            foreach (Transform item in rowsParent) {

                // destroy them to be instantiated
                Destroy(item.gameObject);

            }

            //
            foreach (var item in result.Leaderboard) {

                //
                GameObject newGO = Instantiate(rowPrefab, rowsParent);

                //
                Text[] texts = newGO.GetComponentsInChildren<Text>();

                //
                texts[0].text = (item.Position + 1).ToString();

                //
                texts[1].text = TMPPlayerName.text; //+ item.PlayFabId; //playerName.text + "(" + THIS +")";

                //
                texts[2].text = score.ToString();

                //
                texts[3].text = time.ToString();

                //
                texts[4].text = KOs.ToString();

                //
                Debug.Log(string.Format("RANK: {0} | Text: {1} | VALUE: {2} | VALUE: {3} | VALUE: {4}", item.Position, item.DisplayName, score, time, KOs)); //item.StatValue

                // debug position, ID and score for each item
                //Debug.Log(item.Position + "" + item.PlayFabId + "" + item.StatValue);

            }

        }

    }
}
