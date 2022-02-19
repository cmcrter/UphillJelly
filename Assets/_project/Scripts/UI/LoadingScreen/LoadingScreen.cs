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

namespace L7.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        LoadingScreenUI screenUI;

        #endregion

        #region Unity Methods

        void Start()
        {
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
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
            asyncLoad.allowSceneActivation = false;
            bool bTextShowing = false;

            // Wait until the asynchronous scene fully loads
            while(!asyncLoad.isDone)
            {
                //The level is ready to finish loading
                if(asyncLoad.progress >= 0.9f)
                {
                    if(screenUI && !bTextShowing)
                    {
                        screenUI.TurnOnPressText();
                        bTextShowing = true;
                    }

                    if(Keyboard.current.anyKey.wasReleasedThisFrame || Gamepad.current.buttonEast.wasReleasedThisFrame
                        || Gamepad.current.buttonWest.wasReleasedThisFrame 
                        || Gamepad.current.buttonNorth.wasReleasedThisFrame 
                        || Gamepad.current.buttonSouth.wasReleasedThisFrame)
                    {
                        asyncLoad.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }

        #endregion
    }
}