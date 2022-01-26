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
    public GameObject Character;

    // reference to the outfit changer
    public OutfitChanger outfitChanger;

    //
    public void ChangeCatMesh() {

        // load the current character material that has been saved
        Character.GetComponent<MeshRenderer>().material = outfitChanger.gameObjectCharacterMaterialOptions
            [outfitChanger.currentCharacterint];

    }

}
