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

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject PlayerHatInventory;

    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    public GameObject HatInventoryLayoutGroup;

    void Start() {



        CreateDisplay();

    }

    void Update() {

        UpdateDisplay();

    }


    public void CreateDisplay() {

        for (int i = 0; i < PlayerHatInventory.container.Count; i++) {

            var obj = Instantiate(PlayerHatInventory.container[i].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

            obj.transform.SetParent(HatInventoryLayoutGroup.transform);

            itemsDisplayed.Add(PlayerHatInventory.container[i], obj);

        }



    }


    public void UpdateDisplay() {

        for (int i = 0; i < PlayerHatInventory.container.Count; i++) {

            if (itemsDisplayed.ContainsKey(PlayerHatInventory.container[i])) {

                //itemsDisplayed[PlayerHatInventory.container[i]] == PlayerHatInventory.container[i].amountOfItem.ToString(1);

                PlayerHatInventory.container[i].amountOfItem = 1;

                //if (PlayerHatInventory.container[i].item.prefab.)

            } else {

                var obj = Instantiate(PlayerHatInventory.container[i].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                itemsDisplayed.Add(PlayerHatInventory.container[i], obj);

            }

        }

    }





    /*
    public void CreateDisplay() {

        for (int i = 0; i < PlayerHatInventory.container.Count; i++) {

            if (PlayerHatInventory.container[i].item.prefab.activeInHierarchy) {

                Destroy(PlayerHatInventory.container[i].item.prefab.gameObject);

                break; 

            }else {

                for (int j = 0; j < PlayerHatInventory.container.Count; j++) {

                    var obj = Instantiate(PlayerHatInventory.container[j].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                    obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                    itemsDisplayed.Add(PlayerHatInventory.container[j], obj);

                }

            }

            

        }

        

    }

    */
    /*
    public void UpdateDisplay() {

        for (int i = 0; i < PlayerHatInventory.container.Count; i++) {

            if (itemsDisplayed.ContainsKey(PlayerHatInventory.container[i])) {

                //itemsDisplayed[PlayerHatInventory.container[i]] == PlayerHatInventory.container[i].amountOfItem.ToString(1);

                PlayerHatInventory.container[i].amountOfItem = 1;

            } else {

                var obj = Instantiate(PlayerHatInventory.container[i].item.prefab, HatInventoryLayoutGroup.transform); //, At the position on the layout group

                obj.transform.SetParent(HatInventoryLayoutGroup.transform);

                itemsDisplayed.Add(PlayerHatInventory.container[i], obj);

            }

        }

    }
    */

}
