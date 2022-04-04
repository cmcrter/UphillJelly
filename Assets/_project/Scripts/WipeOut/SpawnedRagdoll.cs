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

    [SerializeField]
    private Rigidbody primaryRigidBody;

    [SerializeField]
    private Rigidbody[] allRigidbody;
    #endregion

    #region Private Variables
    /// <summary>
    /// The player character control that this ragdoll was spawned from
    /// </summary>
    private PlayerController playerSpawnedFrom;
    #endregion

    #region Public Properties
    public Rigidbody PrimaryRigidBody
    {
        get
        {
            return primaryRigidBody;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Called to destroy the ragdoll this is attached to properly 
    /// </summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Called to initialise the ragdoll when its spawned
    /// </summary>
    /// <param name="EventToDestroyOn">The event that will trigger the players ragdoll's destruction, can be left null if the ragdoll will not be destruction</param>
    /// <param name="ragdollSpawningData">The player that will have started the spawning of the ragdoll</param>
    public void Initalise(RagdollDataContainer ragdollSpawningData)
    {
        if (ragdollData == null)
        {
            ragdollData = GetComponent<RagdollDataContainer>();
        }
        //if (EventToDestroyOn != null)
        //{
        //    EventToDestroyOn += DestroySelf;
        //    //this.playerSpawnedFrom = playerSpawnedFrom;
        //    //this.playerSpawnedFrom.onRespawn += DestroySelf;
        //}
        //ragdollData.CopyRagdollBonesPositions(ragdollSpawningData);

        // Copy the characters mesh and materials across
        ragdollData.characterRenderer.sharedMesh = ragdollSpawningData.characterRenderer.sharedMesh;
        ragdollData.characterRenderer.materials = ragdollSpawningData.characterRenderer.materials;

        // Copy animation position
        ragdollData.attachedAnimator.avatar = ragdollSpawningData.attachedAnimator.avatar;

        for (int i = 0; i < ragdollSpawningData.attachedAnimator.layerCount; ++i)
        {
            AnimatorStateInfo animatorStateInfo = ragdollSpawningData.attachedAnimator.GetCurrentAnimatorStateInfo(i);
            tempAnimator.Play(animatorStateInfo.shortNameHash, i, animatorStateInfo.normalizedTime);
        }
        StartCoroutine(DestoryAnimator());

    }

    public void AddForceToRagdollMainRigidbody(Vector3 force, ForceMode forceMode)
    {
        primaryRigidBody.AddForce(force, forceMode);
    }

    public void AddForceToRagdollAllRigidbody(Vector3 force, ForceMode forceMode)
    {
        for (int i = 0; i < allRigidbody.Length; ++i)
        {
            allRigidbody[i].AddForce(force, forceMode);
        }
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
