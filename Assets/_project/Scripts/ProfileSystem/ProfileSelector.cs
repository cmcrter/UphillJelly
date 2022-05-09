////////////////////////////////////////////////////////////
// File: ProfileSelector.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 20/11/21
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: A script to control the text for all of the profiles
//////////////////////////////////////////////////////////// 

using UnityEngine;
using TMPro;
using System.IO;
using L7Games.Loading;

public class ProfileSelector : MonoBehaviour
{
    static ProfileSelector instance;

    // Reference to the scripts
    public StoredPlayerProfile b_player;

    // text input field
    public TMP_InputField tmp_Input;

    // gameobject panel for the starting background
    public GameObject StartingBackground;

    [Header("Profile Selection UI Buttons")]
    public TMP_Text LoadProfile1Text;
    public TMP_Text LoadProfile2Text;
    public TMP_Text LoadProfile3Text;

    private void Awake()
    {
        //Not a singleton
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // if Profile 2 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile1")) 
        {
            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile1");
        }

        // if Profile 2 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile2"))
        {
            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile2");
        }

        // if Profile 2 directory exists
        if (Directory.Exists(Application.persistentDataPath + "/CurrentProfile3"))
        {
            // Delete the file
            Directory.Delete(Application.persistentDataPath + "/CurrentProfile3");
        }

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path1 = Application.persistentDataPath + "/Profile1/ProfileData/Profile1Data.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path2 = Application.persistentDataPath + "/Profile2/ProfileData/Profile2Data.sdat";

        // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
        string path3 = Application.persistentDataPath + "/Profile3/ProfileData/Profile3Data.sdat";

        // if a file exists in the "path"
        if (File.Exists(path1))
        {
            // profile 1 text = Load Profile 1
            LoadProfile1Text.text = "Load Profile 1";
        }
        else
        {
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
        if (File.Exists(path3)) 
        {
            // profile 3 text = Load Profile 3
            LoadProfile3Text.text = "Load Profile 3";
        } 
        else
        {
            // load profile 3 text = new game
            LoadProfile3Text.text = "New Game";
        }
    }

    //Profile Loading Button Pressed
    public void ButtonPressed(int profileSlot)
    {
        LoadingData.playerSlot = profileSlot;

        // if no directory exists
        if (!Directory.Exists(Application.persistentDataPath + "/CurrentProfile" + profileSlot.ToString())) 
        {
            // create a directory for "/CurrentProfile"
            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile" + profileSlot.ToString());
        }

        StoredPlayerProfile player = b_SaveSystem.LoadPlayer(profileSlot);
        LoadingData.player = player;

        b_SaveSystem.SavePlayer(profileSlot);

        // set the starting background to false
        StartingBackground.SetActive(false);
    }
}