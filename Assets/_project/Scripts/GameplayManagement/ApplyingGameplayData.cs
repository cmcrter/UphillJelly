////////////////////////////////////////////////////////////
// File: ApplyingGameplayData.cs
// Author: Charles Carter
// Date Created: 06/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: A place to see the currently loaded data, and also tell the relevant scripts to load it on awake
//////////////////////////////////////////////////////////// 

using L7Games.Loading;
using UnityEngine;

public class ApplyingGameplayData : MonoBehaviour
{
    #region Variables

    public static ApplyingGameplayData instance;

    public LoadCustomizablesInGame loadcustomizables;
    public ReplaySaveManager ghostManager;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if(loadcustomizables != null)
        {
            loadcustomizables.ApplyCustomization();
        }

        if(LoadingData.playerSlot != 0)
        {
            ghostManager.LoadReplay(LoadingData.playerSlot, 1);
        }
    }

    #endregion
 
    #region Private Methods
    #endregion
}
