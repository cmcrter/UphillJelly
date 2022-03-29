using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;

[System.Serializable]
public class NpcIdleState : State
{
    #region Public Variables
    [HideInInspector]
    public IdleReactionNPC brainComponent;

    public NpcIdleState(IdleReactionNPC brainComponent)
    {
        this.brainComponent = brainComponent;
    }
    #endregion

    #region Public Methods
    public override State returnCurrentState()
    {
        //if (brainComponent.toCloseToPlayerCondition.isConditionTrue())
        //{
        //    //return brainComponent.divingState;
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

    #endregion
}
