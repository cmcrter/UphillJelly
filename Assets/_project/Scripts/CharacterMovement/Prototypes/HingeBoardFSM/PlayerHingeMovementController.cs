////////////////////////////////////////////////////////////
// File: PlayerHingeMovementController
// Author: Charles Carter
// Date Created: 29/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 29/11/21
// Brief: A prototype character controller using a board made of 2 RBs with 2 Joints used
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;
using SleepyCat.Utility.StateMachine;
using SleepyCat.Input;
using SleepyCat.Triggerables;
using System.Collections;

namespace SleepyCat.Movement
{
    public class PlayerHingeMovementController : PlayerController
    {
        #region Variables

        [Header("State Machine")]
        public FiniteStateMachine playerStateMachine;

        public HAerialState aerialState = new HAerialState();
        public HGroundedState groundedState = new HGroundedState();
        public HWallRideState wallRideState = new HWallRideState();
        public HGrindedState grindingState = new HGrindedState();

        public HisGroundBelow groundBelow = new HisGroundBelow();
        public HisNextToWallRun nextToWallRun = new HisNextToWallRun();
        public HisOnGrind grindBelow = new HisOnGrind();

        public PlayerCamera playerCamera;
        public float currentTurnInput;
        [Tooltip("The time before the player is at full turn")]
        public float turnDuration;        

        [Header("Player Setup")]
        [SerializeField]
        private GameObject playerModel;

        [SerializeField]
        public GameObject boardObject;
        //Front Rigidbody
        [SerializeField]
        private Rigidbody fRB;
        //Back Rigidbody
        [SerializeField]
        private Rigidbody bRB;

        public SphereCollider ballMovement;
        public PlayerInput input;
        public InputHandler inputHandler;

        [SerializeField]
        private Transform frontWheelPos;
        [SerializeField]
        private Transform backWheelPos;

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
            fRB.isKinematic = false;

            fRB.angularVelocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            boardObject.transform.rotation = initialRot;
            transform.rotation = initialRot;

            fRB.transform.position = initalPos;
            transform.position = initalPos;
        }

        public override void ResetPlayer(Transform point)
        {
            Time.timeScale = 0;

            fRB.isKinematic = false;

            fRB.angularVelocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            bRB.isKinematic = false;

            bRB.angularVelocity = Vector3.zero;
            bRB.velocity = Vector3.zero;

            transform.rotation = point.rotation;
            transform.position = point.position;

            ResetWheelPos();
            AlignWheels();

            Time.timeScale = 1;
        }

        //Both grounded and aerial wants to have the model smooth towards what's below them to a degree
        public void SmoothToGroundRotation(bool bAerial, float smoothness, float speed, HisGroundBelow groundBelow)
        {
            float headingDeltaAngle = 0;
            Quaternion headingDelta = Quaternion.identity;
            Quaternion groundQuat = transform.rotation;

            if(groundBelow.FrontGroundHit.collider && groundBelow.BackGroundHit.collider)
            {
                Vector3 upright = Vector3.Cross(transform.right, -(groundBelow.FrontGroundHit.point - groundBelow.BackGroundHit.point).normalized);

                Debug.DrawRay(bRB.transform.position, -(groundBelow.FrontGroundHit.point - groundBelow.BackGroundHit.point).normalized, Color.green);
                Debug.DrawRay(bRB.transform.position, upright.normalized, Color.red);
                Debug.DrawRay(bRB.transform.position, Vector3.Cross(transform.right, upright).normalized, Color.cyan);

                float angle = Vector3.Angle(upright, transform.up);

                //If it's a reasonable adjustment
                if(angle < 30f && Vector3.Distance(groundBelow.FrontGroundHit.point, groundBelow.BackGroundHit.point) < 7.5f)
                {
                    if(bAerial)
                    {
                        groundQuat = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, upright)), smoothness * Time.deltaTime);
                    }
                    else
                    {
                        groundQuat = Quaternion.LookRotation(Vector3.Cross(transform.right, upright));
                    }
                }
            }

            //GameObject's heading based on the current input
            headingDeltaAngle = speed * 1000 * currentTurnInput * Time.deltaTime;
            headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);

            transform.rotation = groundQuat;
            transform.rotation = transform.rotation * headingDelta;
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
            nextToWallRun.InitialiseCondition(this, fRB);
            grindBelow.InitialiseCondition(fRB, inputHandler);

            groundedState.InitialiseState(this, fRB, bRB, groundBelow, grindBelow);
            aerialState.InitialiseState(this, fRB, bRB, groundBelow, nextToWallRun, grindBelow);
            wallRideState.InitialiseState(this, fRB, nextToWallRun, groundBelow);
            grindingState.InitialiseState(this, fRB, grindBelow);

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
            fRB.transform.parent = null;

            //Setting up model position
            playerModel.transform.position = new Vector3(boardObject.transform.position.x, (ballMovement.transform.position.y - (ballMovement.radius * ballMovement.transform.localScale.y) + 0.0275f), boardObject.transform.position.z);
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
                fRB.AddForce(Vector3.down * AdditionalGravityAmount, ForceMode.Acceleration);
            }
        }
        #endregion

        #region Public Methods

        //A few utility functions
        public override void MoveToPosition(Vector3 positionToMoveTo)
        {
            ballMovement.transform.position = positionToMoveTo;
        }

        public void ResetWheelPos()
        {
            fRB.transform.position = frontWheelPos.transform.position;
            bRB.transform.position = backWheelPos.transform.position;
        }

        public void AlignWheels()
        {
            fRB.transform.up = transform.up;
            fRB.transform.forward = transform.forward;
            bRB.transform.forward = transform.forward;
            bRB.transform.up = transform.up;
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
                if(fRB.velocity.magnitude > 0.3f && playerStateMachine.currentState == groundedState)
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
