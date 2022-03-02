////////////////////////////////////////////////////////////
// File: PlayerController.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: A script for prototype and current player controllers to inherit from (to keep debugging and test scripts easy)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Triggerables;

namespace L7Games.Movement
{
    public abstract class PlayerController : MonoBehaviour
    {
        #region Public Events
        /// <summary>
        /// Called when the player re spawns
        /// </summary>
        public event System.Action onRespawn;
        public event System.Action<Vector3> onWipeout;

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

        #region Protected Methods
        protected void CallOnRespawn()
        {
            if (onRespawn != null)
            {
                onRespawn();
            }
        }

        public void CallOnWipeout(Vector3 vel)
        {
            if(onWipeout != null)
            {
                onWipeout(vel);
            }
        }

        #endregion
    }
}
