////////////////////////////////////////////////////////////
// File: FiniteStateMachine.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: The class which manages the running of states
//////////////////////////////////////////////////////////// 

namespace SleepyCat.Utility.StateMachine
{
    public class FiniteStateMachine
    {
        #region Public Fields

        public State currentState;

        #endregion

        #region Public Methods

        /// <summary>
        /// The constructors for the finite state machine
        /// </summary>
        public FiniteStateMachine()
        {
            //Waiting for a given current state
        }

        public FiniteStateMachine(State startingState)
        {
            //Given a state to start in
            currentState = startingState;
        }

        //The main function that determines what state is currently ran
        public void RunMachine(float dT)
        {
            if (currentState == null)
            {
                return;
            }

            //Tick the current state along
            currentState.Tick(dT);

            //Get the state that it should be in based on the current state
            State thisState = currentState.returnCurrentState();

            //If it has to change state to a new one
            if (!thisState.Equals(currentState))
            {
                //Exit the current state and start the next one
                currentState.OnStateExit();
                currentState = thisState;
                currentState.OnStateEnter();
            }
        }

        //This will run every fixed update
        public void RunPhysicsOnMachine(float dT)
        {
            if(currentState == null)
            {
                return;
            }

            //Tick the current state along
            currentState.PhysicsTick(dT);
        }

        //In-case a state needs to be forced to be on
        public void ForceSwitchToState(State newState)
        {
            currentState = newState;
        }

        #endregion
    }
}
