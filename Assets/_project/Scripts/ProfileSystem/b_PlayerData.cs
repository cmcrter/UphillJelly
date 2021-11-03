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
    public List<int> boughtHatList;

    #endregion

    #region Methods



    // Contructor to tell the PlayerData where to get the data from
    public b_PlayerData(Shop shopData, OutfitChanger outfitChangerData) {

        iCurrency = shopData.Currency;

        boughtHatList = shopData.these;

    }

    #endregion



}
