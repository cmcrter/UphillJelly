////////////////////////////////////////////////////////////
// File: ReplaySaveManager.cs
// Author: Jack Peedle
// Date Created: 04/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 04/10/21
// Brief: Script to store the data of the ghosts poisition and rotation 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ReplaySaveManager : MonoBehaviour
{

    #region variables

    // reference to the ghost
    public Ghost ghost;

    #endregion

    #region replaySaveManager check

    // public static for this replaySaveManager
    public static ReplaySaveManager replaySaveManager;

    // on awake
    void Awake() {
        
        // if the replay save manager is = nothing
        if (replaySaveManager == null) {

            // set the replaysavemanager to this
            replaySaveManager = this;

        } 
        // if the replaysavemanager is not = to this
        else if (replaySaveManager != this) {

            // destroy this
            Destroy(this);

        }

        // do not destroy this on load
        DontDestroyOnLoad(this);
        
    }

    #endregion

    // bool for if there is a save file
    public bool IsReplaySaveFile() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/replay_save");

    }

    #region Methods

    // public void for save replay
    public void SaveReplay() {

        // if there is not a save file for the replay
        if (!IsReplaySaveFile()) {

            // create a save file in the data path folder called /replay_save
            Directory.CreateDirectory(Application.persistentDataPath + "/replay_save");

        }

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/replay_save/replay_data")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/replay_save/replay_data");

        }

        // create a new binary formatter called replay_bf
        BinaryFormatter replay_bf = new BinaryFormatter();

        // create a filestream in the replay data and call it "replay_SavedData"
        FileStream replay_file = File.Create(Application.persistentDataPath + "/replay_save/replay_data/replay_SavedData.txt");

        // pass in ghost object and save public variables
        var json = JsonUtility.ToJson(ghost);

        // serializes the data to binary format in a json file
        replay_bf.Serialize(replay_file, json);

        // close the replay file
        replay_file.Close();


        
    }

    #endregion


    // load the replay
    public void LoadReplay() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/replay_save/replay_data")) {

            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/replay_save/replay_data");

        }

        // create a new binary formatter called replay_bf
        BinaryFormatter replay_bf = new BinaryFormatter();

        // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
        if (File.Exists(Application.persistentDataPath + "/replay_save/replay_data/replay_SavedData.txt")) {

            // open the file "/replay_save/replay_data/replay_SavedData.txt"
            FileStream file = File.Open(Application.persistentDataPath + "/replay_save/replay_data/replay_SavedData.txt", FileMode.Open);

            //deseralize the ghost file 
            JsonUtility.FromJsonOverwrite((string)replay_bf.Deserialize(file), ghost);

            // close the file
            file.Close();

        }

    }



}
