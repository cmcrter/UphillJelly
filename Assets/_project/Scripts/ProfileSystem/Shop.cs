////////////////////////////////////////////////////////////
// File: Shop.cs
// Author: Jack Peedle
// Date Created: 25/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 25/10/21
// Brief: A script to control the shop and all transactions
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    #region Variables

    // int for the currency
    public int Currency;

    // text for the currency text
    public Text currencyText;

    #endregion

    #region Methods

    // On start
    public void Start() {

        // set the currency to 500
        Currency = 500;


    }

    // on update
    public void Update() {

        // set the currency text to "Currency : £ " + currency int
        currencyText.text = "Currency : £ " + Currency;

    }

    #endregion

}
