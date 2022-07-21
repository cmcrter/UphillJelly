////////////////////////////////////////////////////////////
// File: MainMenuLevelDisplay.cs
// Author: Charles Carter
// Date Created: 20/06/22
// Last Edited By: Charles Carter
// Date Last Edited: 20/06/22
// Brief: A script to connect the current levels in the game to the 
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using L7Games;
using L7Games.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace L7Games
{
    public class MainMenuLevelDisplay : MonoBehaviour
    {
        #region Public Fields

        [SerializeField]
        private MainMenuLevelSelect levelSelectManager;

        [Header("Variables to show level details")]
        [SerializeField]
        private Image imageSlot;
        [SerializeField]
        private TextMeshProUGUI LevelNameText;

        //The difficulties are set from just having stars be shown
        [SerializeField]
        private List<Image> StarImages = new List<Image>();

        [SerializeField]
        private LEVEL thisLevel;

        [Header("Functionality for selecting leve")]
        [SerializeField]
        private Toggle thisToggle;
        #endregion

        #region Public Properties
        public Toggle ThisToggle
        {
            get
            {
                return thisToggle;
            }
        }
        #endregion

        #region Unity Methods

        private void Awake()
        {
            thisToggle = thisToggle ?? GetComponent<Toggle>();
        }

        #endregion

        #region Public Methods

        public void SetLevel(LevelData data)
        {
            if (data == null) return;

            if (imageSlot)
            {
                imageSlot.sprite = data.sceneSprite;
                imageSlot.preserveAspect = true;
            }

            if (LevelNameText)
            {
                LevelNameText.text = data.displayName;
            }

            thisLevel = data.levelType;

            if (data.levelType == LEVEL.TUTORIAL && thisToggle != null && LoadingData.currentLevel == LEVEL.MAINMENU)
            {
                thisToggle.isOn = true;
            }

            if ((int)data.difficulty > StarImages.Count)
            {
                return;
            }

            for (int i = (int)data.difficulty; i < StarImages.Count; ++i)
            {
                StarImages[i].enabled = false;
            }
        }

        public void SelectedLevel(bool isOn)
        {
            if (isOn)
            {
                if (levelSelectManager)
                {
                    levelSelectManager.currentLevelSelected = thisLevel;
                }
                else
                {
                    MainMenuLevelSelect.GoToSelectedMap(true, thisLevel);
                }
            }
        }

        #endregion
    }
}