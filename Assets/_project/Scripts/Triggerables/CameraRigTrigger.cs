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
        public PlayFabManager playfabManager;

        public GameObject LeaderboardGO;

        #endregion

        #region Private Methods

        void PlayerEntered(PlayerController player)
        {
            player.OverrideCamera(cameraToUse, true);
            trackedDolly.enabled = true;
            //player.enabled = false;

            StartCoroutine(WaitFor5Seconds());

            if(endTrigger != null)
            {
                endTrigger.Invoke();
            }      
        }

        IEnumerator WaitFor5Seconds() {

            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
            LeaderboardGO.SetActive(true);
            //playfabManager.FinishedLevelTriggered();
        }

        void PlayerExited(PlayerController player)
        {

        }

        #endregion
    }
}
