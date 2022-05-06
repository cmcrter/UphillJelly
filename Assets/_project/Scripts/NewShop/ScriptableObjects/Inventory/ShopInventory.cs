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

public class ShopInventory : MonoBehaviour
{

    public InventoryObject shopHatInventory;
    public InventoryObject playerHatInventory;


    public void BuyItem() {

        var item = EventSystem.current.currentSelectedGameObject.GetComponent<Item>();

        if (item) {

            //shopHatInventory.AddItem(item.item, 1);
            playerHatInventory.AddItem(item.item, 1);



            //shopHatInventory.RemoveItem(item.item, -1);




            //item.gameObject.SetActive(false);
            Destroy(item.gameObject);

        }

    }

    private void OnApplicationQuit() {

        playerHatInventory.container.Clear();

    }

}
