using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using L7Games.Movement;
using UnityEngine.UI;

namespace SleepyCat
{
    public class EnvAudioManager : MonoBehaviour
    {

        public L7Games.Movement.PlayerController playerReference;

        public ReplaySaveManager replaySaveManager;

        private FMOD.Studio.EventInstance mainMenuMusic;
        private FMOD.Studio.EventInstance tutorialMusic;
        private FMOD.Studio.EventInstance cityMusic;
        private FMOD.Studio.EventInstance oldTownMusic;

        private StudioEventEmitter soundEmitter;

        FMOD.Studio.Bus Master;
        FMOD.Studio.Bus Ambient;
        FMOD.Studio.Bus Player;
        FMOD.Studio.Bus Music;

        public float fDeath;

        public string deathParameter;

        public Slider slider1;
        public Slider slider2;
        public Slider slider3;
        public Slider slider4;

        public float masterVolume;
        public float ambientVolume;
        public float playerVolume;
        public float musicVolume;

        //public string Eventthis = "";

        //private StudioEventEmitter secondEmitter;

        private void Awake() {

            Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            Ambient = FMODUnity.RuntimeManager.GetBus("bus:/Ambient");
            Player = FMODUnity.RuntimeManager.GetBus("bus:/Player");
            Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");

        }

        private void OnEnable() {
            playerReference.onWipeout += PlayerWipeOut;

            playerReference.onRespawn += PlayerRespawn;
        }

        private void OnDisable() {
            playerReference.onWipeout -= PlayerWipeOut;

            playerReference.onRespawn -= PlayerRespawn;
        }

        // Start is called before the first frame update
        void Start() {

            

            soundEmitter = GetComponent<FMODUnity.StudioEventEmitter>();

            soundEmitter.SetParameter(deathParameter, fDeath);

            fDeath = 0;

            //secondEmitter.EventInstance

            StartCoroutine(WaitOnStart());

            mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance(null);

            //soundEmitter.SetParameter

            /*
            if (replaySaveManager.isMapCity) {

                //
                cityMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Lv2Music");

                Debug.Log("PlayedCityAudio");
                cityMusic.start();

                

            }

            if (replaySaveManager.isMapOldTown) {

                //
                oldTownMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Lv1Music");

                Debug.Log("PlayedOldTownAudio");
                oldTownMusic.start();

                

            }
            */
        }

        public void PlayMusicForMap() {
            /*
            if (replaySaveManager.isMainMenu) {

                //Play main menu music
                mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/MenuMusicRadio");


                Debug.Log("PlayedMMAudio");
                mainMenuMusic.start();



            }

            if (replaySaveManager.isMapTutorial) {

                mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Lv1Music");

                Debug.Log("PlayedTutorialAudio");
                mainMenuMusic.start();

                //
                //tutorialMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Lv1Music");

                //Debug.Log("PlayedTutorialAudio");
                //tutorialMusic.start();



            }

            if (replaySaveManager.isMapCity) {

                //Play main menu music
                mainMenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Lv2Music");

                Debug.Log("PlayedCityAudio");
                mainMenuMusic.start();



            }
            */

        }

        // Update is called once per frame
        void Update() {
        
            if (fDeath > 5) {
                fDeath = 5;
            }

            if (fDeath < 0) {
                fDeath = 0;
            }

            masterVolume = slider1.value;
            Debug.Log(masterVolume);

            ambientVolume = slider2.value;
            Debug.Log(ambientVolume);

            playerVolume = slider3.value;
            Debug.Log(playerVolume);

            musicVolume = slider4.value;
            Debug.Log(musicVolume);

            Master.setVolume(masterVolume);
            Ambient.setVolume(ambientVolume);
            Player.setVolume(playerVolume);
            Music.setVolume(musicVolume);
        }

        private IEnumerator WaitOnStart() {

            yield return new WaitForSeconds(2f);

            PlayMusicForMap();

            Debug.Log("WaitedForTwoSeconds");

        }

        public void PlayerWipeOut(Vector3 death) {
            Debug.Log("12345");

            fDeath = 5;
        }

        public void LevelEnd() {

            Debug.Log("12345");

            fDeath = 5;

        }

        public void PlayerRespawn() {

        }
    }
}
