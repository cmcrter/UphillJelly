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


namespace SleepyCat.Test
{
    /// <summary>
    /// Class of unit tests for the TrickInputBuffer class
    /// </summary>
    public class InputBufferTesting
    {
        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for complete trick input
        /// </summary>
        [UnityTest]
        public IEnumerator TrickCheckingSuccessTest()
        {
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            yield return null;
            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            // Adding the trick
            testTrick.inputCombo = new List<int>() { 12, 4, 6 };
            inputBuffer.AerialStateTricks = new List<Trick>() { testTrick };
            // Make the combo happen
            inputBuffer.AddInput(5);
            inputBuffer.AddInput(12);
            inputBuffer.AddInput(4);
            inputBuffer.AddInput(6);
            //// Checking if the combo was found
            if (inputBuffer.CheckForTricks(inputBuffer.GetInputBufferCopy(), inputBuffer.AerialStateTricks, out Trick trickFound))
            {
                Assert.AreEqual(trickFound, testTrick);
                yield break;
            }
            Assert.IsTrue(false);
        }

        /// <summary>
        /// Testing if the CheckForTricks function will accept correctly for input that does not make a trick
        /// </summary>
        [UnityTest]
        public IEnumerator TrickCheckingFailTest()
        {
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            yield return null;
            Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            // Adding the trick
            testTrick.inputCombo = new List<int>() { 12, 4, 18 };
            inputBuffer.AerialStateTricks = new List<Trick>() { testTrick };
            // Make the combo happen
            inputBuffer.AddInput(5);
            inputBuffer.AddInput(12);
            inputBuffer.AddInput(4);
            inputBuffer.AddInput(6);
            //// Checking if the combo was found
            if (inputBuffer.CheckForTricks(inputBuffer.GetInputBufferCopy(), inputBuffer.AerialStateTricks, out Trick trickFound))
            {
                // No trick should be found
                Assert.IsTrue(false);
                yield break;
            }
        }


        /// <summary>
        /// Testing if AddInput buffers new input correctly
        /// </summary>
        [UnityTest]
        public IEnumerator AddingInputTest()
        {
            // Use the Assert class to test conditions.
            // Create new game object with an input buffer
            GameObject newGameObject = new GameObject();
            TrickInputBuffer inputBuffer = newGameObject.AddComponent<TrickInputBuffer>();
            yield return null;
            // Adding Input
            int[] addedInput = new int[5] { 421, 14, 1, 4, 41 };
            for (int i = 0; i < addedInput.Length; ++i)
            {
                inputBuffer.AddInput(addedInput[i]);
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
        }
    }
}

