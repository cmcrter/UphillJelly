////////////////////////////////////////////////////////////
// File: YellowLight.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A state in the traffic light based FSM unit testing
//////////////////////////////////////////////////////////// 

using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Tests
{
    public class YellowLight : State
    {
        #region Public Fields

        public float valueGiven = 0;
        public isLessThanZero condition = new isLessThanZero();

        public Condition conditionToMeet;
        public State nextState;

        #endregion

        public YellowLight()
        {

        }

        public YellowLight(State stateToGoTo)
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