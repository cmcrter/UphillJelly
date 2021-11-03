////////////////////////////////////////////////////////////
// File: b_SaveSystem.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 25/10/21
// Brief: A script to control the binary formatters to save and load data using filestreams 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Static so there won't be multiple versions or instantiations of this script
public static class b_SaveSystem
{

    #region Variables

    #endregion

    #region Methods

    // Save the player and take in data from the b_Player script
    public static void SavePlayer(Shop shopData, OutfitChanger outfitChangerData) {

        // new binary formatter
        BinaryFormatter formatter = new BinaryFormatter();

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Player.sdat";

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
    public static b_PlayerData LoadPlayer() {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path = Application.persistentDataPath + "/Player.sdat";

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
