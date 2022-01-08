////////////////////////////////////////////////////////////
// File: OutfitChanger.cs
// Author: Jack Peedle
// Date Created: 24/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief: A script to control the outfit system
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{

    #region Variables

    // reference to the shop script
    public Shop shopScript;

    // Hat selecter game object
    public GameObject hatSelector;



    [Header("Characters In Scene")]
    // reference to the character in the scene
    public GameObject characterObjectInScene;

    // reference to the character in the scene
    public GameObject playableCharacterObjectInScene;

    //
    public GameObject Ghost2CharacterInScene;



    [Header("Hats In Scene")]
    // gameobject hat which will be displayed
    public GameObject hatDisplayGameObject;

    // gameobject hat which will be displayed
    public GameObject playableHatDisplayGameObject;

    //
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


    // current int for the hats
    public int currentGOint = 0;

    // current int for the hat material 
    public int currentGOMaterialint = 0;

    // current character int
    public int currentCharacterint = 0;




    #endregion

    #region Methods

    // on start
    public void Start() {

        // set the current hat object to 4
        currentGOint = 4;

        // set the current hat material to 4
        currentGOMaterialint = 4;

        // set the current character to 2
        currentCharacterint = 2;

        // load the current character material that has been saved
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

    }

    // Loaded customizables 1
    public void LoadedCustomizables1() {

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer1();


        // load the current character material that has been saved
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // load the current character material that has been saved
        playableCharacterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // set the players hat to the current hat
        playableHatDisplayGameObject.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        playableHatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;


        // load the current character material that has been saved
        Ghost2CharacterInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // set the players hat to the current hat
        Ghost2HatInScene.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        Ghost2HatInScene.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;
        

    }

    // Loaded customizables 1
    public void LoadedCustomizables2() {

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer2();


        // load the current character material that has been saved
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // load the current character material that has been saved
        playableCharacterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];


        // set the players hat to the current hat
        playableHatDisplayGameObject.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        playableHatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;

        // load the current character material that has been saved
        Ghost2CharacterInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // set the players hat to the current hat
        Ghost2HatInScene.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        Ghost2HatInScene.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;

    }

    // Loaded customizables 1
    public void LoadedCustomizables3() {

        // set the player data to the loaded player in the save system
        b_PlayerData data = b_SaveSystem.LoadPlayer3();

        // load the current character material that has been saved
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // load the current character material that has been saved
        playableCharacterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];


        // set the players hat to the current hat
        playableHatDisplayGameObject.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        playableHatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;


        // load the current character material that has been saved
        Ghost2CharacterInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // set the players hat to the current hat
        Ghost2HatInScene.GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the players material to the current hat material
        Ghost2HatInScene.GetComponent<MeshFilter>().sharedMesh = hatSelector.GetComponent<MeshFilter>().sharedMesh;

    }

    // next hat option
    public void NextGameObjectOption() {

        // increment the hat and hat material int by 1
        currentGOint++;
        currentGOMaterialint++;

        // if the hat int is more than or = to the hat list.count
        if (currentGOint >= gameObjectOptions.Count) {

            // set the int to 0
            currentGOint = 0;

        }

        // if the hat material int is more than or = to the hat material list.count
        if (currentGOMaterialint >= gameObjectMaterialOptions.Count) {

            // set the int to 0
            currentGOMaterialint = 0;

        }

        // set the hatdisplay object to the int for the current hat
        hatDisplayGameObject = gameObjectOptions[currentGOint];

        // get the mesh renderer material from this game object and make it the current material int
        GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the hat selector mesh filter to the display game objects mesh filter
        hatSelector.GetComponent<MeshFilter>().sharedMesh = hatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh;

        // move to the next hat price
        shopScript.NextHatPrice();

    }

    // previous hat option
    public void PreviousGameObjectOption() {

        // increment the hat and hat material int by - 1
        currentGOint--;
        currentGOMaterialint--;

        // if the hat int is less than or = to 0
        if (currentGOint <= -1) {

            // set the hat int to the list of hats.count - 1
            currentGOint = gameObjectOptions.Count - 1;

        }

        // if the Material hat int is less than or = to 0
        if (currentGOMaterialint <= -1) {

            // set the material hat int to the list of materialsHats.count - 1
            currentGOMaterialint = gameObjectMaterialOptions.Count - 1;

        }

        // set the hatdisplay object to the int for the current hat
        hatDisplayGameObject = gameObjectOptions[currentGOint];

        // get the mesh renderer material from this game object and make it the current material int
        GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        // set the hat selector mesh filter to the display game objects mesh filter
        hatSelector.GetComponent<MeshFilter>().sharedMesh = hatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh;

        // move to the previous hat price
        shopScript.PreviousHatPrice();

    }




    // next character material option
    public void NextCharacterOption() {

        // increment the character int by 1
        currentCharacterint++;

        // if the character int is more than or = to the list of material options
        if (currentCharacterint >= gameObjectCharacterMaterialOptions.Count) {

            // set the character int to 0
            currentCharacterint = 0;

        }

        // get the character object in the scene and the mesh renderer material, set the material list with the current int
        // to the characters material
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // move to the next character price
        shopScript.NextCharacterPrice();

    }

    // Previous character material option
    public void PreviousCharacterOption() {

        // increment the character material int - 1
        currentCharacterint--;

        // if the character material int is less than or = 1
        if (currentCharacterint <= -1) {

            // set the character material int to the material options - 1
            currentCharacterint = gameObjectCharacterMaterialOptions.Count - 1;

        }

        // get the character object in the scene and the mesh renderer material, set the material list with the current int
        // to the characters material
        characterObjectInScene.GetComponent<SkinnedMeshRenderer>().material = gameObjectCharacterMaterialOptions[currentCharacterint];

        // move to the previous character price
        shopScript.PreviousCharacterPrice();

    }

    #endregion

}
