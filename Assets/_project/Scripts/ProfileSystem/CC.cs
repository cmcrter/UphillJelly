////////////////////////////////////////////////////////////
// File: CC.cs
// Author: Jack Peedle
// Date Created: 15/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 15/11/21
// Brief: A script to control the Camera Changer
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC : MonoBehaviour
{
    //
    public Camera CustomizationCam;

    //
    public Camera GameCam;

    //
    public GameObject ProfileCanvas;

    // Start is called before the first frame update
    void Start() {

        //
        CustomizationCam.enabled = true;

        GameCam.enabled = false;


    }

    public void ChangeToGameCam() {

        //
        //Destroy(CustomizationCam);

        CustomizationCam.enabled = false;

        //
        ProfileCanvas.SetActive(false);

        GameCam.enabled = true;

        Debug.Log("WorkyWorky");
    }
}
