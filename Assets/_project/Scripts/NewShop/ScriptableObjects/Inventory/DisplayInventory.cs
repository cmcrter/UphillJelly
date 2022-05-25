////////////////////////////////////////////////////////////
// File: DisplayInventory.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject PlayerHatInventory;

    public GameObject inventoryPrefab;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    public GameObject HatInventoryLayoutGroup;

    

    void Start() {



        CreateDisplay();

    }

    void Update() {

        UpdateDisplay();

    }


    public void CreateDisplay() {

        for (int i = 0; i < PlayerHatInventory.Container.Items.Count; i++) {

            InventorySlot slot = PlayerHatInventory.Container.Items[i];

            var obj = Instantiate(inventoryPrefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                PlayerHatInventory.database.GetItem[slot.item.Id].uiDisplay;

            obj.transform.SetParent(HatInventoryLayoutGroup.transform);

            itemsDisplayed.Add(slot, obj);

        }



    }


    public void UpdateDisplay() {

        for (int i = 0; i < PlayerHatInventory.Container.Items.Count; i++) {

            InventorySlot slot = PlayerHatInventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot)) {

                //itemsDisplayed[PlayerHatInventory.container[i]] == PlayerHatInventory.container[i].amountOfItem.ToString(1);

                slot = PlayerHatInventory.Container.Items[i];

                //if (PlayerHatInventory.container[i].item.prefab.)

            } else {

                var obj = Instantiate(inventoryPrefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                    PlayerHatInventory.database.GetItem[slot.item.Id].uiDisplay;

                obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                itemsDisplayed.Add(slot, obj);

            }

        }

    }

    



    /*
    public void CreateDisplay() {

        for (int i = 0; i < PlayerHatInventory.Container.Items.Count; i++) {

            if (inventoryPrefab.activeInHierarchy) {

                Destroy(inventoryPrefab);

                break; 

            }else {

                for (int j = 0; j < PlayerHatInventory.Container.Items.Count; j++) {

                    var obj = Instantiate(PlayerHatInventory.Container.Items[j].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                    obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                    itemsDisplayed.Add(PlayerHatInventory.Container.Items[j], obj);

                }

            }

            

        }

        

    }

    
    
    public void UpdateDisplay() {

        for (int i = 0; i < PlayerHatInventory.Container.Items.Count; i++) {

            if (itemsDisplayed.ContainsKey(PlayerHatInventory.Container.Items[i])) {

                //itemsDisplayed[PlayerHatInventory.container[i]] == PlayerHatInventory.container[i].amountOfItem.ToString(1);

                PlayerHatInventory.Container.Items[i].amountOfItem = 1;

            } else {

                var obj = Instantiate(PlayerHatInventory.Container.Items[i].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                itemsDisplayed.Add(PlayerHatInventory.Container.Items[i], obj);

            }

        }

    }
    */

}
