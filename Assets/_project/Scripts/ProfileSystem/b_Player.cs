////////////////////////////////////////////////////////////
// File: b_Player.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: A script to call the functions to save and load the player
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using System.Collections.Generic;
using L7Games.Loading;
using UnityEngine;

[Obsolete]
public class b_Player : MonoBehaviour
{
    #region Variables

    // managers and script references
    public ReplaySaveManager replaySaveManager;
    public LoadCustomizablesInGame loadCustomizablesInGame;
    public Shop shop;

    // ghost SO 1 and 2
    public Ghost ghostSO1;
    public Ghost2 ghostSO2;

    // list of the hat bought, character bought bools and ints
    public List<bool> HatBoughtBools;
    public List<bool> CharacterBoughtBools;
    [SerializeField]
    public List<int> hatBoughtInts = new List<int>();

    // int for the current hat and character ints
    public int CurrentGameObjectInt;
    public int CurrentGameObjectMaterialInt;
    public int CharacterMaterialInt;

    // int for currency
    public int Currency;

    #endregion

    #region Methods

    public void Start()
    {
        // set ghost 1 recording and replaying to false
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        // set ghost 2 replaying and replaying to false
        //ghostSO2.isReplaying = false;
        //ghostSO2.isRecording = false;

        // reset ghost 1 data
        ghostSO1.ResetGhostData();
    }

    // update
    public void Update()
    {
        // if the replayGhostSaveManager == null
        if (replaySaveManager == null) 
        {
            // set the ghost replay save manager to the ghostreplay save gameobject with the tag and script
            replaySaveManager = GameObject.FindGameObjectWithTag("GhostReplaySaveSystem").GetComponent<ReplaySaveManager>();
        }
    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void SaveFinalValues()
    {
        //set ghost 1 and ghost 2 recording and replaing to false
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        //ghostSO2.isRecording = false;
        //ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        // Save replay 1 as replay 2
        SavePlayer(LoadingData.playerSlot, 1);

        // set the ghost 2 timestamp to the ghost 1 timestamp
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        // set the ghost 2 position to the ghost 1 position
        ghostSO2.position = ghostSO1.position;

        // set the ghost 2 rotation to the ghost 1 rotation
        ghostSO2.rotation = ghostSO1.rotation;

        // save the second replay for player 1
        SavePlayer(LoadingData.playerSlot, 2);
    }

    // Load the player
    public void LoadPlayer(int playerSlot)
    {
        // set the player data to the loaded player in the save system
        StoredPlayerProfile data = b_SaveSystem.LoadPlayer(playerSlot);

        //Go through the scriptable objects and set them to if they are equipped or not
        
    }

    public void SavePlayer(int playerSlot)
    {
        // save the player and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer(playerSlot);
    }

    // Save the player
    public void SavePlayer(int playerSlot, int replaySlot) 
    {
        if(replaySlot == 1)
        {
            // save the player and pass through the shop and outfitchanger
            b_SaveSystem.SavePlayer(playerSlot);
        }

        // save the replay 1 through the ghost save manager
        replaySaveManager.SaveReplay(playerSlot, replaySlot);

        if(Debug.isDebugBuild)
        {
            Debug.Log("Saved Player: " + playerSlot.ToString() + "in replay slot: " + replaySlot.ToString());
        }
    }

    // play button pressed
    public void PlayPlayer(int playerSlot, int replaySlot)
    {
        // reset ghost data 2
        ghostSO2.ResetGhostData();

        // Load the first second replay for ghost 2
        //replaySaveManager.LoadReplay(playerSlot, replaySlot);

        // set the ghost 1 to recording = true and replaying = false
        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        // set the ghost 2 to recording = false and replaying = true
        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;
    }

    #endregion
}