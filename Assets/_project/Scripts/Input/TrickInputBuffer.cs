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
            /// If the input has yet to have been let go
            /// </summary>
            public bool isHeld;
            /// <summary>
            /// The Id of the input buffered
            /// </summary>
            public int inputID;
            /// <summary>
            /// The amount of time left before the input is discarded
            /// </summary>
            public Timer timeLeft;

            public BufferedInput(bool isHeld, int inputID, Timer timeLeft)
            {
                this.isHeld = isHeld;
                this.inputID = inputID;
                this.timeLeft = timeLeft;
            }
        }

        public Trick activeTrick;
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
        private PlayerHingeMovementController movementController;
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
                Debug.Log("aerialStateTricks is being set");
                aerialStateTricks = value;
                if (movementController != null)
                {
                    Debug.Log("trickForStates is " + trickForStates);
                    if (trickForStates == null)
                    {
                        Debug.Log("trickForStates is made");
                        trickForStates = new Dictionary<Utility.StateMachine.State, List<Trick>>();
                    }
                    trickForStates[movementController.aerialState] = value;
                }
                else
                {
                    Debug.LogWarning("movementController was null when aerial trick were set", this);
                }
            }
        }

        /// <summary>
        /// The player to check the state of
        /// </summary>
        public PlayerHingeMovementController MovementController
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
        /// Delegate for events called when a trick is involved
        /// </summary>
        /// <param name="performedTrick">The trick that the event relates too</param>
        public delegate void TrickInputsDelegate(Trick affectingTrick);
        #endregion

        #region Public Events
        /// <summary>
        /// Event called when a trick has been detected to have been performed by the user
        /// </summary>
        public event TrickInputsDelegate trickPerformed;

        /// <summary>
        /// Event called when a trick has been detected to have been performed by the user
        /// </summary>
        public event TrickInputsDelegate trickStarted;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        private void Start()
        {
            if (bufferedInputs == null)
            {
                bufferedInputs = new List<BufferedInput>();
            }
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
        }

        // Update is called once per frame
        private void Update()
        {
            for (int i = 0; i < bufferedInputs.Count; ++i)
            {
                if (!bufferedInputs[i].isHeld)
                {
                    bufferedInputs[i].timeLeft.Tick(Time.deltaTime);
                }
                // Remove any inputs that have timed out
                if (!bufferedInputs[i].timeLeft.isActive)
                {
                    Debug.Log("Removing buffered input: " + bufferedInputs[i]);
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
        public static bool CheckForTricks(List<BufferedInput> currentlyBufferedInputs, List<Trick> tricksToCheckFor, out Trick tricksFound)
        {
            // Iterate through all the tricks checked against
            Debug.Log("TrickChecked Here 0");
            for (int trickIndex = 0; trickIndex < tricksToCheckFor.Count; ++trickIndex)
            {
                Debug.Log("TrickChecked Here 1");
                if (currentlyBufferedInputs.Count <= 0)
                {
                    Debug.LogWarning("Check for tricks was called when the input buffer was empty");
                    tricksFound = null;
                    return false;
                }
                if (currentlyBufferedInputs[currentlyBufferedInputs.Count - 1].inputID == tricksToCheckFor[trickIndex].inputCombo[tricksToCheckFor[trickIndex].inputCombo.Count - 1])
                {
                    Debug.Log("TrickChecked Here 2");
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


        public void AddHeldInput(int inputID)
        {
            // Add the input
            if (bufferedInputs == null)
            {
                bufferedInputs = new List<BufferedInput>();
            }
            bufferedInputs.Add(new BufferedInput(true, inputID, new Timer(inputStoredDuration)));
            Debug.Log("Happend 0");

            // Check the input against the tricks
            if (movementController != null)
            {
                Debug.Log("Happend 1");
                if (trickForStates == null)
                {
                    trickForStates = new Dictionary<Utility.StateMachine.State, List<Trick>>();
                }
                if (trickForStates.TryGetValue(movementController.playerStateMachine.currentState, out List<Trick> validTricks))
                {
                    Debug.Log("Happend 2");
                    if (CheckForTricks(bufferedInputs, validTricks, out Trick trickFound))
                    {
                        if (trickFound.holdable)
                        {
                            if (trickStarted != null)
                            {
                                trickStarted(trickFound);
                            }
                            activeTrick = trickFound;
                        }
                        else
                        {
                            if (trickPerformed != null)
                            {
                                trickPerformed(trickFound);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("movementController was null when reference in ");
            }
        }

        public void FinishHoldingInput(int inputID)
        {
            // Search for the input being held
            for (int i = 0; i < bufferedInputs.Count; ++i)
            {
                if (bufferedInputs[i].inputID == inputID)
                {
                    if (bufferedInputs[i].isHeld)
                    {
                        bufferedInputs[i] = new BufferedInput(false, inputID, new Timer(inputStoredDuration));
                        
                        // If the active trick was just released then broadcast it as being performed
                        if (activeTrick != null)
                        {
                            if (activeTrick.inputCombo[activeTrick.inputCombo.Count - 1] == inputID)
                            {
                                trickPerformed(activeTrick);
                                activeTrick = null;
                            }
                        }
                        return;
                    }
                }
            }
            Debug.LogWarning("No input matching inputID was being held", this);
        }

        /// <summary>
        /// Adds a given input to the buffer
        /// </summary>
        /// <param name="inputID">The id of the input to add</param>
        public void AddUnHeldInput(int inputID)
        {
            // Add the input
            if (bufferedInputs == null)
            {
                bufferedInputs = new List<BufferedInput>();
            }
            bufferedInputs.Add(new BufferedInput(false, inputID, new Timer(inputStoredDuration)));
            Debug.Log("Happend 0");
            
            // Check the input against the tricks
            if (movementController != null)
            {
                Debug.Log("Happend 1");
                if (trickForStates == null)
                {
                    trickForStates = new Dictionary<Utility.StateMachine.State, List<Trick>>();
                }
                if (trickForStates.TryGetValue(movementController.playerStateMachine.currentState, out List<Trick> validTricks))
                {
                    Debug.Log("Happend 2");
                    if (CheckForTricks(bufferedInputs, validTricks, out Trick trickFound))
                    {
                        Debug.Log("Happend 3");
                        Debug.Log(trickPerformed != null);
                        if (trickPerformed != null)
                        {
                            Debug.Log("Happend 4");
                            Debug.Log("Trick Event Called");
                            trickPerformed(trickFound);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("movementController was null when reference in AddUnHeldInput of trick input buffer", this);
            }
        }
        #endregion
    }
}



