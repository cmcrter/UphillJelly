using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChadLads.Scoreboards

    //stuff we trigger when we cross the finish line! popping up scoreboard, stopping the player controls, placing the player in a idle position, and moving the camera.
{
    public class FinishLine : MonoBehaviour
    {

        public GameObject scoreboard;
        private bool hasPassed = false;
        public GameController gameMaster;
        public GameObject inGameHUD;
        public Transform endPodium;
        public GameObject EndCamera;
        public GameObject endGameUI;

        [SerializeField]
        private PlayerSounds playerSounds;

        [SerializeField]
        private musicSwitch MusicSwitch;



        private FMOD.Studio.EventInstance endCheerSound;
        //use fmod to play a cheesy crowd cheering sound

  

        private void OnTriggerEnter(Collider other)
        {
            if (!hasPassed)
            { 
                //if we haven't already passed the checkpoint, disable controls, put player into idle and move him to the final podium position, bring up ui and scoreboard.
                if (other.gameObject.tag == ("Player"))
                {
                    other.gameObject.GetComponent<HoverboardController>().controlsDisabled = true;
                    other.gameObject.GetComponent<HoverboardController>().charAnim.SetTrigger("StandingIdle");
                    other.gameObject.transform.position = endPodium.position;
                    other.gameObject.transform.rotation = endPodium.rotation;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    inGameHUD.SetActive(false);
                    EndCamera.SetActive(true);
                    endGameUI.SetActive(true);

                    scoreboard.SetActive(true);
                    scoreboard.GetComponent<Scoreboard>().AddPlayerEntry();
                    hasPassed = true;

                    endCheerSound = FMODUnity.RuntimeManager.CreateInstance("event:/EndCheer");

                    endCheerSound.start();

                    gameMaster.finished = true;

                    playerSounds.finishRace();

                    MusicSwitch.finishRace();

                }

            }



        }
    }
}

