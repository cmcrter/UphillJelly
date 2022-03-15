////////////////////////////////////////////////////////////
// File: b_PlayerData.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// save it in file
[System.Serializable]
public class b_PlayerData
{

    #region Variables

    // int for currency
    public int iCurrency;

    // list of the saved hat ints and saved character ints
    public List<int> savedHatList;
    public List<int> savedCharacterList;

    // list of the individual hat prices and individual character prices
    public List<int> iIndividualHatPrices;
    public List<int> iIndividualCharacterPrices;

    // current int for the hats and characters
    public int icurrentGOint;
    public int icurrentGOMaterialint;
    public int icurrentCharacterint;


    #endregion

    #region Methods

    // Contructor to tell the PlayerData where to get the data from
    public b_PlayerData(Shop shopData, OutfitChanger outfitChangerData) {

        // this classes currency = shop data currency
        iCurrency = shopData.Currency;

        // this classes saved hat list ints = shop data saved hat ints
        savedHatList = shopData.iSavedHatInts;

        // this classes saved character list = shop data saved character ints
        savedCharacterList = shopData.iSavedCharacterInts;

        // this classes int individual hat prices = shop data individual hat prices
        iIndividualHatPrices = shopData.IndividualHatPrices;

        // this classes int individual character prices = shop data individual character prices
        iIndividualCharacterPrices = shopData.IndividualCharacterPrices;

        // this classes current hat gameobject int = outfitchanger current gameobject int
        icurrentGOint = outfitChangerData.currentGOint;

        // this classes current character material int = outfitchanger current character material int
        icurrentGOMaterialint = outfitChangerData.currentGOMaterialint;

        // this classes current character gameobject int = outfitchanger current character gameobject int
        icurrentCharacterint = outfitChangerData.currentCharacterint;

    }

    #endregion


}
