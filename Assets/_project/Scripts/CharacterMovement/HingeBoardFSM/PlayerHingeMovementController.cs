////////////////////////////////////////////////////////////
// File: PlayerHingeMovementController
// Author: Charles Carter
// Date Created: 29/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 02/03/22
// Brief: A prototype character controller using a board made of 2 RBs with 2 Joints used
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Utility.StateMachine;
using L7Games.Input;
using L7Games.Triggerables;
using L7Games.Triggerables.CheckpointSystem;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using FMODUnity;
using L7Games.Tricks;

namespace L7Games.Movement
{
    [RequireComponent(typeof(TrickBuffer))]
    public class PlayerHingeMovementController : PlayerController
    {
        #region Variables
        [Header("State Machine")]
        public FiniteStateMachine playerStateMachine;

        public HAerialState aerialState = new HAerialState();
        public HGroundedState groundedState = new HGroundedState();
        public HWallRideState wallRideState = new HWallRideState();
        public HGrindedState grindingState = new HGrindedState();
        public HWipeOutState wipeOutState = new HWipeOutState();

        public HisGroundBelow groundBelow = new HisGroundBelow();
        public HisNextToWallRun nextToWallRun = new HisNextToWallRun();
        public HisOnGrind grindBelow = new HisOnGrind();

        public PlayerCamera playerCamera;
        public CinemachineVirtualCamera camBrain;
        public CinemachineVirtualCamera backwardsCamera;
        public CinemachineVirtualCamera wallRideCam;
        public CinemachineVirtualCamera wipeOutCam;
        public CinemachineVirtualCamera grindCam;

        private float currentTurnInput;

        [Tooltip("The max distance that the hit points of the roll calculating hit points can be from each other whilst still being valid")]
        public float angleCorrectionRaycastValidHitWidth = 2f;
        [Tooltip("The max distance that the hit points of the pitch calculating hit points can be from each other whilst still being valid")]
        public float angleCorrectionRaycastValidHitLength = 2f;

        [Tooltip("The unit interval indicating how close a collision velocity direction must be to the player is going, 1 being the exact same direction, 0 being perpendicular away")]
        [Range(0f, 1f)]
        public float collisionDirectionMinValue = 0.8f;

        [Tooltip("The min unit interval value for a collision to be consider vertical from the normalised dot product")]
        public float verticalCollisionTheshold = 0.75f;

        [Header("Player Setup")]
        [SerializeField]
        private Transform root;

        [SerializeField]
        public GameObject boardObject;
        [SerializeField]
        private GameObject boardModel;
        private Vector3 boardPos;

        //Front Rigidbody
        [SerializeField]
        private Rigidbody fRB;
        public Rigidbody ModelRB;

        public PlayerInput input;
        public InputHandler inputHandler;
        public Animator characterAnimator;
        public TriggerableTrigger triggerObject;
        public TrickBuffer trickBuffer;

        [SerializeField]
        private Transform frontWheelPos;
        [SerializeField]
        private GameObject characterModel;
        [SerializeField]
        private Transform lookAtTransform;

        private bool bCameraLocked = false;

        [SerializeField]
        private float AdditionalGravityAmount = 8;
        public bool bAddAdditionalGravity = true;

        private Vector3 initalPos;
        private Quaternion initialRot;
        private Quaternion initialRootRotation;

        private Coroutine AirturningCo;
        public float turnClamp = 1f;
        [SerializeField]
        private float turnSmoothness = 1.25f;

        [Tooltip("The prefab that is spawned to replace this as a ragdoll Ragdoll used prefab used")]
        [SerializeField]
        private GameObject ragDollPrefab;
        public SpawnedRagdoll currentRagdoll;

        [Tooltip("The ")]
        public RagdollDataContainer ragdollDataContainer;
        public CheckpointManager checkpointManager;

        [SerializeField]
        private List<Collider> playerColliders = new List<Collider>();

        [SerializeField]
        private float airInfluence = 25f;

        public StudioEventEmitter audioEmitter;
        private FMOD.Studio.EventInstance respawnSound;

        #endregion

        #region Public Properties
        public bool IsWipedOut
        {
            get
            {
                return characterModel.activeSelf;
            }
        }
        #endregion

        #region Public Methods

        public override void ResetPlayer()
        {
            bWipeOutLocked = true;

            Time.timeScale = 0;

            collectableCounter = 0;

            fRB.isKinematic = false;
            fRB.useGravity = true;

            bCameraLocked = false;
            OverrideCamera(camBrain);

            fRB.drag = aerialState.AerialDrag;
            ModelRB.drag = aerialState.AerialDrag;

            fRB.centerOfMass = Vector3.zero;

            fRB.angularVelocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            ModelRB.angularVelocity = Vector3.zero;
            ModelRB.velocity = Vector3.zero;

            root.rotation = initialRootRotation;

            Destroy(boardModel.GetComponent<Rigidbody>());
            boardModel.transform.SetParent(root);
            boardModel.transform.localPosition = new Vector3(-0.053f, 0, 0);
            boardModel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

            transform.rotation = initialRot;
            transform.position = initalPos;

            characterModel.SetActive(true);

            camBrain.LookAt = lookAtTransform;
            camBrain.Follow = transform;

            ResetWheelPos();
            AlignWheels();

            if (currentRagdoll != null)
            {
                currentRagdoll.DestroySelf();
            }

            CallOnRespawn();

            trickBuffer.ClearTricks();

            if(playerStateMachine.currentState != null)
            {
                playerStateMachine.currentState.OnStateExit();
                StopAllCoroutines();
            }

            playerStateMachine.ForceSwitchToState(aerialState);
            triggerObject.enabled = true;

            characterAnimator.Play("aerial");
            //characterAnimator.playbackTime = 1f;
            //characterAnimator.SetFloat("crouchingFloat", -1);

            bWipeOutLocked = false;
            Time.timeScale = 1;
        }

        public override void ResetPlayer(Transform point)
        {
            bWipeOutLocked = true;
            Time.timeScale = 0;

            collectableCounter = 0;

            fRB.isKinematic = false;

            fRB.useGravity = true;

            bCameraLocked = false;
            OverrideCamera(camBrain);

            fRB.centerOfMass = Vector3.zero;

            fRB.drag = aerialState.AerialDrag;
            ModelRB.drag = aerialState.AerialDrag;

            fRB.centerOfMass = Vector3.zero;

            fRB.angularVelocity = Vector3.zero;
            fRB.velocity = Vector3.zero;

            ModelRB.angularVelocity = Vector3.zero;
            ModelRB.velocity = Vector3.zero;

            root.rotation = initialRootRotation;

            Destroy(boardModel.GetComponent<Rigidbody>());
            boardModel.transform.SetParent(root);
            boardModel.transform.localPosition = new Vector3(-0.053f, 0, 0);
            boardModel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

            transform.rotation = point.rotation;
            transform.position = point.position;
            Debug.Log("Player Position Reset");

            characterModel.SetActive(true);

            ResetCameraView();

            ResetWheelPos();
            AlignWheels();

            if (currentRagdoll != null)
            {
                currentRagdoll.DestroySelf();
            }
            
            CallOnRespawn();

            trickBuffer.ClearTricks();

            if(playerStateMachine.currentState != null)
            {
                playerStateMachine.currentState.OnStateExit();
                StopAllCoroutines();
            }

            playerStateMachine.ForceSwitchToState(aerialState);
            triggerObject.enabled = true;

            characterAnimator.Play("aerial");
            //characterAnimator.playbackTime = 1f;
            //characterAnimator.SetFloat("crouchingFloat", -1);

            Time.timeScale = 1;
            bWipeOutLocked = false;
        }

        //Both grounded and aerial wants to have the model smooth towards what's below them to a degree
        public void SmoothToGroundRotation(bool bAerial, float smoothness, float speed, HisGroundBelow groundBelow)
        {
            Quaternion groundQuat = transform.rotation;

            // Check which ray-casts should be used by getting the angle distance between the normals
            RaycastHit frontLeftHitToUse = groundBelow.FrontLeftGroundHitLocalDown;
            RaycastHit frontRightHitToUse = groundBelow.FrontRightGroundHitLocalDown;
            RaycastHit backLeftHitToUse = groundBelow.BackLeftGroundHitLocalDown;
            RaycastHit backRightHitToUse = groundBelow.BackRightGroundHitLocalDown;

            if (bAerial)
            {
                frontLeftHitToUse =     groundBelow.FrontLeftGroundHitWorldDown;
                frontRightHitToUse =    groundBelow.FrontRightGroundHitWorldDown;
                backLeftHitToUse =      groundBelow.BackLeftGroundHitWorldDown;
                backRightHitToUse =     groundBelow.BackRightGroundHitWorldDown;
            }


            if (frontRightHitToUse.collider && backRightHitToUse.collider)
            {
                Vector3 upright = Vector3.Cross(transform.right, -(frontRightHitToUse.point - backRightHitToUse.point).normalized);

                // Calculate the roll
                // Find the angles between the width raycasts
                // Check if the smoothing points are a reasonable distance from each other
                float frontAngle;
                if (Vector3.Distance(frontLeftHitToUse.point, frontRightHitToUse.point) < angleCorrectionRaycastValidHitWidth)
                {
                    frontAngle = CalculateSignedSlopeAngle(frontLeftHitToUse.point, frontRightHitToUse.point, Vector3.up);
                }
                else
                {
                    frontAngle = 0f;
                }
                float backAngle;
                if (Vector3.Distance(backLeftHitToUse.point, backRightHitToUse.point) < angleCorrectionRaycastValidHitWidth)
                {
                    backAngle = CalculateSignedSlopeAngle(backLeftHitToUse.point, backRightHitToUse.point, Vector3.up);
                }
                else
                {
                    backAngle = 0f;
                }
                // Use the largest unsigned value
                float unsignedFrontAngle = frontAngle < 0f ? frontAngle * -1f : frontAngle;
                float unsignedBackAngle = backAngle < 0f ? backAngle * -1f : backAngle;
                float roll = unsignedFrontAngle > unsignedBackAngle ? frontAngle : backAngle;

                // Calculate the pitch
                // Find the angles between the length raycasts
                float leftAngle;
                if (Vector3.Distance(frontLeftHitToUse.point, backLeftHitToUse.point) < angleCorrectionRaycastValidHitLength)
                {
                    leftAngle = CalculateSignedSlopeAngle(frontLeftHitToUse.point, backLeftHitToUse.point, Vector3.up);
                }
                else
                {
                    leftAngle = 0f;
                }
                float rightAngle;
                if (Vector3.Distance(frontRightHitToUse.point, backRightHitToUse.point) < angleCorrectionRaycastValidHitLength)
                {
                    rightAngle = CalculateSignedSlopeAngle(frontRightHitToUse.point, backRightHitToUse.point, Vector3.up);
                }
                else
                {
                    rightAngle = 0f;
                }

                // Use the smallest unsigned value
                float unsignedLeftAngle = leftAngle < 0f ? leftAngle * -1f : leftAngle;
                float unsignedRightAngle = rightAngle < 0f ? rightAngle * -1f : rightAngle;
                float pitch = unsignedLeftAngle > unsignedRightAngle ? leftAngle : rightAngle;
                Quaternion newRotation = Quaternion.Euler(new Vector3(pitch, transform.rotation.eulerAngles.y, roll));

                if (Debug.isDebugBuild)
                {
                    Debug.DrawRay(frontRightHitToUse.point, frontRightHitToUse.normal, Color.cyan);
                    Debug.DrawRay(backRightHitToUse.point, backRightHitToUse.normal, Color.cyan);
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
            float headingDeltaAngle = speed * 1000f * currentTurnInput;
            Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);

            transform.rotation = groundQuat;
            
            if(!bAerial)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * headingDelta, turnSmoothness * Time.deltaTime);
            }

            //fRB.transform.rotation = transform.rotation;
        }

        public override void AddWallRide(WallRideTriggerable wallRide)
        {
            nextToWallRun.CheckWall(wallRide);
        }

        public override void RemoveWallRide(WallRideTriggerable wallRide)
        {
            nextToWallRun.LeftWall(wallRide);
        }

        public override void OverrideCamera(CinemachineVirtualCamera camera, bool lockCamera)
        {
            if(bCameraLocked)
            {
                return;
            }

            wipeOutCam.enabled = false;
            wallRideCam.enabled = false;
            camBrain.enabled = false;
            grindCam.enabled = false;
            backwardsCamera.enabled = false;

            camera.enabled = true;

            bCameraLocked = lockCamera;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Setting up the state machine
            groundBelow.InitialiseCondition(transform);
            nextToWallRun.InitialiseCondition(this, fRB);
            grindBelow.InitialiseCondition(fRB, inputHandler);

            groundedState.InitialiseState(this, fRB, groundBelow, grindBelow);
            aerialState.InitialiseState(this, fRB, groundBelow, nextToWallRun, grindBelow);
            wallRideState.InitialiseState(this, fRB, nextToWallRun, groundBelow);
            grindingState.InitialiseState(this, fRB, ModelRB, grindBelow);

            playerStateMachine = new FiniteStateMachine(aerialState);

            camBrain = camBrain ?? FindObjectOfType<CinemachineVirtualCamera>();
            trickBuffer = trickBuffer ?? GetComponent<TrickBuffer>();
        }

        //Adding the inputs to the finite state machine
        private void OnEnable()
        {
            groundedState.RegisterInputs();
            grindingState.RegisterInputs();
            wallRideState.RegisterInputs();
            aerialState.RegisterInputs();

            inputHandler.wipeoutResetStarted += WipeOutResetPressed;
            onWipeout += WipeOutCharacter;
            onRespawn += PlayRespawnSound;
        }

        private void OnDisable()
        {
            groundedState.UnRegisterInputs();
            grindingState.UnRegisterInputs();
            wallRideState.UnRegisterInputs();
            aerialState.UnRegisterInputs();

            inputHandler.wipeoutResetStarted -= WipeOutResetPressed;
            onWipeout -= WipeOutCharacter;
            onRespawn -= PlayRespawnSound;
        }

        private void Start()
        {
            initalPos = transform.position;
            initialRot = transform.rotation;
            fRB.transform.parent = null;
            boardPos = boardModel.transform.position;
            initialRootRotation = root.rotation;

            //characterInitalBones = GetBonesFromObject(characterModel);

            if(checkpointManager == null)
            {
                checkpointManager = FindObjectOfType<CheckpointManager>();
            }

            characterAnimator.SetFloat("crouchingFloat", -1);
            characterAnimator.SetFloat("turnValue", 0);

            respawnSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/Respawn");
        }

        private void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.yKey.isPressed)
            {
                Time.timeScale -= 0.1f * Time.unscaledDeltaTime;
            }
            else if (UnityEngine.InputSystem.Keyboard.current.uKey.isPressed)
            {
                Time.timeScale += 0.1f * Time.unscaledDeltaTime;
            }

            if (characterModel.activeSelf)
            {
                playerStateMachine.RunMachine(Time.deltaTime);
            }

            if (Keyboard.current != null)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    ResetPlayer();
                }
            }

            currentTurnInput = Mathf.Clamp(inputHandler.TurningAxis, -turnClamp, turnClamp);

            if(AirturningCo != null || groundedState.hasRan)
            {
                characterAnimator.SetFloat("turnValue", inputHandler.TurningAxis);
            }
        }

        private void FixedUpdate()
        {
            if (characterModel.activeSelf)
            {
                playerStateMachine.RunPhysicsOnMachine(Time.fixedDeltaTime);
            }

            if(bAddAdditionalGravity)
            {
                ModelRB.AddForce(Vector3.down * AdditionalGravityAmount, ForceMode.Acceleration);
                fRB.AddForce(Vector3.down * AdditionalGravityAmount, ForceMode.Acceleration);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(bWipeOutLocked)
                return;

            if (characterModel.activeSelf)
            {
                for (int i = 0; i < collision.contactCount; ++i)
                {
                    if (collision.contacts[i].thisCollider.gameObject.TryGetComponent(out WipeOutCollider characterCollider))
                    {
                        // Check if it is not a vertical collision
                        bool verticalCheck = characterCollider.ignoreVerticalCollisions;
                        if (!verticalCheck)
                        {
                            float value = Vector3.Dot(collision.relativeVelocity.normalized, transform.up);
                            verticalCheck = verticalCollisionTheshold > value;
                            //Debug.Log(value);
                            Debug.DrawLine(collision.contacts[i].point, collision.contacts[i].point + collision.relativeVelocity.normalized, Color.black);
                            Debug.DrawLine(collision.contacts[i].point, collision.contacts[i].point + transform.up, Color.magenta);
                        }

                        if (verticalCheck)
                        {
                            float verticalDot = Vector3.Dot(collision.relativeVelocity.normalized, collision.contacts[i].normal);

                            float scaler = 1.0f;
                            if (characterCollider.effectedByCollisionAngle)
                            {
                                scaler = Vector3.Dot(collision.relativeVelocity.normalized, collision.contacts[i].normal);
                            }
                            if (scaler > collisionDirectionMinValue)
                            {

                                if (collision.relativeVelocity.magnitude > characterCollider.forceRequiredToWipeOut)
                                {
                                    Debug.Log(collision.relativeVelocity.magnitude + " + " + characterCollider.forceRequiredToWipeOut + " + " + characterCollider.name);
                                    CallOnWipeout(-collision.relativeVelocity);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Methods


        //A few utility functions
        public override void MoveToPosition(Vector3 positionToMoveTo)
        {
            
        }

        public void ResetWheelPos()
        {
            fRB.transform.position = frontWheelPos.transform.position;
        }

        public void AlignWheels()
        {
            fRB.transform.rotation = transform.rotation;
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
                AirturningCo = null;
            }
        }

        public void ResetCameraView()
        {
            camBrain.LookAt = lookAtTransform;
            camBrain.Follow = transform;
        }

        public void WipeOutCharacter(Vector3 currentVelocity)
        {
            if(!characterModel.activeSelf)
            {
                return;
            }

            bWipeOutLocked = true;
            triggerObject.enabled = false;

            playerStateMachine.currentState.OnStateExit();
            playerStateMachine.ForceSwitchToState(null);

            //groundedState.OnStateExit();
            //grindingState.OnStateExit();
            //aerialState.OnStateExit();
            //wallRideState.OnStateExit();

            // Player will leave wall on wiping out
            nextToWallRun.WipeoutReset();

            // Player won't be grinding on wipeout
            grindBelow.WipeOutReset();

            //WipeOut needs to stop most of the players' actions
            input.SwitchCurrentActionMap("WipedOut");

            // Spawn the ragdoll
            GameObject ragdoll = ReplaceWithRagdoll(ragDollPrefab);
            // If there is a root or main rigid body then take that into account, otherwise not a problem
            SpawnedRagdoll ragdollComponent = ragdoll.GetComponent<SpawnedRagdoll>();
            if (ragdollComponent != null)
            {
                ragdollComponent.AddForceToRagdollMainRigidbody(currentVelocity, ForceMode.Impulse);
            }
            ragdollComponent.AddForceToRagdollAllRigidbody(currentVelocity, ForceMode.Impulse);

            boardModel.transform.SetParent(null);
            Rigidbody boardRb =  boardModel.AddComponent<Rigidbody>();
            boardRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            boardRb.AddForce(Vector3.up);
            fRB.isKinematic = true;

            characterAnimator.Play("Wipeout");
            characterModel.SetActive(false);

            OverrideCamera(wipeOutCam, false);
        }

        public void PlayRespawnSound()
        {
            respawnSound.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);

            //if(state != FMOD.Studio.PLAYBACK_STATE.STOPPED)
            //    return;

            respawnSound.start();
        }

        #endregion

        #region Private Methods

        private IEnumerator Co_AirInfluence()
        {
            bool InfluenceDir;
            Timer influenceTimer = new Timer(2.5f);

            while (influenceTimer.isActive)
            {
                InfluenceDir = inputHandler.TurningAxis < 0 ? true : false;
                //Debug.Log("Influence Up", this);

                if(InfluenceDir)
                {
                    yield return new WaitForFixedUpdate();
                    fRB.AddForce(-transform.right * airInfluence, ForceMode.Impulse);
                }
                else if (inputHandler.TurningAxis != 0 )
                {
                    yield return new WaitForFixedUpdate();
                    fRB.AddForce(transform.right * airInfluence, ForceMode.Impulse);
                }

                influenceTimer.Tick(Time.fixedDeltaTime);
                yield return null;
            }
        }

        private float CalculateSignedSlopeAngle(Vector3 startingPoint, Vector3 endPoint, Vector3 flatPlaneNormal)
        {
            Vector3 slopeVector = endPoint - startingPoint;
            Vector3 flatVector = Vector3.ProjectOnPlane(slopeVector, flatPlaneNormal).normalized;
            Vector3 rightFlatVector = Vector3.Cross(flatVector, flatPlaneNormal).normalized;
            return Vector3.SignedAngle(flatVector, slopeVector, rightFlatVector);
        }

        private void HumanoidCollision_lethalCollisionDetected(Vector3 direction)
        {
            if (fRB.velocity.magnitude > 0f)
            {
                CallOnWipeout(fRB.velocity);
            }
        }

        //private System.Collections.Generic.List<Bones> GetBonesFromObject(GameObject currentObject)
        //{
        //    System.Collections.Generic.List<Bones> characterBones = new System.Collections.Generic.List<Bones>();
        //    if (currentObject.CompareTag("Bone"))
        //    {
        //        characterBones.Add(new Bones(currentObject.transform.localPosition, currentObject.transform.localRotation, currentObject.transform.localScale, currentObject));
        //    }
        //    for (int i = 0; i < currentObject.transform.childCount; ++i)
        //    {
        //        characterBones.AddRange(GetBonesFromObject(currentObject.transform.GetChild(i).gameObject));
        //    }
        //    return characterBones;
        //}

        private GameObject ReplaceWithRagdoll(GameObject ragDollPrefab)
        {
            // Spawn the rag-doll
            GameObject ragDoll = GameObject.Instantiate(ragDollPrefab, characterModel.transform.position, characterModel.transform.rotation);
            if (ragDoll.TryGetComponent<SpawnedRagdoll>(out SpawnedRagdoll spawnedRagdoll))
            {
                spawnedRagdoll.Initalise(ragdollDataContainer);
                currentRagdoll = spawnedRagdoll;
            }

            return ragDoll;
        }

        private void WipeOutResetPressed()
        {
            if (characterModel.activeSelf)
            {
                CallOnWipeout(fRB.velocity);
            }
            else
            {
                if (checkpointManager != null)
                {
                    checkpointManager.MovePlayerToTheirLastCheckPoint(this);
                }
            }
        }

        #endregion
    }
}
