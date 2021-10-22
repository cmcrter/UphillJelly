////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using System;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    [Serializable]
    public class isOnGrind : Condition
    {
        #region Public Methods

        public override bool isConditionTrue()
        {
            return false;
        }

        #endregion
    }
}