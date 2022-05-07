//================================================================================================================================================================================================================================================================================================================================================
// File:                CharacterBone.cs
// Author:              Matthew Mason, Charles Carter
// Date Created:        15/02/22
// Last Edited By:      Charles Carter
// Date Last Edited:    06/05/22
// Brief:               A script used to store information about all the bones in a ragdoll
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games
{
    public class RagdollDataContainer : MonoBehaviour
    {
        public Animator attachedAnimator;
        public SkinnedMeshRenderer characterRenderer;
        public Transform HatParent;
        public GameObject HatObject;
    
        //public struct Bones
        //{
        //    public Vector3 position;
        //    public Quaternion rotation;
        //    public Vector3 scale;
        //    public GameObject gameObject;
        //    //public Collider collider;
        //    //public Rigidbody rigidbody;

        //    public Bones(Vector3 position, Quaternion rotation, Vector3 scale, GameObject gameObject)//, Collider collider)//, Rigidbody rigidbody)
        //    {
        //        this.position = position;
        //        this.rotation = rotation;
        //        this.scale = scale;
        //        this.gameObject = gameObject;
        //        //this.collider = collider;
        //        //this.rigidbody = rigidbody;
        //    }
        //}

        //public List<GameObject> bones;

        //public void CopyRagdollBonesPositions(RagdollDataContainer otherRagdollDataContainer)
        //{
        //    // Joint bones should be equal or something has probably gone wrong
        //    #if UNITY_EDITOR || DEBUG
        //    if (otherRagdollDataContainer.bones.Count != bones.Count)
        //    {
        //        Debug.LogWarning("Character and rag-doll bones are not equal");
        //    }
        //    #endif
        //    for (int i = 0; i < otherRagdollDataContainer.bones.Count && i < bones.Count; ++i)
        //    {
        //        otherRagdollDataContainer.bones[i].transform.localPosition = bones[i].transform.localPosition;
        //        otherRagdollDataContainer.bones[i].transform.localRotation = bones[i].transform.localRotation;
        //        otherRagdollDataContainer.bones[i].transform.localScale = bones[i].transform.localScale;
        //    }
        //}
    }

}


