using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;

public class NpcDivingState : State
{
    public NPCBrain controllingBrain;

    public Vector3 chosenDirection;



    public NpcDivingState(NPCBrain controllingBrain)
    {
        this.controllingBrain = controllingBrain;
        if ()
    }

    //Ticking the state along this frame and passing in the deltaTime
    public override void Tick(float dT)
    {

        //To be overridden
    }

    public override void PhysicsTick(float dT)
    {
        //To be overridden
        controllingBrain.npcCharacyerController.Move(chosenDirection * controllingBrain.speed * dT);
    }

    public override void OnStateEnter()
    {
        animator.SetTrigger("Dive");
        //To be overridden
        hasRan = true;
        //Rigidbody newRigidbody = controllingBrain.gameObject.AddComponent<Rigidbody>();
        //Vector3 averagePlayerDirection = Vector3.zero;
        //PlayerController[] closePlayers = controllingBrain.toCloseToPlayerCondition.GetPlayersInRaidus();
        //for (int i = 0; i < closePlayers.Length; ++i)
        //{
        //    averagePlayerDirection += Vector3.Normalize(controllingBrain.transform.position - closePlayers[i].transform.position);
        //}
        //newRigidbody.AddForce(averagePlayerDirection.normalized * 10f, ForceMode.Impulse);
    }

    public override void OnStateExit()
    {
        //To be overridden
        hasRan = false;
    }
}
