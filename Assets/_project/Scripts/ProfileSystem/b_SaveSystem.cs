////////////////////////////////////////////////////////////
// File: b_SaveSystem.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 24/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 23/05/22
// Brief: A script to control the binary formatters to save and load data using filestreams 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using L7Games.Loading;

// Static so there won't be multiple versions or instantiations of this script
public static class b_SaveSystem
{
    #region Methods

    public static IEnumerator Co_SavePlayer(int playerSlot)
    {
        SavePlayer(playerSlot);
        yield return true;
    }

    // Save the player and take in data from the b_Player script
    public static void SavePlayer(int playerSlot)
    {
        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/ProfileData")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/ProfileData");
        }

        // new binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/ProfileData/Profile" + playerSlot.ToString() + "Data.sdat";

        // create a new filestream taking in the "path" string
        FileStream stream = new FileStream(path, FileMode.Create);

        StoredPlayerProfile data = new StoredPlayerProfile();
        if(LoadingData.player != null)
        {
            data = LoadingData.player;
        }

        // serialize the stream and data, data
        formatter.Serialize(stream, data);

        // close the stream
        stream.Close();
    }

    // Load the player and take in data from the b_PlayerData script
    public static StoredPlayerProfile LoadPlayer(int playerSlot)
    {
        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/ProfileData/Profile" + playerSlot.ToString() + "Data.sdat";

        // if a file exists in the "path"
        if (File.Exists(path)) 
        {
            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a new filestream taking in the "path" string and open it
            FileStream stream = new FileStream(path, FileMode.Open);

            // deserialize the stream data as b_PlayerData
            StoredPlayerProfile data = formatter.Deserialize(stream) as StoredPlayerProfile;

            // close the stream
            stream.Close();

            // return the data
            return data;
        } 
        else 
        {
            //  debug log which outputs the save was not found in the "path"
            if(Debug.isDebugBuild)
            {
                Debug.LogError("Save file not found in " + path);
            }

            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile" + playerSlot.ToString());

            // return null
            return new StoredPlayerProfile();
        }
    }
    #endregion
}
