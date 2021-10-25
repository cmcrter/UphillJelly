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

    // reference to the outfit changer script
    public OutfitChanger outfitChanger;

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

        // set the current hat int to outfit changer current hat int
        CurrentGameObjectInt = outfitChanger.currentGOint;

        // set the current hat material int to outfit changer current hat material int
        CurrentGameObjectMaterialInt = outfitChanger.currentGOMaterialint;

        // set this int to the outfit changer character material int
        CharacterMaterialInt = outfitChanger.currentCharacterint;

        // this currency int = outfit changer currency int
        Currency = outfitChanger.Currency;

        // take this save player function and save it using the savesystem script
        b_SaveSystem.SavePlayer(this);

    }

    // Load the player
    public void LoadPlayer() {

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer();

        // set the current hat int in the outfit changer to the current hat gameobject int that was saved
        outfitChanger.currentGOint = CurrentGameObjectInt;

        // set the current hat material int in the outfit changer to the current hat material int that was saved
        outfitChanger.currentGOMaterialint = CurrentGameObjectMaterialInt;

        // set the outfit changer int to the character material int in this class that was saved
        outfitChanger.currentCharacterint = CharacterMaterialInt;

        // call LoadedCustomizables from the Outfitchanger script
        outfitChanger.LoadedCustomizables();

        // set the outfitchanger currency to this currency
        outfitChanger.Currency = Currency;

    }

    #endregion

}
