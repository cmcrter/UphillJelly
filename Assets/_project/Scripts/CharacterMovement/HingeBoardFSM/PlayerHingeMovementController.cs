////////////////////////////////////////////////////////////
// File: PlayerHingeMovementController
// Author: Charles Carter
// Date Created: 29/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 07/01/22
// Brief: A prototype character controller using a board made of 2 RBs with 2 Joints used
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;
using L7Games.Triggerables;
using System.Collections;

namespace L7Games.Movement
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
        private Coroutine AirturningCo;

        private Timer turningTimer;
        public AnimationCurve turnSpeedCurve;
        public float turnClamp = 0.575f;

        #endregion

        #region Public Methods

        public override void ResetPlayer()
        {
            Time.timeScale = 0;

            fRB.isKinematic = false;

            fRB.angularVelocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            bRB.isKinematic = false;

            bRB.angularVelocity = Vector3.zero;
            bRB.velocity = Vector3.zero;

            transform.rotation = initialRot;
            transform.position = initalPos;

            ResetWheelPos();
            AlignWheels();

            Time.timeScale = 1;
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

            if (groundBelow.FrontRightGroundHit.collider && groundBelow.BackRightGroundHit.collider)
            {
                Vector3 upright = Vector3.Cross(transform.right, -(groundBelow.FrontRightGroundHit.point - groundBelow.BackRightGroundHit.point).normalized);

                // Calculate the roll
                // Find the angles between the width raycasts
                float frontAngle = CalculateSignedSlopeAngle(groundBelow.FrontLeftGroundHit.point, groundBelow.FrontRightGroundHit.point, Vector3.up);
                float backAngle = CalculateSignedSlopeAngle(groundBelow.BackLeftGroundHit.point, groundBelow.BackRightGroundHit.point, Vector3.up);
                // Use the largest unsigned value
                float unsignedFrontAngle = frontAngle < 0f ? frontAngle * -1f : frontAngle;
                float unsignedBackAngle = backAngle < 0f ? backAngle * -1f : backAngle;
                float roll = unsignedFrontAngle > unsignedBackAngle ? frontAngle : backAngle;

                // Calculate the pitch
                // Find the angles between the length raycasts
                float leftAngle = CalculateSignedSlopeAngle(groundBelow.FrontLeftGroundHit.point, groundBelow.BackLeftGroundHit.point, Vector3.up);
                //linesToDraw.Add(new LineToDraw(frontLeftHit.point, backLeftHit.point, Color.white));
                float rightAngle = CalculateSignedSlopeAngle(groundBelow.FrontRightGroundHit.point, groundBelow.BackRightGroundHit.point, Vector3.up);
                //linesToDraw.Add(new LineToDraw(frontRightHit.point, backRightHit.point, Color.white));
                // Use the smallest unsigned value
                float unsignedLeftAngle = leftAngle < 0f ? leftAngle * -1f : leftAngle;
                float unsignedRightAngle = rightAngle < 0f ? rightAngle * -1f : rightAngle;
                float pitch = unsignedLeftAngle > unsignedRightAngle ? leftAngle : rightAngle;
                Quaternion newRotation = Quaternion.Euler(new Vector3(pitch, transform.rotation.eulerAngles.y, roll));

                if (Debug.isDebugBuild)
                {
                    Debug.DrawRay(bRB.transform.position, -(groundBelow.FrontRightGroundHit.point - groundBelow.BackRightGroundHit.point).normalized, Color.green);
                    Debug.DrawRay(bRB.transform.position, upright.normalized, Color.red);
                    Debug.DrawRay(bRB.transform.position, Vector3.Cross(transform.right, upright).normalized, Color.cyan);
                    Debug.DrawRay(groundBelow.FrontRightGroundHit.point, groundBelow.FrontRightGroundHit.normal, Color.cyan);
                    Debug.DrawRay(groundBelow.BackRightGroundHit.point, groundBelow.BackRightGroundHit.normal, Color.cyan);
                }

                float angle = Quaternion.Angle(newRotation, Quaternion.LookRotation(transform.forward, transform.up).normalized);

                if(bAerial)
                {
                    groundQuat = Quaternion.Lerp(transform.rotation, newRotation, smoothness * Time.deltaTime);
                }
                else
                {
                    //If it's a reasonable adjustment
                    if (angle < 30f)
                    {
                        groundQuat = newRotation;
                    }
                }
            }

            //GameObject's heading based on the current input
            headingDeltaAngle = speed * 1000 * currentTurnInput * Time.deltaTime;
            headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);

            transform.rotation = groundQuat;
            
            if(!bAerial)
            {
                transform.rotation = transform.rotation * headingDelta;
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
            nextToWallRun.InitialiseCondition(this, fRB);
            grindBelow.InitialiseCondition(fRB, inputHandler);

            groundedState.InitialiseState(this, fRB, bRB, groundBelow, grindBelow);
            aerialState.InitialiseState(this, fRB, bRB, groundBelow, nextToWallRun, grindBelow);
            wallRideState.InitialiseState(this, fRB, bRB, nextToWallRun, groundBelow);
            grindingState.InitialiseState(this, fRB, grindBelow);

            playerStateMachine = new FiniteStateMachine(aerialState);
        }

        //Adding the inputs to the finite state machine
        private void OnEnable()
        {
            groundedState.RegisterInputs();
            grindingState.RegisterInputs();
            wallRideState.RegisterInputs();
        }

        private void OnDisable()
        {
            groundedState.UnRegisterInputs();
            grindingState.UnRegisterInputs();
            wallRideState.UnRegisterInputs();
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
                bRB.AddForce(Vector3.down * AdditionalGravityAmount, ForceMode.Acceleration);
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
                StopCoroutine(turningCo);
            }
        }

        public void StartAirInfluenctCoroutine()
        {
            StopAirInfluenctCoroutine();
            AirturningCo = StartCoroutine(Co_AirInfluence());
        }

        public void StopAirInfluenctCoroutine()
        {
            if(AirturningCo != null)
            {
                StopCoroutine(AirturningCo);
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_AirInfluence()
        {
            bool InfluenceDir;
            Timer influenceTimer = new Timer(5.0f);

            while (influenceTimer.isActive)
            {
                InfluenceDir = inputHandler.TurningAxis < 0 ? true : false;
                Debug.Log("Influence Up", this);

                if(InfluenceDir)
                {
                    fRB.AddForce(-transform.right * 5f, ForceMode.Impulse);
                }
                else if (inputHandler.TurningAxis != 0 )
                {
                    fRB.AddForce(transform.right * 5f, ForceMode.Impulse);
                }

                influenceTimer.Tick(Time.deltaTime);
                yield return null;
            }

            AirturningCo = null;
        }

        private IEnumerator Co_TurnAngle()
        {
            turningTimer = new Timer(turnDuration);
            currentTurnInput = 0;

            bool initialTurningDir = inputHandler.TurningAxis < 0 ? true : false;

            //Whilst the player wants to turn
            while(inputHandler.TurningAxis != 0)
            {
                bool newTurningDir = inputHandler.TurningAxis < 0 ? true : false;

                if(initialTurningDir != newTurningDir)
                {
                    StopTurnCoroutine();
                    break;
                }

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


        private float CalculateSignedSlopeAngle(Vector3 startingPoint, Vector3 endPoint, Vector3 flatPlaneNormal)
        {
            Vector3 slopeVector = endPoint - startingPoint;
            Vector3 flatVector = Vector3.ProjectOnPlane(slopeVector, flatPlaneNormal).normalized;
            Vector3 rightFlatVector = Vector3.Cross(flatVector, flatPlaneNormal).normalized;
            return Vector3.SignedAngle(flatVector, slopeVector, rightFlatVector);
        }
        #endregion
    }
}
