////////////////////////////////////////////////////////////
// File: GreenLight.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A state in the traffic light based FSM unit testing
//////////////////////////////////////////////////////////// 

using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Tests
{
    public class GreenLight : State
    {
        #region Public Fields

        public float valueGiven = 0;
        public isZero condition = new isZero();

        #endregion

        public override State returnCurrentState()
        {
            condition.floatToTestAgainst = valueGiven;

            if (condition.isConditionTrue())
            {
                return new RedLight();
            }

            return this;
        }

        public override void Tick(float dT)
        {
            valueGiven = dT;
        }

        public override void OnStateExit()
        {
            valueGiven = 0;
        }

        #region Private Methods
        #endregion
    }
}
