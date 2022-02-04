////////////////////////////////////////////////////////////
// File: ReplaySaveManager.cs
// Author: Jack Peedle
// Date Created: 04/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
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

    // reference to the ghost
    public Ghost2 ghost2;

    //
    public GameObject Ghost1character;

    //
    public GameObject Ghost2character;

    //
    public GameObject MAPCHECKER;

    //
    public bool isMapTutorial;

    //
    public bool isMapCity;

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

    #region Methods

    //
    public void Start() {
        
        //
        if (MAPCHECKER.tag == "map_tutorial") {

            //
            isMapTutorial = true;

            //
            isMapCity = false;

        }

        //
        if (MAPCHECKER.tag == "map_city") {

            //
            isMapTutorial = false;

            //
            isMapCity = true;

        }

    }

    #region FirstSaveFile

    // bool for if there is a save file
    public bool IsReplaySaveFile1() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile1/Replays");

    }


    // public void for save replay
    public void SaveReplay1() {
        
        //
        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile1()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf1 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file1 = File.Create(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf1.Serialize(replay_file1, json);

            // close the replay file
            replay_file1.Close();

        }

        //
        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile1()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/City_Map");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/City_Map")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/City_Map");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf1 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file1 = File.Create(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf1.Serialize(replay_file1, json);

            // close the replay file
            replay_file1.Close();

        }

    }

    // load the replay
    public void LoadReplay1() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf1 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file1 = File.Open(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf1.Deserialize(file1), ghost);

                // close the file
                file1.Close();

            }

            Debug.Log("Loaded Save 1");
        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf1 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file1 = File.Open(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf1.Deserialize(file1), ghost);

                // close the file
                file1.Close();

            }

            Debug.Log("Loaded Save 1");
        }

        

    }

    // bool for if there is a save file
    public bool IsReplaySecondSaveFile1() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile1/Replays");

    }


    // public void for save replay
    public void SaveSecondReplay1() {

        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile1()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF1 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile1 = File.Create(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SecondSavedData1.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF1.Serialize(replay_secondFile1, json);

            // close the replay file
            replay_secondFile1.Close();

        }

        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile1()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/City_Map");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays/City_Map")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays/City_Map");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF1 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile1 = File.Create(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SecondSavedData1.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF1.Serialize(replay_secondFile1, json);

            // close the replay file
            replay_secondFile1.Close();

        }

    }

    // load the replay
    public void LoadSecondReplay1() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF1 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream replay_secondFile1 = File.Open(Application.persistentDataPath + "/Profile1/Replays/Tutorial_Map/Replay_SavedData1.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF1.Deserialize(replay_secondFile1), ghost2);

                // close the file
                replay_secondFile1.Close();

            }

            Debug.Log("Loaded Second Save 1");

        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile1/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF1 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream replay_secondFile1 = File.Open(Application.persistentDataPath + "/Profile1/Replays/City_Map/Replay_SavedData1.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF1.Deserialize(replay_secondFile1), ghost2);

                // close the file
                replay_secondFile1.Close();

            }

            Debug.Log("Loaded Second Save 1");

        }

    }

    #endregion

    #region SecondSaveFile
    // bool for if there is a save file
    public bool IsReplaySaveFile2() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile2/Replays");

    }


    // public void for save replay
    public void SaveReplay2() {

        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile2()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf2 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file2 = File.Create(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SavedData2.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf2.Serialize(replay_file2, json);

            // close the replay file
            replay_file2.Close();

        }

        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile2()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf2 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file2 = File.Create(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SavedData2.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf2.Serialize(replay_file2, json);

            // close the replay file
            replay_file2.Close();

        }



    }


    // load the replay
    public void LoadReplay2() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf2 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SavedData2.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file2 = File.Open(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SavedData2.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf2.Deserialize(file2), ghost);

                // close the file
                file2.Close();

            }

        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf2 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SavedData2.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file2 = File.Open(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SavedData2.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf2.Deserialize(file2), ghost);

                // close the file
                file2.Close();

            }

        }



    }





    // bool for if there is a save file
    public bool IsReplaySecondSaveFile2() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile2/Replays");

    }


    // public void for save replay
    public void SaveSecondReplay2() {

        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile2()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF2 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile2 = File.Create(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SecondSavedData2.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF2.Serialize(replay_secondFile2, json);

            // close the replay file
            replay_secondFile2.Close();

        }

        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile2()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF2 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile2 = File.Create(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SecondSavedData2.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF2.Serialize(replay_secondFile2, json);

            // close the replay file
            replay_secondFile2.Close();

        }



    }

    // load the replay
    public void LoadSecondReplay2() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF2 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SavedData2.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream secondFile2 = File.Open(Application.persistentDataPath + "/Profile2/Replays/Tutorial_Map/Replay_SavedData2.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF2.Deserialize(secondFile2), ghost2);

                // close the file
                secondFile2.Close();

            }

            Debug.Log("Loaded Second Save 2");

        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile2/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF2 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SavedData2.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream secondFile2 = File.Open(Application.persistentDataPath + "/Profile2/Replays/City_Map/Replay_SavedData2.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF2.Deserialize(secondFile2), ghost2);

                // close the file
                secondFile2.Close();

            }

            Debug.Log("Loaded Second Save 2");

        }



    }

    #endregion

    #region ThirdSaveFile
    // bool for if there is a save file
    public bool IsReplaySaveFile3() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile3/Replays");

    }


    // public void for save replay
    public void SaveReplay3() {

        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile3()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf3 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file3 = File.Create(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SavedData3.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf3.Serialize(replay_file3, json);

            // close the replay file
            replay_file3.Close();

        }

        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySaveFile3()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf3 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_file3 = File.Create(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SavedData3.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_bf3.Serialize(replay_file3, json);

            // close the replay file
            replay_file3.Close();

        }



    }


    // load the replay
    public void LoadReplay3() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf3 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SavedData3.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file3 = File.Open(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SavedData3.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf3.Deserialize(file3), ghost);

                // close the file
                file3.Close();

            }

        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_bf3 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SavedData3.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream file3 = File.Open(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SavedData3.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_bf3.Deserialize(file3), ghost);

                // close the file
                file3.Close();

            }

        }



    }

    // bool for if there is a save file
    public bool IsReplaySecondSaveFile3() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile3/Replays");

    }


    // public void for save replay
    public void SaveSecondReplay3() {

        if (isMapTutorial) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile3()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF3 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile3 = File.Create(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SecondSavedData3.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF3.Serialize(replay_secondFile3, json);

            // close the replay file
            replay_secondFile3.Close();

        }

        if (isMapCity) {

            // if there is not a save file for the replay
            if (!IsReplaySecondSaveFile3()) {

                // create a save file in the data path folder called /replay_save
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF3 = new BinaryFormatter();

            // create a filestream in the replay data and call it "replay_SavedData"
            FileStream replay_secondFile3 = File.Create(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SecondSavedData3.sdat");

            // pass in ghost object and save public variables
            var json = JsonUtility.ToJson(ghost);

            // serializes the data to binary format in a json file
            replay_secondBF3.Serialize(replay_secondFile3, json);

            // close the replay file
            replay_secondFile3.Close();

        }


    }

    // load the replay
    public void LoadSecondReplay3() {

        if (isMapTutorial) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF3 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SavedData3.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream secondFile3 = File.Open(Application.persistentDataPath + "/Profile3/Replays/Tutorial_Map/Replay_SavedData3.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF3.Deserialize(secondFile3), ghost2);

                // close the file
                secondFile3.Close();

            }

            Debug.Log("Loaded Second Save 3");

        }

        if (isMapCity) {

            // if a directory doesn't exists for "/replay_save/replay_data"
            if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

                // create a directory for "/replay_save/replay_data"
                Directory.CreateDirectory(Application.persistentDataPath + "/Profile3/Replays");

            }

            // create a new binary formatter called replay_bf
            BinaryFormatter replay_secondBF3 = new BinaryFormatter();

            // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
            if (File.Exists(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SavedData3.sdat")) {

                // open the file "/replay_save/replay_data/replay_SavedData.txt"
                FileStream secondFile3 = File.Open(Application.persistentDataPath + "/Profile3/Replays/City_Map/Replay_SavedData3.sdat", FileMode.Open);

                //deseralize the ghost file 
                JsonUtility.FromJsonOverwrite((string)replay_secondBF3.Deserialize(secondFile3), ghost2);

                // close the file
                secondFile3.Close();

            }

            Debug.Log("Loaded Second Save 3");

        }

    }
    #endregion

    #endregion
}
