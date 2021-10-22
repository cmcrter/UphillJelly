////////////////////////////////////////////////////////////
// File: isNextToWallRun.cs
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: The condition to meet for the player to start wall running
//////////////////////////////////////////////////////////// 

using System;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isNextToWallRun : Condition
    {
        #region Public Methods

        public override bool isConditionTrue()
        {
            return false;
        }

        #endregion
    }
}