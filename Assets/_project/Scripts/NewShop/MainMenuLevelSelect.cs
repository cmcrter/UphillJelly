////////////////////////////////////////////////////////////
// File: MainMenuLevelSelect.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 25/05/22
// Brief: A script to select which level is wanted within the main menu
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using L7Games.Loading;

namespace L7Games
{
    public class MainMenuLevelSelect : MonoBehaviour
    {
        #region Variables

        public List<Sprite> levelImages;
        public GameObject playButton;

        public Image displayMap;
        public LEVEL currentLevelSelected;

        #endregion

        #region Unity Methods

        private void Start() 
        {
            currentLevelSelected = LEVEL.TUTORIAL;
            UpdateImage();
        }

        #endregion

        #region Public Methods

        //Updating the shown sprite
        public void UpdateImage()
        {
            for(int i = 0; i < 3; ++i)
            {
                if(i == currentLevelSelected - LEVEL.TUTORIAL)
                {
                    displayMap.sprite = levelImages[i];
                }
            }
        }

        public void ButtonPressed()
        {
            GoToSelectedMap(true);
        }

        public void IncrementMap()
        {
            currentLevelSelected++;

            if(currentLevelSelected > LEVEL.OLDTOWN)
            {
                currentLevelSelected = LEVEL.TUTORIAL;
            }

            UpdateImage();
        }

        public void DeIncrementMap()
        {
            currentLevelSelected--;

            if(currentLevelSelected < LEVEL.TUTORIAL)
            {
                currentLevelSelected = LEVEL.OLDTOWN;
            }

            UpdateImage();
        }

        public void GoToSelectedMap(bool save)
        {
            LoadingData.sceneToLoad = LoadingData.getSceneString(currentLevelSelected);
            LoadingData.currentLevel = currentLevelSelected;
            LoadingData.waitForNextScene = true;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        #endregion
    }
}
