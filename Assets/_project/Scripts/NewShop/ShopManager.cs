////////////////////////////////////////////////////////////
// File: ShopManager.cs
// Author: Jack Peedle
// Date Created: 22/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 22/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using L7Games.Input;
using SleepyCat;
using System;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public GameObject hatShopItem;
    public GameObject characterShopMesh;
    public GameObject skateboardShopItem;

    

    public ScriptableObject[] hatSOs;
    public ScriptableObject[] characterSOs;
    public ScriptableObject[] skateboardSOs;

    public ScriptableObject currentEquippedHat;
    public ScriptableObject currentEquippedCharacter;
    public ScriptableObject currentEquippedSkateboard;

    [SerializeField]
    [Tooltip("The Event system in the scene")]
    private UnityEngine.EventSystems.EventSystem eventSystem;



    //
    public List<ShopTabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public ShopTabButton selectedButton;
    public List<GameObject> objectsToSwap;

    public ShopInventory shopInventory;

    public int currentTabInt;
    //

    public TextMeshProUGUI[] objectDescription;
    public Image[] descriptionItemImage;
    public TextMeshProUGUI buyOwnedText;

    public TextMeshProUGUI playerCurrencyText;
    [SerializeField]
    private int playerCurrency;

    public void Start() {

        playerCurrencyText.text = "£" + playerCurrency;

        /*
        var hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        var skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();


        if (hatItem.item.isPurchased == true) {

            hatItem.item.uiDisplay = hatItem.item.boughtUIDisplay;

        }
        */
        selectedButton = eventSystem.firstSelectedGameObject.GetComponent<ShopTabButton>();

        //hatSOs = (HatObject)ScriptableObject.FindObjectsOfType(ScriptableObject);
        //Material characterShopMaterial = characterShopObject.GetComponent<Material>();
    }

    public void hasBoughtItem() {

        //var item = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        var skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();

        if (hatItem) {

            if (playerCurrency >= hatItem.item.ItemPrice) {

                playerCurrency -= hatItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                hatItem.item.isPurchased = true;
                //hatItem.item.uiDisplay = boughtSprite;

                currentEquippedHat = hatItem.item;

                hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;

            } else if (playerCurrency < hatItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");

            }

        }

        if (characterItem) {

            if (playerCurrency >= characterItem.item.ItemPrice) {

                playerCurrency -= characterItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                characterItem.item.isPurchased = true;
                //hatItem.item.uiDisplay = boughtSprite;

                currentEquippedCharacter = characterItem.item;

                characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;

            } else if (playerCurrency < characterItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");

            }

        }

        if (skateboardItem) {

            if (playerCurrency >= skateboardItem.item.ItemPrice) {

                playerCurrency -= skateboardItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                skateboardItem.item.isPurchased = true;

                currentEquippedSkateboard = skateboardItem.item;

                skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;

            } else if (playerCurrency < skateboardItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");

            }

        }
        /*
        if (characterItem) {

            characterItem.item.isPurchased = true;
            //characterItem.item.uiDisplay = boughtSprite;

            currentEquippedCharacter = characterItem.item;

            //characterShopMaterial = characterItem.item.material;
            
            //characterShopObject = characterItem.item.material;
        }

        if (skateboardItem) {

            skateboardItem.item.isPurchased = true;
            //skateboardItem.item.uiDisplay = boughtSprite;

            currentEquippedSkateboard = skateboardItem.item;

            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;

        }
        */
        


    }






    



    public void Subscribe(ShopTabButton button) {

        if (tabButtons == null) {

            tabButtons = new List<ShopTabButton>();

        }

        tabButtons.Add(button);


    }

    public void OnTabHover(ShopTabButton button) {

        
        selectedButton = button.GetComponent<ShopTabButton>();


        ///REMOVE AND REPLACE FOR MORE EFFICIENT
        ///TEST********

        if (selectedButton.GetComponent<HatItem>()) {
            objectDescription[0].text = selectedButton.GetComponent<HatItem>().item.itemDescription;
            descriptionItemImage[0].sprite = selectedButton.GetComponent<HatItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<HatItem>().item.ItemPrice;

            if (selectedButton.GetComponent<HatItem>().item.isPurchased) {
                buyOwnedText.text = "Owned";
            } else {

                buyOwnedText.text = "£ " + selectedButton.GetComponent<HatItem>().item.ItemPrice;

            }
        }
        if (selectedButton.GetComponent<CharacterItem>()) {
            objectDescription[1].text = selectedButton.GetComponent<CharacterItem>().item.itemDescription;
            descriptionItemImage[1].sprite = selectedButton.GetComponent<CharacterItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<CharacterItem>().item.ItemPrice;

            if (selectedButton.GetComponent<CharacterItem>().item.isPurchased) {
                buyOwnedText.text = "Owned";
            } else {

                buyOwnedText.text = "£ " + selectedButton.GetComponent<CharacterItem>().item.ItemPrice;

            }
        }
        if (selectedButton.GetComponent<SkateboardItem>()) {
            objectDescription[2].text = selectedButton.GetComponent<SkateboardItem>().item.itemDescription;
            descriptionItemImage[2].sprite = selectedButton.GetComponent<SkateboardItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<SkateboardItem>().item.ItemPrice;

            if (selectedButton.GetComponent<SkateboardItem>().item.isPurchased) {
                buyOwnedText.text = "Owned";
            } else {

                buyOwnedText.text = "£ " + selectedButton.GetComponent<SkateboardItem>().item.ItemPrice;

            }
        }


        

        ///
        ///

        ResetTabs();

        //Debug.Log("Hovered over button");

        // Only change tab sprite if it is not already selected
        if (selectedButton == null || button != selectedButton) {

            //button.backgroundTabImage.sprite = tabHover;

        }

    }

    public void OnTabExit(ShopTabButton button) {

        ResetTabs();

    }
    
    public void OnTabSelected(ShopTabButton button) {

        selectedButton = button.GetComponent<ShopTabButton>();

        if (selectedButton.GetComponent<HatItem>().item.isPurchased) {

            buyOwnedText.text = "Owned";
            // Do not let them purchase again
            Debug.Log("Already Purchased Item");

        } else {

            if (!selectedButton.GetComponent<HatItem>().item.isPurchased) {
                hasBoughtItem();
            }

        }

        ///
        /// go to buy button, check if have enough money, change buy button text
        ///

        ResetTabs();

        //button.backgroundTabImage.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++) {

            if (i == index) {
                objectsToSwap[i].SetActive(true);
            } else {
                objectsToSwap[i].SetActive(false);
            }

        }

    }
    
    public void ResetTabs() {

        foreach (ShopTabButton button in tabButtons) {

            if (selectedButton != null && button == selectedButton) { continue; }

            //button.backgroundTabImage.sprite = tabIdle;

        }

    }



}
