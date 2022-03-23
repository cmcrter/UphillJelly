using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;

public class NpcAngryState : State
{
    [HideInInspector]
    public NPCBrain brainComponent;

    private MeshRenderer characterMesh;

    public NpcAngryState(NPCBrain brainComponent)
    {
        this.brainComponent = brainComponent;
    }

    public override State returnCurrentState()
    {
        // If the NPC is out of view of the player and out a certain distance then they can return back to their starting spot

        if (brainComponent.toCloseToPlayerCondition.isConditionTrue())
        {
            return brainComponent.divingState;
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
}
