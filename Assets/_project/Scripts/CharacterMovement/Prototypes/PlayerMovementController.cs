////////////////////////////////////////////////////////////
// File: PlayerMovementController
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/11/21
// Brief: The script which will determine the player's movement for a release build
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;
using L7Games.Triggerables;
using System.Collections;

namespace L7Games.Movement
{
    public class PlayerMovementController : PlayerController
    {
        #region Variables

        [Header("State Machine")]
        public FiniteStateMachine playerStateMachine;

        public AerialState aerialState = new AerialState();
        public GroundedState groundedState = new GroundedState();
        public WallRideState wallRideState = new WallRideState();
        public GrindedState grindingState = new GrindedState();

        public isGroundBelow groundBelow = new isGroundBelow();
        public isNextToWallRun nextToWallRun = new isNextToWallRun();
        public isOnGrind grindBelow = new isOnGrind();

        public PlayerCamera playerCamera;
        public float currentTurnInput;
        [Tooltip("The time before the player is at full turn")]
        public float turnDuration;        

        [Header("Player Setup")]
        [SerializeField]
        private GameObject playerModel;
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private Transform groundRaycastPoint;
        [SerializeField]
        private Transform backgroundRaycastPoint;

        public SphereCollider ballMovement;
        public PlayerInput input;
        public InputHandler inputHandler;

        [SerializeField]
        private float AdditionalGravityAmount = 8;
        public bool bAddAdditionalGravity = true;

        private Vector3 initalPos;
        private Quaternion initialRot;
        private Quaternion lastRot = Quaternion.identity;
        private Coroutine turningCo;
        private Timer turningTimer;
        public AnimationCurve turnSpeedCurve;
        public float turnClamp = 0.55f;
        #endregion

        #region Public Methods

        public override void ResetPlayer()
        {
            rb.isKinematic = false;

            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            rb.transform.rotation = initialRot;
            transform.rotation = initialRot;

            rb.transform.position = initalPos;
        }

        //Both grounded and aerial wants to have the model smooth towards what's below them to a degree
        public void SmoothToGroundRotation(bool bAerial, float smoothness, float speed, isGroundBelow groundBelow)
        {
            float headingDeltaAngle;
            Quaternion headingDelta = Quaternion.identity;

            if(!bAerial)
            {
                //GameObject's heading based on the current input
                headingDeltaAngle = speed * 1000 * currentTurnInput * Time.deltaTime;
                headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
            }

            Quaternion groundQuat = transform.rotation;

            if(groundBelow.GroundHit.collider)
            {
                groundQuat = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, groundBelow.GroundHit.normal), groundBelow.GroundHit.normal), Time.deltaTime * smoothness);
            }

            if(Vector3.Angle(groundBelow.GroundHit.normal, transform.up) < 70f)
            {
                transform.rotation = groundQuat;
            }

            lastRot = groundQuat;            
            transform.rotation = transform.rotation * headingDelta;

            //With the hinge, this means that the rb wont just run away without the player
            if(Vector3.Distance(transform.position, rb.transform.position) > 0.15f)
            {
                transform.position = rb.transform.position;
            }
        }

        public override void AddWallRide(WallRideTriggerable wallRide)
        {
            nextToWallRun.CheckWall(wallRide);
        }

        public override void RemoveWallRide(WallRideTriggerable wallRide)
        {
            nextToWallRun.LeftWall(wallRide);
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Setting up the state machine
            groundBelow.InitialiseCondition(transform);
            nextToWallRun.InitialiseCondition(this, rb);
            grindBelow.InitialiseCondition(rb, inputHandler);

            groundedState.InitialiseState(this, rb, groundBelow, grindBelow);
            aerialState.InitialiseState(this, rb, groundBelow, nextToWallRun, grindBelow);
            wallRideState.InitialiseState(this, rb, nextToWallRun, groundBelow);
            grindingState.InitialiseState(this, rb, grindBelow);

            playerStateMachine = new FiniteStateMachine(aerialState);
        }

        //Adding the inputs to the finite state machine
        private void OnEnable()
        {
            groundedState.RegisterInputs();
            grindingState.RegisterInputs();
        }

        private void OnDisable()
        {
            groundedState.UnRegisterInputs();
            grindingState.UnRegisterInputs();
        }

        private void Start()
        {
            initalPos = transform.position;
            initialRot = transform.rotation;
            rb.transform.parent = null;

            //Setting up model position
            playerModel.transform.position = new Vector3(ballMovement.transform.position.x, (ballMovement.transform.position.y - (ballMovement.radius * ballMovement.transform.localScale.y) + 0.0275f), ballMovement.transform.position.z);
        }

        private void Update()
        {
            playerStateMachine.RunMachine(Time.deltaTime);

            if (Keyboard.current != null)
            {
                if (Keyboard.current.escapeKey.isPressed)
                {
                    ResetPlayer();
                }
            }

            if(inputHandler.TurningAxis != 0 && turningCo == null)
            {
                turningCo = StartCoroutine(Co_TurnAngle());
            }
        }

        private void FixedUpdate()
        {
            playerStateMachine.RunPhysicsOnMachine(Time.deltaTime);

            if(bAddAdditionalGravity)
            {
                rb.AddForce(Vector3.down * AdditionalGravityAmount, ForceMode.Acceleration);
            }
        }
        #endregion

        #region Public Methods
        public override void MoveToPosition(Vector3 positionToMoveTo)
        {
            ballMovement.transform.position = positionToMoveTo;
        }

        public void StartTurnCoroutine()
        {
            StopTurnCoroutine();
            turningCo = StartCoroutine(Co_TurnAngle());
        }

        public void StopTurnCoroutine()
        {
            if(turningCo != null) 
            {
                StopCoroutine(Co_TurnAngle());
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_TurnAngle()
        {
            turningTimer = new Timer(turnDuration);
            currentTurnInput = 0;

            //Whilst the player wants to turn
            while(inputHandler.TurningAxis != 0)
            {
                //How far along into the timer is this
                float timeAlongTimer = turnDuration / turningTimer.current_time;
                timeAlongTimer = Mathf.Clamp(timeAlongTimer, 0f, 0.99f);

                float clampedMoveDelta = Mathf.Clamp(inputHandler.TurningAxis, -turnClamp, turnClamp);

                //If the skateboard is moving
                if(rb.velocity.magnitude > 0.3f && playerStateMachine.currentState == groundedState)
                {
                    //Lerping towards the new input by the animation curves amounts (probably increasing over time)
                    currentTurnInput = Mathf.Lerp(currentTurnInput, clampedMoveDelta, 2f * turnSpeedCurve.Evaluate(timeAlongTimer) * Time.deltaTime);
                }
                //Else do a kick turn
                else
                {
                    if(inputHandler.TurningAxis < 0)
                    {
                        currentTurnInput = -1f;
                    }
                    else
                    {
                        currentTurnInput = 1f;
                    }
                }

                Mathf.Clamp(currentTurnInput, -1f, 1f);

                if (!turningTimer.isActive)
                {
                    yield return null;
                }

                //Ticking the timer along
                turningTimer.Tick(Time.deltaTime);
                yield return null;
            }

            currentTurnInput = 0;
            turningCo = null;
        }

        #endregion
    }
}
