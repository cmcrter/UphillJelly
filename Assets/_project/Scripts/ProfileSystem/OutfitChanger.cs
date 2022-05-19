////////////////////////////////////////////////////////////
// File: OutfitChanger.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: A script to control the outfit system
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using L7Games.Loading;
using UnityEngine;

[Obsolete]
public class OutfitChanger : MonoBehaviour
{
    #region Variables

    // reference to the scripts
    public Shop shopScript;
    public GameObject hatSelector;
    public ReplaySaveManager replaySaveManager;
    public b_Player b_player;

    [Header("Characters In Scene")]
    // reference to the character in the scene
    public GameObject characterObjectInScene;

    // reference to the character in the scene
    public GameObject playableCharacterObjectInScene;

    // reference to the ghost 2 character in the scene
    public GameObject Ghost2CharacterInScene;

    [Header("Hats In Scene")]
    // gameobject hat which will be displayed
    public GameObject hatDisplayGameObject;

    // gameobject hat which will be displayed
    public GameObject playableHatDisplayGameObject;

    // gameobject ghost 2 hat which will be displayed
    public GameObject Ghost2HatInScene;

    [Header("Gameobject Options")]
    // list of the hat gameobjects
    public List<GameObject> gameObjectOptions = new List<GameObject>();

    [Header("Material Options")]
    // list of the materials for the hats
    public List<Material> gameObjectMaterialOptions = new List<Material>();

    [Header("Character Options")]
    // list of materials for the player character
    public List<Material> gameObjectCharacterMaterialOptions = new List<Material>();

    #endregion

    #region Methods

    // on start
    public void Start()
    {    
        //  if the map is tutorial and or city
        if (LoadingData.currentLevel == LEVEL.CITY || LoadingData.currentLevel == LEVEL.TUTORIAL) 
        {
            int playerSlot = LoadingData.playerSlot;

            // if no directory exists
            if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile" + playerSlot.ToString()))
            {
                // load player 1
                b_player.LoadPlayer(playerSlot);

                // load customizables 1
                LoadCustomizables();
            }

            b_player.LoadPlayer(playerSlot);
        }
    }

    public void LoadCustomizables()
    {

    }

    #endregion

}