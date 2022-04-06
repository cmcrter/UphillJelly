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

        #endregion

        #region Private Methods

        void PlayerEntered(PlayerController player)
        {
            player.OverrideCamera(cameraToUse, true);
            trackedDolly.enabled = true;
            //player.enabled = false;
        }

        void PlayerExited(PlayerController player)
        {

        }

        #endregion
    }
}
