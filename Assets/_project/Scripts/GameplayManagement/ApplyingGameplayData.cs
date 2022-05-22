////////////////////////////////////////////////////////////
// File: ApplyingGameplayData.cs
// Author: Charles Carter
// Date Created: 06/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 22/05/22
// Brief: A place to see the currently loaded data, and also tell the relevant scripts to load it on awake
//////////////////////////////////////////////////////////// 

using L7Games.Loading;
using UnityEngine;

public class ApplyingGameplayData : MonoBehaviour
{
    #region Variables

    [Header("General Information")]
    public static ApplyingGameplayData instance;

    public int playerSlot;
    public StoredPlayerProfile loadingData;

    [Header("Overriding Values")]
    [SerializeField]
    private bool overrideLevel; 
    [SerializeField]
    private LEVEL NewLevel = LEVEL.TUTORIAL;

    [Header("Applying Classes")]
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

        //Getting the relevant info
        loadingData = LoadingData.player;
        playerSlot = LoadingData.playerSlot;

        //GOverriding if need be
        if(overrideLevel)
        {
            LoadingData.currentLevel = NewLevel;
        }

        //Using the applying classes
        if(loadcustomizables != null)
        {
            loadcustomizables.ApplyCustomization();
        }

        //if(LoadingData.playerSlot != 0)
        //{
        //    ghostManager.LoadReplay(LoadingData.playerSlot, 1);
        //}
    }

    #endregion
 
    #region Private Methods
    #endregion
}
