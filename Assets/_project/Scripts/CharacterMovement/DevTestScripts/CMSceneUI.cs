////////////////////////////////////////////////////////////
// File: CMSceneUI
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/01/22
// Brief: A script for using UI and inspector to test out the character controller in the Character Movement test scene
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Movement;

namespace L7Games.DevScripts
{
    public class CMSceneUI : MonoBehaviour
    {
        public Movement.PlayerController characterController;
        public Rigidbody CharacterRB;
        public GameObject goControlUI;

        #region Public Methods

        /// <summary>
        /// Methods for changing character's movement state
        /// </summary>
        [ContextMenu("Change Test Player To Aerial")]
        public void SetModeToAerial()
        {
            //Moving the character into the air
            characterController.transform.position = new Vector3(0, -40f, 0);
            //Setting it's state
            //characterController.SetState();
            //Making sure it doesn't fall
            CharacterRB.useGravity = false;
        }

        [ContextMenu("Change Test Player To Grounded")]
        public void SetModeToGrounded()
        {
            //Moving the character to the start point
            characterController.transform.position = new Vector3(0, 0.5f, 0);
            //Setting it's state
            //characterController.SetState();
            //Making sure it can fall
            CharacterRB.useGravity = true;
        }

        [ContextMenu("Change Test Player To Grinding")]
        public void SetModeToGrinded()
        {
            //Moving the character to the grind rail point
            //Setting it's state explicitly

            CharacterRB.useGravity = false;
        }

        /// <summary>
        /// Methods for hiding/showing the UI that helps quickly test
        /// </summary>
        [ContextMenu("Hide The Control UI")]
        public void HideControlUI()
        {
            goControlUI.SetActive(false);
        }

        [ContextMenu("Show The Control UI")]
        public void ShowControlUI()
        {
            goControlUI.SetActive(true);
        }

        #endregion
    }
}