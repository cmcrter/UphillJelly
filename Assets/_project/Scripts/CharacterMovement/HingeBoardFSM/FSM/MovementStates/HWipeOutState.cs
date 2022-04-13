using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;

namespace L7Games.Movement
{
    public class HWipeOutState : State
    {
        public override void OnStateEnter()
        {
        }

        public override void OnStateExit()
        {
        }

        public override State returnCurrentState()
        {
            return this;
        }

        public override void Tick(float dT)
        {
        }
    }
}

