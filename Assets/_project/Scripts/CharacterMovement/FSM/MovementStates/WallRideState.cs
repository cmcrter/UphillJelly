////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Utility.StateMachine;
using UnityEngine.InputSystem;

namespace SleepyCat.Movement
{
    public class WallRideState : State
    {
        private PlayerInput pInput;


        #region Public Methods

        public WallRideState()
        {

        }

        public void InitialiseState()
        {

        }

        public override State returnCurrentState()
        {
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
            pInput.SwitchCurrentActionMap("WallRiding");
            hasRan = true;
        }

        public override void OnStateExit()
        {
            //To be overridden
            hasRan = false;
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
