using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;
using L7Games.Movement;

public class NPCBrain : MonoBehaviour
{
    private FiniteStateMachine stateMachine;

    [SerializeField]
    public ToCloseToPlayerCondition toCloseToPlayerCondition;

    public NpcDivingState divingState;

    public NpcIdleState idleNpcState;

    public PlayerPoximityTrigger playerPoximityTrigger;

    public CharacterController npcCharacyerController;

    public Animator animator;

    public float speed;

    public float diveDuration;

    private void OnEnable()
    {
        if (playerPoximityTrigger != null)
        {
            playerPoximityTrigger.playerEnteredTrigger += AddPlayerPositionToCondition;
            playerPoximityTrigger.playerExitedTrigger += AddPlayerPositionToCondition;
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
            playerPoximityTrigger.playerEnteredTrigger -= AddPlayerPositionToCondition;
            playerPoximityTrigger.playerExitedTrigger -= AddPlayerPositionToCondition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        toCloseToPlayerCondition = new ToCloseToPlayerCondition();
        idleNpcState = new NpcIdleState(this);
        divingState = new NpcDivingState(this);

        stateMachine = new FiniteStateMachine(idleNpcState);

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.RunMachine(Time.deltaTime);
    }

    void FixedUpdate()
    {
        stateMachine.RunPhysicsOnMachine(Time.deltaTime);
    }

    private void AddPlayerPositionToCondition(PlayerController playerController)
    {
        toCloseToPlayerCondition.PlayerEnteredRadius(playerController);
    }

    private void RemovePlayerPositionFromCondition(PlayerController playerController)
    {
        toCloseToPlayerCondition.PlayerExitedRadius(playerController);
    }


}
