//========================================================================================================================================================================================================================
//  Name:               IdleReactionNPC.cs
//  Authors:            Matthew Mason
//  Date Created:       29/03/2022
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 30/03/2022
//  Brief:              The Script control an NPC that will perform and Idle animation, react to player proximity, and rag doll if the player gets too close
//========================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;
using L7Games;

/// <summary>
/// The Script control an NPC that will perform and Idle animation, react to player proximity, and rag doll if the player gets too close
/// </summary>
public class IdleReactionNPC : MonoBehaviour
{
    #region Public Variables
    public SpawnedRagdoll currentRagdoll;

    public Animator animator;

    public float speed;

    public float diveDuration;
    #endregion

    #region Private Serialized Fields
    [Tooltip("The prefab that is spawned to replace this as a Ragdoll used prefab used")]
    [SerializeField]
    private GameObject ragDollPrefab;

    [Tooltip("The Player Proximity for this NPC to react to")]
    [SerializeField]
    private PlayerDetectingTrigger playerPoximityTrigger;

    [Tooltip("The data to pass to the ragdoll when it spawns")]
    [SerializeField]
    private RagdollDataContainer ragdollData;

    [Tooltip("The player detection triggers used for detecting when the player collides with NPC's body")]
    [SerializeField]
    private PlayerDetectingTrigger[] bodyCollisionTriggers;

    [SerializeField]
    [Tooltip("The mesh render attached to the rag-doll")]
    private SkinnedMeshRenderer ragdollMeshRenderer;

    [SerializeField]
    [Tooltip("The mesh render attached to the character")]
    private SkinnedMeshRenderer characterMeshRenderer;

    [SerializeField]
    [Tooltip("The distance to check for players before it can reset")]
    private float resetCheckDistance;

    [SerializeField]
    [Tooltip("The players to check look for players in")]
    private LayerMask playerLayerMask;
    #endregion

    #region Private variables
    private bool isReacting;
    #endregion

    #region Public Static Events
    public delegate void IdleReactionNPCEvent(IdleReactionNPC idleReactionNPCcallingEvent);
    public delegate void RagdollToNpcEvent(SpawnedRagdoll originalRagdoll, IdleReactionNPC idleReactionNPCcallingEvent);
    public static event IdleReactionNPCEvent NPCReactingToPlayerProixmity;
    public static event IdleReactionNPCEvent NPCResetingFromReaction;
    public static event RagdollToNpcEvent NPCResetingFromRagdoll;
    #endregion

    private void OnEnable()
    {
        if (playerPoximityTrigger != null)
        {
            playerPoximityTrigger.playerEnteredTrigger += OnPlayerEnteredProximity;
            playerPoximityTrigger.playerEnteredTrigger += AddPlayerPositionToCondition;
            playerPoximityTrigger.playerExitedTrigger += RemovePlayerPositionFromCondition;
        }
        if (bodyCollisionTriggers != null)
        {
            for (int i = 0; i < bodyCollisionTriggers.Length; ++i)
            {
                if (bodyCollisionTriggers[i] != null)
                {
                    bodyCollisionTriggers[i].playerEnteredTrigger += OnBodyCollision;
                }

            }
        }
        #if DEBUG || UNITY_EDITOR
        else
        {
            Debug.LogError("playerPoximityTrigger was null when referenced for event binding in NPCBrain Components", this);
        }
        #endif
    }

    private void OnDisable()
    {
        if (playerPoximityTrigger != null)
        {
            playerPoximityTrigger.playerEnteredTrigger -= OnPlayerEnteredProximity;
            playerPoximityTrigger.playerEnteredTrigger -= AddPlayerPositionToCondition;
            playerPoximityTrigger.playerExitedTrigger -= RemovePlayerPositionFromCondition;
        }

        if (bodyCollisionTriggers != null)
        {
            for (int i = 0; i < bodyCollisionTriggers.Length; ++i)
            {
                if (bodyCollisionTriggers[i] != null)
                {
                    bodyCollisionTriggers[i].playerEnteredTrigger -= OnBodyCollision;
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHingeMovementController playerController = other.transform.root.GetComponentInChildren<PlayerHingeMovementController>();
        if (playerController != null)
        {
            if (ReplaceWithRagdoll(ragDollPrefab, out currentRagdoll))
            {
                currentRagdoll.AddForceToRagdollAllRigidbody(playerController.ModelRB.velocity * 2.5f, ForceMode.Impulse);
                //currentRagdoll.StartCoroutine(testRest());
            }
            StartCoroutine(LoopingResetCheck());
            this.enabled = false;
            characterMeshRenderer.enabled = false;
            ragdollData.HatObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //toCloseToPlayerCondition = new ToCloseToPlayerCondition();
        //idleNpcState = new NpcIdleState(this);
        //divingState = new NpcReactionState(this);

        //stateMachine = new FiniteStateMachine(idleNpcState);
        
    }

    // Update is called once per frame
    void Update()
    {
        //stateMachine.RunMachine(Time.deltaTime);

        //if (toCloseToPlayerCondition.isConditionTrue())
        //{
        //    animator.SetTrigger("PlayerPoximity");
        //}

        if (isReacting)
        {
            // Check if the NPC Is still reacting otherwise it should be ready to react
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (NPCResetingFromReaction != null)
                {
                    NPCResetingFromReaction(this);
                }
                isReacting = false;
            }
        }
    }

    void FixedUpdate()
    {
        //stateMachine.RunPhysicsOnMachine(Time.deltaTime);

        // Gravity
        //npcCharacyerController.Move(Vector3.down * Time.deltaTime * 10f);
    }

    private void AddPlayerPositionToCondition(PlayerController playerController)
    {
        //toCloseToPlayerCondition.PlayerEnteredRadius(playerController);
    }

    private void RemovePlayerPositionFromCondition(PlayerController playerController)
    {
        //toCloseToPlayerCondition.PlayerExitedRadius(playerController);
    }

    private void OnPlayerEnteredProximity(PlayerController playerController)
    {
        // Check if the NPC Is still reacting otherwise it should be ready to react
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            if (NPCReactingToPlayerProixmity != null)
            {
                NPCReactingToPlayerProixmity(this);
            }

            animator.SetTrigger("PlayerPoximity");
            isReacting = true;
        }
    }

    private bool ReplaceWithRagdoll(GameObject ragDollPrefab, out SpawnedRagdoll spawnedRagdoll)
    {
        // Spawn the rag-doll
        GameObject ragDoll = GameObject.Instantiate(ragDollPrefab, transform.position, transform.rotation);

        if (ragDoll.TryGetComponent<SpawnedRagdoll>(out spawnedRagdoll))
        {
            spawnedRagdoll.Initalise(ragdollData);
            ragdollMeshRenderer = ragDoll.GetComponentInChildren<SkinnedMeshRenderer>();

            return spawnedRagdoll;
        }

        return false;
    }

    public void ResetNPCFromRagdoll()
    {
        if (currentRagdoll != null)
        {
            //Clean up the current rag-doll
            Destroy(currentRagdoll.gameObject);
        }

        // re-enable NPC and reset animations
        gameObject.SetActive(true);
        this.enabled = true;
        characterMeshRenderer.enabled = true;
        ragdollData.HatObject.SetActive(true);
        animator.Play("Idle", -1, 0f);
    }

    public IEnumerator testRest()
    {
        yield return new WaitForSeconds(3f);
        ResetNPCFromRagdoll();
    }

    private void OnBodyCollision(PlayerController playerController)
    {
        // Convert The player controller to a hinge controller
        PlayerHingeMovementController hingeController = (PlayerHingeMovementController)playerController;
        if (hingeController != null)
        {
            if (ReplaceWithRagdoll(ragDollPrefab, out currentRagdoll))
            {
                currentRagdoll.AddForceToRagdollAllRigidbody(hingeController.ModelRB.velocity * 2.5f, ForceMode.Impulse);
                //currentRagdoll.StartCoroutine(testRest());
            }
            StartCoroutine(LoopingResetCheck());
            this.enabled = false;
            characterMeshRenderer.enabled = false;
            ragdollData.HatObject.SetActive(false);
        }
    }

    private IEnumerator LoopingResetCheck()
    {
        while (true)
        {

            if (ragdollMeshRenderer != null)
            {
                if (!characterMeshRenderer.isVisible)
                {
                    if (!ragdollMeshRenderer.isVisible)
                    {
                        // Check for player colliders in radius of ragdoll
                        if (Physics.OverlapSphere(currentRagdoll.PrimaryRigidBody.position, resetCheckDistance, playerLayerMask, QueryTriggerInteraction.Ignore).Length == 0)
                        {
                            // Check for player colliders in radius of NPC main position
                            if (Physics.OverlapSphere(gameObject.transform.position, resetCheckDistance, playerLayerMask, QueryTriggerInteraction.Ignore).Length == 0)
                            {
                                ResetNPCFromRagdoll();
                                break;
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
