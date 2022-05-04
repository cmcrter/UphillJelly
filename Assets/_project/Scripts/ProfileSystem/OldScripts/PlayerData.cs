////////////////////////////////////////////////////////////
// File: PlayerData.cs
// Author: Jack Peedle
// Date Created: 22/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 22/10/21
// Brief: A script to control the profile system
//////////////////////////////////////////////////////////// 



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

[Serializable]
public class PlayerData
{

    // int for currency
    public int iCurrency;

    // Contructor to tell the PlayerData where to get the data from
    public PlayerData (Shop shopData, OutfitChanger outfitChangerData) {

        iCurrency = shopData.Currency;

        //
        //saveHatColour = outfitChanger.hatColour;

        //
        //saveHatObject = outfitChanger.hatObject;

        // Take data from profile controller and assign it in this one
        //
        //TMPProfileTextInput = profileController.TMPProfileTextInput;

        //
        //Currency = outfitChanger.Currency;

        //
        //CurrentActivePlayerMaterial = profileController.CurrentActivePlayerMaterial;

        //
        //currentActiveHat = outfitChanger.currentActiveHat;

        //
        //hatSpawnPosition = profileController.hatSpawnPosition;

    }


}


