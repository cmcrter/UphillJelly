using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//script on the main menu, that lets the player change options, as well as setup the customisation

public class MainMenu : MonoBehaviour
{
    FMOD.Studio.EventInstance testSound;

    FMOD.Studio.Bus Music;

    FMOD.Studio.Bus Sounds;

    

    float MusicVolume = 0.5f;

    float SoundsVolume = 0.5f;

    public GameObject MenuCamera;
    public GameObject CustomizeCamera;

    public TMP_InputField nameInput;
    public Toggle controllerActive;

    public Animator menuCatAnim;
   //each customisation option (skins, hats, boards) is a list of items attached to both the character in the menu and the game
    public GameObject catMesh;
    public Material[] skins;
    private int currentSkin = 0;

    public GameObject[] hats;
    private int currentHat = 0;

    public GameObject[] boards;
    private int currentBoard = 0;

    public TMP_Dropdown resDropdown;

    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();
        //get all resolutions available and add them to the drop down list of options
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();

        QualitySettings.SetQualityLevel(2);
        //set all options to 0 to reset the player on start
        PlayerPrefs.SetString("ControllerPref", "K");
        PlayerPrefs.SetInt("CurrentHat", 0);
        PlayerPrefs.SetInt("CurrentBoard", 0);
        PlayerPrefs.SetInt("CurrentSkin", 0);
        PlayerPrefs.SetString("PlayerName", "Mysterious Skater");

        menuCatAnim.SetBool("StandingIdle", true);
    }
    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");

        Sounds = FMODUnity.RuntimeManager.GetBus("bus:/Sounds");

        testSound = FMODUnity.RuntimeManager.CreateInstance("event:/SoundTest");
    }

    void Update()
    {
        Music.setVolume(MusicVolume);

        Sounds.setVolume(SoundsVolume);
    }


    public void MusicVolumeLevel (float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
    }
    //each item has a changeX function, it just iterates along the list of items available as they are selected, and sends the index to player preferences
    // once the game start, the player preferences index is used to show the specific item.
    public void ChangeHat(bool forward)
    {
        if (forward)
        {
            if (currentHat < hats.Length - 1)
            {
                hats[currentHat].SetActive(false);
                currentHat++;
                hats[currentHat].SetActive(true);

            }
            else
            {
                hats[currentHat].SetActive(false);
                currentHat = 0;
                hats[currentHat].SetActive(true);

            }
        }
        else if(!forward)
        {
            if (currentHat > 0)
            {
                hats[currentHat].SetActive(false);
                currentHat--;
                hats[currentHat].SetActive(true);

            }
            else
            {
                hats[currentHat].SetActive(false);
                currentHat = hats.Length -1;
                hats[currentHat].SetActive(true);

            }
        }

        PlayerPrefs.SetInt("CurrentHat", currentHat);
    }

    public void ChangeBoard(bool forward)
    {
        if (forward)
        {
            if (currentBoard < boards.Length - 1)
            {
                boards[currentBoard].SetActive(false);
                currentBoard++;
                boards[currentBoard].SetActive(true);

            }
            else
            {
                boards[currentBoard].SetActive(false);
                currentBoard = 0;
                boards[currentBoard].SetActive(true);

            }
        }
        else if (!forward)
        {
            if (currentBoard > 0)
            {
                boards[currentBoard].SetActive(false);
                currentBoard--;
                boards[currentBoard].SetActive(true);

            }
            else
            {
                boards[currentBoard].SetActive(false);
                currentBoard = boards.Length - 1;
                boards[currentBoard].SetActive(true);

            }
        }

        PlayerPrefs.SetInt("CurrentBoard", currentBoard);
    }

    public void ChangeSkin(bool forward)
    {
        if (forward)
        {
            if (currentSkin < skins.Length - 1)
            {
                currentSkin++;
                catMesh.GetComponent<SkinnedMeshRenderer>().material = skins[currentSkin];
                Debug.Log("Skin = " + currentSkin);
            }
            else
            {
                currentSkin = 0;
                catMesh.GetComponent<SkinnedMeshRenderer>().material = skins[currentSkin];
                Debug.Log("Skin = " + currentSkin);
            }
        }
        else if (!forward)
        {
            if (currentSkin > 0)
            {
                currentSkin--;
                catMesh.GetComponent<SkinnedMeshRenderer>().material = skins[currentSkin];
                Debug.Log("Skin = " + currentSkin);
            }
            else
            {
                currentSkin = skins.Length - 1;
                catMesh.GetComponent<SkinnedMeshRenderer>().material = skins[currentSkin];
                Debug.Log("Skin = " + currentSkin);
            }
        }

        PlayerPrefs.SetInt("CurrentSkin", currentSkin);
    }
    public void SoundsVolumeLevel(float newSoundsVolume)
    {
        SoundsVolume = newSoundsVolume;

        FMOD.Studio.PLAYBACK_STATE pbState;

        testSound.getPlaybackState(out pbState);

        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            testSound.start();
        }
    }
    //basic button fucntions for accessing menus
    public void StartGame()
    {
        SetControllerStatus();
        SceneManager.LoadScene(2);
    }

    public void GoToCustomize()
    {
        MenuCamera.SetActive(false);
        CustomizeCamera.SetActive(true);
    }

    public void GoToMenu()
    {
        MenuCamera.SetActive(true);
        CustomizeCamera.SetActive(false);
    }

    public void SetName()
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);    
    }

    public void SetControllerStatus()
    {
        if (controllerActive.isOn)
        {
            PlayerPrefs.SetString("ControllerPref", "P1");
        }
        else
        {
            PlayerPrefs.SetString("ControllerPref", "K");
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void Quit()
    {
        Application.Quit();
    }

   

}
