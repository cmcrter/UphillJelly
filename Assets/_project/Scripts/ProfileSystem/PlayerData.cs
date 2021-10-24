////////////////////////////////////////////////////////////
// File: PlayerData.cs
// Author: Jack Peedle
// Date Created: 22/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 22/10/21
// Brief: A script to control the profile system
//////////////////////////////////////////////////////////// 



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

[Serializable]
public class PlayerData
{

    //
    public void SaveProfile() {

        //
        ProfileSaveSystem.SaveProfile(this);

    }

    //
    public void LoadProfile() {

        //
        PlayerData data = ProfileSaveSystem.LoadProfile();

    }

    //
    public OutfitChanger outfitChanger;


    //
    //public Material saveHatColour;
    //saveHatColour material

    //
    public Mesh saveHatObject;
    //saveHatObject mesh

    // Reference to the TextMeshPro input field
    //public TMP_InputField TMPProfileTextInput;

    // public int currency for the shop
    //public int Currency;

    //
    //public Material CurrentActivePlayerMaterial;

    //
    //public int currentActiveHat;

    // Possibly change to "float[] position"
    //
    //public GameObject hatSpawnPosition;

    

    // Contructor to tell the PlayerData where to get the data from
    public PlayerData (PlayerData profile) {

        //
        //saveHatColour = outfitChanger.hatColour;

        //
        //saveHatObject = outfitChanger.hatObject;

        // Take data from profile controller and assign it in this one
        //
        //TMPProfileTextInput = profileController.TMPProfileTextInput;

        //
        //Currency = outfitChanger.Currency;

        //
        //CurrentActivePlayerMaterial = profileController.CurrentActivePlayerMaterial;

        //
        //currentActiveHat = outfitChanger.currentActiveHat;

        //
        //hatSpawnPosition = profileController.hatSpawnPosition;

    }


}


