////////////////////////////////////////////////////////////
// File: Shop.cs
// Author: Jack Peedle
// Date Created: 25/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: A script to control the shop and all transactions
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using L7Games.Loading;
using UnityEngine;
using UnityEngine.UI;

[System.Obsolete]
public class Shop : MonoBehaviour
{
    #region Variables

    // reference to the scripts
    public b_Player b_player;
    public OutfitChanger outfitChanger;
    public ReplaySaveManager replaySaveManager;
    public LoadCustomizablesInGame loadCustomizablesInGame;

    // gameobject panel for sure you want to buy hat and sure you want to buy character
    public GameObject SureYouWantToBuyHat;
    public GameObject SureYouWantToBuyCharacter;

    // int for the currency and currency text
    public int Currency; // SAVED
    public Text currencyText;

    // gameobject for the hat and character price panel
    public GameObject HatPricePanel; // SAVED
    public GameObject CharacterPricePanel; // SAVED

    // Buy hat and character button
    public GameObject BuyHatButton;
    public GameObject BuyCharacterButton;

    // current hat price text
    public Text CurrentHatPriceText;

    // public list of the hat and character prices
    public List<int> IndividualHatPrices = new List<int>(); // SAVED
    public List<int> IndividualCharacterPrices = new List<int>(); // SAVED

    // current hat selected as an int and current character selected as an int
    public int CurrentHatSelectedInt;
    public int CurrentCharacterSelectedInt;

    // last hat and character int
    [SerializeField]
    public static int LastHatInt;
    [SerializeField]
    public static int LastCharacterInt;

    // current hat and character price as an int
    public int CurrentHatPriceInt;
    public int CurrentCharacterPriceInt;

    // list of bools for which hat is bought and for which character is bought
    public List<bool> IsHatBought = new List<bool>();
    public List<bool> IsCharacterBought = new List<bool>();

    // List of the saved hat ints to save and of the character materials and gameobjects as ints to save
    public List<int> iSavedHatInts = new List<int>(); // SAVED
    public List<int> iSavedCharacterInts = new List<int>(); // SAVED

    // current character price text
    public Text CurrentCharacterPriceText;

    #endregion

    #region Methods

    public void Start() 
    {

    }

    // on update
    public void Update() 
    {

    }

    // sure you want to buy hat method
    public void SureWantToBuyHat() 
    {

        // set sure you want to buy hat panel to true
        SureYouWantToBuyHat.SetActive(true);
    }

    // sure you want to buy character method
    public void SureWantToBuyCharacter() 
    {
        // set sure you want to buy character panel to true
        SureYouWantToBuyCharacter.SetActive(true);
    }

    // hide sure you want to buy hat method
    public void HideHatBuyPanel()
    {
        // set sure you want to buy hat panel to false
        SureYouWantToBuyHat.SetActive(false);
    }

    // hide sure you want to buy character method
    public void HideCharacterBuyPanel() 
    {
        // set sure you want to buy character panel to false
        SureYouWantToBuyCharacter.SetActive(false);
    }

    // Next hat price which is called in OutfitChanger
    public void NextHatPrice()
    {
        // increment the current hat by 1
        CurrentHatSelectedInt++;
    }

    // previous hat price which is called in OutfitChanger
    public void PreviousHatPrice()
    {
        // increment the current hat by -1
        CurrentHatSelectedInt--;
    }

    // buy the current hat
    public void BuyCurrentHat()
    {
        // sure you want to buy hat panel to false
        SureYouWantToBuyHat.SetActive(false);

        // if the player does not have enough currency for the hat
        if (Currency < CurrentHatPriceInt)
        {
            // Debug
            Debug.Log("Hat Too Expensive");
        }

        // if players currency is more than or = to the current hat price
        if (Currency >= CurrentHatPriceInt)
        {

            // takeaway the current hat price from the currency
            Currency -= CurrentHatPriceInt;

            // set the hat to the hat has been bought with the current hat int
            IsHatBought[CurrentHatSelectedInt] = true;

            // Method to buy the current hat
            BoughtHat();

            // Debug
            Debug.Log("BoughtHat");

            // Add in save after every purchase

        }
    }

    // Bought hat method
    public void BoughtHat() 
    {
        // if i < hatbought count
        for (int i = 0; i < IsHatBought.Count; i++) {
            // if the int for the hat contains i
            if (iSavedHatInts.Contains(i))
            {
                // do nothing
            }

            // if the saved hat int doesn't contain i
            if (!iSavedHatInts.Contains(i))
            {
                // set the hat bought to true
                if (IsHatBought[i] == true)
                {
                    // Do this
                    Debug.Log("Bools are " + i + IsHatBought[i]);

                    // add the hat to the saved hat ints
                    iSavedHatInts.Add(i);
                } 
                else if (IsHatBought[i] == false) 
                {
                    // Do nothing
                }
            }
        }
    }

    // next character price which is called in OutfitChanger
    public void NextCharacterPrice()
    {
        // increment the current character by 1
        CurrentCharacterSelectedInt++;
    }

    // previous character price which is called in OutfitChanger
    public void PreviousCharacterPrice()
    {
        // increment the current character by -1
        CurrentCharacterSelectedInt--;
    }

    // buy the current character
    public void BuyCurrentCharacter() 
    {
        SureYouWantToBuyCharacter.SetActive(false);

        // if players currency is more than or = to the current character price
        if (Currency >= CurrentCharacterPriceInt)
        {
            // takeaway the current character price from the currency
            Currency -= CurrentCharacterPriceInt;

            // set the character to the character has been bought with the current character int
            IsCharacterBought[CurrentCharacterSelectedInt] = true;

            // Method to buy the current Character
            BoughtCharacter();

            // Debug
            Debug.Log("BoughtCharacter");

            // Add in save after every purchase
            b_player.SavePlayer(LoadingData.playerSlot);
        }

        // if the player does not have enough currency for the character
        if (Currency < CurrentCharacterPriceInt)
        {
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
    
    #endregion
}
