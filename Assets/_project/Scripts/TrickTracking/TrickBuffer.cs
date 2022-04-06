using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games;
using L7Games.Input;
using L7Games.Movement;
using L7Games.Tricks;
using UnityEngine.UI;

namespace L7Games.Tricks
{
    public class TrickBuffer : MonoBehaviour
    {
        #region Private Structures
        /// <summary>
        /// Structure for storing information on a scoreable action while it is still in progress
        /// </summary>
        private struct ScorableActionInProgress
        {
            /// <summary>
            /// The time in which the action was started
            /// </summary>
            public float startTime;
            /// <summary>
            /// The ID given the action was first started 
            /// </summary>
            public int ID;
            /// <summary>
            /// The ScoreableAction that is in progress
            /// </summary>
            public ScoreableAction scoreableActionData;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="startTime">The time in which the action was started</param>
            /// <param name="iD">The ID given the action was first started </param>
            /// <param name="scoreableActionData">The ScoreableAction that is in progress</param>
            public ScorableActionInProgress(float startTime, int iD, ScoreableAction scoreableActionData)
            {
                this.startTime = startTime;
                ID = iD;
                this.scoreableActionData = scoreableActionData;
            }
        }
        #endregion

        #region Public Properties
        public bool IsTricking
        {
            get
            {
                return trickPlaying != null;
            }
        }

        public bool WithinInWipeOutTheshold
        {
            get
            {
                return playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f;
            }
        }
        #endregion

        #region Private Serialized Fields
        [SerializeField]
        [Tooltip("The animator used by the player model")]
        private Animator playerAnimator;

        [SerializeField]
        private float ComboTimeOutDuration;

        [SerializeField]
        [Tooltip("How long an input is buffered for before being discarded")]
        private float inputValidityDuration;

        [SerializeField]
        [Tooltip("The input handler to receive processed input from")]
        private InputHandler inputHandler;

        [SerializeField]
        [Tooltip("The PlayerAnimationEventHandler for the player this is also attached to")]
        private PlayerAnimationEventHandler playerAnimationEventHandler;

        [SerializeField]
        [Tooltip("The tricks that are possible to be performed")]
        private Trick[] aerialTricks;

        [SerializeField]
        [Tooltip("The player controller this is attached too")]
        private PlayerHingeMovementController playerHingeMovementController;

        [SerializeField]
        private float scoreMultiplierMultiplier;
        #endregion

        #region Private Variables
        /// <summary>
        /// If a trick is wanted to be performed by the user
        /// </summary>
        private bool trickWanted;

        /// <summary>
        /// If trick's animation is currently playing
        /// </summary>
        private Trick trickPlaying;

        /// <summary>
        /// The timer for when the trick requests will time out
        /// </summary>
        private Timer timeTillTrickWantedTimeOut;

        /// <summary>
        /// Combo time out timer
        /// </summary>
        private Timer comboTimeOutTimer;

        public Text text;


        /// <summary>
        /// The Id for the last state the animator was in before a trick was started
        /// </summary>
        private int previousAnimationStateHash;
        /// <summary>
        /// The current ID to give to new score-able actions
        /// </summary>
        private int currentID = 0;

        /// <summary>
        /// The score-able actions that have been started but not finished
        /// </summary>
        private List<ScorableActionInProgress> scoreableActionsInProgress;
        /// <summary>
        /// The score-able actions that have been complete and how long they were performed for
        /// </summary>
        private List<KeyValuePair<ScoreableAction, float>> scoreableActionsCompleted;
        #endregion

        #region Public Properties
        public float ComboMultiplier
        {
            get
            {
                return (1f + (((float)scoreableActionsCompleted.Count + (float)scoreableActionsInProgress.Count - 1f) * scoreMultiplierMultiplier));
            }
        }
        #endregion

        #region Public Events
        public event System.Action<ScoreableAction> ActionCompleted;
        public event System.Action<ScoreableAction> ActionStarted;
        public event System.Action ComboCompleted;
        public event System.Action ComboBroken;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            timeTillTrickWantedTimeOut = new Timer(0f);
            scoreableActionsInProgress = new List<ScorableActionInProgress>();
            scoreableActionsCompleted = new List<KeyValuePair<ScoreableAction, float>>();
        }

        private void Start()
        {
            comboTimeOutTimer = new Timer(ComboTimeOutDuration);
        }

        private void OnDisable()
        {
            if (inputHandler != null)
            {
                playerAnimationEventHandler.OnTrickAnimationEnded -= PlayerAnimationEventHandler_OnTrickAnimationEnded;
                inputHandler.trickPressed -= InputHandler_TrickInputActionCalled;
                playerHingeMovementController.onWipeout -= PlayerHingeMovementController_onWipeout;
            }
        }
        private void OnEnable()
        {
            if (inputHandler != null)
            {
                playerAnimationEventHandler.OnTrickAnimationEnded += PlayerAnimationEventHandler_OnTrickAnimationEnded;
                inputHandler.trickPressed += InputHandler_TrickInputActionCalled;
                playerHingeMovementController.onWipeout += PlayerHingeMovementController_onWipeout;
            }
        }


        private void Update()
        {
            timeTillTrickWantedTimeOut.Tick(Time.deltaTime);
            if (!timeTillTrickWantedTimeOut.isActive)
            {
                trickWanted = false;
            }


            // If a trick is wanted to be played and another is not already playing then it should play a trick
            if (trickPlaying == null)
            {
                if (trickWanted)
                {
                    StartAerialTrick();
                }
            }


            // If the player is grounded then the countdown must be active for how much trick time is left
            if (playerHingeMovementController.groundedState.hasRan)
            {
                if (ComboTimeOutDuration <= 0f)
                {
                    if (ComboCompleted != null)
                    {
                        ComboCompleted();
                    }
                    ClearBuffer();
                }
                else
                {
                    comboTimeOutTimer.Tick(Time.deltaTime);
                    if (!comboTimeOutTimer.isActive)
                    {
                        if (ComboCompleted != null)
                        {
                            ComboCompleted();
                        }
                        ClearBuffer();
                    }
                }
            }
            else
            {
                if (ComboTimeOutDuration > 0f)
                {
                    if (comboTimeOutTimer.current_time != ComboTimeOutDuration)
                    {
                        comboTimeOutTimer = new Timer(ComboTimeOutDuration);
                    }
                }
            }

            //text.text = GetComboText();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Forcibly stop the buffer and clear everything
        /// </summary>
        public void ClearTricks()
        {
            ClearBuffer();
            trickPlaying = null;
        }

        /// <summary>
        /// Returns the score value of the current combo
        /// </summary>
        /// <returns>The score value of the current combo</returns>
        public float GetScoreFromCurrentCombo()
        {
            float baseScore = 0f;
            for (int i = 0; i < scoreableActionsCompleted.Count; ++i)
            {
                if (scoreableActionsCompleted[i].Key == null)
                {
                    Debug.Log("Here");
                }
                baseScore += scoreableActionsCompleted[i].Key.initalScoreValue;
                baseScore += scoreableActionsCompleted[i].Key.scorePerSecond * scoreableActionsCompleted[i].Value;
            }
            for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
            {
                baseScore += scoreableActionsInProgress[i].scoreableActionData.initalScoreValue;
                baseScore += scoreableActionsInProgress[i].scoreableActionData.scorePerSecond * (Time.time - scoreableActionsInProgress[i].startTime);
            }
            return baseScore * ComboMultiplier;
        }

        /// <summary>
        /// Adds a Scoreable action to the list of ones in progress
        /// </summary>
        /// <param name="scoreableActionData">The data on the scoreable action to add</param>
        /// <returns>An id for the action in progress that was just added, used to access it later externally</returns>
        public int AddScoreableActionInProgress(ScoreableAction scoreableActionData)
        {
            // Add the new scoreable action then increment the current Id for the next usage
            scoreableActionsInProgress.Add(new ScorableActionInProgress(Time.time, currentID, scoreableActionData));

            if (ActionStarted != null)
            {
                ActionStarted(scoreableActionData);
            }



            return currentID++;
        }

        public string GetComboText()
        {
            string comboText = GetScoreFromCurrentCombo().ToString() + " X " + ComboMultiplier.ToString() + '\n';
            for (int i = 0; i < scoreableActionsCompleted.Count; ++i)
            {
                comboText += scoreableActionsCompleted[i].Key.trickName;
                comboText += " + ";
            }
            for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
            {
                comboText += scoreableActionsInProgress[i].scoreableActionData.trickName;
                comboText += " + ";
            }
            return comboText.Trim(new char[] { ' ', '+' });
        }

        /// <summary>
        /// Called to add a scoreable action that has no duration but has been completed (air tricks being an example of this
        /// </summary>
        /// <param name="scoreableActionData">The action to be added</param>
        public void AddOneShotCompletedAction(ScoreableAction scoreableActionData)
        {
            scoreableActionsCompleted.Add(new KeyValuePair<ScoreableAction, float>(scoreableActionData, 0f));

            if (ActionCompleted != null)
            {
                ActionCompleted(scoreableActionData);
            }

            if (scoreableActionsCompleted[scoreableActionsCompleted.Count - 1].Key == null)
            {
                Debug.Log("Here");
            }
        }
        /// <summary>
        /// Called to mark a scoreable action in progress as finished
        /// </summary>
        /// <param name="actionInProgressID">The Id of the action to finish, should have been given when the score was first added</param>
        public void FinishScorableActionInProgress(int actionInProgressID)
        {
            // Find the action in progress that matches the id
            for (int i = 0; i < scoreableActionsInProgress.Count; ++i)
            {
                if (scoreableActionsInProgress[i].ID == actionInProgressID)
                {
                    ScorableActionInProgress actionFinished = scoreableActionsInProgress[i];
                    scoreableActionsInProgress.RemoveAt(i);

                    scoreableActionsCompleted.Add(new KeyValuePair<ScoreableAction, float>(actionFinished.scoreableActionData, Time.time - actionFinished.startTime));
                    if (scoreableActionsCompleted[scoreableActionsCompleted.Count - 1].Key == null)
                    {
                        Debug.Log("Here");
                    }
                    if (ActionCompleted != null)
                    {
                        ActionCompleted(actionFinished.scoreableActionData);
                    }

                    return;
                }
            }
        }
        #endregion

        #region Private Methods
        private void ClearBuffer()
        {
            scoreableActionsInProgress.Clear();
            scoreableActionsCompleted.Clear();
        }

        private void InputHandler_TrickInputActionCalled()
        {
            timeTillTrickWantedTimeOut = new Timer(inputValidityDuration);
            trickWanted = true;
        }

        private void StartAerialTrick()
        {
            previousAnimationStateHash = playerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            // Pick a random aerial trick to play 
            Trick aerialTrick = aerialTricks[Random.Range(0, aerialTricks.Length)];

            playerAnimator.Play(aerialTrick.TrickAnimStateName);

            trickWanted = false;
            trickPlaying = aerialTrick;
        }

        private void EndAerialTrick()
        {
            //Guard clause to make sure there's no issues
            if(trickPlaying == null) return;

            AddOneShotCompletedAction(trickPlaying.scoreableDetails);
            trickPlaying = null;
        }

        private void PlayerAnimationEventHandler_OnTrickAnimationEnded()
        {
            EndAerialTrick();
            // also start a new one if an input has been buffered
            if (trickWanted)
            {
                StartAerialTrick();
            }
        }

        private void PlayerHingeMovementController_onWipeout(Vector3 obj)
        {
            // Break combo
            if (scoreableActionsInProgress.Count > 0 || scoreableActionsCompleted.Count > 0)
            {
                if (ComboBroken != null)
                {
                    ComboBroken();
                }
                ClearBuffer();
            }
            // End current trick if there is one going 
            trickPlaying = null;
        }
        #endregion
    }
}
