////////////////////////////////////////////////////////////
// File: PlayerController.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 19/05/22
// Brief: A script for prototype and current player controllers to inherit from (to keep debugging and test scripts easy)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Triggerables;
using Cinemachine;
using System.Collections;
using L7Games.Input;

namespace L7Games.Movement
{
    public abstract class PlayerController : MonoBehaviour
    {
        public bool ignoreNextWipeoutOnWipeoutCount;

        #region Public Events

        /// <summary>
        /// Called when the player re spawns
        /// </summary>
        public event System.Action onRespawn;
        public event System.Action<Vector3> onWipeout;

        #endregion

        #region Variables

        public InputHandler inputHandler; 

        [SerializeField]
        private CineLockCameraZ cameraZ;

        public bool bWipeOutLocked = false;

        //Used to pitch up the collectable sounds when in a spree of picking them up
        public int collectableCounter = 0;

        private Coroutine CollectableCooldownCoroutine;
        [SerializeField]
        private float cooldownDuration = 1f;
        private Timer cooldownTimer;

        public float collectableScore = 0;
        public Rigidbody fRB;

        public int KOCount = 0;

        #endregion

        public virtual void ResetPlayer()
        {
        
        }

        public virtual void ResetPlayer(Transform point)
        {

        }

        public virtual void MoveToPosition(Vector3 positionToMoveTo)
        {

        }

        public virtual void AddWallRide(WallRideTriggerable wallRide)
        {

        }

        public virtual void RemoveWallRide(WallRideTriggerable wallRide)
        {

        }

        public virtual void OverrideCamera(CinemachineVirtualCamera camera)
        {
            
        }

        public void CollectableSoundCooldown()
        {
            collectableCounter++;
            collectableCounter = Mathf.Clamp(collectableCounter, 0, 8);

            if(CollectableCooldownCoroutine == null)
            {
                CollectableCooldownCoroutine = StartCoroutine(Co_CollectablePitchCooldown());
            }
            else if (cooldownTimer != null)
            {
                cooldownTimer.OverrideCurrentTime(cooldownDuration - cooldownTimer.current_time);
            }
        }

        public Rigidbody GetRB()
        {
            return fRB;
        }


        #region Protected Methods

        protected void CallOnRespawn()
        {
            if (onRespawn != null)
            {
                onRespawn();
                if (!ignoreNextWipeoutOnWipeoutCount)
                {
                    KOCount++;
                    ignoreNextWipeoutOnWipeoutCount = false;
                }

            }
        }

        public void CallOnWipeout(Vector3 vel)
        {
            if(onWipeout != null && !bWipeOutLocked)
            {
                onWipeout(vel);
                cameraZ.SwitchOnWipeoutCam(Vector3.zero);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_CollectablePitchCooldown()
        {
            cooldownTimer = new Timer(cooldownDuration);

            while(cooldownTimer.isActive)
            {
                cooldownTimer.Tick(Time.deltaTime);
                yield return null;
            }

            collectableCounter = 0;
            CollectableCooldownCoroutine = null;
        }

        public virtual void OverrideCamera(CinemachineVirtualCamera camera, bool lockCamera)
        {

        }

        #endregion
    }
}
