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

    public InventoryObject shopInventory;

    public void BuyItem() {

        var item = EventSystem.current.currentSelectedGameObject.GetComponent<Item>();

        if (item) {

            shopInventory.AddItem(item.item, 1);

            item.gameObject.SetActive(false);

        }

    }


}
