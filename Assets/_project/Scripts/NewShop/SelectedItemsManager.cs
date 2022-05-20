using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using L7Games.Input;
using UnityEngine.EventSystems;

namespace L7Games.UI
{
    /// <summary>
    /// The controller for the in-game pause menu, is in charge if operations for the menu as well as pausing the games functions,
    /// </summary>
    public class SelectedItemsManager : MonoBehaviour
    {




        /*
        #region Private Serialized Field
        [SerializeField]
        [Tooltip("The buttons in pause menu from top to bottom in layout order")]
        private Button[] menuButtons;


        [SerializeField]
        [Tooltip("The Event system in the scene")]
        private UnityEngine.EventSystems.EventSystem eventSystem;


        #endregion

        #region Private Variables
        [SerializeField]
        private int selectedButtonIndex;


        #endregion



        #region Unity Methods
        
        public void OnEnable() {
            
            eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

        }

        public void OnDisable() {
            
        }

        private void OnDrawGizmos() {
            if (eventSystem != null) {
                if (eventSystem.currentSelectedGameObject != null) {
                    Gizmos.DrawSphere(eventSystem.currentSelectedGameObject.transform.position, 20f);
                }
            }
        }

        public void Start() {
            eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
        }

        public void OnHoverGO() {

            

        }


        /*
        public void OnPointerEnter(PointerEventData eventData) {
            DoThis(this);
        }

        void DoThis(Button button) {

            // Only change tab sprite if it is not already selected
            if (menuButtons[selectedButtonIndex] == null || button != menuButtons[selectedButtonIndex]) {

                Debug.Log("Entered");

                //button.backgroundTabImage.sprite = tabHover;

            }

        }

        public void OnPointerExit(Button button) {
            //ResetTabs();
            Debug.Log("Exited");
        }
        */



        /*

        #endregion

        #region Public Method
        #region Button Functionality

        /// <summary>
        /// Include Functionality here
        /// </summary>
        public void OnOptionMenuClose() {
            eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
        }
        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Include Functionality + reference here (OnStart or switch panel)
        /// </summary>
        private void SetFirstButtonAsSelected() {
            eventSystem.SetSelectedGameObject(menuButtons[0].gameObject);
        }

        
        #endregion

        */
    }


}