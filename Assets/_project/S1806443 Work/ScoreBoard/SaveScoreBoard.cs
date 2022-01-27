////////////////////////////////////////////////////////////
// File: SaveScoreBoard.cs
// Author: Jack Peedle
// Date Created: 27/01/21
// Last Edited By: Jack Peedle
// Date Last Edited: 27/01/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

namespace SleepyCat
{
    public static class SaveScoreBoard{

        //
        public static void SaveLeaderBoard(Score score) {

            // if a directory doesn't exists for "/LeaderBoard"
            if (!Directory.Exists(Application.persistentDataPath + "/LeaderBoard")) {

                // create a directory for "/LeaderBoard"
                Directory.CreateDirectory(Application.persistentDataPath + "/LeaderBoard");

            }

            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a string called "path" which is the persistent data path %AppData% and call the file LeaderBoardData.sdat
            string path = Application.persistentDataPath + "/LeaderBoard/LeaderBoardData.sdat";

            // create a new filestream taking in the "path" string
            FileStream stream = new FileStream(path, FileMode.Create);

            // 
            SavedScoreData data = new SavedScoreData(score);

            // serialize the stream and data, data
            formatter.Serialize(stream, data);

            // close the stream
            stream.Close();

        }


        // Load the player and take in data from the b_PlayerData script
        public static SavedScoreData LoadScoreBoard() {

            // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
            string path = Application.persistentDataPath + "/LeaderBoard/LeaderBoardData.sdat";

            // if a file exists in the "path"
            if (File.Exists(path)) {

                // new binary formatter
                BinaryFormatter formatter = new BinaryFormatter();

                // create a new filestream taking in the "path" string and open it
                FileStream stream = new FileStream(path, FileMode.Open);

                // deserialize the stream data as b_PlayerData
                SavedScoreData data = formatter.Deserialize(stream) as SavedScoreData;

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

    }
}
