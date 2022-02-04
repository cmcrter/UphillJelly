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
        public RaycastHit FrontLeftGroundHit;
        public RaycastHit FrontRightGroundHit;
        public RaycastHit BackLeftGroundHit;
        public RaycastHit BackRightGroundHit;

        [SerializeField]
        private Transform frontLeftRaycastPointTransform;
        [SerializeField]
        private Transform frontRightRaycastPointTransform;
        [SerializeField]
        private Transform backLeftRaycastPointTransform;
        [SerializeField]
        private Transform backRightRaycastPointTransform;

        [SerializeField]
        private float groundDist = 1.69f;

        #endregion

        #region Public Methods

        public HisGroundBelow()
        {
        }

        public void InitialiseCondition(Transform player)
        {

        }

        public override bool isConditionTrue() 
        {
            //Checking if anything is below it
            if(Physics.Raycast(frontLeftRaycastPointTransform.position, -Vector3.up, out RaycastHit hit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if(Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(frontLeftRaycastPointTransform.position, frontLeftRaycastPointTransform.position + (-Vector3.up * groundDist), Color.magenta);

                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }

                FrontLeftGroundHit = hit;
            }

            //Checking if anything is below it
            if (Physics.Raycast(frontRightRaycastPointTransform.position, -Vector3.up, out RaycastHit hit2, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(frontRightRaycastPointTransform.position, frontRightRaycastPointTransform.position + (-Vector3.up * groundDist), Color.magenta);

                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }

                FrontRightGroundHit = hit2;
            }

            //Checking if anything is below it
            if (Physics.Raycast(backLeftRaycastPointTransform.position, -Vector3.up, out RaycastHit hit3, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    //Debug.Log("Hit: " + hit.transform.name);
                    //Debug.Log(hit.transform.name + " " + hit.normal);
                    Debug.DrawLine(backLeftRaycastPointTransform.position, backLeftRaycastPointTransform.position + (-Vector3.up * groundDist), Color.magenta);

                    //Debug.DrawLine(transform.position, transform.position + (-transform.up * 1f), Color.blue);
                    //Debug.DrawRay(rb.position + rb.centerOfMass, rb.transform.forward, Color.cyan);
                }

                BackLeftGroundHit = hit3;
            }

            if (Physics.Raycast(backRightRaycastPointTransform.position, -Vector3.up, out RaycastHit backHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Could use backhit to help smoothing with the board against the ground
                //Player is about to hit a ramp
                BackRightGroundHit = backHit;
            }

            //This hit may still be used for smoothing when the player is in the air
            if(FrontLeftGroundHit.distance <= groundDist || FrontRightGroundHit.distance <= groundDist || BackLeftGroundHit.distance <= groundDist || BackRightGroundHit.distance <= groundDist)
            {
                return true;
            }

            return false;         
        }

        #endregion
    }
}
