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

    // Save the player
    public void SavePlayer() {

        b_SaveSystem.SavePlayer(shop, outfitChanger);


        /*

        // set the current hat int to outfit changer current hat int
        CurrentGameObjectInt = outfitChanger.currentGOint;

        // set the current hat material int to outfit changer current hat material int
        CurrentGameObjectMaterialInt = outfitChanger.currentGOMaterialint;

        // set this int to the outfit changer character material int
        CharacterMaterialInt = outfitChanger.currentCharacterint;

        // this currency int = shop currency int
        Currency = shop.Currency;

        //hatBoughtInts = shop.BoughtHatInts;

        

        //
        hatBoughtInts = shop.these;



        //
        //HatBoughtBools.

        //
        //CharacterBoughtBools = shop.IsCharacterBought;

        //
        //HatBoughtBools = shop.IsHatBought;

        //SaveHats();

        

        // take this save player function and save it using the savesystem script
        //b_SaveSystem.SavePlayer(this);

        // this works and it tells you the saved amount of numbers
        Debug.Log("Saved " + hatBoughtInts.Count + " Hat Ints"); // hatBoughtInts  shop.these.Count

        Debug.Log("SavedThePlayer");

        */
    }

    /*

    public void SaveHats() {

        //
        hatBoughtInts = shop.BoughtHatInts;

        //shop.IsHatBought += shop.these;

        //shop.IsHatBought[i] = false;

    }

    */

    // Load the player
    public void LoadPlayer() {

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer();

        shop.Currency = data.iCurrency;

        shop.these = data.boughtHatList;

        for (int i = 0; i < shop.IsHatBought.Count; i++) {


            for (int j = 0; j < shop.these.Count; j++) {

                if (i == shop.these[j]) {

                    shop.IsHatBought[i] = true;

                }

            }

            


            /*
            if (shop.these.IndexOf(i) == shop.IsHatBought.IndexOf(i)) {

                //
                //shop.these[i] = 1;

            }
            */

            

        }
        /*

        // set the current hat int in the outfit changer to the current hat gameobject int that was saved
        outfitChanger.currentGOint = CurrentGameObjectInt;

        // set the current hat material int in the outfit changer to the current hat material int that was saved
        outfitChanger.currentGOMaterialint = CurrentGameObjectMaterialInt;

        // set the outfit changer int to the character material int in this class that was saved
        outfitChanger.currentCharacterint = CharacterMaterialInt;

        // call LoadedCustomizables from the Outfitchanger script
        outfitChanger.LoadedCustomizables();

        // set the shop currency to this currency
        shop.Currency = Currency;

        //
        //shop.BoughtHatInts = hatBoughtInts;

        //shop.Loaded = true;

        //
        //shop.these = hatBoughtInts;

        for (int i = 0; i < shop.these.Count; i++) {

            shop.these[i] = hatBoughtInts[i];

        }

        //
        //shop.IsHatBought= hatBoughtInts;

        //
        //shop.ResetBools();

        //
        //shop.IsCharacterBought = CharacterBoughtBools;

        //
        //shop.IsHatBought = HatBoughtBools;


        */
    }

    #endregion

}
