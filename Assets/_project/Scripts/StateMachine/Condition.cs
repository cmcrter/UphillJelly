////////////////////////////////////////////////////////////
// File: Condition.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A condition for state machines to use to determine current state
//////////////////////////////////////////////////////////// 

using System;

namespace L7Games.Utility.StateMachine
{
    [Serializable]
    public abstract class Condition
    {
        #region Public Methods

        public virtual bool isConditionTrue()
        {
            //To be overridden
            return false;
        }

        #endregion
    }
}
