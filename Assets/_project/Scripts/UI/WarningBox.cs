//========================================================================================================================================================================================================================================================================
// File:                WarningBox.cs
// Author:              Matthew Mason
// Date Created:        03/05/2022
// Last Edited By:      Matthew Mason
// Date Last Edited:    03/05/2022
// Brief:               Warning box used to warn the players of risks of a choice in the UI before it is made
//========================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using L7Games.Input;

public class WarningBox : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private Button buttonPrefab;

    private LayoutGroup buttonsParentGroup;

    private Button confirmButton;

    private Button cancelButton;

    private InputHandler[] inputHandlersInScene;

    #region Unity Methods
    public void Start()
    {
        // Find all the players that are added to the scene
        if (inputHandlersInScene == null)
        {
            UpdateInputHandlersFromScene();
        }
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

    public void SetText(string warningBoxText)
    {
        if (textMesh != null)
        {
            textMesh.text = warningBoxText;
        }
    }

    public Button AddButton(string buttonText)
    {
        GameObject gameObjectButton = GameObject.Instantiate(buttonPrefab.gameObject, buttonsParentGroup.transform.parent);
        Button newButton = gameObjectButton.GetComponent<Button>();
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        return newButton;
    }

    public Button AddConfirmButton(string buttonText)
    {
        confirmButton = AddButton(buttonText);
        return confirmButton;
    }

    public Button AddCancelButton(string buttonText)
    {
        cancelButton = AddButton(buttonText);
        return cancelButton;
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
}
