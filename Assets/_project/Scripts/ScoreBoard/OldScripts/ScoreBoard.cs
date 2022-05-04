////////////////////////////////////////////////////////////
// File: ScoreBoard.cs
// Author: Jack Peedle
// Date Created: 22/01/22
// Last Edited By: Jack Peedle
// Date Last Edited: 22/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SleepyCat
{
    public class ScoreBoard : MonoBehaviour
    {

        //
        [SerializeField] private int maxScoreboardEntries = 10;

        //
        [SerializeField] private Transform highScoreHolderTransform = null;

        //
        [SerializeField] private GameObject scoreBoardEntryObject = null;

        [Header("Test")]
        [SerializeField] ScoreBoardEntryData testEntryData;

        //
        public ScoreBoardSavedData scoreBoardSavedData;

        //
        public ScoreBoardData scoreBoardEntryScores;

        //
        //private string SavePath => $"{Application.persistentDataPath}/highscores.json";

        //
        private void Start() {

            SaveHighScores();

            //
            ScoreBoardSavedData savedScores = LoadHighScores();

            //
            UpdateUI(savedScores);

            //
            SaveScores(savedScores);

        }

        //
        [ContextMenu("Add Test Entry")]
        public void AddTestEntry() {

            //
            AddEntry(testEntryData);

        }


        //
        
        public void AddEntry(ScoreBoardEntryData scoreBoardEntryData) {

            //
            ScoreBoardSavedData savedScores = LoadHighScores();

            //
            bool scoreAdded = false;

            //
            for (int i = 0; i < savedScores.highscores.Count; i++) {

                //
                if (scoreBoardEntryData.entryScore > savedScores.highscores[i].entryScore) {

                    //
                    savedScores.highscores.Insert(i, scoreBoardEntryData);

                    //
                    scoreAdded = true;

                    //
                    break;

                }


            }

            //
            if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries) {

                //
                savedScores.highscores.Add(scoreBoardEntryData);

            }

            //
            if(savedScores.highscores.Count > maxScoreboardEntries) {

                //
                savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);

            }

            //
            UpdateUI(savedScores);

            //
            SaveHighScores();

        }

        //
        private void UpdateUI(ScoreBoardSavedData savedScores) {

            //
            foreach(Transform child in highScoreHolderTransform) {

                //
                Destroy(child.gameObject);

            }

            //
            foreach(ScoreBoardEntryData highscore in savedScores.highscores) {

                //
                Instantiate(scoreBoardEntryObject, highScoreHolderTransform).GetComponent<ScoreBoardUI>().initialise(highscore);

            }


        }

        // Save the player3
        public void SaveHighScores() {

            // save the player 3 and pass through the shop and outfitchanger
            SaveScores(scoreBoardSavedData);

            Debug.Log("Saved HighScores");

        }

        // Get the saved scores
        private static void SaveScores(ScoreBoardSavedData scoreBoardSavedData) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Highscores")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Highscores");

            }


            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
            string path = Application.persistentDataPath + "/Highscores/Highscores.sdat";

            // create a new filestream taking in the "path" string
            FileStream stream = new FileStream(path, FileMode.Create);

            // reference b_PlayerData, called data (New b_PlayerData)
            ScoreBoardData data = new ScoreBoardData(scoreBoardSavedData);

            // serialize the stream and data, data
            formatter.Serialize(stream, data);

            // close the stream
            stream.Close();


        }

        // Load the player and take in data from the b_PlayerData script
        public ScoreBoardSavedData LoadHighScores() {

            // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
            string path = Application.persistentDataPath + "/Highscores/Highscores.sdat";

            // if a file exists in the "path"
            if (File.Exists(path)) {

                // new binary formatter
                BinaryFormatter formatter = new BinaryFormatter();

                // create a new filestream taking in the "path" string and open it
                FileStream stream = new FileStream(path, FileMode.Open);

                // deserialize the stream data as b_PlayerData
                ScoreBoardSavedData data = formatter.Deserialize(stream) as ScoreBoardSavedData;

                // close the stream
                stream.Close();

                // return the data
                return data;



            } else {

                //  debug log which outputs the save was not found in the "path"
                Debug.LogError("Save file not found in " + path);

                // return null
                return null;

            }


        }

        /*
        //
        private void SaveScores(ScoreBoardSavedData scoreboardSaveData) {

            //
            using (StreamWriter stream = new StreamWriter(SavePath)) {

                //
                string json = JsonUtility.ToJson(scoreboardSaveData, true);
                stream.Write(json);

            }

        }
        */

    }
}
