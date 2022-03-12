////////////////////////////////////////////////////////////
// File: ShopText.cs
// Author: Jack Peedle
// Date Created: 20/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 12/03/22
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
    public OutfitChanger outfitChanger;

    // Shop Reference
    public Shop shop;

    // Reference to the b_Player
    public b_Player b_player; 

    // text input field
    public TMP_InputField tmp_Input;

    // gameobject panel for the starting background
    public GameObject StartingBackground;

    [Header("Starting Menu Buttons")]

    // first button text
    public TMP_Text LoadProfile1Text;

    // second button text
    public TMP_Text LoadProfile2Text;

    // third button text
    public TMP_Text LoadProfile3Text;

    [Header("Play Buttons")]

    // button for starting the game1
    public GameObject PlayButton1;

    // button for starting the game2
    public GameObject PlayButton2;

    // button for starting the game3
    public GameObject PlayButton3;

    //
    public TempScoreSystem tempScoreSystem;

    //
    public LoadCustomizablesInGame loadCustomizablesInGame;

    // first button pressed
    public void ButtonPressed1() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile1/Replays")) {

            // if the replay folder does not exist, clear the ghost 2 data
            b_player.ghostSO2.ResetGhostData();

        }

        #region CheckCurrent(isSave1)

        // if no directory exists
        if (!Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) {

            // create a directory for "/CurrentProfile"
            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile1");

        }

        // if Profile 2 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile2")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile2");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        // if Profile 3 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile3")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile3");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        #endregion

        //
        //File.Create(Application.persistentDataPath + "/CurrentProfile/CurrentProfile1");



        // set the player 1 to true
        b_player.isSave1 = true;

        // set the second play button to false
        PlayButton2.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);

        // load player 1 data
        b_player.LoadPlayer1();

        // save the second replay for the first save
        b_player.SavePlayer1Second();

        //
        loadCustomizablesInGame.LoadInts();

    }

    // second button pressed
    public void ButtonPressed2() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile2/Replays")) {

            // if the replay folder does not exist, clear the ghost 2 data
            b_player.ghostSO2.ResetGhostData();

        }

        #region CheckCurrent(isSave2)

        // if no directory exists
        if (!Directory.Exists(Application.persistentDataPath + "/CurrentProfile2")) {

            // create a directory for "/CurrentProfile"
            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile2");

        }

        // if Profile 1 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile1");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        // if Profile 3 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile3")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile3");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        #endregion


        


        // set the player 2 to true
        b_player.isSave2 = true;

        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the third play button to false
        PlayButton3.SetActive(false);

        // set the starting background to false
        StartingBackground.SetActive(false);

        // load player 1 data
        b_player.LoadPlayer2();

        // save the second replay for the second save
        b_player.SavePlayer2Second();

        //
        loadCustomizablesInGame.LoadInts();

    }

    // third button pressed
    public void ButtonPressed3() {

        // if a directory doesn't exists for "/replay_save/replay_data"
        if (!Directory.Exists(Application.persistentDataPath + "/Profile3/Replays")) {

            // if the replay folder does not exist, clear the ghost 2 data
            b_player.ghostSO2.ResetGhostData();

        }

        #region CheckCurrent(isSave3)

        // if no directory exists
        if (!Directory.Exists(Application.persistentDataPath + "/CurrentProfile3")) {

            // create a directory for "/CurrentProfile"
            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile3");

        }

        // if Profile 1 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile1");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        // if Profile 3 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile2")) {

            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile2");

            //
            UnityEditor.AssetDatabase.Refresh();

        }

        #endregion

        //
        //File.Create(Application.persistentDataPath + "/CurrentProfile/CurrentProfile3.sdat");



        // set the player 3 to true
        b_player.isSave3 = true;

        // set the first play button to false
        PlayButton1.SetActive(false);

        // set the second play button to false
        PlayButton2.SetActive(false);


        // set the starting background to false
        StartingBackground.SetActive(false);


        // load player 1 data
        b_player.LoadPlayer3();

        // save the second replay for the third save
        b_player.SavePlayer3Second();

        //
        loadCustomizablesInGame.LoadInts();

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

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        //
        loadCustomizablesInGame.LoadInts();

        //
        b_player.SavePlayer1();

        // Load all of the data from the first player
        b_player.LoadPlayer1();

        //loadCustomizablesInGame.CHI = shop.CurrentHatSelectedInt;

        

        

        // play 1
        b_player.PlayPlayer1();

        //
        tempScoreSystem.TimerActive = true;

        // Save the static ints in Shop
        //outfitChanger.SaveStaticInts();

        Debug.Log("88888888888");

    }

    // press play button 2
    public void PressPlayButton2() {

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsHatBought[shop.CurrentHatSelectedInt]) {

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        //
        loadCustomizablesInGame.LoadInts();

        //
        b_player.SavePlayer2();

        // Load all of the data from the second player
        b_player.LoadPlayer2();

        

        // play 2
        b_player.PlayPlayer2();

        //
        tempScoreSystem.TimerActive = true;

        // Save the static ints in Shop
        //outfitChanger.SaveStaticInts();

    }

    // press play button 3
    public void PressPlayButton3() {

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsHatBought[shop.CurrentHatSelectedInt]) {

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        // If the current hat is not one that is bought then don't start the game
        if (!shop.IsCharacterBought[shop.CurrentCharacterSelectedInt]) {

            Debug.Log("DOES NOT OWN ALL CURRENTLY ACTIVE ITEMS");

            return;

        }

        //
        loadCustomizablesInGame.LoadInts();

        //
        b_player.SavePlayer3();

        // Load all of the data from the third player
        b_player.LoadPlayer3();


        // play 3
        b_player.PlayPlayer3();

        //
        tempScoreSystem.TimerActive = true;

        // Save the static ints in Shop
        //outfitChanger.SaveStaticInts();

    }


}
