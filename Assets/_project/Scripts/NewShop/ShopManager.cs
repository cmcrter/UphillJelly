////////////////////////////////////////////////////////////
// File: ShopManager.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 22/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 25/05/22
// Brief: A script to manage the shop system
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
    #region Variables
    public GameObject hatShopItem;
    public GameObject characterShopMesh;
    public GameObject skateboardShopItem;

    public ScriptableObject[] hatSOs;
    public ScriptableObject[] characterSOs;
    public ScriptableObject[] skateboardSOs;

    public ItemObjectSO currentEquippedHat;
    public ItemObjectSO currentEquippedCharacter;
    public ItemObjectSO currentEquippedSkateboard;

    [SerializeField]
    [Tooltip("The Event system in the scene")]
    private UnityEngine.EventSystems.EventSystem eventSystem;

    public List<ShopTabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public ShopTabButton selectedButton;
    public List<GameObject> objectsToSwap;

    public ShopInventory shopInventory;

    public int currentTabInt;

    public TextMeshProUGUI[] objectDescription;
    public Image[] descriptionItemImage;
    public TextMeshProUGUI buyOwnedText;

    public TextMeshProUGUI playerCurrencyText;
    public int playerCurrency;

    public GameObject lastSelectedGO;

    #endregion

    #region Unity Methods

    public void Start()
    {
        playerCurrencyText.text = "£" + playerCurrency;
    }

    public void Update()
    {      
        if (lastSelectedGO != eventSystem.currentSelectedGameObject) 
        {
            if (eventSystem.currentSelectedGameObject != null) 
            {
                if (eventSystem.currentSelectedGameObject.TryGetComponent<ShopTabButton>(out ShopTabButton button))
                {
                    OnTabHover(button);
                }
            }
        }

        lastSelectedGO = eventSystem.currentSelectedGameObject;     
    }

    #endregion

    public void SwitchToShop() 
    {
        eventSystem.SetSelectedGameObject(selectedButton.gameObject);    
    }

    public void hasBoughtItem()
    {
        HatItem hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        CharacterItem characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        SkateboardItem skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();

        if (hatItem && !hatItem.item.isPurchased)
        {
            if (playerCurrency >= hatItem.item.ItemPrice)
            {

                playerCurrency -= hatItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                hatItem.item.isPurchased = true;
                currentEquippedHat = hatItem.item;

                hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;

                currentEquippedHat = selectedButton.GetComponent<HatItem>().item;

            } 
            else if (playerCurrency < hatItem.item.ItemPrice) 
            {
                Debug.Log("Doesn't have enough to buy");   
            }

        } 
        else if (hatItem && hatItem.item.isPurchased)
        {
            hatShopItem.GetComponent<MeshFilter>().sharedMesh = hatItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            hatShopItem.GetComponent<MeshRenderer>().material = hatItem.item.material;
            currentEquippedHat = selectedButton.GetComponent<HatItem>().item;
        }


        if (characterItem && !characterItem.item.isPurchased)
        {
            if (playerCurrency >= characterItem.item.ItemPrice)
            {

                playerCurrency -= characterItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                characterItem.item.isPurchased = true;

                currentEquippedCharacter = characterItem.item;

                characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;

                currentEquippedCharacter = selectedButton.GetComponent<CharacterItem>().item;

            } 
            else if (playerCurrency < characterItem.item.ItemPrice)
            {

                Debug.Log("Doesn't have enough to buy");

            }

        }
        else if (characterItem && characterItem.item.isPurchased)
        {
            characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = characterItem.item.material;
            currentEquippedCharacter = selectedButton.GetComponent<CharacterItem>().item;
        }

        if (skateboardItem && !skateboardItem.item.isPurchased)
        {
            if (playerCurrency >= skateboardItem.item.ItemPrice)
            {

                playerCurrency -= skateboardItem.item.ItemPrice;

                playerCurrencyText.text = "£" + playerCurrency;
                buyOwnedText.text = "Owned";

                skateboardItem.item.isPurchased = true;

                currentEquippedSkateboard = skateboardItem.item;

                skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;

                currentEquippedSkateboard = selectedButton.GetComponent<SkateboardItem>().item;

            } 
            else if (playerCurrency < skateboardItem.item.ItemPrice)
            {
                Debug.Log("Doesn't have enough to buy");
            }

        } 
        else if (skateboardItem && skateboardItem.item.isPurchased) 
        {
            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboardItem.item.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboardItem.item.material;
            currentEquippedSkateboard = selectedButton.GetComponent<SkateboardItem>().item;
        }
    }

    public void Subscribe(ShopTabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<ShopTabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabHover(ShopTabButton button)
    {
        selectedButton = button.GetComponent<ShopTabButton>();

        ///REMOVE AND REPLACE FOR MORE EFFICIENT
        ///TEST********

        if (selectedButton.GetComponent<HatItem>()) 
        {

            objectDescription[0].text = selectedButton.GetComponent<HatItem>().item.itemDescription;
            descriptionItemImage[0].sprite = selectedButton.GetComponent<HatItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<HatItem>().item.ItemPrice;

            if (selectedButton.GetComponent<HatItem>().item.isPurchased) 
            {
                buyOwnedText.text = "Owned";
            } 
            else 
            {
                buyOwnedText.text = "£ " + selectedButton.GetComponent<HatItem>().item.ItemPrice;
            }
        }


        if (selectedButton.GetComponent<CharacterItem>())
        {
            objectDescription[1].text = selectedButton.GetComponent<CharacterItem>().item.itemDescription;
            descriptionItemImage[1].sprite = selectedButton.GetComponent<CharacterItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<CharacterItem>().item.ItemPrice;

            if (selectedButton.GetComponent<CharacterItem>().item.isPurchased)
            {
                buyOwnedText.text = "Owned";
            }
            else 
            {
                buyOwnedText.text = "£ " + selectedButton.GetComponent<CharacterItem>().item.ItemPrice;
            }
        }


        if (selectedButton.GetComponent<SkateboardItem>()) 
        {
            objectDescription[2].text = selectedButton.GetComponent<SkateboardItem>().item.itemDescription;
            descriptionItemImage[2].sprite = selectedButton.GetComponent<SkateboardItem>().item.uiDisplay;
            ///shopPriceText.text = "£ " + selectedButton.GetComponent<SkateboardItem>().item.ItemPrice;

            if (selectedButton.GetComponent<SkateboardItem>().item.isPurchased)
            {
                buyOwnedText.text = "Owned";
            }
            else
            {
                buyOwnedText.text = "£ " + selectedButton.GetComponent<SkateboardItem>().item.ItemPrice;
            }
        }

        ResetTabs();

        //Debug.Log("Hovered over button");

        // Only change tab sprite if it is not already selected
        if (selectedButton == null || button != selectedButton) 
        {
            //button.backgroundTabImage.sprite = tabHover;
        }
    }

    public void OnTabExit(ShopTabButton button) 
    {
        ResetTabs();
    }
    
    public void OnTabSelected(ShopTabButton button)
    {
        selectedButton = button.GetComponent<ShopTabButton>();

        ///
        /// go to buy button, check if have enough money, change buy button text
        ///

        hasBoughtItem();
        ResetTabs();

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            } 
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
    
    public void ResetTabs()
    {
        foreach (ShopTabButton button in tabButtons)
        {
            if (selectedButton != null && button == selectedButton)
            { 
                continue;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            Gizmos.DrawSphere(eventSystem.currentSelectedGameObject.transform.position, 2);
        }              
    }

    /// <summary>
    /// Functions to connect the system with the rest of the codebase such as the profiles
    /// </summary>
    public void UpdateCurrencyText()
    {
        playerCurrencyText.text = "£" + playerCurrency;
    }

    public void ApplyEquipmentToPreview(ItemObjectSO hat, ItemObjectSO character, ItemObjectSO skateboard)
    {
        if (character)
        {
            characterShopMesh.gameObject.GetComponent<SkinnedMeshRenderer>().material = character.material;
        }

        if (hat)
        {
            hatShopItem.GetComponent<MeshFilter>().sharedMesh = hat.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            hatShopItem.GetComponent<MeshRenderer>().material = hat.material;
        }

        if (skateboard)
        {
            skateboardShopItem.GetComponent<MeshFilter>().sharedMesh = skateboard.objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            skateboardShopItem.GetComponent<MeshRenderer>().material = skateboard.material;
        }
    }

    public ItemObjectSO[] ReturnEquippedObjects()
    {
        ItemObjectSO[] returnItems = new ItemObjectSO[3];

        //Body, Hat, Board
        returnItems[0] = currentEquippedCharacter;
        returnItems[1] = currentEquippedHat;
        returnItems[2] = currentEquippedSkateboard;

        return returnItems;
    }

    public ItemObjectSO FindHat(int ID)
    {
        ItemObjectSO item = null;

        for (int i = 0; i < hatSOs.Length; ++i)
        { 
            if (((ItemObjectSO)hatSOs[i]).Id == ID)
            {
                item = (ItemObjectSO)hatSOs[i];
                return item;
            }
        }

        return item;
    }

    public ItemObjectSO FindChar(int ID)
    {
        ItemObjectSO item = null;

        for (int i = 0; i < characterSOs.Length; ++i)
        {
            if (((ItemObjectSO)characterSOs[i]).Id == ID)
            {
                item = (ItemObjectSO)characterSOs[i];
                return item;
            }
        }

        return item;
    }

    public ItemObjectSO FindSkateboard(int ID)
    {
        ItemObjectSO item = null;

        for (int i = 0; i < skateboardSOs.Length; ++i)
        {
            if (((ItemObjectSO)skateboardSOs[i]).Id == ID)
            {
                item = (ItemObjectSO)skateboardSOs[i];
                return item;
            }
        }

        return item;
    }
}
