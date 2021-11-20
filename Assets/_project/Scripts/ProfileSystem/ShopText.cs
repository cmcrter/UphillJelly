////////////////////////////////////////////////////////////
// File: ShopText.cs
// Author: Jack Peedle
// Date Created: 20/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 20/11/21
// Brief: A script to control the text for all of the profiles
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ShopText : MonoBehaviour
{

    //
    public b_Player b_Player; 

    //
    public TMP_InputField tmp_Input;

    //
    public GameObject StartingBackground;

    [Header("Save And Load Buttons")]
    //
    public GameObject SavePlayer1;

    //
    public GameObject LoadPlayer1;

    //
    public GameObject SavePlayer2;

    //
    public GameObject LoadPlayer2;

    //
    public GameObject SavePlayer3;

    //
    public GameObject LoadPlayer3;


    [Header("Starting Menu Buttons")]

    //
    public Text LoadProfile1Text;

    [Header("Play Buttons")]

    //
    public GameObject PlayButton1;


    //
    public void ButtonPressed1() {

        //
        SavePlayer2.SetActive(false);

        //
        LoadPlayer2.SetActive(false);

        //
        SavePlayer3.SetActive(false);

        //
        LoadPlayer3.SetActive(false);

        //
        StartingBackground.SetActive(false);

        //
        b_Player.LoadPlayer1();

        //
        LoadProfile1Text.text = "Load " + tmp_Input.text;

    }

    //
    public void ButtonPressed2() {

        //


    }

    //
    public void ButtonPressed3() {

        //


    }


    // Start is called before the first frame update
    void Start()
    {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path1 = Application.persistentDataPath + "/Profile1.sdat";

        // if a file exists in the "path"
        if (File.Exists(path1)) {

            //
            LoadProfile1Text.text = "Load Profile 1";

        } else {

            //
            LoadProfile1Text.text = "New Game";

        }

            

    }

    public void PressPlayButton1() {

        //
        b_Player.PlayPlayer1();

    }


}
