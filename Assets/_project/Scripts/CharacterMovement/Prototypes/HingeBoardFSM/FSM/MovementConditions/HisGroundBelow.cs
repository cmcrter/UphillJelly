////////////////////////////////////////////////////////////
// File: isGroundBelow.cs
// Author: Charles Carter
// Date Created: 10/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/01/22
// Brief: A condition for changing movement states, whether it's on the ground or not
//////////////////////////////////////////////////////////// 

using System;
using UnityEngine;
using L7Games.Utility.StateMachine;

namespace L7Games.Movement
{
    [Serializable]
    public class HisGroundBelow : Condition
    {
        #region Variables

        public LayerMask playerMask;
        public RaycastHit FrontGroundHit;
        public RaycastHit BackGroundHit;

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

        public HisGroundBelow()
        {
        }

        public void InitialiseCondition(Transform player)
        {
            playerTransform = player;
        }

        public override bool isConditionTrue() 
        {
            //Checking if anything is below it
            if(Physics.Raycast(frontraycastPointTransform.position, -Vector3.up, out RaycastHit hit, 25f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if(Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(frontraycastPointTransform.position, frontraycastPointTransform.position + (-Vector3.up * groundDist), Color.magenta);
                    Debug.DrawLine(backRaycastPointTransform.position, backRaycastPointTransform.position + (-Vector3.up * groundDist), Color.magenta);

                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }

                FrontGroundHit = hit;
            }

            if(Physics.Raycast(backRaycastPointTransform.position, -Vector3.up, out RaycastHit backHit, 25f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Could use backhit to help smoothing with the board against the ground
                //Player is about to hit a ramp
                BackGroundHit = backHit;
            }

            //This hit may still be used for smoothing when the player is in the air
            if(BackGroundHit.distance <= groundDist || FrontGroundHit.distance <= groundDist)
            {
                return true;
            }

            return false;         
        }

        #endregion
    }
}
