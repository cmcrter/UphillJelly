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



    [Header("Hat Colour To Change")]
    //
    public Material hatColour;

    [Header("Hat Object To Change")]
    //
    public Mesh hatObject;

    //
    public GameObject go_ToChange;

    //
    public List<Material> hatColourOptions = new List<Material>();

    //
    public List<Mesh> hatObjectOptions = new List<Mesh>();

    //
    private int currentHatOption = 0;

    //
    private int currentHatColourOption = 0;



    //
    void start() {

        //
        //GetComponent<MeshFilter>().mesh = hatObjectOptions;

    }

    //
    public void Update() {

        //
        go_ToChange.GetComponent<MeshRenderer>().material = hatColour;

        //
        go_ToChange.GetComponent<MeshFilter>().mesh = hatObject;

        //
        go_ToChange.transform.position = new Vector3(0, 0, 0);

        //
        //hatObject = go_ToChange.GetComponent<Mesh>().mesh;

    }

    //

    //
    public void NextHatOption() {

        //
        currentHatOption++;

        //
        currentHatColourOption++;
        //
        //
        //
        //
        if (currentHatColourOption >= hatColourOptions.Count) {

            //
            currentHatColourOption = 0;

        }

        //
        hatColour = hatColourOptions[currentHatColourOption];

        //
        //
        //
        //
        //
        //

        //
        if (currentHatOption >= hatObjectOptions.Count) {

            //
            currentHatOption = 0;

        }

        //
        hatObject = hatObjectOptions[currentHatOption];

    }



    //
    public void PreviousHatOption() {

        //
        currentHatColourOption--;

        //
        currentHatOption--;

        //
        if (currentHatColourOption <= 0) {

            //
            currentHatColourOption = hatColourOptions.Count - 1;

        }

        //
        hatColour = hatColourOptions[currentHatColourOption];

        //
        //
        //
        //
        //
        //

        //
        if (currentHatOption <= 0) {

            //
            currentHatOption = hatObjectOptions.Count - 1;

        }

        // Public mesh    List Mesh        Int
        hatObject = hatObjectOptions[currentHatOption];

    }


}
