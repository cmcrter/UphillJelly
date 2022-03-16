using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;

public class NpcMoveToLocationState : State
{
    public override State returnCurrentState()
    {
        // If at desintaion then idle

        // if can't reach Desination then give up and idle

        //if (brainComponent.toCloseToPlayerCondition.isConditionTrue())
        //{
        //    return brainComponent.divingState;
        //}
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
}
