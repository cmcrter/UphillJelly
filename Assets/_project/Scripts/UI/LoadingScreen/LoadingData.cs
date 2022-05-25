////////////////////////////////////////////////////////////
// File: LoadingData.cs
// Author: Charles Carter
// Date Created: 04/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/2022
// Brief: A small script to take over set data from the main menu to load in the loading screen and vice versa
//////////////////////////////////////////////////////////// 

using System;

namespace L7Games.Loading
{
    public enum LEVEL
    {
        NONE,
        MAINMENU,
        TUTORIAL,
        CITY,
        OLDTOWN,
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
        public static InventoryItemData[] shopItems;

        /// <summary>
        ///A variable for the next replay to load
        /// </summary>
        public static Ghost replayToLoad;

        /// <summary>
        /// Utility to get a string from each level
        /// </summary>
        /// <param name="thisLevel"></param>
        /// <returns></returns>
        public static string getSceneName(LEVEL thisLevel)
        {
            string levelname = "Tutorial";

            switch(thisLevel)
            {
                case LEVEL.CITY:
                    levelname = "City";
                    break;
                case LEVEL.OLDTOWN:
                    levelname = "OldTown";
                    break;
            }

            return levelname;
        }
    }
}
