////////////////////////////////////////////////////////////
// File: ProfileSaveAndLoad.cs
// Author: Jack Peedle
// Date Created: 12/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 12/10/21
// Brief: A script to save and load the players profiles and information
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ProfileSaveAndLoad : MonoBehaviour
{

    #region Variables

    // Reference to the TextMeshPro input field
    public TMP_InputField TMPProfileTextInput;

    #endregion

    #region Methods

    // bool for if there is a save file
    public bool IsProfileSaveFile() {

        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profiles");

    }


    // public void for save Profile
    public void SaveProfile() {

        // if there is not a save file for the profile
        if (!IsProfileSaveFile()) {

            // create a save file in the data path folder called /Profiles
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles");

        }

        // if a directory doesn't exists for "/Profiles/Profile1_Data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/Profile1_Data")) {

            // create a directory for "/Profiles/Profile1_Data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/Profile1_Data");

        }

        // create a new binary formatter called profile_bf
        BinaryFormatter profile_bf = new BinaryFormatter();

        // create a filestream in the profile data and call it "Profile1_SavedData.txt"
        FileStream profile_file = File.Create(Application.persistentDataPath + "/Profiles/Profile1_Data/Profile1_SavedData.txt");

        // pass in TMP text and save player typed text
        var json = JsonUtility.ToJson(TMPProfileTextInput);

        // serializes the data to binary format in a json file
        profile_bf.Serialize(profile_file, json);

        // close the replay file
        profile_file.Close();

        // Debug
        if (Debug.isDebugBuild) {

            Debug.Log("SavedProfile");

        }

    }


    // load the profile
    public void LoadProfile() {

        // if a directory doesn't exists for "/Profiles/Profile1_Data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profiles/Profile1_Data")) {

            // create a directory for "/Profiles/Profile1_Data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles/Profile1_Data");

        }

        // create a new binary formatter called profile_bf
        BinaryFormatter profile_bf = new BinaryFormatter();

        // if a file exists called "/Profiles/Profile1_Data/Profile1_SavedData.txt"
        if (File.Exists(Application.persistentDataPath + "/Profiles/Profile1_Data/Profile1_SavedData.txt")) {

            // open the file "/Profiles/Profile1_Data/Profile1_SavedData.txt"
            FileStream Pfile = File.Open(Application.persistentDataPath + "/Profiles/Profile1_Data/Profile1_SavedData.txt", FileMode.Open);

            //deseralize the profile file 
            JsonUtility.FromJsonOverwrite((string)profile_bf.Deserialize(Pfile), TMPProfileTextInput);

            // close the file
            Pfile.Close();

            //Debug
            if (Debug.isDebugBuild) {

                Debug.Log("LoadedProfile");

            }

        }

    }


    #endregion

}
