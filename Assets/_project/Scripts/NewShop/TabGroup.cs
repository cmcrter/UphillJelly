////////////////////////////////////////////////////////////
// File: TabGroup.cs
// Author: Jack Peedle
// Date Created: 10/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 21/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SleepyCat
{
    public class TabGroup : MonoBehaviour
    {
        /*
        public List<ShopTabButton> tabButtons;
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;
        public ShopTabButton selectedTab;
        public List<GameObject> objectsToSwap;

        public ShopInventory shopInventory;

        public int currentTabInt;

        //public TextMeshProUGUI objectDescription;

        public enum ShopTabEnum
        {

            Hat,
            Character,
            Skateboard

        };

        public ShopTabEnum shopTabEnum;

        public void Subscribe(ShopTabButton button) {

            if (tabButtons == null) {

                tabButtons = new List<ShopTabButton>();

            }

            tabButtons.Add(button);


        }

        public void OnTabHover(ShopTabButton button) {

            ResetTabs();

            shopInventory.currentlySelectedButton = button.gameObject;

            Debug.Log("Hovered over button");

            // Only change tab sprite if it is not already selected
            if (selectedTab == null || button != selectedTab) {

                button.backgroundTabImage.sprite = tabHover;

            }

        }

        public void OnTabExit(ShopTabButton button) {

            ResetTabs();

        }

        public void OnTabSelected(ShopTabButton button) {

            selectedTab = button;

            ResetTabs();

            button.backgroundTabImage.sprite = tabActive;

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

            foreach(ShopTabButton button in tabButtons) {

                if (selectedTab!=null && button == selectedTab) { continue; }

                button.backgroundTabImage.sprite = tabIdle;

            }

        }
        */

    }

    /*
    [SerializeField]
    public class ShopItemGroup : MonoBehaviour
    {
        public List<ShopItemButton> shopButtons;
        public ShopItemButton selectedTab;
        public GameObject selectedButtonGO;
        public List<GameObject> objectsToSwap;

        public TextMeshProUGUI objectDescription;

        //public TextMeshProUGUI objectDescription;


        public void Subscribe(ShopItemButton button) {

            if (shopButtons == null) {

                shopButtons = new List<ShopItemButton>();

            }

            shopButtons.Add(button);

        }

        public void OnTabHover(ShopItemButton button) {

            ResetTabs();

            // Only change tab sprite if it is not already selected
            if (selectedTab == null || button != selectedTab) {

                selectedButtonGO = selectedTab.gameObject;
                selectedButtonGO = EventSystem.current.currentSelectedGameObject.GetComponent<HatItem>().gameObject;
                objectDescription.text = selectedTab.GetComponent<HatItem>().item.itemDescription;

            }

        }

        public void OnTabExit(ShopItemButton button) {

            ResetTabs();

        }

        public void OnTabSelected(ShopItemButton button) {

            selectedTab = button;

            ResetTabs();

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

            foreach (ShopItemButton button in shopButtons) {

                if (selectedTab != null && button == selectedTab) { continue; }


            }

        }

    }
    */


}
