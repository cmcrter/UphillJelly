////////////////////////////////////////////////////////////
// File: WallRideState.cs
// Author: Charles Carter
// Date Created: 29/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 29/10/21
// Brief: The state the player is in when they're riding along the wall
//////////////////////////////////////////////////////////// 
///
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Movement
{
    public class WallRideState : State
    {
        private PlayerInput pInput;

        [SerializeField]
        private float coyoteDuration;
        private Timer coyoteTimer;
        private Coroutine Co_CoyoteCoroutine;

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

        private IEnumerator Co_CoyoteTime()
        {
            coyoteTimer = new Timer(coyoteDuration);

            yield return true;
        }

        #endregion
    }
}
