////////////////////////////////////////////////////////////
// File: isZero.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A simple condition for the traffic light based FSM unit test to use
//////////////////////////////////////////////////////////// 

using SleepyCat.Utility.StateMachine;

public class isZero : Condition
{
    #region Public Fields

    public float floatToTestAgainst = 0;

    #endregion

    #region Public Methods

    public override bool isConditionTrue()
    {
        if (floatToTestAgainst == 0)
        {
            return true;
        }

        return false;
    }

    #endregion
}