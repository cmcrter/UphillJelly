////////////////////////////////////////////////////////////
// File: b_Player.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 25/10/21
// Brief: A script to call the functions to save and load the player
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_Player : MonoBehaviour
{

    #region Variables

    //
    public GameObject LoadPlayer1Button;

    //
    public GameObject LoadPlayer2Button;

    //
    public GameObject LoadPlayer3Button;


    // gameobject for the hat price panel
    //public GameObject go_HatPricePanel1; // SAVED
    //
    // gameobject for the character price panel
    //public GameObject go_CharacterPricePanel1; // SAVED


    //
    public List<bool> HatBoughtBools;

    //
    public List<bool> CharacterBoughtBools;


    [SerializeField]
    public List<int> hatBoughtInts = new List<int>();

    // reference to the outfit changer script
    public OutfitChanger outfitChanger;

    //
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

    



    // Load the player
    public void LoadPlayer1() {


        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer1();

        

        shop.Currency = data.iCurrency;
        
        shop.iSavedHatInts = data.savedHatList;

        shop.iSavedCharacterInts = data.savedCharacterList;

        outfitChanger.currentGOint = data.icurrentGOint;

        outfitChanger.currentGOMaterialint = data.icurrentGOMaterialint;

        outfitChanger.currentCharacterint = data.icurrentCharacterint;


        for (int i = 0; i < shop.IsHatBought.Count; i++) {

            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                if (i == shop.iSavedHatInts[j]) {

                    shop.IsHatBought[i] = true;

                    Debug.Log(shop.IsHatBought[i]);

                }

            }

        }


        for (int i = 0; i < shop.IsCharacterBought.Count; i++) {

            for (int j = 0; j < shop.iSavedCharacterInts.Count; j++) {

                if (i == shop.iSavedCharacterInts[j]) {

                    shop.IsCharacterBought[i] = true;

                    Debug.Log(shop.IsCharacterBought[i]);

                }

            }

        }

        // Include the other things to load

    }




    // Load the player
    public void LoadPlayer2() {

        //
        LoadPlayer1Button.SetActive(false);

        //
        LoadPlayer3Button.SetActive(false);

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer2();



        shop.Currency = data.iCurrency;

        shop.iSavedHatInts = data.savedHatList;



        for (int i = 0; i < shop.IsHatBought.Count; i++) {


            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                if (i == shop.iSavedHatInts[j]) {

                    shop.IsHatBought[i] = true;

                } 

            }

        }

        // Include the other things to load

    }




    // Load the player
    public void LoadPlayer3() {

        //
        LoadPlayer2Button.SetActive(false);

        //
        LoadPlayer1Button.SetActive(false);

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer3();



        shop.Currency = data.iCurrency;

        shop.iSavedHatInts = data.savedHatList;



        for (int i = 0; i < shop.IsHatBought.Count; i++) {


            for (int j = 0; j < shop.iSavedHatInts.Count; j++) {

                if (i == shop.iSavedHatInts[j]) {

                    shop.IsHatBought[i] = true;

                } 

            }

        }

        // Include the other things to load



    }


    // Save the player
    public void SavePlayer1() {

        b_SaveSystem.SavePlayer1(shop, outfitChanger);

        // Go onto the game

    }


    // Save the player
    public void SavePlayer2() {

        b_SaveSystem.SavePlayer2(shop, outfitChanger);

        // Go onto the game

    }


    // Save the player
    public void SavePlayer3() {

        b_SaveSystem.SavePlayer3(shop, outfitChanger);

        // Go onto the game

    }


    #endregion

}
