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

namespace L7Games
{
    public class PlayFabManager : MonoBehaviour//, ITriggerable
    {

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        //GameObject ITriggerable.ReturnGameObject() => gameObject;
        //void ITriggerable.Trigger(PlayerController player) => PlayerEnteredLeaderboard(player);
        //void ITriggerable.UnTrigger(PlayerController player) => PlayerExitedLeaderboard(player);

        //public GameObject leaderboardGO;

        //
        public ReplaySaveManager replaySaveManager;

        //
        public TempScoreSystem tempScoreSystem;

        public HUD HUDScript;

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

        public bool hasFetchedLeaderboard;

        [SerializeField]
        public static TMP_InputField PlayerName;
        

        /*
        void PlayerEnteredLeaderboard(PlayerController player) {

            FinishedLevelTriggered();
            Debug.Log("TRIGGEREDTRIGGERED");

        }

        void PlayerExitedLeaderboard(PlayerController player) {

            // do nothing

        }
        */

        //
        void Start() {

            //leaderboardGO.SetActive(false);

            //
            Login();

            hasFetchedLeaderboard = false;

        }

        public void Update() {


            //HUDScript.storedScore.ToString(score);
            //score = HUDScript.storedScore;

            score = Mathf.RoundToInt(HUDScript.storedScore);

            //Debug.Log(score);


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

        #region Get Main Menu Leaderboards
        public void GetMainMenuTutorialLeaderboard() {

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

            Debug.Log("Main menu tutorial leaderboard got");

        }

        public void GetMainMenuCityLeaderboard() {

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

            Debug.Log("Main menu city leaderboard got");

        }

        public void GetMainMenuOldTownLeaderboard() {

            //
            var OldTownScorerequest = new GetLeaderboardRequest {

                //
                StatisticName = "OldTown_Score",
                StartPosition = 0 //,
                                  //MaxResultsCount = 3,

            };

            //
            var OldTownTimerequest = new GetLeaderboardRequest {

                //
                StatisticName = "OldTown_Time",
                //StartPosition = 0 //,
                //MaxResultsCount = 3,

            };

            //
            var OldTownKOsrequest = new GetLeaderboardRequest {

                //
                StatisticName = "OldTown_KOs",
                StartPosition = 0 //,
                                  //MaxResultsCount = 3,

            };

            //
            PlayFabClientAPI.GetLeaderboard(OldTownScorerequest, OnLeaderBoardGet, OnError);

            //
            PlayFabClientAPI.GetLeaderboard(OldTownTimerequest, OnLeaderBoardGet, OnError);

            //
            PlayFabClientAPI.GetLeaderboard(OldTownKOsrequest, OnLeaderBoardGet, OnError);

            Debug.Log("Main menu old town leaderboard got");

        }

        #endregion

        /*
        public void TriggerLeaderboardEndMap(PlayerController player) {

            Debug.Log("F");

            // ^^^ how does this know it has hit the map end trigger?
            //
            // Money collected by the player = score to send to leaderboard
            // Submit leaderboard method

            FinishedLevelTriggered();

        }

        public void UnTriggerLeaderboardEndMap() {

            //Doesn't do anything but in-case there's needed functionality later          


        }
        */

        public void FinishedLevelTriggered() {

            //leaderboardGO.SetActive(true);

            Debug.Log("FINISHLEVELTRIGGERED");

            //if (hasFetchedLeaderboard == true) {

            //    return;

            //}

            //
            SendLeaderBoards();

            Debug.Log("SENTTHEBOARD");

            //
            GetLeaderBoard();
            Debug.Log("GETTHEBOARD");

            //hasFetchedLeaderboard = true;

            WaitToUpdateLeaderBoard();

        }

        private IEnumerator WaitToUpdateLeaderBoard() {

            yield return new WaitForSeconds(3f);

            SendLeaderBoards();
            GetLeaderBoard();

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
            //Directory.CreateDirectory(Application.persistentDataPath + "/" + TMPPlayerName.text);

            //File.Create(Application.persistentDataPath + "/" + TMPPlayerName.text);

            //PlayerName.text = Application.persistentDataPath + "/" + TMPPlayerName.text;

            //PlayerName.text = TMPPlayerName.text;

            //Debug.Log(PlayerName.text);

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

                Debug.Log("Tutorial Triggered");

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

            if (replaySaveManager.isMapOldTown) {

                //
                SendOldTownScoreLeaderBoard(score);

                //
                SendOldTownTimeLeaderBoard(time);

                //
                SendOldTownKOsLeaderBoard(KOs);

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
        public void SendOldTownScoreLeaderBoard(int score) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "OldTown_Score",

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
        public void SendOldTownTimeLeaderBoard(int time) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "OldTown_Time",

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
        public void SendOldTownKOsLeaderBoard(int KOs) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "OldTown_KOs",

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
                Debug.Log("GotTutorialScoreRequest");

                //
                PlayFabClientAPI.GetLeaderboard(TutorialScorerequest, OnLeaderBoardGet, OnError);

                //
                var TutorialTimerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "Tutorial_Time",
                    //StartPosition = 0 //,
                    //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(TutorialTimerequest, OnLeaderBoardGet, OnError);

                //
                var TutorialKOsrequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "Tutorial_KOs",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

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
                PlayFabClientAPI.GetLeaderboard(CityScorerequest, OnLeaderBoardGet, OnError);

                //
                var CityTimerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "City_Time",
                    //StartPosition = 0 //,
                    //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(CityTimerequest, OnLeaderBoardGet, OnError);

                //
                var CityKOsrequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "City_KOs",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(CityKOsrequest, OnLeaderBoardGet, OnError);

            }


            if (replaySaveManager.isMapOldTown) {

                //
                var OldTownScorerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "OldTown_Score",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(OldTownScorerequest, OnLeaderBoardGet, OnError);

                //
                var OldTownTimerequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "OldTown_Time",
                    //StartPosition = 0 //,
                    //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(OldTownTimerequest, OnLeaderBoardGet, OnError);

                //
                var OldTownKOsrequest = new GetLeaderboardRequest {

                    //
                    StatisticName = "OldTown_KOs",
                    StartPosition = 0 //,
                                      //MaxResultsCount = 3,

                };

                //
                PlayFabClientAPI.GetLeaderboard(OldTownKOsrequest, OnLeaderBoardGet, OnError);

            }



        }

        //
        void OnLeaderBoardGet(GetLeaderboardResult result) {

            //Debug.Log("TTTTTTTTTTTTTTTTTTTTT");

            // for each row in the leaderboard
            foreach (Transform item in rowsParent) {

                // destroy them to be instantiated
                Destroy(item.gameObject);

            }

            //
            foreach (var item in result.Leaderboard) {

                //
                GameObject newGO = Instantiate(rowPrefab, rowsParent);

                Debug.Log("INSTANTIATED ROW PREFAB");

                //
                TMP_Text[] texts = newGO.GetComponentsInChildren<TMP_Text>();

                //
                texts[0].text = (item.Position + 1).ToString();

                //
                //texts[1].text = TMPPlayerName.text; //+ item.PlayFabId; //playerName.text + "(" + THIS +")";

                texts[1].text = item.DisplayName;

                //
                //texts[2].text = score.ToString();

                texts[2].text = item.StatValue.ToString();

                Debug.Log("3 Texts GOT");

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
