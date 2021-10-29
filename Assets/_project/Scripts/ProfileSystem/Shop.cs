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

    //
    public OutfitChanger outfitChanger;

    // int for the currency
    public int Currency;

    // text for the currency text
    public Text currencyText;


    //
    public bool IsBought;


    //
    public Text CurrentHatPriceText;

    // 
    public List<int> IndividualHatPrices = new List<int>();

    //
    public int CurrentHatSelectedInt;

    //
    public int CurrentHatPriceInt;



    //
    public List<bool> IsHatBought = new List<bool>();

    //
    public List<bool> IsCharacterBought = new List<bool>();



    //
    public Text CurrentCharacterPriceText;

    // 
    public List<int> IndividualCharacterPrices = new List<int>();

    //
    public int CurrentCharacterSelectedInt;

    //
    public int CurrentCharacterPriceInt;

    #endregion

    #region Methods

    // On start
    public void Start() {

        // set the currency to 500
        Currency = 500;

        //
        CurrentHatSelectedInt = 4;

        //
        CurrentCharacterSelectedInt = 2;


        

    }

    // on update
    public void Update() {

        // set the currency text to "Currency : £ " + currency int
        currencyText.text = "Currency : £ " + Currency;



        //
        CurrentHatSelectedInt = outfitChanger.currentGOint;

        //
        CurrentHatPriceInt = IndividualHatPrices[CurrentHatSelectedInt];

        //
        CurrentHatPriceText.text = "This hat costs £" + CurrentHatPriceInt;




        //
        CurrentCharacterSelectedInt = outfitChanger.currentCharacterint;

        //
        CurrentCharacterPriceInt = IndividualCharacterPrices[CurrentCharacterSelectedInt];

        //
        CurrentCharacterPriceText.text = "This Character costs £" + CurrentCharacterPriceInt;


    }

    //
    public void NextHatPrice() {

        //
        CurrentHatSelectedInt++;

        /*
        // if the hat int is more than or = to the hat list.count
        if (CurrentHatSelectedInt >= IndividualHatPrices.Count) {

            // set the int to 0
            CurrentHatSelectedInt = 0;

        }
        */
    }

    //
    public void PreviousHatPrice() {

        //
        CurrentHatSelectedInt--;

        /*
        // if the hat int is less than or = to 0
        if (CurrentHatSelectedInt == -1) {

            // set the hat int to the list of hats.count - 1
            CurrentHatSelectedInt = IndividualHatPrices.Count - 1;

        }
        */

    }

    //
    public void BuyCurrentHat() {

        //


    }





    //
    public void NextCharacterPrice() {

        //
        CurrentCharacterSelectedInt++;

        /*
        // if the hat int is more than or = to the hat list.count
        if (CurrentCharacterSelectedInt >= IndividualCharacterPrices.Count) {

            // set the int to 0
            CurrentCharacterSelectedInt = 0;

        }
        */

    }

    //
    public void PreviousCharacterPrice() {

        //
        CurrentCharacterSelectedInt--;

        /*
        // if the hat int is less than or = to 0
        if (CurrentCharacterSelectedInt == -1) {

            // set the hat int to the list of hats.count - 1
            CurrentCharacterSelectedInt = IndividualCharacterPrices.Count - 1;

        }
        */

    }

    //
    public void BuyCurrentCharacter() {

        //


    }



    #endregion

}
