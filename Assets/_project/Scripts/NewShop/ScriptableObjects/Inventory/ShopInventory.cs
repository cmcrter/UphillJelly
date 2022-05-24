////////////////////////////////////////////////////////////
// File: ShopInventory.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using L7Games.Input;

public class ShopInventory : MonoBehaviour
{
    public int layoutPriority;
    //public InventoryObject shopHatInventory;
    //public InventoryObject playerHatInventory;

    //
    public List<ScriptableObject> purchasedItems;
    public List<ScriptableObject> equippedItems;

    public Transform physicalCharacter;

    public Button[] itemButtons;
    //

    [SerializeField]
    [Tooltip("The Event system in the scene")]
    private UnityEngine.EventSystems.EventSystem eventSystem;

    [SerializeField]
    private InputHandler inputHandlerInScene;


    //public List<Button> shopButtons;
    //public GameObject currentlySelectedButton;
    public ShopManager shopManager;

    ///public TextMeshProUGUI[] objectDescription;
    ///public Image[] descriptionItemImage;

    private int rotationSpeed = 25;


    public int currentTabSelected;
    public GameObject[] mainTabs;

    public Sprite boughtSprite;

    public void OnEnable() {


        inputHandlerInScene.TabLeftPerformed += MainMenuController_TabLeftAction_Performed;

        inputHandlerInScene.TabRightPerformed += MainMenuController_TabRightAction_Performed;

        inputHandlerInScene.MenuUpPerformed += MainMenuController_Up_Performed;
        inputHandlerInScene.MenuDownPerformed += MainMenuController_Down_Performed;
        inputHandlerInScene.MenuLeftPerformed += MainMenuController_Left_Performed;
        inputHandlerInScene.MenuRightPerformed += MainMenuController_Right_Performed;
        inputHandlerInScene.MenuConfirmedPerformed += MainMenuController_Confirm_Performed;
        inputHandlerInScene.MenuCancelPerformed += MainMenuController_Cancel_Performed;

        //eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

        inputHandlerInScene.RotateLeftPerformed += MainMenuController_RotateLeftAction_Performed;

        inputHandlerInScene.RotateRightPerformed += MainMenuController_RotateRightAction_Performed;

    }


    public void OnDisable() {
        

    }



    public void Start() {

        currentTabSelected = 0;
        //shopHatInventory.Load();
        ///playerHatInventory.Load();
        //inputHandlersInScene[i].TabLeftPerformed += MainMenuController_TabLeftAction_Performed;
    }

    public void Update() {

        /*
        //currentlySelectedButton = itemButtons[0].gameObject;
        ///currentlySelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>().gameObject;
        objectDescription[0].text = shopManager.selectedButton.GetComponent<HatItem>().item.itemDescription;
        descriptionItemImage[0].sprite = shopManager.selectedButton.GetComponent<HatItem>().item.uiDisplay;

        ///REMOVE AND REPLACE FOR MORE EFFICIENT
        ///TEST********
        objectDescription[1].text = shopManager.selectedButton.GetComponent<CharacterItem>().item.itemDescription;
        descriptionItemImage[1].sprite = shopManager.selectedButton.GetComponent<CharacterItem>().item.uiDisplay;

        objectDescription[2].text = shopManager.selectedButton.GetComponent<SkateboardItem>().item.itemDescription;
        descriptionItemImage[2].sprite = shopManager.selectedButton.GetComponent<SkateboardItem>().item.uiDisplay;

        //if (EventSystem.current.currentSelectedGameObject == shopButtons[1]) {
        //    Debug.Log(this.shopButtons.name + " was selected");
        //}

        */

        ///objectDescription[2].text = currentlySelectedButton.GetComponent<SkateboardItem>().item.itemDescription;
        ///descriptionItemImage[2].sprite = currentlySelectedButton.GetComponent<SkateboardItem>().item.uiDisplay;


    }

    private void MainMenuController_TabLeftAction_Performed() {

        ///mainTabs[0].SetActive(false);
        ///mainTabs[1].SetActive(false);
        ///mainTabs[2].SetActive(true);

        currentTabSelected -= 1;

        if (currentTabSelected < 0) {
            currentTabSelected = 2;
        }
        //currentTabSelected--;
        UpdateTabUI();
        
    }

    private void MainMenuController_TabRightAction_Performed() {
        ///mainTabs[0].SetActive(false);
        ///mainTabs[1].SetActive(true);
        ///mainTabs[2].SetActive(false);
        
        currentTabSelected += 1;

        if (currentTabSelected > 2) {
            currentTabSelected = 0;
        }

        //currentTabSelected++;
        UpdateTabUI();
        
    }

    private void MainMenuController_RotateLeftAction_Performed() {

        Debug.Log("RotateLeft");
        physicalCharacter.transform.Rotate(new Vector3(0f, -50f, 0f) * rotationSpeed);

    }

    private void MainMenuController_RotateRightAction_Performed() {

        Debug.Log("RotateRight");
        physicalCharacter.transform.Rotate(new Vector3(0f, 50f, 0f) * rotationSpeed);

    }

    public void UpdateTabUI() {


        for (int i = 0; i < mainTabs.Length; i++) {
            if (currentTabSelected == i) {
                mainTabs[i].gameObject.SetActive(true);
            } else {
                mainTabs[i].gameObject.SetActive(false);
            }
        }

        ///Tabs[3].gameObject.SetActive(false);

        ///for (int i = 0; i < Tabs.Length; i++) {
        ///    Tabs[currentTabSelected].gameObject.SetActive(true);
        ///}

    }

    private void MainMenuController_Up_Performed() {

    }

    private void MainMenuController_Down_Performed() {

    }

    private void MainMenuController_Left_Performed() {

    }

    private void MainMenuController_Right_Performed() {

    }

    private void MainMenuController_Confirm_Performed() {

    }

    private void MainMenuController_Cancel_Performed() {

    }

    public void hasBoughtItem() {

        //var item = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var hatItem = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();
        var characterItem = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterItem>();
        var skateboardItem = EventSystem.current.currentSelectedGameObject.GetComponent<SkateboardItem>();

        if (hatItem) {

            hatItem.item.isPurchased = true;
            hatItem.item.uiDisplay = boughtSprite;

        }

        if (characterItem) {

            characterItem.item.isPurchased = true;
            characterItem.item.uiDisplay = boughtSprite;

        }

        if (skateboardItem) {

            skateboardItem.item.isPurchased = true;
            skateboardItem.item.uiDisplay = boughtSprite;

        }

    }
    /*
    public void BuyItem() {

        var item = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>();

        if (item) {

            //shopHatInventory.AddItem(item.item, 1);
            playerHatInventory.AddItem(new Item(item.item)); //1);

            purchasedItems.Add(item.item);

            //shopHatInventory.RemoveItem(item.item, -1);

            //item.gameObject.SetActive(false);
            Destroy(item.gameObject);

        }

        //
        //shopHatInventory.Save();
        playerHatInventory.Save();


    }
    */
    private void OnApplicationQuit() {

        ///playerHatInventory.Container.Items.Clear();

    }

}
