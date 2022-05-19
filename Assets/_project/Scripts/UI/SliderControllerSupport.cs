using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using L7Games.Input;

public class SliderControllerSupport : MonoBehaviour
{
    [SerializeField]
    private Slider sliderControlled;
    [SerializeField]
    private Animator animator;

    private EventSystem sceneEventSystem;
    private InputHandler[] inputHandlers;


    private Selectable leftNavigationTarget;
    private Selectable rightNavigationTarget;

    private bool isSliderBeingControlled = false;

    private void Awake()
    {
        leftNavigationTarget = sliderControlled.navigation.selectOnLeft;
        rightNavigationTarget = sliderControlled.navigation.selectOnRight;

        UpdateInputHandlersFromScene();

        sceneEventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < inputHandlers.Length; ++i)
        {
            inputHandlers[i].MenuConfirmedPerformed += InputHandler_MenuConfirmedPerformed;
            inputHandlers[i].MenuCancelPerformed += InputHandler_MenuCancelPerformed;
        }
        // Get the input handlers and event systems in the scene 
    }

    private void OnDisable()
    {
        for (int i = 0; i < inputHandlers.Length; ++i)
        {
            inputHandlers[i].MenuConfirmedPerformed -= InputHandler_MenuConfirmedPerformed;
            inputHandlers[i].MenuCancelPerformed -= InputHandler_MenuCancelPerformed;
        }
    }



    private void Update()
    {
        if (isSliderBeingControlled)
        {
            if (sceneEventSystem.currentSelectedGameObject != sliderControlled.gameObject)
            {
                Debug.Log("Selected Object is: " + sceneEventSystem.currentSelectedGameObject.name);
                RemoveControl();
            }
        }
    }

    private void InputHandler_MenuConfirmedPerformed()
    {
        Debug.Log("Confirm Pressed");
        if (sceneEventSystem.currentSelectedGameObject == sliderControlled.gameObject)
        {
            Debug.Log("Confirm Pressed 2");
            if (!isSliderBeingControlled)
            {
                SetupControl();
            }
            else
            {
                RemoveControl();
            }
        }
    }

    private void InputHandler_MenuCancelPerformed()
    {
        if (sceneEventSystem.currentSelectedGameObject == sliderControlled.gameObject)
        {
            RemoveControl();
        }
    }

    private void SetupControl()
    {
        Debug.Log("Control Setup");
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = sliderControlled.navigation.selectOnDown;
        navigation.selectOnLeft = null;
        navigation.selectOnRight = null;
        navigation.selectOnUp = sliderControlled.navigation.selectOnUp;
        sliderControlled.navigation = navigation;
        isSliderBeingControlled = true;
        animator.SetTrigger("BeingMoved");
    }

    private void RemoveControl()
    {
        Debug.Log("Control removed");
        Navigation navigation = new Navigation();
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = sliderControlled.navigation.selectOnDown;
        navigation.selectOnLeft = leftNavigationTarget;
        navigation.selectOnRight = rightNavigationTarget;
        navigation.selectOnUp = sliderControlled.navigation.selectOnUp;
        sliderControlled.navigation = navigation;
        isSliderBeingControlled = false;
    }

    /// <summary>
    /// Check through the scene for all the input handlers 
    /// </summary>
    private void UpdateInputHandlersFromScene()
    {
        inputHandlers = FindObjectsOfType<InputHandler>();
        #if DEBUG || UNITY_EDITOR
        if (inputHandlers.Length == 0)
        {
            Debug.LogWarning("No input handlers could be found in scene by the pause menu controller");
        }
        #endif
    }
}
