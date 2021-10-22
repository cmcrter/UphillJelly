////////////////////////////////////////////////////////////
// File: SaveSystem.cs
// Author: Jack Peedle
// Date Created: 18/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 18/10/21
// Brief: A script to save and load the players customization gameobjects and other data like currency
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveSystem : MonoBehaviour
{

    #region Variables

    //
    [SerializeField] CustomizationData prefabHat; //go_currentActiveHat;

    //
    ProfileController profileController;

    // public static reference to the list and not to the save system, for the hats
    public static List<ProfileController> Hats = new List<ProfileController>();

    // public static reference to the list and not to the save system, for the hats
    public static List<ProfileController> PlayerMaterials = new List<ProfileController>();

    // public static reference to the list and not to the save system, for the hats
    //public static List<ProfileController> Skateboards = new List<ProfileController>();



    // Constant string for the player data
    const string PLAYER_SUB = "/ProfileData";

    // Constant string for the player data
    const string PLAYER_COUNT_SUB = "/ProfileData.count";


    #endregion

    #region Methods

    //
    public void LoadGame() {

        //
        LoadPlayerData();

    }

    //
    public void SaveGame() {

        //
        SavePlayerData();

    }


    // Save player data function
    void SavePlayerData() {

        // new binary formatter
        BinaryFormatter bf_formatter = new BinaryFormatter();

        // create a path in where the data will be stored          This means the data will save exclusive to the current scene, for scene 0 e.g = "/ProfileData0)
        string path = Application.persistentDataPath + PLAYER_SUB; //+ SceneManager.GetActiveScene().buildIndex;

        // create a Countpath in where the data will be stored          This means the data will save exclusive to the current scene, for scene 0 e.g = "/ProfileData0)
        string countpath = Application.persistentDataPath + PLAYER_COUNT_SUB; //+ SceneManager.GetActiveScene().buildIndex;



        // Count stream for saving the player counts for objects for when the game is closed
        FileStream countStream = new FileStream(countpath, FileMode.Create);

        // serialize (save) count stream hat data count
        bf_formatter.Serialize(countStream, Hats.Count);

        // serialize (save) count stream playerMaterials data count
        bf_formatter.Serialize(countStream, PlayerMaterials.Count);

        // Close the count stream
        countStream.Close();



        // for each hat.count
        for (int i = 0; i < Hats.Count; i++) {

            // create a new filestream that takes in the path ^^ and i for each hat and creates save data file
            FileStream hatStream = new FileStream(path + i, FileMode.Create);

            // Hat data to put into the file with the array of hats
            CustomizationData hatData = new CustomizationData(Hats[i]);

            // serialize (save) hat stream hat data
            bf_formatter.Serialize(hatStream, hatData);

            // close hat stream
            hatStream.Close();

        }


        // for each playerMaterial.count
        for (int i = 0; i < PlayerMaterials.Count; i++) {

            // create a new filestream that takes in the path ^^ and i for each playerMaterial and creates save data file
            FileStream playerMaterialsStream = new FileStream(path + i, FileMode.Create);

            // playerMaterial data to put into the file with the array of PlayerMaterials
            CustomizationData playerMaterialsData = new CustomizationData(PlayerMaterials[i]);

            // serialize (save) playerMaterial stream and playerMaterial data
            bf_formatter.Serialize(playerMaterialsStream, playerMaterialsData);

            // close playerMaterial stream
            playerMaterialsStream.Close();

        }

    }




    //
    void LoadPlayerData() {

        // new binary formatter
        BinaryFormatter bf_formatter = new BinaryFormatter();

        // create a path in where the data will be stored          This means the data will save exclusive to the current scene, for scene 0 e.g = "/ProfileData0)
        string path = Application.persistentDataPath + PLAYER_SUB; //+ SceneManager.GetActiveScene().buildIndex;

        // create a Countpath in where the data will be stored          This means the data will save exclusive to the current scene, for scene 0 e.g = "/ProfileData0)
        string countpath = Application.persistentDataPath + PLAYER_COUNT_SUB; //+ SceneManager.GetActiveScene().buildIndex;

        //
        int hatCount = 0;

        //
        int playerMaterialCount = 0;

        //
        //int skateboardCount = 0;

        // if the countpath file exists
        if (File.Exists(countpath)) {

            // create a new file stream that takes in the count path and open it
            FileStream countStream = new FileStream(countpath, FileMode.Open);

            // deserialize (Load) the hat count as an integer
            hatCount = (int)bf_formatter.Deserialize(countStream);

            // deserialize (Load) the hat count as an integer
            playerMaterialCount = (int)bf_formatter.Deserialize(countStream);

            // close the count stream
            countStream.Close();


        } else {

            // if is debug build
            if (Debug.isDebugBuild) {

                // show debug error Path not found in countpath
                Debug.LogError("Path not found in " + countpath);

            }

        }


        // for i is less than hatcount
        for (int i = 0; i < hatCount; i++) {

            if (File.Exists(path + i)) {

                // create a new filestream that takes in path and i and open it
                FileStream stream = new FileStream(path + i, FileMode.Open);

                // deserialize the stream as the customization data
                CustomizationData custData = bf_formatter.Deserialize(stream) as CustomizationData;

                // close the stream
                stream.Close();

                // load the hats spawn position as a new vector 3 at customizationData.hatSpawningPosition[x,y,z]
                Vector3 hatSpawnPosition = new Vector3(custData.hatSpawningPosition[0], custData.hatSpawningPosition[1], custData.hatSpawningPosition[2]);

                // instantiate the current active hat at the hats spawn position and no rotation
                //CustomizationData customizationData = Instantiate(prefabHat, hatSpawnPosition, Quaternion.identity);

                //
                //customizationData.name = customizationData.TMPProfileTextInput.name;


            } else {

                // if is debug build
                if (Debug.isDebugBuild) {

                    // show debug error Path not found in countpath
                    Debug.LogError("Path not found in " + path + i);

                }

            }

        }



        // for i is less than hatcount
        for (int i = 0; i < playerMaterialCount; i++) {

            if (File.Exists(path + i)) {

                // create a new filestream that takes in path and i and open it
                FileStream stream = new FileStream(path + i, FileMode.Open);

                // deserialize the stream as the customization data
                CustomizationData customizationData = bf_formatter.Deserialize(stream) as CustomizationData;

                // close the stream
                stream.Close();

            } else {

                // if is debug build
                if (Debug.isDebugBuild) {

                    // show debug error Path not found in countpath
                    Debug.LogError("Path not found in " + path + i);

                }

            }

        }


    }

    #endregion

}
