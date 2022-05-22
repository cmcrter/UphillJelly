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

public class ShopManager : MonoBehaviour
{

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
    public List<Button> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public Button selectedButton;
    public List<GameObject> objectsToSwap;

    public ShopInventory shopInventory;

    public int currentTabInt;
    //


    public void Start() {

        //hatSOs = (HatObject)ScriptableObject.FindObjectsOfType(ScriptableObject);

    }

    public void hasBoughtItem() {

        //var item = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        var skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();

        if (hatItem) {

            hatItem.item.isPurchased = true;
            //hatItem.item.uiDisplay = boughtSprite;

            currentEquippedHat = hatItem.item;

        }

        if (characterItem) {

            characterItem.item.isPurchased = true;
            //characterItem.item.uiDisplay = boughtSprite;

            currentEquippedCharacter = characterItem.item;

        }

        if (skateboardItem) {

            skateboardItem.item.isPurchased = true;
            //skateboardItem.item.uiDisplay = boughtSprite;

            currentEquippedSkateboard = skateboardItem.item;

        }

    }






    



    public void Subscribe(Button button) {

        if (tabButtons == null) {

            tabButtons = new List<Button>();

        }

        tabButtons.Add(button);


    }

    public void OnTabHover(Button button) {

        ResetTabs();

        shopInventory.currentlySelectedButton = button.gameObject;

        Debug.Log("Hovered over button");

        // Only change tab sprite if it is not already selected
        if (selectedButton == null || button != selectedButton) {

            //button.backgroundTabImage.sprite = tabHover;

        }

    }

    public void OnTabExit(Button button) {

        ResetTabs();

    }

    public void OnTabSelected(Button button) {

        selectedButton = button;

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

        foreach (Button button in tabButtons) {

            if (selectedButton != null && button == selectedButton) { continue; }

            //button.backgroundTabImage.sprite = tabIdle;

        }

    }





}
