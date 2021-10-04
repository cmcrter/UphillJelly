////////////////////////////////////////////////////////////
// File: State.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: An abstract for each state of each state machine to inherit from
//////////////////////////////////////////////////////////// 

using System;

namespace SleepyCat.Utility.StateMachine
{
    [Serializable]
    public abstract class State
    {
        #region Public Fields

        //Whether the state was running last frame
        public bool hasRan;

        //Just a base condition for generic states
        public Condition conditionToMeet;
        public State nextState;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructors for the state
        /// </summary>
        public State()
        {

        }

        public virtual State returnCurrentState()
        {
            if (conditionToMeet.isConditionTrue())
            {
                return nextState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public virtual void Tick(float dT)
        {
            //To be overridden
        }

        public virtual void OnStateEnter()
        {
            //To be overridden
            hasRan = true;
        }

        public virtual void OnStateExit()
        {
            //To be overridden
            hasRan = false;
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
