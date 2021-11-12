//================================================================================================================================================================================================================================================================================================================================================
//  Name:               TrickInputBuffer.cs
//  Author:             Matthew Mason
//  Date Created:       09/11/2021
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 12/11/2021
//  Brief:              Script used to buffer input as it is performed by the player to detect when they have performed a trick
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat;
using SleepyCat.Movement;

namespace SleepyCat.Input
{
    /// <summary>
    /// Script used to buffer input as it is performed by the player to detect when they have performed a trick
    /// </summary>
    public class TrickInputBuffer : MonoBehaviour
    {
        #region Public Structures
        /// <summary>
        /// Used to store data on a single buffered input
        /// </summary>
        public struct BufferedInput
        {
            /// <summary>
            /// The Id of the input buffered
            /// </summary>
            public int inputID;
            /// <summary>
            /// The amount of time left before the input is discarded
            /// </summary>
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
        private float inputStoredDuration = 0.16f;

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

        #region Public Properties
        /// <summary>
        /// How long the input are store for before being removed
        /// </summary>
        public float InputStoredDuration
        {
            get
            {
                return inputStoredDuration;
            }
            set
            {
                inputStoredDuration = value;
            }
        }

        /// <summary>
        /// The tricks that can be performed in the Ariel state
        /// </summary>
        public List<Trick> AerialStateTricks
        {
            get
            {
                return aerialStateTricks;
            }
            set
            {
                aerialStateTricks = value;
                if (movementController != null)
                {
                    trickForStates[movementController.aerialState] = aerialStateTricks;
                }
            }
        }

        /// <summary>
        /// The player to check the state of
        /// </summary>
        public PlayerMovementController MovementController
        {
            get
            {
                return movementController;
            }
            set
            {
                movementController = value;
            }
        }
        #endregion

        #region Public Delegates
        /// <summary>
        /// Delegate for events called when a trick has been detected to have been performed by the user
        /// </summary>
        /// <param name="performedTrick">The trick that was just detected as being performed</param>
        public delegate void TrickInputsPerformedDelegate(Trick performedTrick);
        #endregion

        #region Public Events
        /// <summary>
        /// Event called when a trick has been detected to have been performed by the user
        /// </summary>
        public event TrickInputsPerformedDelegate trickPerformed;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        private void Start()
        {
            bufferedInputs = new List<BufferedInput>();
            if (aerialStateTricks == null)
            {
                aerialStateTricks = new List<Trick>();
            }
            if (trickForStates == null)
            {
                trickForStates = new Dictionary<Utility.StateMachine.State, List<Trick>>();
            }
            if (movementController != null)
            {
                trickForStates[movementController.aerialState] = aerialStateTricks;
            }
            //// Adding the trick
            //Trick testTrick = ScriptableObject.CreateInstance<Trick>();
            //testTrick.inputCombo = new List<int>() { 12, 4, 6 };
            //AerialStateTricks = new List<Trick>() { testTrick };
            //// Make the combo happen
            //AddInput(5);
            //AddInput(12);
            //AddInput(4);
            //AddInput(6);
            ////// Checking if the combo was found
            //if (CheckForTricks(GetInputBufferCopy(), AerialStateTricks, out Trick trickFound))
            //{
            //    int i = 0;
            //}
        }

        // Update is called once per frame
        private void Update()
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

        /// <summary>
        /// Called to check a given input buffer list for a given selection of trick
        /// </summary>
        /// <param name="currentlyBufferedInputs">The current inputs that have been buffered to check</param>
        /// <param name="tricksToCheckFor">The tricks to check against the inputs in the buffer for</param>
        /// <param name="tricksFound">The trick that was found in the buffer (null if one was not found)</param>
        /// <returns>True if a trick was found</returns>
        public bool CheckForTricks(List<BufferedInput> currentlyBufferedInputs, List<Trick> tricksToCheckFor, out Trick tricksFound)
        {
            // Iterate through all the tricks checked against
            for (int trickIndex = 0; trickIndex < tricksToCheckFor.Count; ++trickIndex)
            {
                if (currentlyBufferedInputs[currentlyBufferedInputs.Count - 1].inputID == tricksToCheckFor[trickIndex].inputCombo[tricksToCheckFor[trickIndex].inputCombo.Count - 1])
                {
                    // Loop forward from the matching ID to see if it matches the combo
                    bool matchingCombo = true;
                    for (int subsectionIndex = 0; subsectionIndex < tricksToCheckFor[trickIndex].inputCombo.Count; ++subsectionIndex)
                    {
                        if (subsectionIndex >= currentlyBufferedInputs.Count)
                        {
                            matchingCombo = false;
                            break;
                        }
                        if (currentlyBufferedInputs[currentlyBufferedInputs.Count - 1 - subsectionIndex].inputID != tricksToCheckFor[trickIndex].inputCombo[tricksToCheckFor[trickIndex].inputCombo.Count - 1 - subsectionIndex])
                        {
                            matchingCombo = false;
                            break;
                        }
                    }
                    if (matchingCombo)
                    {
                        tricksFound = tricksToCheckFor[trickIndex];
                        return true;
                    }
                }
            }
            tricksFound = null;
            return false;
        }

        /// <summary>
        /// Return a new list containing value equal to the input buffer
        /// </summary>
        /// <returns>A new list containing value equal to the input buffer</returns>
        public List<BufferedInput> GetInputBufferCopy()
        {
            return new List<BufferedInput>(bufferedInputs);
        }

        /// <summary>
        /// Adds a given input to the buffer
        /// </summary>
        /// <param name="inputID">The id of the input to add</param>
        public void AddInput(int inputID)
        {
            bufferedInputs.Add(new BufferedInput(inputID, new Timer(inputStoredDuration)));
            if (movementController != null)
            {
                if (trickForStates.TryGetValue(movementController.playerStateMachine.currentState, out List<Trick> validTricks))
                {
                    if (CheckForTricks(bufferedInputs, validTricks, out Trick trickFound))
                    {
                        if (trickPerformed != null)
                        {
                            trickPerformed(trickFound);
                        }
                    }
                }
            }
        }
        #endregion
    }
}



