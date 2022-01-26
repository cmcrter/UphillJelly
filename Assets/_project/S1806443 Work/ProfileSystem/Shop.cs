////////////////////////////////////////////////////////////
// File: Shop.cs
// Author: Jack Peedle
// Date Created: 25/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief: A script to control the shop and all transactions
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    #region Variables

    // gameobject panel for sure you want to buy hat
    public GameObject SureYouWantToBuyHat;

    // gameobject panel for sure you want to buy character
    public GameObject SureYouWantToBuyCharacter;

    // reference to the b_player
    public b_Player b_player;

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

    // Buy hat button
    public GameObject BuyHatButton;

    // buy character button
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


    // List of the saved hat ints to save
    public List<int> iSavedHatInts = new List<int>(); // SAVED

    // List of the character materials and gameobjects as ints to save
    public List<int> iSavedCharacterInts = new List<int>(); // SAVED


    // current character price text
    public Text CurrentCharacterPriceText;

    //
    //
    //
    public ReplaySaveManager replaySaveManager;
    //
    //
    //
    
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

            // set the hat buy button to false (cant buy a hat they have bought)
            BuyHatButton.SetActive(false);

        }

        // if the hat is not bought with the current selected int of that hat
        if (!IsHatBought[CurrentHatSelectedInt]) {

            // set the hat price panel to true
            HatPricePanel.SetActive(true);

            // set the hat buy button to true (buy a hat they don't have)
            BuyHatButton.SetActive(true);

        }

        // if the character is bought with the current selected int of that character
        if (IsCharacterBought[CurrentCharacterSelectedInt]) {

            // set the character price panel to false
            CharacterPricePanel.SetActive(false);

            // set the character buy button to false (cant buy a character they have bought)
            BuyCharacterButton.SetActive(false);

        }

        // if the character is not bought with the current selected int of that character
        if (!IsCharacterBought[CurrentCharacterSelectedInt]) {

            // set the character price panel to true
            CharacterPricePanel.SetActive(true);

            // set the character buy button to true (buy a character they don't have)
            BuyCharacterButton.SetActive(true);

        }

        

    }

    // sure you want to buy hat method
    public void SureWantToBuyHat() {

        // set sure you want to buy hat panel to true
        SureYouWantToBuyHat.SetActive(true);

    }

    // sure you want to buy character method
    public void SureWantToBuyCharacter() {

        // set sure you want to buy character panel to true
        SureYouWantToBuyCharacter.SetActive(true);

    }

    // hide sure you want to buy hat method
    public void HideHatBuyPanel() {

        // set sure you want to buy hat panel to false
        SureYouWantToBuyHat.SetActive(false);

    }

    // hide sure you want to buy character method
    public void HideCharacterBuyPanel() {

        // set sure you want to buy character panel to false
        SureYouWantToBuyCharacter.SetActive(false);

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

        // sure you want to buy hat panel to false
        SureYouWantToBuyHat.SetActive(false);

        // if the player does not have enough currency for the hat
        if (Currency < CurrentHatPriceInt) {

            // Debug
            Debug.Log("Hat Too Expensive");

        }

        // if players currency is more than or = to the current hat price
        if (Currency >= CurrentHatPriceInt) {

            // takeaway the current hat price from the currency
            Currency -= CurrentHatPriceInt;

            // set the hat to the hat has been bought with the current hat int
            IsHatBought[CurrentHatSelectedInt] = true;

            // Method to buy the current hat
            BoughtHat();

            // Debug
            Debug.Log("BoughtHat");

            // Add in save after every purchase
            // if the current save is profile 1
            if (b_player.isSave1 == true) {

                Debug.Log("SAVED1");

                // save the player 1
                b_player.SavePlayer1();

            }

            // if the current save is profile 2
            if (b_player.isSave2 == true) {

                Debug.Log("SAVED2");

                // save the player 2
                b_player.SavePlayer2();

            }

            // if the current save is profile 3
            if (b_player.isSave3 == true) {

                Debug.Log("SAVED3");

                // save the player 3
                b_player.SavePlayer3();

            }

        }

        

    }


    // Bought hat method
    public void BoughtHat() {

        // if i < hatbought count
        for (int i = 0; i < IsHatBought.Count; i++) {

            // if the int for the hat contains i
            if (iSavedHatInts.Contains(i)) {

                // do nothing
            }

            // if the saved hat int doesn't contain i
            if (!iSavedHatInts.Contains(i)) {

                // set the hat bought to true
                if (IsHatBought[i] == true) {

                    // Do this
                    Debug.Log("Bools are " + i + IsHatBought[i]);

                    // add the hat to the saved hat ints
                    iSavedHatInts.Add(i);

                    // If the hat [i] is false
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

        //
        SureYouWantToBuyCharacter.SetActive(false);

        // if players currency is more than or = to the current character price
        if (Currency >= CurrentCharacterPriceInt) {

            // takeaway the current character price from the currency
            Currency -= CurrentCharacterPriceInt;

            // set the character to the character has been bought with the current character int
            IsCharacterBought[CurrentCharacterSelectedInt] = true;

            // Method to buy the current Character
            BoughtCharacter();

            // Debug
            Debug.Log("BoughtCharacter");

            // Add in save after every purchase
            // if the current save is profile 1
            if (b_player.isSave1 == true) {

                Debug.Log("SAVED1");

                // save the player 1
                b_player.SavePlayer1();

            }

            // if the current save is profile 2
            if (b_player.isSave2 == true) {

                Debug.Log("SAVED2");

                // save the player 2
                b_player.SavePlayer2();

            }

            // if the current save is profile 3
            if (b_player.isSave3 == true) {

                Debug.Log("SAVED3");

                // save the player 3
                b_player.SavePlayer3();

            }

        }

        // if the player does not have enough currency for the character
        if (Currency < CurrentCharacterPriceInt) {

            // Debug
            Debug.Log("Character Too Expensive");

        }

        

    }



    // bought character method
    public void BoughtCharacter() {

        // if i < characterbought count
        for (int i = 0; i < IsCharacterBought.Count; i++) {

            // if the int for the character contains i
            if (iSavedCharacterInts.Contains(i)) {

                // do nothing
            }

            // if the saved character int doesn't contain i
            if (!iSavedCharacterInts.Contains(i)) {

                // set the character bought to true
                if (IsCharacterBought[i] == true) {

                    // Do this
                    Debug.Log("Bools are " + i + IsCharacterBought[i]);

                    // add the character to the saved character ints
                    iSavedCharacterInts.Add(i);

                    // If the character [i] is false
                } else if (IsCharacterBought[i] == false) {

                    // Do nothing


                }

            }

        }

    }

    // method to save the replays (before game complete)
    public void lookatsaves() {

        //
        // Where is this method caled?
        // is it needed?
        // why am i not loading the second save 2 and 3?
        //
        //

        if (b_player.isSave1 == true) {

            // load the replay 1
            replaySaveManager.LoadReplay1();

            // load the second replay 1
            replaySaveManager.LoadSecondReplay1();

            Debug.Log("2");

        }

        if (b_player.isSave2 == true) {

            //
            replaySaveManager.LoadReplay2();

        }

        if (b_player.isSave3 == true) {

            //
            replaySaveManager.LoadReplay3();

        }



    }
    


    #endregion

}
