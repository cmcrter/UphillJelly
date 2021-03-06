////////////////////////////////////////////////////////////
// File: isGroundBelow.cs
// Author: Charles Carter
// Date Created: 10/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/11/21
// Brief: A condition for changing movement states, whether it's on the ground or not
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using L7Games.Utility.StateMachine;

namespace L7Games.Movement
{
    [Serializable]
    public class isGroundBelow : Condition
    {
        #region Variables

        public LayerMask playerMask;
        public RaycastHit GroundHit;
        public RaycastHit FrontGroundHit;

        private Transform playerTransform;
        [SerializeField]
        private Transform frontraycastPointTransform;
        [SerializeField]
        private Transform backRaycastPointTransform;

        [SerializeField]
        private float groundDist = 0.09f;
        [SerializeField]
        private float rampDist = 0.6f;

        #endregion

        #region Public Methods

        public isGroundBelow()
        {
        }

        public void InitialiseCondition(Transform player)
        {
            playerTransform = player;
        }

        public override bool isConditionTrue() 
        {
            FrontGroundHit.normal = Vector3.zero;

            //Checking if anything is below it
            if(Physics.Raycast(frontraycastPointTransform.position, -playerTransform.up, out RaycastHit hit, 25f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if(Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(frontraycastPointTransform.position, frontraycastPointTransform.position + (-playerTransform.up * groundDist), Color.magenta);
                    Debug.DrawLine(frontraycastPointTransform.position, frontraycastPointTransform.position + (playerTransform.forward * rampDist), Color.magenta);

                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }

                if(Physics.Raycast(frontraycastPointTransform.position, playerTransform.forward, out RaycastHit fronthit, rampDist, ~playerMask, QueryTriggerInteraction.Ignore))
                {
                    //Player is about to hit a ramp
                    FrontGroundHit = fronthit;
                }

                GroundHit = hit;

                //This hit may still be used for smoothing when the player is in the air
                if(hit.distance <= groundDist)
                {
                    return true;
                }
            }

            if(Physics.Raycast(backRaycastPointTransform.position, -playerTransform.up, out RaycastHit backHit, 25f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Could use backhit to help smoothing with the board against the ground

                //This hit may mean the board is still on some ground
                if(backHit.distance <= groundDist)
                {
                    return true;
                }
            }

            return false;         
        }

        #endregion
    }
}
