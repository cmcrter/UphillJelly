//========================================================================================================================================================================================================================
//  Name:               IdleReactionNPC.cs
//  Authors:            Matthew Mason
//  Date Created:       29/03/2022
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 29/03/2022
//  Brief:              The Script control an NPC that will perform and Idle animation, react to player proximity, and rag doll if the player gets too close
//========================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;
using L7Games;

/// <summary>
/// The Script control an NPC that will perform and Idle animation, react to player proximity, and rag doll if the player gets too close
/// </summary>
public class IdleReactionNPC : MonoBehaviour
{
    #region Public Variables
    public PlayerPoximityTrigger playerPoximityTrigger;

    public GameObject currentRagdoll;

    public Animator animator;

    public float speed;

    public float diveDuration;
    #endregion

    #region Private Serialized Fields
    [Tooltip("The prefab that is spawned to replace this as a Ragdoll used prefab used")]
    [SerializeField]
    private GameObject ragDollPrefab;

    [Tooltip("The data to pass to the ragdoll when it spawns")]
    [SerializeField]
    private RagdollDataContainer ragdollData;
    #endregion

    private void OnEnable()
    {
        if (playerPoximityTrigger != null)
        {
            playerPoximityTrigger.playerEnteredTrigger += OnPlayerEnteredProximity;
            playerPoximityTrigger.playerEnteredTrigger += AddPlayerPositionToCondition;
            playerPoximityTrigger.playerExitedTrigger += RemovePlayerPositionFromCondition;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHingeMovementController playerController = other.transform.root.GetComponentInChildren<PlayerHingeMovementController>();
        if (playerController != null)
        {
            
            currentRagdoll = ReplaceWithRagdoll(ragDollPrefab);
            if (currentRagdoll.TryGetComponent<SpawnedRagdoll>(out SpawnedRagdoll spawnedRagdoll))
            {
                spawnedRagdoll.AddForceToRagdollAllRigidbody(playerController.ModelRB.velocity * 2.5f, ForceMode.Impulse);
            }
            gameObject.SetActive(false);
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
    }

    void FixedUpdate()
    {
        //stateMachine.RunPhysicsOnMachine(Time.deltaTime);

        // Gravity
        //npcCharacyerController.Move(Vector3.down * Time.deltaTime * 10f);
    }

    private void AddPlayerPositionToCondition(PlayerHingeMovementController playerController)
    {
        //toCloseToPlayerCondition.PlayerEnteredRadius(playerController);
    }

    private void RemovePlayerPositionFromCondition(PlayerHingeMovementController playerController)
    {
        //toCloseToPlayerCondition.PlayerExitedRadius(playerController);
    }

    private void OnPlayerEnteredProximity(PlayerHingeMovementController playerController)
    {
        // Check if the NPC Is still reacting otherwise it should be ready to react
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            animator.SetTrigger("PlayerPoximity");
        }
    }

    private GameObject ReplaceWithRagdoll(GameObject ragDollPrefab)
    {
        // Spawn the rag-doll
        GameObject ragDoll = GameObject.Instantiate(ragDollPrefab, transform.position, transform.rotation);
        if (ragDoll.TryGetComponent<SpawnedRagdoll>(out SpawnedRagdoll spawnedRagdoll))
        {
            spawnedRagdoll.Initalise(ragdollData);

        }

        currentRagdoll = ragDoll;
        return ragDoll;
    }
}
