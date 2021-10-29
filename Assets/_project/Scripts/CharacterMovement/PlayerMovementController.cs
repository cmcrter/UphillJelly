////////////////////////////////////////////////////////////
// File: PlayerMovementController
// Author: Charles Carter
// Date Created: 22/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/10/21
// Brief: The script which will determine the player's movement for a release build
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Utility.StateMachine;
using UnityEngine.InputSystem;

namespace SleepyCat.Movement
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

        [Header("Player Setup")]
        [SerializeField]
        private GameObject playerModel;
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private Transform groundRaycastPoint;
        [SerializeField]
        public SphereCollider ballMovement;
        [SerializeField]
        public PlayerInput input;

        [SerializeField]
        private float AdditionalGravityAmount = 8;
        public bool bAddAdditionalGravity = true;

        private Vector3 initalPos;
        private Quaternion initialRot;

        #endregion

        #region Public Methods

        public void ResetBoard()
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            rb.transform.rotation = initialRot;
            transform.rotation = initialRot;

            rb.transform.position = initalPos;
        }

        //Both grounded and aerial wants to have the model smooth towards what's below them to a degree
        public void SmoothToGroundRotation(float smoothness, float speed, RaycastHit hit, RaycastHit frontHit)
        {
            //GameObject's heading
            float headingDeltaAngle = speed * 1000 * currentTurnInput * Time.deltaTime;
            Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
            Quaternion groundQuat = transform.rotation;

            if(Vector3.Angle(hit.normal, Vector3.up) < 70f)
            {
                groundQuat = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime * smoothness);

                if(frontHit.collider && Vector3.Angle(frontHit.normal, hit.normal) < 60)
                {
                    groundQuat = Quaternion.Slerp(groundQuat, Quaternion.FromToRotation(transform.up, frontHit.normal) * transform.rotation, Time.deltaTime * smoothness);
                }
            }

            transform.rotation = groundQuat;
            transform.rotation = transform.rotation * headingDelta;
            transform.position = rb.transform.position;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Setting up the state machine
            groundBelow.InitialiseCondition(transform, groundRaycastPoint);
            nextToWallRun.InitialiseCondition();
            grindBelow.InitialiseCondition(rb);

            groundedState.InitialiseState(this, rb, groundBelow);
            aerialState.InitialiseState(this, rb, groundBelow, nextToWallRun, grindBelow);
            wallRideState.InitialiseState();
            grindingState.InitialiseState(this, rb, grindBelow);

            playerStateMachine = new FiniteStateMachine(groundedState);
        }

        private void Start()
        {
            initalPos = transform.position;
            initialRot = transform.rotation;
            rb.transform.parent = null;

            //Setting up model position
            playerModel.transform.position = new Vector3(ballMovement.transform.position.x, ballMovement.transform.position.y - ballMovement.radius + 0.001f, ballMovement.transform.position.z);
        }

        private void Update()
        {
            playerStateMachine.RunMachine(Time.deltaTime);

            if(Keyboard.current.escapeKey.isPressed) 
            {
                ResetBoard();
            }

            //Multiple states need to know the current turning of the user
            currentTurnInput = 0;

            if(Keyboard.current.aKey.isPressed)
            {
                if(rb.velocity.magnitude < 2)
                {
                    currentTurnInput = -1f;
                }
                else
                {
                    currentTurnInput -= 0.25f * rb.velocity.magnitude * 0.3f;
                }
            }

            if(Keyboard.current.dKey.isPressed)
            {
                if(rb.velocity.magnitude < 2)
                {
                    currentTurnInput = 1f;
                }
                else
                {
                    currentTurnInput += 0.25f * rb.velocity.magnitude * 0.3f;
                }
            }

            Mathf.Clamp(currentTurnInput, -1, 1);
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

        #region Private Methods



        #endregion
    }
}
