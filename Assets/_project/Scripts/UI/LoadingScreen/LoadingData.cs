////////////////////////////////////////////////////////////
// File: LoadingData.cs
// Author: Charles Carter
// Date Created: 04/02/2022
// Last Edited By: Charles Carter
// Date Last Edited: 13/04/2022
// Brief: A small script to take over set data from the main menu to load in the loading screen
//////////////////////////////////////////////////////////// 

using System;

namespace L7.Loading
{
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
    }
}
