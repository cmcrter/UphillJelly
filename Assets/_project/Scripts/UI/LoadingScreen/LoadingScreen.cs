////////////////////////////////////////////////////////////
// File: LoadingScreen.cs
// Author: Charles Carter
// Date Created: 04/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 04/02/22
// Brief: An Async loading screen to load levels etc
//////////////////////////////////////////////////////////// 

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace L7Games.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private LoadingScreenUI screenUI;

        [Header("Override Loading Data")]
        [SerializeField]
        private bool OverrideValues = false;

        [SerializeField]
        private string sceneToLoad;
        [SerializeField]
        private bool waitForNextScene;
        [SerializeField]
        private LEVEL levelToLoad;
        [SerializeField]
        private bool bSaveProfile;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if(OverrideValues && LoadingData.sceneToLoad == null)
            {
                LoadingData.sceneToLoad = sceneToLoad;
                LoadingData.waitForNextScene = waitForNextScene;
                LoadingData.currentLevel = levelToLoad;
                LoadingData.SavePlayer = bSaveProfile;
            }
        }

        void Start()
        {
            screenUI.TurnOffPressText();

            //Starting loading the correct scene
            StartCoroutine(LoadYourAsyncScene());
        }

        #endregion

        #region Private Methods

        private IEnumerator LoadYourAsyncScene()
        {
            //Having a default just in case
            if(LoadingData.sceneToLoad == null)
            {
                LoadingData.sceneToLoad = "MainMenu";
                LoadingData.currentLevel = LEVEL.MAINMENU;
                LoadingData.waitForNextScene = false;
                LoadingData.SavePlayer = false;
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

            //Saving the player profile if need be (after levels)
            if(LoadingData.SavePlayer && LoadingData.playerSlot > 0 && LoadingData.playerSlot < 4)
            {
                yield return StartCoroutine(b_SaveSystem.Co_SavePlayer(LoadingData.playerSlot));
                LoadingData.SavePlayer = false;
            }

            asyncLoad.allowSceneActivation = false;
            bool bTextShowing = false;

            // Wait until the asynchronous scene fully loads
            while(!asyncLoad.isDone)
            {
                //The level is ready to finish loading
                if(asyncLoad.progress >= 0.9f)
                {
                    if (!LoadingData.waitForNextScene)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }

                    if (screenUI && !bTextShowing && LoadingData.waitForNextScene)
                    {
                        screenUI.TurnOnPressText();
                        bTextShowing = true;
                    }

                    if (Keyboard.current != null)
                    {
                        if (Keyboard.current.anyKey.wasPressedThisFrame)
                        {
                            asyncLoad.allowSceneActivation = true;

                            //Setting it back to default value
                            LoadingData.waitForNextScene = false;
                        }
                    }
                    
                    if (Gamepad.current != null)
                    {
                        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
                        {
                            asyncLoad.allowSceneActivation = true;

                            //Setting it back to default value
                            LoadingData.waitForNextScene = false;
                        }
                    }
                }

                yield return null;
            }
        }

        #endregion
    }
}