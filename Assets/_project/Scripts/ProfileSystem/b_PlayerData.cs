////////////////////////////////////////////////////////////
// File: b_PlayerData.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 25/10/21
// Brief: A script to control the outfit system
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// save it in file
[System.Serializable]
public class b_PlayerData
{

    #region Variables

    // int for currency
    public int iCurrency;

    //
    public List<int> savedHatList;

    //
    public List<int> savedCharacterList;

    // gameobject for the hat price panel
    //public GameObject go_HatPricePanel;

    // gameobject for the character price panel
    //public GameObject go_CharacterPricePanel;

    //
    public List<int> iIndividualHatPrices;

    //
    public List<int> iIndividualCharacterPrices;

    // current int for the hats
    public int icurrentGOint;

    // current int for the hat material 
    public int icurrentGOMaterialint;

    // current character int
    public int icurrentCharacterint;


    #endregion

    #region Methods



    // Contructor to tell the PlayerData where to get the data from
    public b_PlayerData(Shop shopData, OutfitChanger outfitChangerData) {

        iCurrency = shopData.Currency;


        savedHatList = shopData.iSavedHatInts;

        savedCharacterList = shopData.iSavedCharacterInts;


        iIndividualHatPrices = shopData.IndividualHatPrices;

        iIndividualCharacterPrices = shopData.IndividualCharacterPrices;


        icurrentGOint = outfitChangerData.currentGOint;

        icurrentGOMaterialint = outfitChangerData.currentGOMaterialint;

        icurrentCharacterint = outfitChangerData.currentCharacterint;

    }

    #endregion



}
