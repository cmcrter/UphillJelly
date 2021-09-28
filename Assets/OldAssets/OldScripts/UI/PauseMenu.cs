using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//script attached to the pause menu, stops the game and lets the player adjust options
public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    FMOD.Studio.EventInstance testSound;

    FMOD.Studio.Bus Music; //In Fmod all the sound files are put into different catagories (buses). By doing this it means all the sounds of a specific catagory can be affected while ignoring others.

    FMOD.Studio.Bus Sounds;

    //float values for the music and sound

    float MusicVolume = 0.5f;

    float SoundsVolume = 0.5f;

    public GameObject pauseMenuUI;

    public Toggle controllerActive;

    public HoverboardController player;

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;

        //used for letting the player manually adjust the music volume. As the music is normally playing on loop, there was no need to include a sample sound for reference
    }

    public void SoundsVolumeLevel(float newSoundsVolume)
    {
        SoundsVolume = newSoundsVolume;

        FMOD.Studio.PLAYBACK_STATE pbState;

        testSound.getPlaybackState(out pbState);

        if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            testSound.start();

           //Plays a sample sound when the player adjusts the sound volume to let them know what the exact volume is
        }
    }

    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");   //References to all the sounds affected by the music slider

        Sounds = FMODUnity.RuntimeManager.GetBus("bus:/Sounds"); //References to all the sounds affected by the sound slider

        testSound = FMODUnity.RuntimeManager.CreateInstance("event:/SoundTest"); //Used for the sample sound when adjusting the sound volume
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }

        }

        Music.setVolume(MusicVolume);

        Sounds.setVolume(SoundsVolume);
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void SetControllerStatus()
    {
        if (controllerActive.isOn)
        {
            PlayerPrefs.SetString("ControllerPref", "P1");
            player.playerPrefix = "P1";
            
        }
        else
        {
            PlayerPrefs.SetString("ControllerPref", "K");
            player.playerPrefix = "K";
        }
    }

    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
}
