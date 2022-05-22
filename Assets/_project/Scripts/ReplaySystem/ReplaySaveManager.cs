////////////////////////////////////////////////////////////
// File: ReplaySaveManager.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 04/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: Script to store the data of the ghosts poisition and rotation 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using L7Games.Loading;

public class ReplaySaveManager : MonoBehaviour
{
    #region variables

    public Ghost ghost;
    public Ghost2 ghost2;
    public GameObject Ghost1character;
    public GameObject Ghost2character;

    public GameObject goPlaybacking;
    public GameObject goRecording;

    [Tooltip("This is the object which has the skinned mesh renderer on the player")]
    public GameObject goToRecord;

    [Header("Overriding Replay Value. mainly to see replays from different player slots on map")]
    [SerializeField]
    private bool overrideReplay = false;
    [SerializeField]
    private int profile = 1;

    private static string level;

    [Header("Recording/Playback of Ghosts")]
    [SerializeField]
    private bool bRecordGhost;
    [SerializeField]
    private bool bPlaybackGhost;

    #endregion

    #region Methods

    private void Awake()
    {
        if(overrideReplay)
        {
            LoadingData.playerSlot = profile;
        }

        ghost.isRecording = bRecordGhost;
        ghost2.isReplaying = bPlaybackGhost;

        goPlaybacking.transform.position = goToRecord.transform.position;
        goRecording.transform.position = goToRecord.transform.position;
    }

    private void Start()
    {
        level = LoadingData.getSceneName(LoadingData.currentLevel) + "_Map";

        if(bPlaybackGhost)
        {
            LoadReplay(LoadingData.playerSlot, ((LEVEL.TUTORIAL - LoadingData.currentLevel) * -1));
            Debug.Log("Playback from player: " + LoadingData.playerSlot + " on map: " + LoadingData.currentLevel.ToString() + " index being: " + ((LEVEL.TUTORIAL - LoadingData.currentLevel) * -1).ToString());
        }

        Debug.Log(level);
    }

    // bool for if there is a save file
    public static bool IsReplaySaveFile(int playerSlot)
    {
        // return the applications persistent data path of the replay save if the save file exists
        return Directory.Exists(Application.persistentDataPath + "/Profile" + playerSlot.ToString() +"/Replays");
    }

    public void SaveCurrentReplay()
    {
        Debug.Log("Saving Replay from player: " + LoadingData.playerSlot + " from level: " + ((LEVEL.TUTORIAL - LoadingData.currentLevel) * -1).ToString());
        ghost.isRecording = false;
        SaveReplay(LoadingData.playerSlot, ((LEVEL.TUTORIAL - LoadingData.currentLevel) * -1));
    }

    // public void for save replay
    public void SaveReplay(int playerSlot, int replaySlot)
    {
        // if there is not a save file for the replay
        if(!IsReplaySaveFile(playerSlot))
        {
            // create a save file in the data path folder called /replay_save
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level);
        }

        // if a directory doesn't exists for "/replay_save/replay_data"
        if(!Directory.Exists(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level))
        {
            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level);
        }

        // create a new binary formatter called replay_bf
        BinaryFormatter replay_bf = new BinaryFormatter();

        // create a filestream in the replay data and call it "replay_SavedData"
        FileStream replay_file = File.Create(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level + "/Replay_SavedData" + replaySlot.ToString() + ".sdat");

        // pass in ghost object and save public variables
        var json = JsonUtility.ToJson(ghost);

        // serializes the data to binary format in a json file
        replay_bf.Serialize(replay_file, json);

        // close the replay file
        replay_file.Close();
    }

    // load the replay
    public void LoadReplay(int playerSlot, int replaySlot)
    {
        // if a directory doesn't exists for "/replay_save/replay_data"
        if(!Directory.Exists(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays"))
        {
            // create a directory for "/replay_save/replay_data"
            Directory.CreateDirectory(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays");
        }

        // create a new binary formatter called replay_bf
        BinaryFormatter replay_secondBF = new BinaryFormatter();

        // if a file exists called "/replay_save/replay_data/replay_SavedData.txt"
        if(File.Exists(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level + "/Replay_SavedData" + replaySlot.ToString() + ".sdat"))
        {
            // open the file "/replay_save/replay_data/replay_SavedData.txt"
            FileStream replay_secondFile = File.Open(Application.persistentDataPath + "/Profile" + playerSlot.ToString() + "/Replays/" + level + "/Replay_SavedData" + replaySlot.ToString() + ".sdat", FileMode.Open);

            //deseralize the ghost file 
            JsonUtility.FromJsonOverwrite((string)replay_secondBF.Deserialize(replay_secondFile), ghost2);
            ghost2.isReplaying = true;

            // close the file
            replay_secondFile.Close();
        }
    }

    #endregion
}