////////////////////////////////////////////////////////////
// File: PlayFabManager.cs
// Author: Jack Peedle
// Date Created: 29/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 12/03/22
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

        // Manager references
        public ReplaySaveManager replaySaveManager;
        public TempScoreSystem tempScoreSystem;

        // Prefabs for the rows and display objects for the leaderboard
        public GameObject rowPrefab;
        public Transform rowsParent;
        public TMP_InputField TMPPlayerName;

        // Ints to send and display in the leaderboard
        public int score;
        public int time;
        public int KOs;

        // Visuals for the player name
        public GameObject SubmittedNameImage;
        public GameObject SubmitNameButtonGameObject;

        #region Start And Login
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

        #endregion


        // on success of a login result
        void OnSuccess(LoginResult result) {

            // SUCCESS
            Debug.Log("Successful login / account create!");

            // set the name to null
            string name = null;

            // if name = null, set to the input player name
            if (result.InfoResultPayload.PlayerProfile != null)
                
                name = result.InfoResultPayload.PlayerProfile.DisplayName;

        }

        // submit the player name
        public void SubmitNameButton() {

            // Request to update DisplayName
            var request = new UpdateUserTitleDisplayNameRequest {

                // Set the name with TMP
                DisplayName = TMPPlayerName.text,

            };

            // Update Display name function
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);

            // Set the submit name button to false etc
            SubmittedNameImage.SetActive(false);
            SubmitNameButtonGameObject.SetActive(false);

        }

        // Display name updated passing the result
        public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result) {

            // Debug
            Debug.Log("Updated Display Name");

        }

        // On Error
        void OnError(PlayFabError error) {

            // Debug
            Debug.Log("Error login / Creating Account");

            // Generate error report
            Debug.Log(error.GenerateErrorReport());

        }


        // set the scores
        public void SetScore() {

            // set the score to player score
            score = tempScoreSystem.PlayerScore;

            // set player time
            time = Mathf.RoundToInt(tempScoreSystem.PlayerTime);

        }

        // Send leaderboards to PlayFab
        public void SendLeaderBoards() {

            // Check map
            if (replaySaveManager.isMapTutorial) {

                // Send score values
                SendTutorialScoreLeaderBoard(score);
                SendTutorialTimeLeaderBoard(time);
                SendTutorialKOsLeaderBoard(KOs);

            }

            // Check map
            if (replaySaveManager.isMapCity) {

                // Send score values
                SendCityScoreLeaderBoard(score);
                SendCityTimeLeaderBoard(time);
                SendCityKOsLeaderBoard(KOs);

            }



        }


        // Send tutorial score values
        public void SendTutorialScoreLeaderBoard(int score) {

            // request statistics
            var request = new UpdatePlayerStatisticsRequest {

                // update statistics with new list of statistics
                Statistics = new List<StatisticUpdate> {

                    // new statistic update
                    new StatisticUpdate {

                        // tutorial values
                        StatisticName = "Tutorial_Score",
                        Value = score,

                    }

                }


            };

            // send player updated statistics
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);

        }

        // Send city score values
        public void SendCityScoreLeaderBoard(int score) {

            // request statistics
            var request = new UpdatePlayerStatisticsRequest {

                // update statistics with new list of statistics
                Statistics = new List<StatisticUpdate> {

                    // new statistic update
                    new StatisticUpdate {

                        // city values
                        StatisticName = "City_Score",
                        Value = score,

                    }

                }


            };

            // send player updated statistics
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
