////////////////////////////////////////////////////////////
// File: RedLight.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A state to go into the unit tested state machine
//////////////////////////////////////////////////////////// 

using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Tests
{
    public class RedLight : State
    {
        #region Public Fields

        public float valueGiven = 0;
        public isGreaterThanZero condition = new isGreaterThanZero();

        #endregion
        public RedLight()
        {

        }

        public RedLight(State stateToGoTo)
        {
            nextState = stateToGoTo;
        }

        public override State returnCurrentState()
        {
            condition.floatToTestAgainst = valueGiven;

            conditionToMeet = condition;
            return base.returnCurrentState();
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