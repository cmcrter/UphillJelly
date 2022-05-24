//========================================================================================================================================================================================================================================================================
// File:                PauseMenuController.cs
// Author:              Matthew Mason, Charles Carter
// Date Created:        03/05/2022
// Last Edited By:      Charles Carter
// Date Last Edited:    24/05/2022
// Brief:               The controller for the in-game pause menu, is in charge if operations for the menu as well as pausing the games functions
//========================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        [Tooltip("The buttons in pause menu from top to bottom in layout order")]
        private Button[] menuButtons;

        [SerializeField]
        [Tooltip("The menu element that will be shown and hidden depending on the pause state")]
        private GameObject menuUiElement;

        [SerializeField]
        [Tooltip("The Setting panel that can be opened through this pause menu")]
        private GameObject settingPanel;

        [SerializeField]
        [Tooltip("The Event system in the scene")]
        private UnityEngine.EventSystems.EventSystem eventSystem;

        [SerializeField]
        [Tooltip("The Canvas that warning boxes will be put on")]
        private Canvas canvasToUse;

        #endregion

        #region Private Variables
        private int selectedButtonIndex;

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
            eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

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

        private void Update()
        {

            //eventSystem.sele
            //menuButtons[0].Selec = Selectable.Transition.ColorTint;
        }

        private void OnDrawGizmos()
        {
            if (eventSystem != null)
            {
                if (eventSystem.currentSelectedGameObject != null)
                {
                    Gizmos.DrawSphere(eventSystem.currentSelectedGameObject.transform.position, 20f);
                }
            }
        }
        #endregion

        #region Public Method
        #region Button Functionality
        public void MainMenuButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(canvasToUse, eventSystem, "Are you sure you want to restart level? Your level progress will not be saved", SetFirstButtonAsSelected, MoveToMainMenu);
        }

        public static void GoToTutorial(bool save)
        {
            LoadingData.sceneToLoad = "TutorialTrackWhitebox";
            LoadingData.currentLevel = LEVEL.TUTORIAL;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        public static void GoToCity(bool save)
        {
            LoadingData.sceneToLoad = "XanmanCity";
            LoadingData.currentLevel = LEVEL.CITY;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        public static void GoToOldTown(bool save)
        {
            LoadingData.sceneToLoad = "OldTown_Whitebox";
            LoadingData.currentLevel = LEVEL.OLDTOWN;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        public void RestartLevelButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(canvasToUse, eventSystem, "Are you sure you want to restart level? Your level progress will not be saved", SetFirstButtonAsSelected, RestartLevel);
        }
        public void ResumeGameButton()
        {
            UnpauseGame();
        }

        public void SettingsButton()
        {
            settingPanel.SetActive(true);
            // Set the currently selected object to first one found in the panel
            eventSystem.SetSelectedGameObject(settingPanel.GetComponentInChildren<Selectable>().gameObject);
        }

        public void QuitButton()
        {
            // This should have a confirmation box (probably)
            WarningBox.CreateConfirmCancelWarningBox(canvasToUse, eventSystem, "Quit to desktop? Your level progress will not be saved", SetFirstButtonAsSelected, Application.Quit);
        }

        public void OnOptionMenuClose()
        {
            eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
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
                    eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
                    menuUiElement.SetActive(true);
                }

                for (int i = 0; i < inputHandlersInScene.Length; ++i)
                {
                    savedInputActionMaps[i] = inputHandlersInScene[i].AttachedPlayerInput.currentActionMap;
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
            if (Time.timeScale > 0f)
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
        }
        private void RestartLevel()
        {
            UnpauseGame();
            Time.timeScale = 1f;
            LoadingData.sceneToLoad = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("LoadingScene");

        }
        private void MoveToMainMenu()
        {
            UnpauseGame();
            LoadMainMenu();
        }

        private static void LoadMainMenu()
        {
            LoadingData.sceneToLoad = "MainMenu";
            LoadingData.currentLevel = LEVEL.MAINMENU;

            SceneManager.LoadScene("LoadingScene");
        }

        private static void LoadMainMenu(bool save)
        {
            LoadingData.sceneToLoad = "MainMenu";
            LoadingData.currentLevel = LEVEL.MAINMENU;
            LoadingData.SavePlayer = save;

            SceneManager.LoadScene("LoadingScene");
        }

        private void SetFirstButtonAsSelected()
        {
            eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
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
