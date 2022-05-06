//========================================================================================================================================================================================================================================================================
// File:                PauseMenuController.cs
// Author:              Matthew Mason
// Date Created:        03/05/2022
// Last Edited By:      Matthew Mason
// Date Last Edited:    03/05/2022
// Brief:               The controller for the in-game pause menu, is in charge if operations for the menu as well as pausing the games functions
//========================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using L7Games.Input;
using L7Games.Loading;


namespace L7Games.UI
{
    /// <summary>
    /// The controller for the in-game pause menu, is in charge if operations for the menu as well as pausing the games functions,
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        #region Private Serialized Field
        [SerializeField]
        [Tooltip("The menu element that will be shown and hidden depending on the pause state")]
        private GameObject menuUiElement;

        [SerializeField]
        [Tooltip("The Setting panel that can be opened through this pause menu")]
        private GameObject settingPanel;
        #endregion

        #region Private Variables
        /// <summary>
        /// All the input handlers that are currently in the scene
        /// </summary>
        private InputHandler[] inputHandlersInScene;

        /// <summary>
        /// All the action maps that have been switch out of for the menu action map
        /// </summary>
        private InputActionMap[] savedInputActionMaps;
        #endregion

        #region Unity Methods
        public void Start()
        {
            // Find all the players that are added to the scene
            if (inputHandlersInScene == null)
            {
                UpdateInputHandlersFromScene();
            }
        }
        public void OnEnable()
        {
            // Find all the players that are added to the scene if they are not already
            if (inputHandlersInScene == null)
            {
                UpdateInputHandlersFromScene();
            }

            for (int i = 0; i < inputHandlersInScene.Length; ++i)
            {
                inputHandlersInScene[i].PauseActionPerformed += PauseMenuController_PauseActionPerformed;
            }
        }

        public void OnDisable()
        {
            if (inputHandlersInScene != null)
            {
                for (int i = 0; i < inputHandlersInScene.Length; ++i)
                {
                    inputHandlersInScene[i].PauseActionPerformed -= PauseMenuController_PauseActionPerformed;
                }
            }
        }
        #endregion

        #region Public Method
        #region Button Functionality
        public void MainMenuButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(transform.root.GetComponent<Canvas>(), "Are you sure you want to restart level? Your level progress will not be saved", null, MoveToMainMenu);
        }
        public void RestartLevelButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(transform.root.GetComponent<Canvas>(), "Are you sure you want to restart level? Your level progress will not be saved", null, RestartLevel);
        }
        public void ResumeGameButton()
        {
            UnpauseGame();
        }

        public void SettingsButton()
        {
            settingPanel.SetActive(true);
        }

        public void QuitButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(transform.root.GetComponent<Canvas>(), "Quit to desktop? Your level progress will not be saved", null, Application.Quit);
        }
        #endregion

        /// <summary>
        /// Pauses the gameplay and brings up the pause menu
        /// </summary>
        public void PauseGame()
        {
            if (PauseManager.instance != null)
            {
                // Check if the game is not already somehow paused
                if (!PauseManager.instance.IsPaused)
                {
                    PauseManager.instance.PauseGame();
                    menuUiElement.SetActive(true);
                }

                for (int i = 0; i < inputHandlersInScene.Length; ++i)
                {
                    savedInputActionMaps[i] = inputHandlersInScene[i].AttachedPlayerInput.currentActionMap;
                    Debug.Log(savedInputActionMaps[i].name);
                    inputHandlersInScene[i].AttachedPlayerInput.SwitchCurrentActionMap("Menu");
                }
            }
        }

        /// <summary>
        /// Unpauses the game and closes the pause menu
        /// </summary>
        public void UnpauseGame()
        {
            // Check if the game is not already unpaused
            if (PauseManager.IsInstancePaused)
            {
                PauseManager.instance.UnpauseGame();
                menuUiElement.SetActive(false);

                for (int i = 0; i < inputHandlersInScene.Length; ++i)
                {
                    inputHandlersInScene[i].AttachedPlayerInput.SwitchCurrentActionMap(savedInputActionMaps[i].name);
                    Debug.Log(savedInputActionMaps[i].name);
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called by any pause input from the players to call pause or unpause
        /// </summary>
        private void PauseMenuController_PauseActionPerformed()
        {
            if (menuUiElement.activeSelf)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
        private void RestartLevel()
        {
            UnpauseGame();
            LoadingData.sceneToLoad = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("LoadingScene");
        }
        private void MoveToMainMenu()
        {
            UnpauseGame();
            LoadingData.sceneToLoad = "MainMenu";
            LoadingData.currentLevel = LEVEL.MAINMENU;

            SceneManager.LoadScene("LoadingScene");
        }

        /// <summary>
        /// Check through the scene for all the input handlers 
        /// </summary>
        private void UpdateInputHandlersFromScene()
        {
            inputHandlersInScene = FindObjectsOfType<InputHandler>();
            savedInputActionMaps = new InputActionMap[inputHandlersInScene.Length];
            #if DEBUG || UNITY_EDITOR
            if (inputHandlersInScene.Length == 0)
            {
                Debug.LogWarning("No input handlers could be found in scene by the pause menu controller");
            }
            #endif
        }
        #endregion
    }
}
