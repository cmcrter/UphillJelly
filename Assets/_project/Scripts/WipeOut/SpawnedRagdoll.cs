//================================================================================================================================================================================================================================================================================================================================================
// File:                SpawnedRagdoll.cs
// Author:              Matthew Mason
// Date Created:        19/02/22
// Last Edited By:      Matthew Mason
// Date Last Edited:    19/02/22
// Brief:               Script attached to the Rag-doll when they are spawned by the player 
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games;
using L7Games.Movement;

/// <summary>
/// Script attached to the Rag-doll when they are spawned by the player 
/// </summary>
[RequireComponent(typeof(RagdollDataContainer))]
public class SpawnedRagdoll : MonoBehaviour
{
    #region Private Serialized Fields Variables
    /// <summary>
    /// The ragdollData attached to the same ragdoll as this
    /// </summary>
    [SerializeField]
    private RagdollDataContainer ragdollData;

    [SerializeField]
    private Animator tempAnimator;
    #endregion

    #region Private Variables
    /// <summary>
    /// The player character control that this ragdoll was spawned from
    /// </summary>
    private PlayerController playerSpawnedFrom;
    #endregion

    #region Public Methods
    /// <summary>
    /// Called to destroy the ragdoll this is attached to properly 
    /// </summary>
    public void DestroySelf()
    {
        if (playerSpawnedFrom != null)
        {
            this.playerSpawnedFrom.onRespawn -= DestroySelf;
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Called to initialise the ragdoll when its spawned
    /// </summary>
    /// <param name="playerSpawnedFrom">The player that will have started the spawning of the ragdoll</param>
    /// <param name="ragdollSpawningData">The player that will have started the spawning of the ragdoll</param>
    public void Initalise(PlayerController playerSpawnedFrom, RagdollDataContainer ragdollSpawningData, Animator orignalAnimator)
    {
        if (ragdollData == null)
        {
            ragdollData = GetComponent<RagdollDataContainer>();
        }
        if (playerSpawnedFrom != null)
        {
            this.playerSpawnedFrom = playerSpawnedFrom;
            this.playerSpawnedFrom.onRespawn += DestroySelf;
        }
        ragdollData.CopyRagdollBonesPositions(ragdollSpawningData);

        // Copy animation position
        tempAnimator.avatar = orignalAnimator.avatar;

        for (int i = 0; i < orignalAnimator.layerCount; ++i)
        {
            AnimatorStateInfo animatorStateInfo = orignalAnimator.GetCurrentAnimatorStateInfo(i);
            tempAnimator.Play(animatorStateInfo.shortNameHash, i, animatorStateInfo.normalizedTime);
        }
        StartCoroutine(DestoryAnimator());
    }

    private IEnumerator DestoryAnimator()
    {
        yield return new WaitForEndOfFrame();
        Destroy(tempAnimator);
    }
    #endregion

    #region Unity Methods
    public void Start()
    {
        if (ragdollData == null)
        {
            ragdollData = GetComponent<RagdollDataContainer>();
        }
    }
    #endregion
}
