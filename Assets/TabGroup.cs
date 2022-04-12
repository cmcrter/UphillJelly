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

namespace SleepyCat
{
    public class TabGroup : MonoBehaviour
    {
        public List<ShopTabButton> tabButtons;
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;

        public void Subscribe(ShopTabButton button) {

            if (tabButtons == null) {

                tabButtons = new List<ShopTabButton>();

            }

            tabButtons.Add(button);

        }

        public void OnTabEnter(ShopTabButton button) {

            ResetTabs();

            button.background.sprite = tabHover;

        }

        public void OnTabExit(ShopTabButton button) {

            ResetTabs();

        }

        public void OnTabSelected(ShopTabButton button) {

            ResetTabs();

            button.background.sprite = tabActive;

        }

        public void ResetTabs() {

            foreach(ShopTabButton button in tabButtons) {

                button.background.sprite = tabIdle;

            }

        }

    }
}
