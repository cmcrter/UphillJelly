using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerSounds : MonoBehaviour
{

    //This script controls almost all the sounds that originate from the player, such as the score sound and the pickup sound.

    public float velocity;
    public GameObject player;

    //Lists all the fmod audio references

    private FMOD.Studio.EventInstance GS;

    private FMOD.Studio.EventInstance pickupSound;

    private FMOD.Studio.EventInstance respawnSound;

    private FMOD.Studio.EventInstance scoreSound;

    private FMOD.Studio.EventInstance specialScoreSound;

    private FMOD.Studio.EventInstance specialScoreSound2;

    private FMOD.Studio.EventInstance ComboSoundFail;

    // A bunch of variables that affect different audio triggers

    public bool grindOn;

    bool inAir;


    bool hasCombo;

    //This int is linked to a parameter in fmod that increases the pitch of the sound

    public int itemCombo;

    public float comboTime;

    public float startComboTime;

    bool hasScore;

    public int scoreCombo;



    public float death;

    public float levelEnd;

    private StudioEventEmitter musicEmitter;

    public string parameterName;

    public string parameterName2;

    bool respawnSwitch;

    // Start is called before the first frame update
    void Start()
    {
        itemCombo = 0;

        scoreCombo = 0;

        startComboTime = comboTime;

        //startScoreTime = scoreTime;

        hasCombo = false;

        hasScore = false;

        musicEmitter = GetComponent<FMODUnity.StudioEventEmitter>();



 

  

        ComboSoundFail.start();
    }


    // Update is called once per frame
   private void Update()
    {

        //These are here to make sure the timers don't overflow

        if (velocity > 1) velocity = 1;

        if (velocity < 0) velocity = 0;

        if (death > 5) death = 5;

        if (death < 0) death = 0;

        if (levelEnd > 100) levelEnd = 100;

        if (levelEnd < 0) levelEnd = 0;

 

        //speed goes about 0 to 55

        if (hasCombo == true)
        {
            startComboTime -= Time.deltaTime;
        }

     
        //Simple timer

        if (startComboTime <=0)
        {
            startComboTime = comboTime;

            itemCombo = 0;

            hasCombo = false;
        }


  

        //Change the FMOD Velocity variable to the Player Rigidbody speed(velocity.magnitude), divided by the maximum speed(observed as 55 as of 28 / 02) to range between 0 and 1.
        if (player.GetComponent<Rigidbody>() != null)
        {
            if (MusicController.Instance != null)
            {
                MusicController.Instance.ChangeVelocity(player.GetComponent<Rigidbody>().velocity.magnitude / 55);
            }

        }


        //can be used to check player maximum speed if it needs changing.
        //Debug.Log(player.GetComponent<Rigidbody>().velocity.magnitude);

        pickupSound.setParameterByName("ItemCombo", itemCombo);

        scoreSound.setParameterByName("ItemCombo", scoreCombo);
;        
        //This is to make sure that the score sound pitch increase caps out to stop it getting too distorted

        if (scoreCombo > 8)
        {
            scoreCombo = 8;
        }




    }

    //These are both remnants from previous code but they are referenced in other scripts so I didn't want to risk deleting them

    public void accelerate()
    {

    }


    public void decelerate()
    {

    }



    //used for setting up the land sound. Although right now it won't detect a landing if the player hasn't jumped

    public void set_inAir(bool set)
    {
        inAir = set;
    }

 
    //Use this if you want to play a simple sound effect in fmod that doesn't use any parameters

    public void playJumpSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Jump", GetComponent<Transform>().position);
    }

    public void playLandSound()
    {

        if (inAir)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Land2", GetComponent<Transform>().position);

            inAir = false;
        }

       
    }

    //Increases the int every time an item is picked up, increasing the pitch up to a certain cap

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == ("Collectible"))
        {
            pickupSound = FMODUnity.RuntimeManager.CreateInstance("event:/ItemPickup");

            pickupSound.start();

            hasCombo = true;

            itemCombo++;
        }
    }

    //Same logic as the item pick up sound where the pitch increases as the value of the int increases however this has two special sounds that play instead on certain combo milestones

    public void playScoreSound()
    {

        hasScore = true;

        scoreCombo++;

        if (scoreCombo == 3)
        {
            specialScoreSound = FMODUnity.RuntimeManager.CreateInstance("event:/ScoreSpecialCombo");

            specialScoreSound.start();
        } else if (scoreCombo == 6)
        {
          specialScoreSound2 = FMODUnity.RuntimeManager.CreateInstance("event:/ScoreSpecialCombo2");

            specialScoreSound2.start();
        } else
        {
            scoreSound = FMODUnity.RuntimeManager.CreateInstance("event:/ScoreSound");
        }

      

        scoreSound.start();

      

    }


    public void resetScoreSound()
    {
       
        scoreCombo = 0;
    }

    public void StartGrindSound()
    {
        if (grindOn == true)
        {
            return;// returns ensure that the sound only plays once
        }

        GS = FMODUnity.RuntimeManager.CreateInstance("event:/GrindRail2");
        GS.start();

        grindOn = true;
       
    }

    public void StopGrindSound()
    {
        if (grindOn == false)
        {
            return;
        }
       

        GS.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        GS.release();

        grindOn = false;
    }


    public void isDead()
    {
        respawnSwitch = true;

        if (scoreCombo >= 3)
        {
            //plays the combofail sound if the player wipes out after building up at least a small combo

            ComboSoundFail = FMODUnity.RuntimeManager.CreateInstance("event:/ComboFail");

            ComboSoundFail.start();
        }

        if (musicEmitter != null)
        {

            //the death float is linked to a value in fmod that causes pitch distortion, so when the player dies it slowly increases, causing the music audio to be all goofy

            musicEmitter.SetParameter(parameterName, death);

            death += 0.004f;
        }
    }


    public void isAlive()
    {
        if (musicEmitter != null)
        {
            //makes it so that the distortion on the music almost instantly reverts back to normal once the player respawns

            musicEmitter.SetParameter(parameterName, death);

            death -= 0.5f;

        }

        if (respawnSwitch == false)
        {
            return;
        }

        //plays the respawn sound on respawn

        respawnSound = FMODUnity.RuntimeManager.CreateInstance("event:/Respawn");
        respawnSound.start();


        respawnSwitch = false;



    }

    public void finishRace()
    {
        if (musicEmitter != null)
        {
            levelEnd = 100f;

            musicEmitter.SetParameter(parameterName2, levelEnd);

          
        }

    }

}
