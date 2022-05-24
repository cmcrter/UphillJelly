//========================================================================================================================================================================================================================================================================
// File:                PauseMenuController.cs
// Author:              Matthew Mason
// Date Created:        03/05/2022
// Last Edited By:      Matthew Mason
// Date Last Edited:    03/05/2022
// Brief:               A MonoBehaviour-Singleton used to control the pause state of the game and act as a single point of interaction for pause functionality
//========================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    /// <summary>
    /// the single instance of this class
    /// </summary>
    public static PauseManager instance;

    /// <summary>
    /// Checks if there is an instance that is paused, if there is no instance there is no pause
    /// </summary>
    public static bool IsInstancePaused
    {
        get
        {
            if (instance == null)
            {
                return false;
            }
            return instance.IsPaused;
        }
    }

    /// <summary>
    /// The number of different elements in the game that are wanting it to be paused
    /// </summary>
    private int pausingFactorsCount;

    /// <summary>
    /// The time scale before the game was paused
    /// </summary>
    private float originalTimeScale;

    public bool IsPaused
    {
        get
        {
            return pausingFactorsCount > 0;
        }
    }

    #region Public Events
    public System.Action OnPaused;

    public System.Action OnUnpaused;
    #endregion

    #region Public Method
    /// <summary>
    /// Pauses the gameplay and brings up the pause menu
    /// </summary>
    public void PauseGame()
    {
        if (!IsPaused)
        {
            // Check if the game is not already somehow paused
            if (Time.timeScale > 0f)
            {
                // Record the current time scale
                originalTimeScale = Time.timeScale;
                // Set the time scale to zero so nothing will move in the game
                Time.timeScale = 0f;

                ++pausingFactorsCount;
                if (OnPaused != null)
                {
                    OnPaused();
                }

            }
        }
        else
        {
            ++pausingFactorsCount;
        }
    }

    /// <summary>
    /// Unpauses the game and closes the pause menu
    /// </summary>
    public void UnpauseGame()
    {
        if (IsPaused)
        {
            // Check if the game is not already unpaused
            if (!(Time.timeScale > 0f) && originalTimeScale > 0f)
            {
                // Set the time scale to what it was before the pause
                Time.timeScale = originalTimeScale;

                if (pausingFactorsCount > 0)
                {
                    --pausingFactorsCount;
                }

                if (OnUnpaused != null)
                {
                    OnUnpaused();
                }
            }
        }
    }
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Debug.LogError("Another instance of a pause manager already exist on the" + "\"" + instance.gameObject + "\"" + " GameObject, this instance will be destroyed", instance);
                Destroy(this);
            }
        }
        else
        {
            instance = this;
        }
    }
}
