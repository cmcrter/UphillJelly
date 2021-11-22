////////////////////////////////////////////////////////////
// File: ShopText.cs
// Author: Jack Peedle
// Date Created: 20/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 22/11/21
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

    // Reference to the b_Player
    public b_Player b_Player; 

    // text input field
    public TMP_InputField tmp_Input;

    // gameobject panel for the starting background
    public GameObject StartingBackground;

    [Header("Save And Load Buttons")]

    // button for save player 1
    public GameObject SavePlayer1;

    // button for load player 1
    public GameObject LoadPlayer1;

    // button for save player 2
    public GameObject SavePlayer2;

    // button for load player 2
    public GameObject LoadPlayer2;

    // button for save player 3
    public GameObject SavePlayer3;

    // button for load player 3
    public GameObject LoadPlayer3;


    [Header("Starting Menu Buttons")]

    // first button text
    public Text LoadProfile1Text;

    // second button text
    public Text LoadProfile2Text;

    // third button text
    public Text LoadProfile3Text;

    [Header("Play Buttons")]

    // button for starting the game1
    public GameObject PlayButton1;

    // button for starting the game2
    public GameObject PlayButton2;

    // button for starting the game3
    public GameObject PlayButton3;


    // first button pressed
    public void ButtonPressed1() {

        
        // set the save player 2 button to false
        SavePlayer2.SetActive(false);

        // set the load player 2 button to false
        LoadPlayer2.SetActive(false);

        // set the save player 3 button to false
        SavePlayer3.SetActive(false);

        // set the load player 3 button to false
        LoadPlayer3.SetActive(false);


        // set the second play button to false
        PlayButton2.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);


        // load player 1 data
        b_Player.LoadPlayer1();

        // set the load profile text to load (Players name)
        LoadProfile1Text.text = "Load " + tmp_Input.text;

    }

    // second button pressed
    public void ButtonPressed2() {

        // set the save player 1 button to false
        SavePlayer1.SetActive(false);

        // set the load player 1 button to false
        LoadPlayer1.SetActive(false);

        // set the save player 3 button to false
        SavePlayer3.SetActive(false);

        // set the load player 3 button to false
        LoadPlayer3.SetActive(false);


        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);


        // load player 1 data
        b_Player.LoadPlayer2();

        // set the load profile text to load (Players name)
        LoadProfile2Text.text = "Load " + tmp_Input.text;

    }

    // third button pressed
    public void ButtonPressed3() {

        // set the save player 2 button to false
        SavePlayer2.SetActive(false);

        // set the load player 2 button to false
        LoadPlayer2.SetActive(false);

        // set the save player 1 button to false
        SavePlayer1.SetActive(false);

        // set the load player 1 button to false
        LoadPlayer1.SetActive(false);


        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the second play button to false
        PlayButton2.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);


        // load player 1 data
        b_Player.LoadPlayer3();

        // set the load profile text to load (Players name)
        LoadProfile3Text.text = "Load " + tmp_Input.text;

    }


    // Start is called before the first frame update
    void Start()
    {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path1 = Application.persistentDataPath + "/Profile1.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path2 = Application.persistentDataPath + "/Profile2.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path3 = Application.persistentDataPath + "/Profile3.sdat";

        // if a file exists in the "path"
        if (File.Exists(path1)) {

            // profile 1 text = Load Profile 1
            LoadProfile1Text.text = "Load Profile 1";

        } else {

            // load profile 1 text = new game
            LoadProfile1Text.text = "New Game";

        }

        // if a file exists in the "path"
        if (File.Exists(path2)) {

            // profile 2 text = Load Profile 2
            LoadProfile2Text.text = "Load Profile 2";

        } else {

            // load profile 2 text = new game
            LoadProfile2Text.text = "New Game";

        }

        // if a file exists in the "path"
        if (File.Exists(path3)) {

            // profile 3 text = Load Profile 3
            LoadProfile3Text.text = "Load Profile 3";

        } else {

            // load profile 3 text = new game
            LoadProfile3Text.text = "New Game";

        }


    }

    // press play button 1
    public void PressPlayButton1() {

        // Load all of the data from the first player
        b_Player.LoadPlayer1();

        // play 1
        b_Player.PlayPlayer1();

    }

    // press play button 2
    public void PressPlayButton2() {

        // Load all of the data from the second player
        b_Player.LoadPlayer2();

        // play 2
        b_Player.PlayPlayer2();

    }

    // press play button 3
    public void PressPlayButton3() {

        // Load all of the data from the third player
        b_Player.LoadPlayer3();

        // play 3
        b_Player.PlayPlayer3();

    }


}
