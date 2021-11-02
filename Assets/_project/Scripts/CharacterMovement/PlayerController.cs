////////////////////////////////////////////////////////////
// File: PlayerController.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: A script for prototype and current player controllers to inherit from (to keep debugging and test scripts easy)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Triggerables;

namespace SleepyCat.Movement
{
    public abstract class PlayerController : MonoBehaviour
    {
        public virtual void ResetPlayer()
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
    }
}
