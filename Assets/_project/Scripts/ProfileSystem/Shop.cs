////////////////////////////////////////////////////////////
// File: Shop.cs
// Author: Jack Peedle
// Date Created: 25/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 29/10/21
// Brief: A script to control the shop and all transactions
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    #region Variables

    // Outfitchanger reference
    public OutfitChanger outfitChanger;

    // int for the currency
    public int Currency; // SAVED

    // text for the currency text
    public Text currencyText;

    // gameobject for the hat price panel
    public GameObject HatPricePanel; // SAVED

    // gameobject for the character price panel
    public GameObject CharacterPricePanel; // SAVED

    //
    public GameObject BuyHatButton;

    //
    public GameObject BuyCharacterButton;

    // current hat price text
    public Text CurrentHatPriceText;

    // public list of the hat prices
    public List<int> IndividualHatPrices = new List<int>(); // SAVED

    //  public list of all the character prices
    public List<int> IndividualCharacterPrices = new List<int>(); // SAVED


    // current hat selected as an int
    public int CurrentHatSelectedInt;

    // current character selected as an int
    public int CurrentCharacterSelectedInt;



    // current hat price as an int
    public int CurrentHatPriceInt;

    // current character price as an int
    public int CurrentCharacterPriceInt;

    

    // list of bools for which hat is bought
    public List<bool> IsHatBought = new List<bool>();

    // list of bools for which character is bought
    public List<bool> IsCharacterBought = new List<bool>();


    //
    public List<int> iSavedHatInts = new List<int>(); // SAVED

    //
    public List<int> iSavedCharacterInts = new List<int>(); // SAVED


    // current character price text
    public Text CurrentCharacterPriceText;

    
    #endregion

    #region Methods

    // On start
    public void Start() {

        // set the currency to 500
        Currency = 500;

        // set the current hat to 4 (No hat)
        CurrentHatSelectedInt = 4;

        // set the current character to 2(one of the middle materials)
        CurrentCharacterSelectedInt = 2;


        

    }

    // on update
    public void Update() {

        // set the currency text to "Currency : £ " + currency int
        currencyText.text = "Currency : £ " + Currency;



        // set the current hat selected int to the outfits current hat int
        CurrentHatSelectedInt = outfitChanger.currentGOint;

        // set the current hat price to the individual hat price of that int
        CurrentHatPriceInt = IndividualHatPrices[CurrentHatSelectedInt];

        // set the hat text to display the price of the hat
        CurrentHatPriceText.text = "This hat costs £" + CurrentHatPriceInt;




        // set the current character selected int to the outfits current character int
        CurrentCharacterSelectedInt = outfitChanger.currentCharacterint;

        // set the current character price to the individual character price of that int
        CurrentCharacterPriceInt = IndividualCharacterPrices[CurrentCharacterSelectedInt];

        // set the character text to display the price of the character
        CurrentCharacterPriceText.text = "This Character costs £" + CurrentCharacterPriceInt;



        // if the hat is bought with the current selected int of that hat
        if (IsHatBought[CurrentHatSelectedInt]) {

            // set the hat price panel to false
            HatPricePanel.SetActive(false);

            //
            BuyHatButton.SetActive(false);

        }

        // if the hat is not bought with the current selected int of that hat
        if (!IsHatBought[CurrentHatSelectedInt]) {

            // set the hat price panel to true
            HatPricePanel.SetActive(true);

            //
            BuyHatButton.SetActive(true);

        }






        // if the character is bought with the current selected int of that character
        if (IsCharacterBought[CurrentCharacterSelectedInt]) {

            // set the character price panel to false
            CharacterPricePanel.SetActive(false);

            //
            BuyCharacterButton.SetActive(false);

        }

        // if the character is not bought with the current selected int of that character
        if (!IsCharacterBought[CurrentCharacterSelectedInt]) {

            // set the character price panel to true
            CharacterPricePanel.SetActive(true);

            //
            BuyCharacterButton.SetActive(true);

        }

    }

    


    // Next hat price which is called in OutfitChanger
    public void NextHatPrice() {

        // increment the current hat by 1
        CurrentHatSelectedInt++;

    }

    // previous hat price which is called in OutfitChanger
    public void PreviousHatPrice() {

        // increment the current hat by -1
        CurrentHatSelectedInt--;

    }

    // buy the current hat
    public void BuyCurrentHat() {

        // if players currency is more than or = to the current hat price
        if (Currency >= CurrentHatPriceInt) {

            // takeaway the current hat price from the currency
            Currency -= CurrentHatPriceInt;

            // set the hat to the hat has been bought with the current hat int
            IsHatBought[CurrentHatSelectedInt] = true;

            //
            BoughtHat();

            // Debug
            Debug.Log("BoughtHat");

        }
        
        // if the player does not have enough currency for the hat
        if (Currency < CurrentHatPriceInt) {

            // Debug
            Debug.Log("Hat Too Expensive");

        }

        

    }


    //
    public void BoughtHat() {

        //
        for (int i = 0; i < IsHatBought.Count; i++) {

            if (iSavedHatInts.Contains(i)) {

                // do nothing
            }

            if (!iSavedHatInts.Contains(i)) {

                //
                if (IsHatBought[i] == true) {

                    // Do this
                    Debug.Log("Bools are " + i + IsHatBought[i]);

                    //
                    iSavedHatInts.Add(i);

                } else if (IsHatBought[i] == false) {

                    // Do nothing


                }

            }

        }

    }





    // next character price which is called in OutfitChanger
    public void NextCharacterPrice() {

        // increment the current character by 1
        CurrentCharacterSelectedInt++;

    }

    // previous character price which is called in OutfitChanger
    public void PreviousCharacterPrice() {

        // increment the current character by -1
        CurrentCharacterSelectedInt--;

    }

    // buy the current character
    public void BuyCurrentCharacter() {

        // if players currency is more than or = to the current character price
        if (Currency >= CurrentCharacterPriceInt) {

            // takeaway the current character price from the currency
            Currency -= CurrentCharacterPriceInt;

            // set the character to the character has been bought with the current character int
            IsCharacterBought[CurrentCharacterSelectedInt] = true;

            //
            BoughtCharacter();

            // Debug
            Debug.Log("BoughtCharacter");

        }

        // if the player does not have enough currency for the character
        if (Currency < CurrentCharacterPriceInt) {

            // Debug
            Debug.Log("Character Too Expensive");

        }

    }



    //
    public void BoughtCharacter() {

        //
        for (int i = 0; i < IsCharacterBought.Count; i++) {

            if (iSavedCharacterInts.Contains(i)) {

                // do nothing
            }

            if (!iSavedCharacterInts.Contains(i)) {

                //
                if (IsCharacterBought[i] == true) {

                    // Do this
                    Debug.Log("Bools are " + i + IsCharacterBought[i]);

                    //
                    iSavedCharacterInts.Add(i);

                } else if (IsCharacterBought[i] == false) {

                    // Do nothing


                }

            }

        }

    }




    #endregion

}
