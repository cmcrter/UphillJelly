////////////////////////////////////////////////////////////
// File: CameraRigTrigger.cs
// Author: Charles Carter.cs
// Date Created: 30/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 30/03/22
// Brief: A trigger for the camera to follow a path (used for intro cinematic)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using Cinemachine;
using L7Games.Movement;
using UnityEngine.Events;
using System.Collections;

namespace L7Games
{
    public class CameraRigTrigger : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        GameObject ITriggerable.ReturnGameObject() => gameObject;
        void ITriggerable.Trigger(PlayerController player) => PlayerEntered(player);
        void ITriggerable.UnTrigger(PlayerController player) => PlayerExited(player);

        #endregion

        #region Variables

        [SerializeField]
        private CinemachineVirtualCamera cameraToUse;
        [SerializeField]
        private CinemachineDollyCart trackedDolly;
        [SerializeField]
        private UnityEvent endTrigger;

        //public b_Player b_player;
        public LevelFinishUIController uiController;

        public GameObject LeaderboardGO;

        [SerializeField]
        private float waitForTimer = 1f;

        private bool bTriggered = false;

        #endregion

        #region Private Methods

        void PlayerEntered(PlayerController player)
        {
            if(bTriggered)
            {
                return;
            }

            player.OverrideCamera(cameraToUse, true);
            player.CallOnWipeout(player.GetRB().velocity);
            trackedDolly.enabled = true;
            //player.enabled = false;

            bTriggered = true;

            StartCoroutine(WaitFor5Seconds());

            if(endTrigger != null)
            {
                endTrigger.Invoke();
            }      
        }

        private IEnumerator WaitFor5Seconds() 
        {
            uiController.LoginPlayfab();

            yield return new WaitForSeconds(waitForTimer);

            uiController.PopulateInformation();
            yield return new WaitForSeconds(0.1f);

            LeaderboardGO.SetActive(true);
        }

        void PlayerExited(PlayerController player)
        {

        }

        #endregion
    }
}
