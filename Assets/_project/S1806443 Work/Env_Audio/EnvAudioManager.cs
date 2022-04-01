using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace SleepyCat
{
    public class EnvAudioManager : MonoBehaviour
    {

        public ReplaySaveManager replaySaveManager;

        private FMOD.Studio.EventInstance mainMenuMusic;
        private FMOD.Studio.EventInstance tutorialMusic;
        private FMOD.Studio.EventInstance cityMusic;
        private FMOD.Studio.EventInstance oldTownMusic;

        private StudioEventEmitter soundEmitter;

        //public string Eventthis = "";

        //private StudioEventEmitter secondEmitter;

        // Start is called before the first frame update
        void Start() {

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
        
        }

        private IEnumerator WaitOnStart() {

            yield return new WaitForSeconds(2f);

            PlayMusicForMap();

            Debug.Log("WaitedForTwoSeconds");

        }

    }
}
