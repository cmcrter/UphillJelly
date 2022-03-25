////////////////////////////////////////////////////////////
// File: LoadCustomizablesInGame.cs
// Author: Jack Peedle
// Date Created: 15/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 13/03/22
// Brief: A script to load the character materials in game
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCustomizablesInGame : MonoBehaviour
{

    // reference to the reference scripts
    public OutfitChanger outfitChanger;
    public Shop shop;
    public b_Player b_player;
    public ReplaySaveManager replaySaveManager;

    [Header("Fluctuating Ints")]

    // Current ghost and player character
    public int CurrentPlayerCharacter;
    public int CurrentGhostCharacter;

    // current ghost and player hat
    public int CurrentPlayerHat;
    public int CurrentGhostHat;

    public Material cMP;

    public GameObject cHat;

    public Material cHatM;

    public GameObject CatMeshRef;

    public GameObject HatRef;

    public GameObject GhostCatMeshRef;

    public GameObject GhostHatRef;


    public void Start() {

        // Load customizables
        LoadTheCustomizables();

        Debug.Log("STARTED");

        LoadInts();

        // if directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) {

            // is save 1 = true
            b_player.isSave1 = true;

            // load player 1
            b_player.LoadPlayer1();

            // load customizables 1
            //outfitChanger.LoadedCustomizables1();

            Debug.Log("SAVE 1");

        }

        // if no directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile2")) {

            // is save 2 = true
            b_player.isSave2 = true;

            // load player 2
            b_player.LoadPlayer2();

            // load customizables 2
            outfitChanger.LoadedCustomizables2();

        }

        // if no directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile3")) {

            // is save 3 = true
            b_player.isSave3 = true;

            // load player 2
            b_player.LoadPlayer3();

            // load customizables 3
            outfitChanger.LoadedCustomizables3();

        }

    }

    void Update() {

        
        CurrentPlayerCharacter = shop.CurrentCharacterSelectedInt;

        CurrentGhostCharacter = shop.CurrentCharacterSelectedInt;

        CurrentPlayerHat = shop.CurrentHatSelectedInt;

        CurrentGhostHat = shop.CurrentHatSelectedInt;



        // Current Material
        cMP = outfitChanger.gameObjectCharacterMaterialOptions[CurrentPlayerCharacter];

        cHat = outfitChanger.gameObjectOptions[CurrentPlayerHat];

        cHatM = outfitChanger.gameObjectMaterialOptions[CurrentPlayerHat];



        HatRef.GetComponent<MeshFilter>().sharedMesh = cHat.gameObject.GetComponent<MeshFilter>().sharedMesh;

        HatRef.GetComponent<MeshRenderer>().material = cHatM;

        CatMeshRef.GetComponent<SkinnedMeshRenderer>().material = cMP;

        //shop.CurrentHatSelectedInt = CurrentPlayerHat;

        //shop.CurrentCharacterSelectedInt = CurrentPlayerCharacter;


        GhostHatRef.GetComponent<MeshFilter>().sharedMesh = cHat.gameObject.GetComponent<MeshFilter>().sharedMesh;

        GhostHatRef.GetComponent<MeshRenderer>().material = cHatM;

        GhostCatMeshRef.GetComponent<SkinnedMeshRenderer>().material = cMP;






    }

    public void LoadTheCustomizables() {

        

    }

    // MIGHT BE USELESS
    public void LoadInts() {

        // set all character, ghost and hat ints to the currently selected ones from the main menu
        CurrentPlayerCharacter = shop.CurrentCharacterSelectedInt;
        CurrentGhostCharacter = shop.CurrentCharacterSelectedInt;
        CurrentPlayerHat = shop.CurrentHatSelectedInt;
        CurrentGhostHat = shop.CurrentHatSelectedInt;

    }

}