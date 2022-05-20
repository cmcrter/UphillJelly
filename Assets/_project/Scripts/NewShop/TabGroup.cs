////////////////////////////////////////////////////////////
// File: TabGroup.cs
// Author: Jack Peedle
// Date Created: 10/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 10/04/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SleepyCat
{
    public class TabGroup : MonoBehaviour
    {
        public List<ShopTabButton> tabButtons;
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;
        public ShopTabButton selectedTab;
        public List<GameObject> objectsToSwap;

        public void Subscribe(ShopTabButton button) {

            if (tabButtons == null) {

                tabButtons = new List<ShopTabButton>();

            }

            tabButtons.Add(button);

        }

        public void OnTabHover(ShopTabButton button) {

            ResetTabs();

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

    }
}
