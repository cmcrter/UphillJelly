////////////////////////////////////////////////////////////
// File: b_SaveSystem.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: A script to control the binary formatters to save and load data using filestreams 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

// Static so there won't be multiple versions or instantiations of this script
public static class b_SaveSystem
{


    #region Methods

    // Save the player and take in data from the b_Player script
    public static void SavePlayer1(Shop shopData, OutfitChanger outfitChangerData) {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile1/ProfileData")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/ProfileData");

        }

        // new binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile1/ProfileData/Profile1Data.sdat";

        // create a new filestream taking in the "path" string
        FileStream stream = new FileStream(path, FileMode.Create);

        // reference b_PlayerData, called data (New b_PlayerData)
        b_PlayerData data = new b_PlayerData(shopData, outfitChangerData);

        // serialize the stream and data, data
        formatter.Serialize(stream, data);

        // close the stream
        stream.Close();

    }


    // Save the player and take in data from the b_Player script
    public static void SavePlayer2(Shop shopData, OutfitChanger outfitChangerData) {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile2/ProfileData")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/ProfileData");

        }

        // new binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile2/ProfileData/Profile2Data.sdat";

        // create a new filestream taking in the "path" string
        FileStream stream = new FileStream(path, FileMode.Create);

        // reference b_PlayerData, called data (New b_PlayerData)
        b_PlayerData data = new b_PlayerData(shopData, outfitChangerData);

        // serialize the stream and data, data
        formatter.Serialize(stream, data);

        // close the stream
        stream.Close();

    }


    // Save the player and take in data from the b_Player script
    public static void SavePlayer3(Shop shopData, OutfitChanger outfitChangerData) {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile3/ProfileData")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/ProfileData");

        }

        // new binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile3/ProfileData/Profile3Data.sdat";

        // create a new filestream taking in the "path" string
        FileStream stream = new FileStream(path, FileMode.Create);

        // reference b_PlayerData, called data (New b_PlayerData)
        b_PlayerData data = new b_PlayerData(shopData, outfitChangerData);

        // serialize the stream and data, data
        formatter.Serialize(stream, data);

        // close the stream
        stream.Close();

    }


    // Load the player and take in data from the b_PlayerData script
    public static b_PlayerData LoadPlayer1() {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile1/ProfileData/Profile1Data.sdat";

        // if a file exists in the "path"
        if (File.Exists(path)) {

            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a new filestream taking in the "path" string and open it
            FileStream stream = new FileStream(path, FileMode.Open);

            // deserialize the stream data as b_PlayerData
            b_PlayerData data = formatter.Deserialize(stream) as b_PlayerData;

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


    // Load the player and take in data from the b_PlayerData script
    public static b_PlayerData LoadPlayer2() {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile2/ProfileData/Profile2Data.sdat";

        // if a file exists in the "path"
        if (File.Exists(path)) {

            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a new filestream taking in the "path" string and open it
            FileStream stream = new FileStream(path, FileMode.Open);

            // deserialize the stream data as b_PlayerData
            b_PlayerData data = formatter.Deserialize(stream) as b_PlayerData;

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


    // Load the player and take in data from the b_PlayerData script
    public static b_PlayerData LoadPlayer3() {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Profile3/ProfileData/Profile3Data.sdat";

        // if a file exists in the "path"
        if (File.Exists(path)) {

            // new binary formatter
            BinaryFormatter formatter = new BinaryFormatter();

            // create a new filestream taking in the "path" string and open it
            FileStream stream = new FileStream(path, FileMode.Open);

            // deserialize the stream data as b_PlayerData
            b_PlayerData data = formatter.Deserialize(stream) as b_PlayerData;

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

    #endregion

}
