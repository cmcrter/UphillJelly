////////////////////////////////////////////////////////////
// File: InventoryObject.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    //
    public List<InventorySlot> container = new List<InventorySlot>();

    public void AddItem(ItemObjectSO _item, int _amountOfItem) {

        bool hasItem = false;

        for (int i = 0; i < container.Count; i++) {

            if (container[i].item == _item) {

                container[i].AddAmountOfItems(_amountOfItem);
                hasItem = true;
                break;

            }

        }

        if (!hasItem) {
            container.Add(new InventorySlot(_item, _amountOfItem));
        }

    }

    /*
    public void RemoveItem(ItemObjectSO _item, int _amountOfItem) {

        bool hasItem = true;

        for (int i = 0; i > container.Count; i--) {

            if (container[i].item == _item) {

                container[i].RemoveAmountOfItems(_amountOfItem);
                hasItem = false;
                break;

            }

        }

        if (hasItem) {
            container.Remove(new InventorySlot(_item, _amountOfItem));
        }

    }
    */

}

[System.Serializable]
public class InventorySlot
{

    public ItemObjectSO item;
    public int amountOfItem;

    //
    public InventorySlot(ItemObjectSO _item, int _amountOfItem) {

        item = _item;
        amountOfItem = _amountOfItem;

    }

    public void AddAmountOfItems(int value) {

        amountOfItem += value;

    }

    /*
    public void RemoveAmountOfItems(int value) {

        amountOfItem -= value;

    }
    */

}