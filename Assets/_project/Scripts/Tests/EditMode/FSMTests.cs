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
            RedLight red = new RedLight();
            FiniteStateMachine FSM = new FiniteStateMachine(red);

            FSM.RunMachine(0.1f);

            Assert.IsTrue(FSM.currentState.GetType() == typeof(YellowLight));

            FSM.RunMachine(-0.1f);

            Assert.IsTrue(FSM.currentState.GetType() == typeof(GreenLight));

            FSM.RunMachine(0f);

            Assert.IsTrue(FSM.currentState.GetType() == typeof(RedLight));
        }
    }
}
