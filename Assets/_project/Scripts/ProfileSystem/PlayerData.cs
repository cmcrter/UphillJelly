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

[System.Serializable]
public class PlayerData
{

    // Reference to the TextMeshPro input field
    //public TMP_InputField TMPProfileTextInput;

    // public int currency for the shop
    public int Currency;

    //
    //public Material CurrentActivePlayerMaterial;

    //
    public int currentActiveHat;

    // Possibly change to "float[] position"
    //
    //public GameObject hatSpawnPosition;



    // Contructor to tell the PlayerData where to get the data from
    public PlayerData (ProfileController profileController) {

        // Take data from profile controller and assign it in this one
        //
        //TMPProfileTextInput = profileController.TMPProfileTextInput;

        //
        Currency = profileController.Currency;

        //
        //CurrentActivePlayerMaterial = profileController.CurrentActivePlayerMaterial;

        //
        currentActiveHat = profileController.currentActiveHat;

        //
        //hatSpawnPosition = profileController.hatSpawnPosition;

    }


}
