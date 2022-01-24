////////////////////////////////////////////////////////////
// File: b_Player.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief: A script to call the functions to save and load the player
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class b_Player : MonoBehaviour
{

    #region Variables

    // replay save manager
    public ReplaySaveManager replaySaveManager;

    // ghost SO 1
    public Ghost ghostSO1;

    // ghost SO 2
    public Ghost2 ghostSO2;

    // is save 1
    public bool isSave1;

    // is save 2
    public bool isSave2;

    // is save 3
    public bool isSave3;

    // CC script
    public CC changeCamera;

    // load customizables in game
    public LoadCustomizablesInGame loadCustomizablesInGame;

    // reference to the ReplaySaveManager
    public ReplaySaveManager replayGhostSaveManager;


    // list of the hat bought bools
    public List<bool> HatBoughtBools;

    // list of the character bought bools
    public List<bool> CharacterBoughtBools;

    // list of hatboughtints
    [SerializeField]
    public List<int> hatBoughtInts = new List<int>();

    // reference to the outfit changer script
    public OutfitChanger outfitChanger;

    // reference to the shop script
    public Shop shop;

    //
    //public SleepyCat.ScoreBoardSavedData scoreBoardSavedData;

    // int for the current hat gameobject
    public int CurrentGameObjectInt;

    // int for the current hat material
    public int CurrentGameObjectMaterialInt;

    // character material int
    public int CharacterMaterialInt;


    // int for currency
    public int Currency;

    #endregion

    #region Methods

    //
    // Not needed???
    public void TESTTEST1() {

        //
        SavePlayer1();

    }
    //
    //

    //
    public void Start() {

        // set ghost 1 recording to false
        ghostSO1.isRecording = false;

        // set ghost 1 replaying to false
        ghostSO1.isReplaying = false;


        // set ghost 2 replaying to false
        ghostSO2.isReplaying = false;

        // set ghost 2 recording to false
        ghostSO2.isRecording = false;


        // is save 1 = false
        isSave1 = false;

        // is save 2 = false
        isSave2 = false;

        // is save 3 = false
        isSave3 = false;

        // reset ghost 1 data
        ghostSO1.ResetGhostData();

    }

    // update
    public void Update() {

        // if the replayGhostSaveManager == null
        if (replayGhostSaveManager == null) {

            // set the ghost replay save manager to the ghostreplay save gameobject with the tag and script
            replayGhostSaveManager = GameObject.FindGameObjectWithTag("GhostReplaySaveSystem").GetComponent<ReplaySaveManager>();

        }

    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void TestPressed1Update() {

        // set ghost 1 and ghost 2 recording and replaing to false
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        // Save replay 1 as replay 2
        SavePlayer1();

        // set the ghost 2 timestamp to the ghost 1 timestamp
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        // set the ghost 2 position to the ghost 1 position
        ghostSO2.position = ghostSO1.position;

        // set the ghost 2 rotation to the ghost 1 rotation
        ghostSO2.rotation = ghostSO1.rotation;

        // save the second replay for player 1
        SavePlayer1Second();

        Debug.Log("Saved Replay 1's");

    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void TestPressed2Update() {

        // set ghost 1 and ghost 2 recording and replaing to false
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        // Save replay 1 as replay 2
        SavePlayer2();

        // set the ghost 2 timestamp to the ghost 1 timestamp
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        // set the ghost 2 position to the ghost 1 position
        ghostSO2.position = ghostSO1.position;

        // set the ghost 2 rotation to the ghost 1 rotation
        ghostSO2.rotation = ghostSO1.rotation;

        // save the second replay for player 2
        SavePlayer2Second();

        Debug.Log("Saved Replay 2's");

    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void TestPressed3Update() {

        // set ghost 1 and ghost 2 recording and replaing to false
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        // Save replay 1 as replay 2
        SavePlayer3();

        // set the ghost 2 timestamp to the ghost 1 timestamp
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        // set the ghost 2 position to the ghost 1 position
        ghostSO2.position = ghostSO1.position;

        // set the ghost 2 rotation to the ghost 1 rotation
        ghostSO2.rotation = ghostSO1.rotation;

        // save the second replay for player 3
        SavePlayer3Second();

        Debug.Log("Saved Replay 3's");

    }


    // Load the player1
    public void LoadPlayer1() {


        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer1();

        // shop currency = data currency
        shop.Currency = data.iCurrency;
        
        // shop saved hat ints = data saved hat ints
        shop.iSavedHatInts = data.savedHatList;

        // shop saved character ints = data saved character ints
        shop.iSavedCharacterInts = data.savedCharacterList;


        // outfit changer current gameobject int = data current gameobject int
        outfitChanger.currentGOint = data.icurrentGOint;

        // outfitchanger current material int = data current material int
        outfitChanger.currentGOMaterialint = data.icurrentGOMaterialint;

        // outfit changer current character int = data current character int
        outfitChanger.currentCharacterint = data.icurrentCharacterint;

        // for i < shop hat bought count
        for (int i = 0; i < shop.IsHatBought.Count; i++) {

            // for j < shop.saved hat ints count
            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                // if i == saved hat ints with an index of j
                if (i == shop.iSavedHatInts[j]) {

                    // shop is hat bought with an index of i = true
                    shop.IsHatBought[i] = true;

                    // debug the is hat bought with the current hat index
                    Debug.Log(shop.IsHatBought[i]);

                }

            }

        }

        // for i < shop character bought count
        for (int i = 0; i < shop.IsCharacterBought.Count; i++) {

            // for j < shop.saved character ints count
            for (int j = 0; j < shop.iSavedCharacterInts.Count; j++) {

                // if i == saved character ints with an index of j
                if (i == shop.iSavedCharacterInts[j]) {

                    // shop is character bought with an index of i = true
                    shop.IsCharacterBought[i] = true;

                    // debug the is character bought with the current hat index
                    Debug.Log(shop.IsCharacterBought[i]);

                }

            }

        }

        // Load the first customizables
        outfitChanger.LoadedCustomizables1();

    }

    
    // Load the player2
    public void LoadPlayer2() {


        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer2();

        // shop currency = data currency
        shop.Currency = data.iCurrency;

        // shop saved hat ints = data saved hat ints
        shop.iSavedHatInts = data.savedHatList;

        // shop saved character ints = data saved character ints
        shop.iSavedCharacterInts = data.savedCharacterList;

        // outfit changer current gameobject int = data current gameobject int
        outfitChanger.currentGOint = data.icurrentGOint;

        // outfitchanger current material int = data current material int
        outfitChanger.currentGOMaterialint = data.icurrentGOMaterialint;

        // outfit changer current character int = data current character int
        outfitChanger.currentCharacterint = data.icurrentCharacterint;

        // for i < shop hat bought count
        for (int i = 0; i < shop.IsHatBought.Count; i++) {

            // for j < shop.saved hat ints count
            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                // if i == saved hat ints with an index of j
                if (i == shop.iSavedHatInts[j]) {

                    // shop is hat bought with an index of i = true
                    shop.IsHatBought[i] = true;

                    // debug the is hat bought with the current hat index
                    Debug.Log(shop.IsHatBought[i]);

                }

            }

        }

        // for i < shop character bought count
        for (int i = 0; i < shop.IsCharacterBought.Count; i++) {

            // for j < shop.saved character ints count
            for (int j = 0; j < shop.iSavedCharacterInts.Count; j++) {

                // if i == saved character ints with an index of j
                if (i == shop.iSavedCharacterInts[j]) {

                    // shop is character bought with an index of i = true
                    shop.IsCharacterBought[i] = true;

                    // debug the is character bought with the current hat index
                    Debug.Log(shop.IsCharacterBought[i]);

                }

            }

        }

        // Load the second customizables
        outfitChanger.LoadedCustomizables2();

    }




    // Load the player3
    public void LoadPlayer3() {


        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer3();

        // shop currency = data currency
        shop.Currency = data.iCurrency;

        // shop saved hat ints = data saved hat ints
        shop.iSavedHatInts = data.savedHatList;

        // shop saved character ints = data saved character ints
        shop.iSavedCharacterInts = data.savedCharacterList;

        // outfit changer current gameobject int = data current gameobject int
        outfitChanger.currentGOint = data.icurrentGOint;

        // outfitchanger current material int = data current material int
        outfitChanger.currentGOMaterialint = data.icurrentGOMaterialint;

        // outfit changer current character int = data current character int
        outfitChanger.currentCharacterint = data.icurrentCharacterint;

        // for i < shop hat bought count
        for (int i = 0; i < shop.IsHatBought.Count; i++) {

            // for j < shop.saved hat ints count
            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                // if i == saved hat ints with an index of j
                if (i == shop.iSavedHatInts[j]) {

                    // shop is hat bought with an index of i = true
                    shop.IsHatBought[i] = true;

                    // debug the is hat bought with the current hat index
                    Debug.Log(shop.IsHatBought[i]);

                }

            }

        }

        // for i < shop character bought count
        for (int i = 0; i < shop.IsCharacterBought.Count; i++) {

            // for j < shop.saved character ints count
            for (int j = 0; j < shop.iSavedCharacterInts.Count; j++) {

                // if i == saved character ints with an index of j
                if (i == shop.iSavedCharacterInts[j]) {

                    // shop is character bought with an index of i = true
                    shop.IsCharacterBought[i] = true;

                    // debug the is character bought with the current hat index
                    Debug.Log(shop.IsCharacterBought[i]);

                }

            }

        }

        // Load the third customizables
        outfitChanger.LoadedCustomizables3();

    }


    // Save the player1
    public void SavePlayer1() {

        // save the player 1 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer1(shop, outfitChanger);

        // save the replay 1 through the ghost save manager
        replayGhostSaveManager.SaveReplay1();

        Debug.Log("Saved Player 1");

    }

    // Save the player1's second ghost replay
    public void SavePlayer1Second() {

        // save the replay 1 through the ghost save manager
        replayGhostSaveManager.SaveSecondReplay1();

        Debug.Log("Saved Player 1S");

    }


    // Save the player2
    public void SavePlayer2() {

        // save the player 2 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer2(shop, outfitChanger);

        // save the replay 2 through the ghost save manager
        replayGhostSaveManager.SaveReplay2();

        Debug.Log("Saved Player 2");

    }

    // Save the player2
    public void SavePlayer2Second() {

        // save the replay 2 through the ghost save manager
        replayGhostSaveManager.SaveSecondReplay2();

        Debug.Log("Saved Player 2S");

    }

    // Save the player3
    public void SavePlayer3() {

        // save the player 3 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer3(shop, outfitChanger);

        // save the replay 3 through the ghost save manager
        replayGhostSaveManager.SaveReplay3();

        Debug.Log("Saved Player 3");

    }

    // Save the player3
    public void SavePlayer3Second() {

        // save the replay 3 through the ghost save manager
        replayGhostSaveManager.SaveSecondReplay3();

        Debug.Log("Saved Player 3");

    }


    // play button 1 pressed
    public void PlayPlayer1() {

        // reset ghost data 2
        ghostSO2.ResetGhostData();

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay1();

        // set the ghost 1 to recording = true and replaying = false
        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        // set the ghost 2 to recording = false and replaying = true
        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }

    // play button 2 pressed
    public void PlayPlayer2() {

        // reset ghost data 2
        ghostSO2.ResetGhostData();

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay2();

        // set the ghost 1 to recording = true and replaying = false
        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        // set the ghost 2 to recording = false and replaying = true
        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }

    // play button 3 pressed
    public void PlayPlayer3() {

        // reset ghost data 2
        ghostSO2.ResetGhostData();

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay3();

        // set the ghost 1 to recording = true and replaying = false
        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        // set the ghost 2 to recording = false and replaying = true
        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }


    #endregion

}
