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
        public RaycastHit FrontLeftGroundHitLocalDown;
        public RaycastHit FrontRightGroundHitLocalDown;
        public RaycastHit BackLeftGroundHitLocalDown;
        public RaycastHit BackRightGroundHitLocalDown;

        public RaycastHit FrontLeftGroundHitWorldDown;
        public RaycastHit FrontRightGroundHitWorldDown;
        public RaycastHit BackLeftGroundHitWorldDown;
        public RaycastHit BackRightGroundHitWorldDown;

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

        [SerializeField]
        [Tooltip("The distance to the ground used if the local rotation raycasts are hitting something that could be ground")]
        private float localTransformExtendGroundDist;
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
            // Front left local
            //Checking if anything is below it
            if(Physics.Raycast(frontLeftRaycastPointTransform.position, -frontLeftRaycastPointTransform.up, out RaycastHit hit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if(Debug.isDebugBuild)
                {
                    Debug.DrawLine(frontLeftRaycastPointTransform.position, frontLeftRaycastPointTransform.position + (-frontLeftRaycastPointTransform.up * hit.distance), Color.red);
                    //Debug.DrawLine(frontLeftRaycastPointTransform.position, frontLeftRaycastPointTransform.position + (-frontLeftRaycastPointTransform.up * 50f), Color.magenta);
                }
                FrontLeftGroundHitLocalDown = hit;
            }

            // Front Left World
            //Checking if anything is below it
            if (Physics.Raycast(frontLeftRaycastPointTransform.position, -Vector3.up, out RaycastHit worldHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(frontLeftRaycastPointTransform.position, frontLeftRaycastPointTransform.position + (-Vector3.up * worldHit.distance), Color.red);
                    //Debug.DrawLine(frontLeftRaycastPointTransform.position, frontLeftRaycastPointTransform.position + (-Vector3.up * 50f), Color.magenta);
                }
                FrontLeftGroundHitWorldDown = worldHit;
            }

            // Front Right Local
            // Checking if anything is below it
            if (Physics.Raycast(frontRightRaycastPointTransform.position, -frontRightRaycastPointTransform.up, out RaycastHit hit2, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(frontRightRaycastPointTransform.position, frontRightRaycastPointTransform.position + (-frontRightRaycastPointTransform.up * hit2.distance), Color.red);
                    //Debug.DrawLine(frontRightRaycastPointTransform.position, frontRightRaycastPointTransform.position + (-frontRightRaycastPointTransform.up * 50f), Color.magenta);
                }

                FrontRightGroundHitLocalDown = hit2;
            }

            // Front Right World
            // Checking if anything is below it
            if (Physics.Raycast(frontRightRaycastPointTransform.position, -Vector3.up, out worldHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(frontRightRaycastPointTransform.position, frontRightRaycastPointTransform.position + (-Vector3.up * worldHit.distance), Color.red);
                    //Debug.DrawLine(frontRightRaycastPointTransform.position, frontRightRaycastPointTransform.position + (-Vector3.up * 50f), Color.magenta);
                }

                FrontRightGroundHitWorldDown = worldHit;
            }

            // Back Left Local
            // Checking if anything is below it
            if (Physics.Raycast(backLeftRaycastPointTransform.position, -backLeftRaycastPointTransform.up, out RaycastHit hit3, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                // Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(backLeftRaycastPointTransform.position, backLeftRaycastPointTransform.position + (-backLeftRaycastPointTransform.up * hit3.distance), Color.red);
                    //Debug.DrawLine(backLeftRaycastPointTransform.position, backLeftRaycastPointTransform.position + (-backLeftRaycastPointTransform.up * 50f), Color.magenta);
                }

                BackLeftGroundHitLocalDown = hit3;
            }

            // Back Left World
            // Checking if anything is below it
            if (Physics.Raycast(backLeftRaycastPointTransform.position, -Vector3.up, out worldHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                // Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(backLeftRaycastPointTransform.position, backLeftRaycastPointTransform.position + (-Vector3.up * worldHit.distance), Color.red);
                    //Debug.DrawLine(backLeftRaycastPointTransform.position, backLeftRaycastPointTransform.position + (-Vector3.up * 50f), Color.magenta);
                }

                BackLeftGroundHitWorldDown = worldHit;
            }

            // Back Right Local
            if (Physics.Raycast(backRightRaycastPointTransform.position, -backRightRaycastPointTransform.up, out RaycastHit backHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                // Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(backRightRaycastPointTransform.position, backRightRaycastPointTransform.position + (-backRightRaycastPointTransform.up * backHit.distance), Color.red);
                    //Debug.DrawLine(backRightRaycastPointTransform.position, backRightRaycastPointTransform.position + (-backRightRaycastPointTransform.up * 50f), Color.magenta);
                }

                //Could use backhit to help smoothing with the board against the ground
                //Player is about to hit a ramp
                BackRightGroundHitLocalDown = backHit;
            }

            // Back Right World
            if (Physics.Raycast(backRightRaycastPointTransform.position, -Vector3.up, out worldHit, 50f, ~playerMask, QueryTriggerInteraction.Ignore))
            {
                // Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(backRightRaycastPointTransform.position, backRightRaycastPointTransform.position + (-Vector3.up * worldHit.distance), Color.red);
                    //Debug.DrawLine(backRightRaycastPointTransform.position, backRightRaycastPointTransform.position + (-Vector3.up * 50f), Color.magenta);
                }

                //Could use backhit to help smoothing with the board against the ground
                //Player is about to hit a ramp
                BackRightGroundHitWorldDown = worldHit;
            }

            // If the local rotation raycast are touching ground then increase the distance the ground downwards check is allowed
            float worldDownGroundCheckDistance = groundDist;
            if (FrontLeftGroundHitLocalDown.distance <= groundDist || FrontRightGroundHitLocalDown.distance <= groundDist || BackLeftGroundHitLocalDown.distance <= groundDist || BackRightGroundHitLocalDown.distance <= groundDist)
            {
                worldDownGroundCheckDistance = localTransformExtendGroundDist;
            }

            //This hit may still be used for smoothing when the player is in the air
            if (FrontLeftGroundHitWorldDown.distance <= worldDownGroundCheckDistance || FrontRightGroundHitWorldDown.distance <= worldDownGroundCheckDistance || BackLeftGroundHitWorldDown.distance <= worldDownGroundCheckDistance || BackRightGroundHitWorldDown.distance <= worldDownGroundCheckDistance)
            {
                return true;
            }

            return false;         
        }

        #endregion
    }
}
