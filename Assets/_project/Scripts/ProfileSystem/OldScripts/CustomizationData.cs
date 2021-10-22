////////////////////////////////////////////////////////////
// File: CustomizationData.cs
// Author: Jack Peedle
// Date Created: 18/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 18/10/21
// Brief: A script to save and load the players customization gameobjects and other data like currency
//////////////////////////////////////////////////////////// 

using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class CustomizationData
{

    // Reference to the TextMeshPro input field
    public TMP_InputField TMPProfileTextInput;

    // players current material
    public Material playerCurrentMaterial;

    // current active hat on the player
    public GameObject theCurrentActiveHat;

    // public int currency for the shop
    public int Currency;

    // public float for the hat spawn position
    public float[] hatSpawningPosition;

    // public float for the skateboard spawn position
    //public float[] skateboardSpawnPosition;

    // Customization data controller
    public CustomizationData(ProfileController profileController) {

        //
        TMPProfileTextInput = profileController.TMPProfileTextInput;

        //
        playerCurrentMaterial = profileController.CurrentActivePlayerMaterial;

        //
        //theCurrentActiveHat = profileController.currentActiveHat;

        // Add current active skateboard
        // In this space

        //
        Vector3 hatSpawnPosition = profileController.hatSpawnPosition.transform.position;

        //
        hatSpawningPosition = new float[] {

            //
            hatSpawnPosition.x, hatSpawnPosition.y, hatSpawnPosition.z

        };

        //
        Currency = profileController.Currency;

    }

}































/*


FishData.cs class BELOW

using UnityEngine;

[System.Serializable]
public class FishData
{
    //Replace these example variable with your objects variables
    //that you wish to save
    public int health;
    public string name;
    public float[] position;

    public FishData(Fish fish) {
        health = fish.health;
        name = fish.name;

        Vector3 fishPos = fish.transform.position;

        position = new float[]
        {
            fishPos.x, fishPos.y, fishPos.z
        };
    }
}

SaveSystem.cs class BELOW

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Fish fishPrefab;

    //Angled brackets not allowed in desc, replace the
    //parenthesis in (Fish) with angled brackets
    public static List(Fish) fishes = new List(Fish)();

    //Rename your strings according to what your saving
    const string FISH_SUB = "/fish";
    const string FISH_COUNT_SUB = "/fish.count";

    void Awake() {
        LoadFish();
    }

    //Use if Android
    //void OnApplicationPause(bool pause)
    //{
    //    SaveFish();
    //}

    void OnApplicationQuit() {
        SaveFish();
    }

    void SaveFish() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + FISH_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + FISH_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;

        FileStream countStream = new FileStream(countPath, FileMode.Create);

        formatter.Serialize(countStream, fishes.Count);
        countStream.Close();

        //Replace "lessThan" with a left angled bracket
        for (int i = 0; i lessThan fishes.Count; i++)
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            FishData data = new FishData(fishes[i]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    void LoadFish() {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + FISH_SUB + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + FISH_COUNT_SUB + SceneManager.GetActiveScene().buildIndex;
        int fishCount = 0;

        if (File.Exists(countPath)) {
            FileStream countStream = new FileStream(countPath, FileMode.Open);

            fishCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        } else {
            Debug.LogError("Path not found in " + countPath);
        }

        //Replace "lessThan" with an left angled bracket
        for (int i = 0; i lessThan fishCount; i++)
        {
            if (File.Exists(path + i)) {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                FishData data = formatter.Deserialize(stream) as FishData;

                stream.Close();

                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);

                Fish fish = Instantiate(fishPrefab, position, Quaternion.identity);

                fish.health = data.health;
                fish.name = data.name;
            } else {
                Debug.LogError("Path not found in " + (path + i));
            }
        }
    }
}
*/