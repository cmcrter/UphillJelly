////////////////////////////////////////////////////////////
// File: LoadingData.cs
// Author: Charles Carter
// Date Created: 04/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 16/06/2022
// Brief: A small script to take over set data from the main menu to load in the loading screen and vice versa
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;

namespace L7Games.Loading
{
    public enum LEVEL
    {
        NONE,
        MAINMENU,
        TUTORIAL,
        CITY,
        OLDTOWN,
        NEWLEVEL1,
        NEWLEVEL2,
        NEWLEVEL3,
        NEWLEVEL4,
        NEWLEVEL5,
        COUNT
    }

    [Serializable]
    public class LoadingData
    {
        /// <summary>
        /// The static variable to carry which scene to load in the loading screen
        /// </summary>
        public static string sceneToLoad;

        /// <summary>
        /// The static variable for the loading screen to know if something needs to be pressed to go to next scene
        /// </summary>
        public static bool waitForNextScene;

        /// <summary>
        /// The static variable to know the current level from anywhere
        /// </summary>
        public static LEVEL currentLevel;

        /// <summary>
        /// Knowing which profile is being used and what it contains
        /// </summary>
        public static int playerSlot;
        public static StoredPlayerProfile player;

        /// <summary>
        /// Whether in the loading screen the players' profile will be saved
        /// </summary>
        public static bool SavePlayer;

        /// <summary>
        ///A variable for the player's equipped shop items
        /// </summary>
        public static ItemObjectSO[] shopItems;

        /// <summary>
        ///A variable for the next replay to load
        /// </summary>
        public static Ghost replayToLoad;

        /// <summary>
        /// Utility to get a string from each level
        /// </summary>
        /// <param name="thisLevel"></param>
        /// <returns></returns>
        public static string getLevelString(LEVEL thisLevel)
        {
            string levelname = "MainMenu";

            if(LevelManager.ConfirmedLevels != null)
            {
                for(int i = 0; i < LevelManager.ConfirmedLevels.Length; ++i)
                {
                    if(LevelManager.ConfirmedLevels[i].levelType == thisLevel)
                    {
                        levelname = LevelManager.ConfirmedLevels[i].levelName;
                    }
                }
            }
            else if (Debug.isDebugBuild)
            {
                Debug.LogError("No Level Manager Has Ran");
            }

            return levelname;
        }

        /// <summary>
        /// Utility to get scene name when loading
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string getSceneString(LEVEL level)
        {
            string levelName = "MainMenu";

            if(LevelManager.ConfirmedLevels != null)
            {
                for(int i = 0; i < LevelManager.ConfirmedLevels.Length; ++i)
                {
                    if(LevelManager.ConfirmedLevels[i].levelType == level)
                    {
                        levelName = LevelManager.ConfirmedLevels[i].sceneName;
                    }
                }
            }
            else if(Debug.isDebugBuild)
            {
                Debug.LogError("No Level Manager Has Ran");
            }

            return levelName;
        }
    }
}
