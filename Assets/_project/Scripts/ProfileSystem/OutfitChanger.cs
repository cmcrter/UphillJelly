////////////////////////////////////////////////////////////
// File: OutfitChanger.cs
// Author: Jack Peedle
// Date Created: 23/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 23/10/21
// Brief: A script to control the outfit system
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{

    public GameObject hatSelector;

    public Material hatSelectorMaterial;



    [Header("Hat displayer")]
    //
    public GameObject hatDisplayGameObject;

    [Header("Gameobject Options")]
    //
    public List<GameObject> gameObjectOptions = new List<GameObject>();

    [Header("Material Options")]
    //
    public List<Material> gameObjectMaterialOptions = new List<Material>();

    //
    public int currentGOint = 0;

    //
    public int currentGOMaterialint = 0;

    //
    //public MeshRenderer thisMeshRenderer;

    //
    //public MeshFilter thisMeshFilter;

    public void LoadedHats() {

        //
        //
        hatDisplayGameObject = gameObjectOptions[currentGOint];


        //
        GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];

        //
        hatSelector.GetComponent<MeshFilter>().sharedMesh = hatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh;

    }

    //
    public void NextGameObjectOption() {

        //
        currentGOint++;

        currentGOMaterialint++;

        if (currentGOint >= gameObjectOptions.Count) {

            //
            currentGOint = 0;

        }

        if (currentGOMaterialint >= gameObjectMaterialOptions.Count) {

            //
            currentGOMaterialint = 0;

        }

        //
        hatDisplayGameObject = gameObjectOptions[currentGOint];


        //
        GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];




        //////////
        //hatSelectorMaterial = gameObjectMaterialOptions[currentGOMaterialint];

        //
        hatSelector.GetComponent<MeshFilter>().sharedMesh = hatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh;

        //
        //thisMeshFilter = hatDisplayGameObject.GetComponent<MeshFilter>();

        //hatSelectorMaterial = hatDisplayGameObject.GetComponent<MeshRenderer>();

    }


    //
    public void PreviousGameObjectOption() {

        //
        currentGOint--;

        //
        currentGOMaterialint--;

        if (currentGOint <= 0) {

            //
            currentGOint = gameObjectOptions.Count - 1;

        }

        if (currentGOMaterialint <= 0) {

            //
            currentGOMaterialint = gameObjectMaterialOptions.Count - 1;

        }


        //
        hatDisplayGameObject = gameObjectOptions[currentGOint];



        //
        GetComponent<MeshRenderer>().material = gameObjectMaterialOptions[currentGOMaterialint];



        //////////
        //hatSelectorMaterial = gameObjectMaterialOptions[currentGOMaterialint];

        //
        //thisMeshRenderer = hatDisplayGameObject.GetComponent<MeshRenderer>();

        //
        //thisMeshFilter = hatDisplayGameObject.GetComponent<MeshFilter>();

        //
        hatSelector.GetComponent<MeshFilter>().sharedMesh = hatDisplayGameObject.GetComponent<MeshFilter>().sharedMesh;

        //hatSelectorMaterial = hatDisplayGameObject.GetComponent<MeshRenderer>();

    }






















    /*

    //
    public GameObject[] hatGameObjectOptions;

    //
    public int currentHatNumber;

    //
    //public List<GameObject> hatGameObjectOptions = new List<GameObject>();

    //
    private int currentHatOption = 0;




    //
    void start() {


    }

    //
    public void Update() {

        
        //
        for (int i = 0; i < hatGameObjectOptions.Length; i++) {

            //
            currentHatNumber = i;

        }
        

        currentHatOption = currentHatNumber;

    }

    public void NextHatOption() {

        //
        currentHatOption++;

        //
        if (currentHatOption >= hatGameObjectOptions.Length) {

            //
            currentHatOption = 0;

        }

        //
        hatDisplayGameObject = hatGameObjectOptions[currentHatOption];

    }



    //
    public void PreviousHatOption() {

        //
        currentHatOption--;


        //
        if (currentHatOption <= 0) {

            //
            currentHatOption = hatGameObjectOptions.Length - 1;

        }

        // Public mesh    List Mesh        Int
        hatDisplayGameObject = hatGameObjectOptions[currentHatOption];

    }

    */
}
