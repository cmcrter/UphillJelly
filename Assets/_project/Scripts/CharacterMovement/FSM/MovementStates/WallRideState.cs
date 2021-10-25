////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    public class WallRideState : State
    {
        #region Public Methods

        public WallRideState()
        {

        }

        public override State returnCurrentState()
        {
            if(conditionToMeet.isConditionTrue())
            {
                return nextState;
            }

            return this;
        }

        //Ticking the state along this frame and passing in the deltaTime
        public override void Tick(float dT)
        {
            //To be overridden
        }

        public override void PhysicsTick(float dT)
        {
            //To be overridden
        }

        public override void OnStateEnter()
        {
            //To be overridden
            hasRan = true;
        }

        public override void OnStateExit()
        {
            //To be overridden
            hasRan = false;
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
