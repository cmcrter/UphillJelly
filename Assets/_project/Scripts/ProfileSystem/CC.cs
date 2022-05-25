////////////////////////////////////////////////////////////
// File: CC.cs
// Author: Jack Peedle
// Date Created: 15/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief: A script to control the Camera Changer
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC : MonoBehaviour
{
    // customization camera
    public Camera CustomizationCam;

    // game camera
    public Camera GameCam;

    // profile canvas
    public GameObject ProfileCanvas;

    // Start is called before the first frame update
    void Start() {

        // on start set the current camera to the customization camera
        CustomizationCam.enabled = true;

        // set the game camera to false
        GameCam.enabled = false;


    }

    public void ChangeToGameCam() {

        // set the customization camera to false
        CustomizationCam.enabled = false;

        // set the profile canvas to false
        ProfileCanvas.SetActive(false);

        // enable the game camera
        GameCam.enabled = true;

        //Debug.Log("WorkyWorky");
    }
}
