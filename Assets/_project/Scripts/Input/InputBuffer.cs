//================================================================================================================================================================================================================================================================================================================================================
//  Name:               InputBuffer.cs
//  Author:             Matthew Mason
//  Date Created:       09/11/2021
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 09/11/2021
//  Brief:              Script used to buffer input as it is performed by the player
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat;
using SleepyCat.Movement;

namespace SleepyCat.Input
{
    public class InputBuffer : MonoBehaviour
    {
        #region Private Structures
        private struct BufferedInput
        {
            public int inputID;
            public Timer timeLeft;

            public BufferedInput(int inputID, Timer timeLeft)
            {
                this.inputID = inputID;
                this.timeLeft = timeLeft;
            }
        }
        #endregion

        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("How long the input are store for before being removed")]
        private int inputStoredDuration;

        [SerializeField]
        [Tooltip("The tricks that can be performed in the Ariel state")]
        private List<Trick> aerialStateTricks;

        [SerializeField]
        [Tooltip("The player to check the state of")]
        private PlayerMovementController movementController;
        #endregion

        #region Private Variables
        /// <summary>
        /// The tricks that each state will use
        /// </summary>
        private Dictionary<Utility.StateMachine.State, List<Trick>> trickForStates;

        /// <summary>
        /// The inputs currently buffered
        /// </summary>
        private List<BufferedInput> bufferedInputs;
        #endregion

        #region Public Delegates
        /// <summary>
        /// Delegate for events called when the next input is ready to be processed
        /// </summary>
        /// <param name="InputID"></param>
        public delegate void NextInputReadyDelegate(int InputID);

        public delegate void TrickInputsPerformedDelegate(Trick performedTrick);
        #endregion

        #region Public Events
        public event NextInputReadyDelegate nextInputReady;

        public event TrickInputsPerformedDelegate trickPerformed;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            trickForStates.Add(movementController.aerialState, aerialStateTricks);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < bufferedInputs.Count; ++i)
            {
                bufferedInputs[i].timeLeft.Tick(Time.deltaTime);
                // Remove any inputs that have timed out
                if (!bufferedInputs[i].timeLeft.isActive)
                {
                    bufferedInputs.RemoveAt(i);
                }
            }
        }
        #endregion

        #region Public Methods
        public void InputProcessed()
        {
            if (bufferedInputs.Count > 0)
            {
                bufferedInputs.RemoveAt(0);
            }
            if (bufferedInputs.Count > 0)
            {
                if (nextInputReady != null)
                {
                    nextInputReady(bufferedInputs[0].inputID);
                }
            }
        }

        /// <summary>
        /// Adds a given input to the buffer
        /// </summary>
        /// <param name="inputID">The id of the input to add</param>
        public void AddInput(int inputID)
        {
            bufferedInputs.Add(new BufferedInput(inputID, new Timer(inputStoredDuration)));
        }
        #endregion

        #region Private Methods
        private void CheckForTricks()
        {
            List<Trick> tricksToCheckFor = new List<Trick>();
            if (movementController.playerStateMachine.currentState == movementController.aerialState)
            {

            }

        }
        #endregion
    }
}


