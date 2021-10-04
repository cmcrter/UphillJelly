////////////////////////////////////////////////////////////
// File: FSMTests.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: The unit testing script for the finite state machine system
//////////////////////////////////////////////////////////// 

using NUnit.Framework;
using SleepyCat.Utility.StateMachine;

namespace SleepyCat.Tests
{
    public class FSMTests
    {
        [Test]
        public void FSMTestsFullLoop()
        {
            //Setting up the states as a simple triangle loop
            RedLight red = new RedLight();
            GreenLight green = new GreenLight(red);
            YellowLight yellow = new YellowLight(green);
            red.nextState = yellow;

            //Creating the state machine
            FiniteStateMachine FSM = new FiniteStateMachine(red);

            //Going through the loop a single time making sure each state is correct along the way
            FSM.RunMachine(0.1f);
            Assert.IsTrue(FSM.currentState == yellow);

            FSM.RunMachine(-0.1f);
            Assert.IsTrue(FSM.currentState == green);

            FSM.RunMachine(0f);
            Assert.IsTrue(FSM.currentState == red);
        }
    }
}
