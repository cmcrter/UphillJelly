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
using UnityEngine;

public class LoadCustomizablesInGame : MonoBehaviour
{

    // reference to the character
    public SkinnedMeshRenderer CharacterMesh;

    // reference to the outfit changer
    public OutfitChanger outfitChanger;

    //
    public b_Player b_player;

    //
    public void Start() {

        /*
        // load the current character material that has been saved
        Character.GetComponent<MeshRenderer>().material = outfitChanger.gameObjectCharacterMaterialOptions
            [outfitChanger.currentCharacterint];
        */

        outfitChanger.LoadedCustomizables1();

        b_player.LoadPlayer1();

        CharacterMesh.GetComponent<SkinnedMeshRenderer>().material = 
            outfitChanger.gameObjectCharacterMaterialOptions[outfitChanger.currentCharacterint];

    }

    /*
    //
    public void ChangeCatMesh() {

        // load the current character material that has been saved
        Character.GetComponent<MeshRenderer>().material = outfitChanger.gameObjectCharacterMaterialOptions
            [outfitChanger.currentCharacterint];

    }
    */

}
