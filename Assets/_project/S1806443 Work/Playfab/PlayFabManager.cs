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
using Newtonsoft.Json;
using UnityEngine.UI;

namespace SleepyCat
{
    public class PlayFabManager : MonoBehaviour
    {

        //
        public GameObject rowPrefab;

        //
        public Transform rowsParent;

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
                CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true

            };

            // Loging with ID, request and a success and error function
            PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);

        }

        //
        void OnSuccess(LoginResult result) {

            //
            Debug.Log("Successful login / account create!");

        }

        //
        void OnError(PlayFabError error) {

            //
            Debug.Log("Error login / Creating Account");

            //
            Debug.Log(error.GenerateErrorReport());

        }

        //
        public void SendLeaderBoard(int score) {

            //
            var request = new UpdatePlayerStatisticsRequest {

                //
                Statistics = new List<StatisticUpdate> {

                    //
                    new StatisticUpdate {

                        //
                        StatisticName = "Score",

                        //
                        Value = score

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

            //
            var request = new GetLeaderboardRequest {

                //
                StatisticName = "Score",
                StartPosition = 0 //,
                //MaxResultsCount = 10

            };

            //
            PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);

        }

        //
        void OnLeaderBoardGet(GetLeaderboardResult result) {

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
                texts[1].text = item.PlayFabId;

                //
                texts[2].text = item.StatValue.ToString();

                //
                Debug.Log(string.Format("RANK: {0} | ID: {1} | VALUE: {2}", item.Position, item.PlayFabId, item.StatValue));

                // debug position, ID and score for each item
                //Debug.Log(item.Position + "" + item.PlayFabId + "" + item.StatValue);

            }

        }

    }
}
