//==================================================================================================================================================================================================================================================
// File:                PlayerAnimationEventHandler.cs
// Author:              Matthew Mason
// Date Created:        01/04/22
// Last Edited By:      Matthew Mason
// Date Last Edited:    01/04/22
// Brief:               Script that receives and distributes animation events from player
//==================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that receives and distributes animation events from player
/// </summary>
public class PlayerAnimationEventHandler : MonoBehaviour
{
    #region Public Events
    /// <summary>
    /// Event called when of the "TrickEnded" animation events are triggered
    /// </summary>
    public event System.Action OnTrickAnimationEnded;
    #endregion

    /// <summary>
    /// Called by unity when the "TrickEnded" animation events are triggered 
    /// </summary>
    public void TrickEnded()
    {
        if (OnTrickAnimationEnded != null)
        {
            OnTrickAnimationEnded();
        }
    }
}
