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
    public GhostPlayer ghostPlayer;

    //
    public CC changeCamera;

    //
    public LoadCustomizablesInGame loadCustomizablesInGame;

    // reference to the ReplaySaveManager
    public ReplaySaveManager replayGhostSaveManager;

    // reference to the 1st load player button
    public GameObject LoadPlayer1Button;

    // reference to the 2nd load player button
    public GameObject LoadPlayer2Button;

    // reference to the 3rd load player button
    public GameObject LoadPlayer3Button;

    // reference to the 1st save player button
    public GameObject SavePlayer1Button;

    // reference to the 2nd save player button
    public GameObject SavePlayer2Button;

    // reference to the 3rd save player button
    public GameObject SavePlayer3Button;

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
    //public ShopText shopText;

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

    // update
    public void Update() {

        // if the replayGhostSaveManager == null
        if (replayGhostSaveManager == null) {

            // set the ghost replay save manager to the ghostreplay save gameobject with the tag and script
            replayGhostSaveManager = GameObject.FindGameObjectWithTag("GhostReplaySaveSystem").GetComponent<ReplaySaveManager>();

        }

        if (Keyboard.current.escapeKey.isPressed) {

            //ghostPlayer.isFirstRecord = false;

            //
            SavePlayer1();

            //
            // save the replay 1 through the ghost save manager
            //replayGhostSaveManager.SaveReplay1();

        }


    }


    // Load the player1
    public void LoadPlayer1() {


        // Set the load player button 2 to false
        LoadPlayer2Button.SetActive(false);

        // Set the load player button 3 to false
        LoadPlayer3Button.SetActive(false);

        // Set the save player button 2 to false
        SavePlayer2Button.SetActive(false);

        // Set the save player button 3 to false
        SavePlayer3Button.SetActive(false);

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


        outfitChanger.LoadedCustomizables1();

        //shopText.LoadProfile1Text.text = data.TMP1ToSave.text;

        

    }

    public void PlayPlayer1() {

        // load the first replay through the ghost save manager
        replayGhostSaveManager.LoadReplay1();

        changeCamera.ChangeToGameCam();

    }


    // Load the player2
    public void LoadPlayer2() {

        // Set the load player button 1 to false
        LoadPlayer1Button.SetActive(false);

        // Set the load player button 3 to false
        LoadPlayer3Button.SetActive(false);

        // Set the save player button 1 to false
        SavePlayer1Button.SetActive(false);

        // Set the save player button 3 to false
        LoadPlayer3Button.SetActive(false);

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

        // load the second replay through the ghost save manager
        replayGhostSaveManager.LoadReplay2();

    }




    // Load the player3
    public void LoadPlayer3() {

        // Set the load player button 2 to false
        LoadPlayer2Button.SetActive(false);

        // Set the load player button 1 to false
        LoadPlayer1Button.SetActive(false);

        // Set the save player button 2 to false
        SavePlayer2Button.SetActive(false);

        // Set the save player button 1 to false
        SavePlayer1Button.SetActive(false);

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

        // load the third replay through the ghost save manager
        replayGhostSaveManager.LoadReplay3();

    }


    // Save the player1
    public void SavePlayer1() {

        // save the player 1 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer1(shop, outfitChanger);

        // save the replay 1 through the ghost save manager
        replayGhostSaveManager.SaveReplay1();

    }


    // Save the player2
    public void SavePlayer2() {

        // save the player 2 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer2(shop, outfitChanger);

        // save the replay 2 through the ghost save manager
        replayGhostSaveManager.SaveReplay2();

    }


    // Save the player3
    public void SavePlayer3() {

        // save the player 3 and pass through the shop and outfitchanger
        b_SaveSystem.SavePlayer3(shop, outfitChanger);

        // save the replay 3 through the ghost save manager
        replayGhostSaveManager.SaveReplay3();

    }


    #endregion

}
