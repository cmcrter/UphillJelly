////////////////////////////////////////////////////////////
// File: b_Player.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 15/11/21
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

    //
    public Ghost ghostSO1;

    //
    public Ghost2 ghostSO2;

    //
    public bool isSave1;

    //
    public bool isSave2;

    //
    public bool isSave3;

    //
    public CC changeCamera;

    //
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

    public void TESTTEST1() {

        //
        SavePlayer1();

    }

    //
    public void Start() {

        //
        ghostSO1.isRecording = false;

        //
        ghostSO1.isReplaying = false;


        //
        ghostSO2.isReplaying = false;

        //
        ghostSO2.isRecording = false;


        //
        isSave1 = false;

        //
        isSave2 = false;

        //
        isSave3 = false;

        //
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

        //
        ghostSO1.isRecording = false;

        //
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        //
        // Save replay 1 as replay 2
        //
        SavePlayer1();

        //
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        //
        ghostSO2.position = ghostSO1.position;

        //
        ghostSO2.rotation = ghostSO1.rotation;

        //
        SavePlayer1Second();

        Debug.Log("11111111111");

    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void TestPressed2Update() {

        //
        ghostSO1.isRecording = false;

        //
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        //
        // Save replay 1 as replay 2
        //
        SavePlayer2();

        //
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        //
        ghostSO2.position = ghostSO1.position;

        //
        ghostSO2.rotation = ghostSO1.rotation;

        //
        SavePlayer2Second();

        Debug.Log("22222222");

    }

    // IF GET HIGH SCORE SAVE FIRST AND SECOND REPLAY (DONT NEED TO SAVE FIRST REPLAY)
    public void TestPressed3Update() {

        //
        ghostSO1.isRecording = false;

        //
        ghostSO2.isReplaying = false;

        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        //
        // Save replay 1 as replay 2
        //
        SavePlayer3();

        //
        ghostSO2.timeStamp = ghostSO1.timeStamp;

        //
        ghostSO2.position = ghostSO1.position;

        //
        ghostSO2.rotation = ghostSO1.rotation;

        //
        SavePlayer3Second();

        Debug.Log("3333333333");

    }




    //
    public void TestPressed2() {

        //
        // Save Replay 1 as replay 2 (ghost 1 only records (check if better than the ghost replay 2),
        // Ghost 2 replays best time 
        //
        // 
        //
        SavePlayer1();

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

        // save the player 1 and pass through the shop and outfitchanger
        //b_SaveSystem.SavePlayer1(shop, outfitChanger);

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

        // save the player 2 and pass through the shop and outfitchanger
        //b_SaveSystem.SavePlayer2(shop, outfitChanger);

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

        // save the player 3 and pass through the shop and outfitchanger
        //b_SaveSystem.SavePlayer3(shop, outfitChanger);

        // save the replay 3 through the ghost save manager
        replayGhostSaveManager.SaveSecondReplay3();

        Debug.Log("Saved Player 3");

    }


    // play button 1 pressed
    public void PlayPlayer1() {

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay1();

        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }

    // play button 2 pressed
    public void PlayPlayer2() {

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay2();

        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }

    // play button 3 pressed
    public void PlayPlayer3() {

        // Load the first second replay for ghost 2
        replayGhostSaveManager.LoadSecondReplay3();

        ghostSO1.isRecording = true;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = true;

        // Change to game camera
        changeCamera.ChangeToGameCam();

    }

    //
    public void ExitedTheGame() {

        //
        ghostSO1.isRecording = false;
        ghostSO1.isReplaying = false;

        ghostSO2.isRecording = false;
        ghostSO2.isReplaying = false;

    }

    #endregion

}
