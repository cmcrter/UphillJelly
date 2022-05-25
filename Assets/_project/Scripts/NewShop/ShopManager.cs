////////////////////////////////////////////////////////////
// File: ShopManager.cs
// Author: Jack Peedle
// Date Created: 22/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 25/05/22
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

    public GameObject lastSelectedGO;

    public void Start() {

        playerCurrencyText.text = "£" + playerCurrency;

        /// Set the event system everytime goes over to shop
        ///selectedButton = eventSystem.firstSelectedGameObject.GetComponent<ShopTabButton>();

        

    }

    public void Update() {
        
        if (lastSelectedGO != eventSystem.currentSelectedGameObject) {
            Debug.Log("ChangedItem0");
            //
            if (eventSystem.currentSelectedGameObject != null) {
                Debug.Log("ChangedItem1");
                if (eventSystem.currentSelectedGameObject.TryGetComponent<ShopTabButton>(out ShopTabButton button)) {
                    Debug.Log("ChangedItem2");
                    OnTabHover(button);

                }

            }

        }

        lastSelectedGO = eventSystem.currentSelectedGameObject;
        
    }

    public void SwitchToShop() {

        eventSystem.SetSelectedGameObject(selectedButton.gameObject);
        
    }

    public void hasBoughtItem() {

        //var item = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        var skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();

        /*
        ///
        ///
        if (hatItem == eventSystem.currentSelectedGameObject.GetComponent<HatItem>() && 
            eventSystem.currentSelectedGameObject.GetComponent<HatItem>().item.isPurchased) {

            hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;

        } else if (hatItem == eventSystem.currentSelectedGameObject.GetComponent<HatItem>() &&
            !eventSystem.currentSelectedGameObject.GetComponent<HatItem>().item.isPurchased) {

            //do nothing

        }
        ///
        ///

        if (characterItem == eventSystem.currentSelectedGameObject.GetComponent<CharacterItem>() &&
            eventSystem.currentSelectedGameObject.GetComponent<CharacterItem>().item.isPurchased) {

            characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;

        } else if (characterItem == eventSystem.currentSelectedGameObject.GetComponent<CharacterItem>() &&
            !eventSystem.currentSelectedGameObject.GetComponent<CharacterItem>().item.isPurchased) {

            //do nothing

        }

        if (skateboardItem == eventSystem.currentSelectedGameObject.GetComponent<SkateboardItem>() &&
            eventSystem.currentSelectedGameObject.GetComponent<SkateboardItem>().item.isPurchased) {

            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;

        } else if (skateboardItem == eventSystem.currentSelectedGameObject.GetComponent<SkateboardItem>() &&
            !eventSystem.currentSelectedGameObject.GetComponent<SkateboardItem>().item.isPurchased) {

            //do nothing

        }
        */






        if (hatItem && !hatItem.item.isPurchased) {

            if (playerCurrency >= hatItem.item.ItemPrice) {

                playerCurrency -= hatItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                hatItem.item.isPurchased = true;
                currentEquippedHat = hatItem.item;

                hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;

                currentEquippedHat = selectedButton.GetComponent<HatItem>().item;

            } else if (playerCurrency < hatItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");
                
            }

        } else if (hatItem && hatItem.item.isPurchased) {

            hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;
            currentEquippedHat = selectedButton.GetComponent<HatItem>().item;
        }


        if (characterItem && !characterItem.item.isPurchased) {

            if (playerCurrency >= characterItem.item.ItemPrice) {

                playerCurrency -= characterItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                characterItem.item.isPurchased = true;

                currentEquippedCharacter = characterItem.item;

                characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;

                currentEquippedCharacter = selectedButton.GetComponent<CharacterItem>().item;

            } else if (playerCurrency < characterItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");

            }

        } else if (characterItem && characterItem.item.isPurchased) {

            characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;
            currentEquippedCharacter = selectedButton.GetComponent<CharacterItem>().item;
        }
        /*
        if (characterItem.item.isPurchased) {
            characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;
        }
        if (skateboardItem.item.isPurchased) {
            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;
        }

        8*/
        if (skateboardItem && !skateboardItem.item.isPurchased) {

            if (playerCurrency >= skateboardItem.item.ItemPrice) {

                playerCurrency -= skateboardItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                skateboardItem.item.isPurchased = true;

                currentEquippedSkateboard = skateboardItem.item;

                skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;

                currentEquippedSkateboard = selectedButton.GetComponent<SkateboardItem>().item;

            } else if (playerCurrency < skateboardItem.item.ItemPrice) {

                Debug.Log("Doesn't have enough to buy");

            }

        } else if (skateboardItem && skateboardItem.item.isPurchased) {

            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;
            currentEquippedSkateboard = selectedButton.GetComponent<SkateboardItem>().item;
        }


    }






    



    public void Subscribe(ShopTabButton button) {

        if (tabButtons == null) {

            tabButtons = new List<ShopTabButton>();

        }

        tabButtons.Add(button);


    }

    public void OnTabHover(ShopTabButton button) {

        ///eventSystem.SetSelectedGameObject(selectedButton.gameObject);
        selectedButton = button.GetComponent<ShopTabButton>();
        //eventSystem.SetSelectedGameObject(selectedButton.gameObject);

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
        Debug.Log("ChangedItem4");
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
        Debug.Log("ChangedItem5");
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

        ///
        /// go to buy button, check if have enough money, change buy button text
        ///


        hasBoughtItem();
        ResetTabs();

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

        }

    }

    public void OnDrawGizmos() {

        if (eventSystem.currentSelectedGameObject != null) {
            Gizmos.DrawSphere(eventSystem.currentSelectedGameObject.transform.position, 2);
        }

        
        

    }


}
