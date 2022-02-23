////////////////////////////////////////////////////////////
// File: LoadCustomizablesInGame.cs
// Author: Jack Peedle
// Date Created: 15/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief: A script to load the character materials in game
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCustomizablesInGame : MonoBehaviour
{

    // reference to the character
    //public SkinnedMeshRenderer CharacterMesh;

    // reference to the outfit changer
    public OutfitChanger outfitChanger;

    //
    public Shop shop;

    //
    public b_Player b_player;

    //
    public ReplaySaveManager replaySaveManager;

    [Header("Fluctuating Ints")]
    

    //
    public int CurrentPlayerCharacter;

    //
    public int CurrentGhostCharacter;

    //
    public int CurrentPlayerHat;

    //
    public int CurrentGhostHat;


    //
    //public SkinnedMeshRenderer PlayableMesh;



    //
    public void Start() {

        //
        LoadTheCustomizables();
        /*
        //
        if (replaySaveManager.isMapTutorial) {

            //
            outfitChanger.currentCharacterint = shop.CurrentCharacterSelectedInt;

            // load the current character material that has been saved
            outfitChanger.characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material 
                = outfitChanger.gameObjectCharacterMaterialOptions[shop.CurrentCharacterSelectedInt];

        }

        //
        if (replaySaveManager.isMapCity) {

            //

        }

        //
        if (!replaySaveManager.isMapTutorial || !replaySaveManager.isMapCity) {

            //
            return;

        }
        */

    }

    
    //
    public void LoadTheCustomizables() {

        // if no directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) {

            //
            b_player.isSave1 = true;

            //
            b_player.LoadPlayer1();

            //
            outfitChanger.LoadedCustomizables1();

        }

        // if no directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile2")) {

            b_player.isSave2 = true;

            //
            outfitChanger.LoadedCustomizables2();

        }

        // if no directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile3")) {

            b_player.isSave3 = true;

            //
            outfitChanger.LoadedCustomizables3();

        }

    }

    //
    public void LoadInts() {

        //
        CurrentPlayerCharacter = shop.CurrentCharacterSelectedInt;

        //
        CurrentGhostCharacter = shop.CurrentCharacterSelectedInt;


        //
        CurrentPlayerHat = shop.CurrentHatSelectedInt;

        //
        CurrentGhostHat = shop.CurrentHatSelectedInt;

    }

    /*
    //
    public void ChangeCatMesh() {

        // load the current character material that has been saved
        CharacterMesh.GetComponent<SkinnedMeshRenderer>().material = outfitChanger.gameObjectCharacterMaterialOptions
            [outfitChanger.currentCharacterint];

    }
    */


}
