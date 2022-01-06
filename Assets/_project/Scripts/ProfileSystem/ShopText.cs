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

    //
    public Shop shop;

    // Reference to the b_Player
    public b_Player b_player; 

    // text input field
    public TMP_InputField tmp_Input;

    // gameobject panel for the starting background
    public GameObject StartingBackground;

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

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays")) {

            //
            b_player.ghostSO2.ResetGhostData();

        }

        //
        b_player.isSave1 = true;

        // set the second play button to false
        PlayButton2.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);

        // load player 1 data
        b_player.LoadPlayer1();

        //
        b_player.SavePlayer1Second();

    }

    // second button pressed
    public void ButtonPressed2() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

            //
            b_player.ghostSO2.ResetGhostData();

        }

        //
        b_player.isSave2 = true;

        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);

        // set the starting background to false
        StartingBackground.SetActive(false);

        // load player 1 data
        b_player.LoadPlayer2();

        //
        b_player.SavePlayer2Second();

    }

    // third button pressed
    public void ButtonPressed3() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

            //
            b_player.ghostSO2.ResetGhostData();

        }

        //
        b_player.isSave3 = true;

        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the second play button to false
        PlayButton2.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);


        // load player 1 data
        b_player.LoadPlayer3();

        //
        b_player.SavePlayer3Second();

    }


    // Start is called before the first frame update
    void Start()
    {

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path1 = Application.persistentDataPath + "/Profile1/ProfileData/Profile1Data.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path2 = Application.persistentDataPath + "/Profile2/ProfileData/Profile2Data.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path3 = Application.persistentDataPath + "/Profile3/ProfileData/Profile3Data.sdat";

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

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsHatBought[shop.CurrentHatSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // Load all of the data from the first player
        b_player.LoadPlayer1();

        // play 1
        b_player.PlayPlayer1();

    }

    // press play button 2
    public void PressPlayButton2() {

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsHatBought[shop.CurrentHatSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // Load all of the data from the second player
        b_player.LoadPlayer2();

        // play 2
        b_player.PlayPlayer2();

    }

    // press play button 3
    public void PressPlayButton3() {

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsHatBought[shop.CurrentHatSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            //
            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // Load all of the data from the third player
        b_player.LoadPlayer3();

        // play 3
        b_player.PlayPlayer3();

    }


}
