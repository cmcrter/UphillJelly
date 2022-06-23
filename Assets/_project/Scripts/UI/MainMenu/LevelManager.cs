////////////////////////////////////////////////////////////
// File: LevelManager.cs
// Author: Charles Carter
// Date Created: 16/06/22
// Last Edited By: Charles Carter
// Date Last Edited: 16/06/22
// Brief: A script to allow level designers to easily add/edit level data
//////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using L7Games.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace L7Games
{
    public enum LevelDifficulty
    {
        NONE,
        VERY_EASY,
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD,
        COUNT,
    }

    [Serializable]
    public class LevelData
    {
        public LEVEL levelType;
        public string sceneName;
    
        /// <summary>
        /// This is used for leaderboards so dont change
        /// </summary>
        public string levelName;
        public string displayName;
        public Sprite sceneSprite;
        public LevelDifficulty difficulty;
    }

    public class LevelManager : MonoBehaviour
    {
        #region Variables

        [Header("All Levels Within Game")]
        [SerializeField]
        private List<LevelData> Levels = new List<LevelData>();

        /// <summary>
        /// Use variable in Start or after, as it's set in Awake
        /// </summary>
        public static LevelData[] ConfirmedLevels;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if(ConfirmedLevels != null)
            {
                return;
            }

            //Use Confirmed Levels in Start in other scripts
            ConfirmedLevels = Levels.ToArray();

            //Add one level for the loading screen scene
            if(SceneManager.sceneCountInBuildSettings != ConfirmedLevels.Length + 1)
            {
                Debug.Log("Not all levels are in build settings or build setting contain too many builds", this);
            }
        }

        #endregion
    }
}
