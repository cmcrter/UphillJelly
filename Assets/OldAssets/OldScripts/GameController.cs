using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //short script to trigger a countdown and the start, and spawn the players in based on their character choice.

    //since multiple character types got canceled, this does nothing.
    public GameObject[] characters; //0 = cat, 1 = bird, 2 = croc, 3 = turtle
    public Transform spawnPoint;


    [SerializeField]
    public float CountdownFrom;
    [SerializeField]
    public Text TimerText;

    private float Timer;

    public Image countDownImage;
    public Sprite[] countDownTextures;

    [HideInInspector]
    public string playerTime;

    public bool finished = false;

    public HoverboardController player;

    private bool countingDown = true;

    private FMOD.Studio.EventInstance countdownSound;

    void Start()
    {
        //scrapped code to spawn player based on their character choice in the menu
        //int selectedCharacter = PlayerPrefs.GetInt("KselectedCharacter");
        //GameObject prefab = characters[selectedCharacter];
        //GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        //disable controls until countdown ends
        player.controlsDisabled = true;

        countdownSound = FMODUnity.RuntimeManager.CreateInstance("event:/Countdown");

        countdownSound.start();




    }

    // Update is called once per frame
    void Update()
    {

        // 3 2 1 go! and player is given controls.
        if (countingDown)
        {
            player.controlsDisabled = true;
            float time = CountdownFrom - Time.timeSinceLevelLoad;
           


            if (time >= 3f){
                countDownImage.sprite = countDownTextures[0];
            }
            if (time >= 2f && time < 3f)
            {
                countDownImage.sprite = countDownTextures[1];
            }
            if (time >= 1f && time < 2f)
            {
                countDownImage.sprite = countDownTextures[2];
            }
            if (time >= 0f && time < 1f)
            {
                countDownImage.sprite = countDownTextures[3];
            }

            if (time <= 0f)
            {
                TimeUp();
            }
        }
        else
        {
            //once the start countdown has finished, start the timer to track the players time in the race
            if (!finished)
            {
                countDownImage.enabled = false;
                Timer += Time.deltaTime;
                int minutes = Mathf.FloorToInt(Timer / 60F);
                int seconds = Mathf.FloorToInt(Timer % 60F);
                int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
                playerTime = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
                TimerText.text = playerTime;
            }

        }


    }

    void TimeUp()
    {
        //GO!
        countingDown = false;

        player.controlsDisabled = false;
    }
}
