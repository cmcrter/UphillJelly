////////////////////////////////////////////////////////////
// File: MainMenuLevelSelect.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/06/22
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

        public List<MainMenuLevelDisplay> levelDisplayers;
        public GameObject playButton;
        public LEVEL currentLevelSelected;

        private int levelCount;

        #endregion

        #region Unity Methods

        private void Start() 
        {
            currentLevelSelected = LEVEL.TUTORIAL;
            levelCount = (int)LEVEL.OLDTOWN - 1;

            if(LevelManager.ConfirmedLevels != null)
            {
                //Main Menu is in 0th slot
                for(int i = 0; i < levelDisplayers.Count; ++i)
                {
                    if(i < LevelManager.ConfirmedLevels.Length - 1)
                    {
                        levelDisplayers[i].SetLevel(LevelManager.ConfirmedLevels[i + 1]);
                    }
                    else
                    {
                        levelDisplayers[i].gameObject.SetActive(false);
                    }
                }

                //Removing the main menu
                levelCount = LevelManager.ConfirmedLevels.Length;
            }
        }

        #endregion

        #region Public Methods

        public void ButtonPressed()
        {
            GoToSelectedMap(true, currentLevelSelected);
        }

        //public void IncrementMap()
        //{
        //    currentLevelSelected++;

        //    if(currentLevelSelected > (LEVEL)levelCount)
        //    {
        //        currentLevelSelected = LEVEL.TUTORIAL;
        //    }

        //    UpdateImage();
        //}

        //public void DeIncrementMap()
        //{
        //    currentLevelSelected--;

        //    if(currentLevelSelected < LEVEL.TUTORIAL)
        //    {
        //        currentLevelSelected = LEVEL.OLDTOWN;
        //    }

        //    UpdateImage();
        //}

        public static void GoToSelectedMap(bool save, LEVEL level)
        {
            LoadingData.sceneToLoad = LoadingData.getSceneString(level);
            LoadingData.currentLevel = level;
            LoadingData.waitForNextScene = true;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        #endregion
    }
}
