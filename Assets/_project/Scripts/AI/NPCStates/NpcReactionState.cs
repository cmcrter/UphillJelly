//======================================================================================================================================================================================================================
//  Name:               NpcReactionState.cs
//  Authors:            Matthew Mason
//  Date Created:       29/03/2022
//  Last Modified by:   Matthew Mason
//  Date Last Modified: 29/03/2022
//  Brief:              State Machine State for when and NPC is reacting too the player passing too close by
//======================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;
using L7Games;

/// <summary>
/// State Machine State for when and NPC is reacting too the player passing too close by
/// </summary>
public class NpcReactionState : State
{
    /// <summary>
    /// The NPC with the state machine running this state
    /// </summary>
    public IdleReactionNPC controllingBrain;

    public NpcReactionState(IdleReactionNPC controllingBrain)
    {
        this.controllingBrain = controllingBrain;
        
    }

    public override State returnCurrentState()
    {
        // Check if animation is complete
        if (controllingBrain.animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            if (controllingBrain.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                //return controllingBrain.idleNpcState;
            }
        }

        return this;
    }

    //Ticking the state along this frame and passing in the deltaTime
    public override void Tick(float dT)
    {
        //diveTimer.Tick(dT);
        //To be overridden


    }

    public override void PhysicsTick(float dT)
    {
        //To be overridden
        //controllingBrain.npcCharacyerController.Move(chosenDirection * controllingBrain.speed * dT);

    }

    public override void OnStateEnter()
    {
        //PlayerHingeMovementController[] playersInRange = controllingBrain.toCloseToPlayerCondition.GetPlayersInRaidus();
        //// Calculate average Player Velocity
        //Vector3[] playerVelocities = new Vector3[playersInRange.Length];
        //for (int i = 0; i < playersInRange.Length; ++i)
        //{
        //    // Might be worth making the velocity of the rigid-body public
        //    playerVelocities[i] = playersInRange[i].GetComponent<Rigidbody>().velocity;
        //}
        //chosenDirection = GetDiveDirection(playerVelocities);

        controllingBrain.animator.SetTrigger("PlayerPoximity");

        //if (diveTimer == null)
        //{
        //    diveTimer = new Timer(controllingBrain.diveDuration);
        //}
        //else
        //{
        //    diveTimer.OverrideCurrentTime(controllingBrain.diveDuration);
        //    diveTimer.isActive = true;
        //}

        //diveTimer.isActive = true;
        ////To be overridden
        //hasRan = true;
        ////Rigidbody newRigidbody = controllingBrain.gameObject.AddComponent<Rigidbody>();
        ////Vector3 averagePlayerDirection = Vector3.zero;
        ////PlayerController[] closePlayers = controllingBrain.toCloseToPlayerCondition.GetPlayersInRaidus();
        ////for (int i = 0; i < closePlayers.Length; ++i)
        ////{
        ////    averagePlayerDirection += Vector3.Normalize(controllingBrain.transform.position - closePlayers[i].transform.position);
        ////}
        ////newRigidbody.AddForce(averagePlayerDirection.normalized * 10f, ForceMode.Impulse);
    }

    public override void OnStateExit()
    {
        //To be overridden
        hasRan = false;
    }

    //private Vector3 GetDiveDirection(Vector3[] velocities)
    //{
    //    Vector3 avargeVelocity = Vector3.zero;
    //    Vector3 newDirection = Vector3.zero;
    //    for (int i = 0; i < velocities.Length; ++i)
    //    {
    //        avargeVelocity += velocities[i];
    //    }
    //    avargeVelocity /= velocities.Length;
    //    return Vector3.Cross(new Vector3(avargeVelocity.x, 0f, avargeVelocity.z).normalized, Vector3.up);
    //}
}
