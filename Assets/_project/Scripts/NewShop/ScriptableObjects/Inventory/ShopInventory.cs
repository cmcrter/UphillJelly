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

public class ShopInventory : MonoBehaviour
{
    public int layoutPriority;
    public InventoryObject shopHatInventory;
    public InventoryObject playerHatInventory;

    //
    public List<ScriptableObject> purchasedItems;
    public List<ScriptableObject> equippedItems;
    //


    //public List<Button> shopButtons;
    public GameObject currentlySelectedButton;

    public TextMeshProUGUI objectDescription;
    public Image descriptionItemImage;


    public void Start() {

        //shopHatInventory.Load();
        playerHatInventory.Load();   

    }

    public void Update() {

        currentlySelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>().gameObject;
        objectDescription.text = currentlySelectedButton.GetComponent<HatItem>().item.itemDescription;
        descriptionItemImage.sprite = currentlySelectedButton.GetComponent<HatItem>().item.uiDisplay;

        //if (EventSystem.current.currentSelectedGameObject == shopButtons[1]) {
        //    Debug.Log(this.shopButtons.name + " was selected");
        //}

    }

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

    private void OnApplicationQuit() {

        playerHatInventory.Container.Items.Clear();

    }

}
