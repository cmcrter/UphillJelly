////////////////////////////////////////////////////////////
// File: MainMenuLevelSelect.cs
// Author: Jack Peedle
// Date Created: 29/02/22
// Last Edited By: Jack Peedle
// Date Last Edited: 29/02/22
// Brief: 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SleepyCat
{
    public class MainMenuLevelSelect : MonoBehaviour
    {

        public Sprite tutorialImage;
        public Sprite cityImage;
        public Sprite oldTownImage;

        public GameObject playTutorialButton;
        public GameObject playCityButton;
        public GameObject playOldTownButton;

        public Image displayMap;

        public int currentLevelInt;

        private void Start() {

            currentLevelInt = 0;
            playTutorialButton.SetActive(true);
            playCityButton.SetActive(false);
            playOldTownButton.SetActive(false);

        }

        //
        public void NextLevelOption() {

            // 
            currentLevelInt++;

            // 
            if (currentLevelInt >= 3) {

                // 
                currentLevelInt = 0;

            }


            UpdateImage();

        }


        //
        public void PreviousLevelOption() {

            //
            currentLevelInt--;

            //
            if (currentLevelInt <= -1) {

                //
                currentLevelInt = 2;

            }


            UpdateImage();

        }

        //
        public void UpdateImage() {

            if (currentLevelInt == 0) {

                //
                displayMap.sprite = tutorialImage;

                playTutorialButton.SetActive(true);
                playCityButton.SetActive(false);
                playOldTownButton.SetActive(false);

            }

            if (currentLevelInt == 1) {

                //
                displayMap.sprite = cityImage;

                playTutorialButton.SetActive(false);
                playCityButton.SetActive(true);
                playOldTownButton.SetActive(false);

            }

            if (currentLevelInt == 2) {

                //
                displayMap.sprite = oldTownImage;

                playTutorialButton.SetActive(false);
                playCityButton.SetActive(false);
                playOldTownButton.SetActive(true);

            }

        }

        public void LoadTutorialMap() {

            //
            Debug.Log("LOADEDTUTORIAL");

            SceneManager.LoadScene("TutorialTrackWhitebox");

        }

        public void LoadCityMap() {

            //
            Debug.Log("LOADEDCITY");

            SceneManager.LoadScene("XanmanCity");

        }

        public void LoadOldTownMap() {

            //
            Debug.Log("LOADEDOLDTOWN");

            SceneManager.LoadScene("OldTown_Whitebox");

        }

    }
}
