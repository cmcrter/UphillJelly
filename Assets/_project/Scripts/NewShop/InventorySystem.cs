////////////////////////////////////////////////////////////
// File: InventorySystem.cs
// Author: Jack Peedle
// Date Created: 24/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 24/04/22
// Brief: 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace L7Games {

    public class InventorySystem : MonoBehaviour
    {

        private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
        public List<InventoryItem> inventory { get; private set; }

        private void Awake() {

            inventory = new List<InventoryItem>();
            m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();

        }

        // If item exists then add to stack
        public void Add(InventoryItemData referenceData) {

            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {

                value.AddToStack();

            } else {

                // Create new instance and add to inventory
                InventoryItem newItem = new InventoryItem(referenceData);
                inventory.Add(newItem);
                m_itemDictionary.Add(referenceData, newItem);

            }


        }

        // Opposite of add function
        public void Remove(InventoryItemData referenceData) {

            if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)) {

                value.RemoveFromStack();

                // If value is 0, remove item from inventory
                if (value.stackSize == 0) {

                    inventory.Remove(value);
                    m_itemDictionary.Remove(referenceData);

                }


            }

        }

    }

    [Serializable]
    public class InventoryItem {

        // Reference to data and stack size
        public InventoryItemData data { get; private set; }
        public int stackSize { get; private set; }

        // pass and set inventory item data
        public InventoryItem(InventoryItemData source) {

            data = source;
            AddToStack();

        }

        // Add or remove number of items in the stack
        public void AddToStack() {

            stackSize++;

        }

        public void RemoveFromStack() {

            stackSize--;

        }

    }

    public class ItemObject : MonoBehaviour {

        public InventoryItemData referenceItem;

        public void OnBoughtItem() {

            // Singleton To add the current item
            //InventorySystem.current.Add(referenceItem);

        }


    }

}


