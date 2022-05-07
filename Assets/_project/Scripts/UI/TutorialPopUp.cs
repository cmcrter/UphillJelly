using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using L7Games.Input;

public class TutorialPopUp : MonoBehaviour
{
    public enum InputType
    {
        Trick,
        Jump,
        Crouch,
        Reset,
        Pause,
        Push,
        Brake,
    }

    [SerializeField]
    private List<InputType> inputTypes;

    private InputHandler playerInput; 

    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindObjectOfType<InputHandler>();

        for (int i = 0; i < inputTypes.Count; ++i)
        {
            switch (inputTypes[i])
            {
                case InputType.Trick:
                    playerInput.trickPressed += OnActionPerformed;
                    playerInput.TrickInputActionCalled += OnActionPerformed;
                    break;
                case InputType.Jump:
                    playerInput.grindingJumpUpActionPerformed += OnActionPerformed;
                    playerInput.wallRidingJumpUpAction += OnActionPerformed;
                    //playerInput.groundedJumpUpPerformed += OnActionPerformed; Think this one is obsolete
                    playerInput.pressDownEnded += OnActionPerformed;
                    break;
                case InputType.Crouch:
                    playerInput.pressDownStarted += OnActionPerformed;
                    break;
                case InputType.Reset:
                    playerInput.wipeoutResetStarted += OnActionPerformed;
                    break;
                case InputType.Pause:
                    playerInput.PauseActionPerformed += OnActionPerformed;
                    break;
                case InputType.Push:
                    playerInput.pushStarted += OnActionPerformed;
                    break;
                case InputType.Brake:
                    playerInput.brakeStarted += OnActionPerformed;
                    break;
                default:
                    break;
            }
        }

        PauseManager.instance.PauseGame();
    }

    private void OnActionPerformed()
    {
        PauseManager.instance.UnpauseGame();
        Destroy(gameObject);
    }
}