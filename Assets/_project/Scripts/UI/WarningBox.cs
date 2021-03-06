//========================================================================================================================================================================================================================================================================
// File:                WarningBox.cs
// Author:              Matthew Mason
// Date Created:        03/05/2022
// Last Edited By:      Matthew Mason
// Date Last Edited:    04/05/2022
// Brief:               Warning box used to warn the players of risks of a choice in the UI before it is made
//========================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using L7Games.Input;

/// <summary>
/// Warning box used to warn the players of risks of a choice in the UI before it is made
/// </summary>
public class WarningBox : MonoBehaviour
{
    #region Private Serialized Fields
    /// <summary>
    /// The prefab used to instantiate buttons
    /// </summary>
    [SerializeField]
    [Tooltip("The buttons prefab instantiated for all the buttons")]
    private Button buttonPrefab;

    /// <summary>
    /// The layout group controlling the placement of the buttons in the warning box
    /// </summary>
    [SerializeField]
    [Tooltip("The Layout group that the buttons will be given too")]
    private LayoutGroup buttonsParentGroup;

    /// <summary>
    /// The UI text mesh controlling the text warning shown to the user
    /// </summary>
    [SerializeField]
    [Tooltip("The UI text mesh showing the warning text")]
    private TextMeshProUGUI textMesh;
    #endregion

    #region Private Variables
    /// <summary>
    /// The button that will respond to cancel inputs
    /// </summary>
    private Button cancelButton;
    /// <summary>
    /// The button that will respond to confirm inputs
    /// </summary>
    private Button confirmButton;

    /// <summary>
    /// The buttons added to the box
    /// </summary>
    private List<Button> boxButtons;

    /// <summary>
    /// The Event system in the scene
    /// </summary>
    private UnityEngine.EventSystems.EventSystem eventSystem;

    /// <summary>
    /// The input handlers that are found in the scene 
    /// </summary>
    private InputHandler[] inputHandlersInScene;
    #endregion

    #region Unity Methods
    public void Start()
    {
        // Find all the players that are added to the scene
        if (inputHandlersInScene == null)
        {
            UpdateInputHandlersFromScene();
        }
        boxButtons = new List<Button>();
    }
    public void OnEnable()
    {
        // Find all the players that are added to the scene if they are not already
        if (inputHandlersInScene == null)
        {
            UpdateInputHandlersFromScene();
        }

        for (int i = 0; i < inputHandlersInScene.Length; ++i)
        {
        }

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        }

    }

    public void OnDisable()
    {
        if (inputHandlersInScene != null)
        {
            for (int i = 0; i < inputHandlersInScene.Length; ++i)
            {
            }
        }
    }
    #endregion

    #region Public Methods
    #region Static Methods
    /// <summary>
    /// Creates a warning box by loading a prefab of one from resources, will have no text or buttons until added
    /// </summary>
    /// <param name="canvasShownOn">The Canvas that the warning box will be spawned and shown from</param>
    /// <param name="eventSystemInScene">The event system controlling all the UI events in the scene</param>
    /// <returns>The new warning box component attached to the newly spawned UI GameObject</returns>
    public static WarningBox CreateWarningBox(Canvas canvasShownOn, EventSystem eventSystemInScene, string boxText)
    {
        GameObject warningBoxPrefab = GameObject.Instantiate(Resources.Load<GameObject>("WarningBox"), canvasShownOn.transform);
        WarningBox returnedWarningBox = warningBoxPrefab.GetComponent<WarningBox>();
        returnedWarningBox.eventSystem = eventSystemInScene;
        returnedWarningBox.SetText(boxText);
        return returnedWarningBox;
    }

    /// <summary>
    /// Creates a warning box that has only one button for confirming and closing the message
    /// </summary>
    /// <param name="canvasShownOn">The Canvas that the warning box will be spawned and shown from</param>
    /// <param name="eventSystemInScene">The event system controlling all the UI events in the scene</param>
    /// <param name="boxText">The text that will be shown in the box</param>
    /// <param name="confirmAction">The action that will happen once the confirm button is pressed</param>
    /// <param name="confirmButtonText">The text that will be shown on the confirm button</param>
    /// <returns>The new warning box component attached to the newly spawned UI GameObject</returns>
    public static WarningBox CreateConfirmOnlyWarningBox(Canvas canvasShownOn, EventSystem eventSystemInScene, 
        string boxText, UnityAction confirmAction, string confirmButtonText = "Confirm")
    {
        WarningBox newWarningBox = CreateWarningBox(canvasShownOn, eventSystemInScene, boxText);
        Button confirmButton = newWarningBox.AddConfirmButton(confirmButtonText);
        confirmButton.onClick.AddListener(confirmAction);
        confirmButton.onClick.AddListener(newWarningBox.CloseBox);
        return newWarningBox;
    }

    /// <summary>
    /// Creates a warning box that has two buttons for confirming or cancelling the message
    /// </summary>
    /// <param name="canvasShownOn">The Canvas that the warning box will be spawned and shown from</param>
    /// <param name="eventSystemInScene">The event system controlling all the UI events in the scene</param>
    /// <param name="boxText">The text that will be shown in the box</param>
    /// <param name="cancelAction">The action that will happen once the cancel button is pressed</param>
    /// <param name="confirmAction">The action that will happen once the confirm button is pressed</param>
    /// <param name="cancelButtonText">Text that will be shown on the cancel button</param>
    /// <param name="confirmButtonText">Text that will be shown on the confirm button</param>
    /// <returns>The new warning box component attached to the newly spawned UI GameObject</returns>
    public static WarningBox CreateConfirmCancelWarningBox(Canvas canvasShownOn, EventSystem eventSystemInScene, 
        string boxText, UnityAction cancelAction, UnityAction confirmAction, string cancelButtonText = "Cancel", 
        string confirmButtonText = "Confirm")
    {
        WarningBox newWarningBox = CreateWarningBox(canvasShownOn, eventSystemInScene, boxText);

        // Adding the Cancel Buttons
        Button cancelButton = newWarningBox.AddCancelButton(cancelButtonText);
        if (cancelAction != null)
        {
            cancelButton.onClick.AddListener(cancelAction);
        }
        cancelButton.onClick.AddListener(newWarningBox.CloseBox);

        // Adding the Confirm Button
        Button confirmButton = newWarningBox.AddConfirmButton(confirmButtonText);
        if (confirmAction != null)
        {
            confirmButton.onClick.AddListener(confirmAction);
        }
        confirmButton.onClick.AddListener(newWarningBox.CloseBox);

        return newWarningBox;
    }
    #endregion

    /// <summary>
    /// Adds a button to the right of to the ones currently shown on the Warning box.
    /// This returns the button so listener can be added said button as needed.
    /// </summary>
    /// <param name="buttonText">The text used in the new button</param>
    /// <returns>The newly created button</returns>
    public Button AddButton(string buttonText)
    {
        if (boxButtons == null)
        {
            boxButtons = new List<Button>();
        }
        GameObject gameObjectButton = GameObject.Instantiate(buttonPrefab.gameObject, buttonsParentGroup.transform);
        Button newButton = gameObjectButton.GetComponent<Button>();
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        boxButtons.Add(newButton);
        // If the new button is the first one added to the warning box
        // Once the button the navigation is set up so the buttons will navigate the too the buttons to the left and right,
        // with navigation that reaches either end of the box wrapping around to the other side
        if (boxButtons.Count == 1)
        {
            Debug.Log("Event System: " + eventSystem);
            Debug.Log("gameObjectButton: " + gameObjectButton);
            eventSystem.SetSelectedGameObject(gameObjectButton);
            Navigation newNavigation = new Navigation();
            newNavigation.mode = Navigation.Mode.Explicit;
            boxButtons[0].navigation = newNavigation;
        }
        else
        {
            Navigation newNavigation = new Navigation();
            newNavigation.selectOnLeft = boxButtons[boxButtons.Count - 2];
            newNavigation.selectOnRight = boxButtons[0];
            newNavigation.mode = Navigation.Mode.Explicit;
            boxButtons[boxButtons.Count - 1].navigation = newNavigation;

            newNavigation = boxButtons[boxButtons.Count - 2].navigation;
            newNavigation.selectOnRight = boxButtons[boxButtons.Count - 1];
            boxButtons[boxButtons.Count - 2].navigation = newNavigation;

            newNavigation = boxButtons[0].navigation;
            newNavigation.selectOnLeft = boxButtons[boxButtons.Count - 1];
            boxButtons[0].navigation = newNavigation;
        }
        
        return newButton;
    }

    /// <summary>
    /// Added a new button to the right of the current ones that will respond to confirmation input.
    /// This returns the button so listener can be added to said button as needed.
    /// </summary>
    /// <param name="buttonText">The text to use on the new button</param>
    /// <returns>The newly created button</returns>
    public Button AddConfirmButton(string buttonText)
    {
        confirmButton = AddButton(buttonText);
        return confirmButton;
    }

    /// <summary>
    /// Added a new button to the right of the current ones that will respond to confirmation input.
    /// This returns the button so listener can be added to said button as needed.
    /// </summary>
    /// <param name="buttonText">The text to use on the new button</param>
    /// <returns>The newly created button</returns>
    public Button AddCancelButton(string buttonText)
    {
        cancelButton = AddButton(buttonText);
        return cancelButton;
    }

    /// <summary>
    /// Sets the main text of the warning box to a given value
    /// </summary>
    /// <param name="warningBoxText">The text to change the warning box to</param>
    public void SetText(string warningBoxText)
    {
        if (textMesh != null)
        {
            textMesh.text = warningBoxText;
        }
    }
    #endregion

    #region Private Methods
    public void CloseBox()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Check through the scene for all the input handlers 
    /// </summary>
    private void UpdateInputHandlersFromScene()
    {
        inputHandlersInScene = FindObjectsOfType<InputHandler>();
        #if DEBUG || UNITY_EDITOR
        if (inputHandlersInScene.Length == 0)
        {
            Debug.LogWarning("No input handlers could be found in scene by the Warning Box", this);
        }
        #endif
    }
    #endregion
}
