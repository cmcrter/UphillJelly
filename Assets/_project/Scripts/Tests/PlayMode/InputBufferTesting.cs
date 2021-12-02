//================================================================================================================================================================================================================================================================================================================================================
//  Name:               InputBufferTesting.cs
//  Author:             Matthew Mason
//  Date Created:       09/11/2021
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 12/11/2021
//  Brief:              Class of unit tests for the TrickInputBuffer class
//================================================================================================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SleepyCat.Input;
using SleepyCat.Movement;


namespace SleepyCat.Test
{


    /// <summary>
    /// Class of unit tests for the TrickInputBuffer class
    /// </summary>
    public class InputBufferTesting
    {
        private bool trickPeformedEventReceived;

        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for complete trick input
        /// </summary>
        [UnityTest]
        public IEnumerator TestGettingCopyOfBufferedInput()
        {
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            inputBuffer.MovementController = SetUpFakePlayer();
            inputBuffer.InputStoredDuration = 100f;

            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            // Adding the trick
            testTrick.inputCombo = new List<int>() { 12, 4, 6 };
            // Make the combo happen
            inputBuffer.AddUnHeldInput(5);
            inputBuffer.AddUnHeldInput(12);
            inputBuffer.AddUnHeldInput(4);
            inputBuffer.AddHeldInput(6);

            List<TrickInputBuffer.BufferedInput> bufferedInputsCopy = inputBuffer.GetInputBufferCopy();
            Assert.AreEqual(4, bufferedInputsCopy.Count);
            Assert.AreEqual(5,      bufferedInputsCopy[0].inputID);
            Assert.AreEqual(false,  bufferedInputsCopy[0].isHeld);
            Assert.AreEqual(12,     bufferedInputsCopy[1].inputID);
            Assert.AreEqual(false,  bufferedInputsCopy[1].isHeld);
            Assert.AreEqual(4,      bufferedInputsCopy[2].inputID);
            Assert.AreEqual(false,  bufferedInputsCopy[2].isHeld);
            Assert.AreEqual(6,      bufferedInputsCopy[3].inputID);
            Assert.AreEqual(true,   bufferedInputsCopy[3].isHeld);
            yield return null;
            bufferedInputsCopy = inputBuffer.GetInputBufferCopy();
            Assert.AreEqual(4, bufferedInputsCopy.Count);
            Assert.AreEqual(5, bufferedInputsCopy[0].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[0].isHeld);
            Assert.AreEqual(12, bufferedInputsCopy[1].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[1].isHeld);
            Assert.AreEqual(4, bufferedInputsCopy[2].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[2].isHeld);
            Assert.AreEqual(6, bufferedInputsCopy[3].inputID);
            Assert.AreEqual(true, bufferedInputsCopy[3].isHeld);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(4, bufferedInputsCopy.Count);
            bufferedInputsCopy = inputBuffer.GetInputBufferCopy();
            Assert.AreEqual(5, bufferedInputsCopy[0].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[0].isHeld);
            Assert.AreEqual(12, bufferedInputsCopy[1].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[1].isHeld);
            Assert.AreEqual(4, bufferedInputsCopy[2].inputID);
            Assert.AreEqual(false, bufferedInputsCopy[2].isHeld);
            Assert.AreEqual(6, bufferedInputsCopy[3].inputID);
            Assert.AreEqual(true, bufferedInputsCopy[3].isHeld);
        }

        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for complete trick input
        /// </summary>
        [UnityTest]
        public IEnumerator TrickCheckingSuccessTest()
        {
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            inputBuffer.MovementController = SetUpFakePlayer();
            inputBuffer.InputStoredDuration = 100f;

            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            // Adding the trick
            testTrick.inputCombo = new List<int>() { 12, 4, 6 };
            // Make the combo happen
            inputBuffer.AddUnHeldInput(5);
            inputBuffer.AddUnHeldInput(12);
            inputBuffer.AddUnHeldInput(4);
            inputBuffer.AddHeldInput(6);
            //// Checking if the combo was found
            if (TrickInputBuffer.CheckForTricks(inputBuffer.GetInputBufferCopy(), new List<Trick>(1) {testTrick }, out Trick trickFound))
            {
                Assert.AreEqual(trickFound, testTrick);
                yield break;
            }
            Debug.LogAssertion("Trick was not found");
            Assert.IsTrue(false);
        }

        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for complete trick input
        /// </summary>
        [UnityTest]
        public IEnumerator TrickCheckingSuccessEventTest()
        {
            trickPeformedEventReceived = false;

            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();

            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            testTrick.animationClipToPerform = null;
            testTrick.baseScorePerSecond = 1f;
            testTrick.holdable = false;
            testTrick.inputCombo = new List<int>(3) { 12, 4, 6 };

            PlayerHingeMovementController playerMovementController = SetUpFakePlayer();
            playerMovementController.playerStateMachine.currentState = playerMovementController.aerialState;

            inputBuffer.MovementController = playerMovementController;
            inputBuffer.AerialStateTricks = new List<Trick>() { testTrick };
            inputBuffer.trickPerformed += ReceiveTrickPeformedEvent;

            // Make the combo happen
            inputBuffer.AddUnHeldInput(5);
            inputBuffer.AddUnHeldInput(12);
            inputBuffer.AddUnHeldInput(4);
            inputBuffer.AddUnHeldInput(6);
            ////// Checking if the combo was found
            //if (TrickInputBuffer.CheckForTricks(inputBuffer.GetInputBufferCopy(), inputBuffer.AerialStateTricks, out Trick trickFound))
            //{
            //    Assert.AreEqual(trickFound, testTrick);
            //    yield break;
            //}
            Assert.AreEqual(true, trickPeformedEventReceived);

            inputBuffer.trickPerformed -= ReceiveTrickPeformedEvent;
            yield break;
        }

        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for input that does not make a trick
        /// </summary>
        [UnityTest]
        public IEnumerator TrickCheckingFailTest()
        {
            trickPeformedEventReceived = false;

            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();

            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            testTrick.animationClipToPerform = null;
            testTrick.baseScorePerSecond = 1f;
            testTrick.holdable = false;
            testTrick.inputCombo = new List<int>(3) { 12, 4, 6 };

            PlayerHingeMovementController playerMovementController = SetUpFakePlayer();
            playerMovementController.playerStateMachine.currentState = playerMovementController.aerialState;

            inputBuffer.MovementController = playerMovementController;
            inputBuffer.AerialStateTricks = new List<Trick>() { testTrick };
            inputBuffer.trickPerformed += ReceiveTrickPeformedEvent;

            // Make the combo happen
            inputBuffer.AddUnHeldInput(5);
            inputBuffer.AddUnHeldInput(12);
            inputBuffer.AddUnHeldInput(4);
            inputBuffer.AddUnHeldInput(26);
            Assert.AreEqual(false, trickPeformedEventReceived);

            testTrick.inputCombo = new List<int>(3) { 0, 1 };

            inputBuffer.AddUnHeldInput(1);
            inputBuffer.AddHeldInput(0);
            Assert.AreEqual(false, trickPeformedEventReceived);
            inputBuffer.trickPerformed -= ReceiveTrickPeformedEvent;
            yield break;
        }


        /// <summary>
        /// Testing if AddInput buffers new input correctly
        /// </summary>
        [UnityTest]
        public IEnumerator AddingUnheldInputTest()
        {
            // Use the Assert class to test conditions.
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            inputBuffer.MovementController = SetUpFakePlayer();
            // Adding Input
            int[] addedInput = new int[5] { 421, 14, 1, 4, 41 };
            for (int i = 0; i < addedInput.Length; ++i)
            {
                inputBuffer.AddUnHeldInput(addedInput[i]);
            }
            if (addedInput.Length != inputBuffer.GetInputBufferCopy().Count)
            {
                Assert.IsTrue(false);
            }
            else
            {
                for (int i = 0; i < inputBuffer.GetInputBufferCopy().Count; ++i)
                {
                    Assert.IsTrue(inputBuffer.GetInputBufferCopy()[i].inputID == addedInput[i]);
                    Assert.IsTrue(inputBuffer.GetInputBufferCopy()[i].timeLeft.current_time == inputBuffer.InputStoredDuration);
                }
            }
            yield break;
        }


        /// <summary>
        /// Testing if AddInput buffers new input correctly
        /// </summary>
        [UnityTest]
        public IEnumerator AddingHeldInputTest()
        {
            // Use the Assert class to test conditions.
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            inputBuffer.MovementController = SetUpFakePlayer();
            inputBuffer.InputStoredDuration = 3.0f;
            // Adding Input
            int[] addedInput = new int[5] { 421, 14, 1, 4, 41 };
            bool isHeld = true;
            for (int i = 0; i < addedInput.Length; ++i)
            {
                Debug.Log(isHeld);
                if (isHeld)
                {
                    inputBuffer.AddHeldInput(addedInput[i]);
                }
                else
                {
                    inputBuffer.AddUnHeldInput(addedInput[i]);
                }
                isHeld = !isHeld;
            }

            // Wait for long enough for the input to be cleared
            yield return new WaitForSeconds(inputBuffer.InputStoredDuration + 1f);
            Assert.AreEqual(3, inputBuffer.GetInputBufferCopy().Count);
            Assert.AreEqual(addedInput[0], inputBuffer.GetInputBufferCopy()[0].inputID);
            Assert.AreEqual(addedInput[2], inputBuffer.GetInputBufferCopy()[1].inputID);
            Assert.AreEqual(addedInput[4], inputBuffer.GetInputBufferCopy()[2].inputID);
        }

        /// <summary>
        /// Testing if AddInput buffers new input correctly
        /// </summary>
        [UnityTest]
        public IEnumerator UnHoldingInput()
        {
            // Use the Assert class to test conditions.
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            inputBuffer.InputStoredDuration = 1f;
            inputBuffer.MovementController = SetUpFakePlayer();

            // Adding Input
            inputBuffer.AddUnHeldInput(0);
            inputBuffer.AddHeldInput(12);
            inputBuffer.AddUnHeldInput(4);
            Assert.AreEqual(3, inputBuffer.GetInputBufferCopy().Count);

            // Wait for long enough for the input to be cleared
            yield return new WaitForSeconds(inputBuffer.InputStoredDuration + 1f);

            Assert.AreEqual(1, inputBuffer.GetInputBufferCopy().Count);
            Assert.AreEqual(inputBuffer.GetInputBufferCopy()[0].inputID, 12);
            inputBuffer.FinishHoldingInput(12);

            // Wait for long enough for the input to be cleared
            yield return new WaitForSeconds(inputBuffer.InputStoredDuration);
            Assert.AreEqual(0, inputBuffer.GetInputBufferCopy().Count);

        }

        private void ReceiveTrickPeformedEvent(Trick trick)
        {
            trickPeformedEventReceived = true;
        }

        private PlayerHingeMovementController SetUpFakePlayer()
        {
            GameObject fakePlayerObject = GameObject.Instantiate(Resources.Load<GameObject>("FakePlayer"));

            PlayerHingeMovementController playerMovementController = fakePlayerObject.GetComponentInChildren<PlayerHingeMovementController>();
            return playerMovementController;
        }
    }
}

