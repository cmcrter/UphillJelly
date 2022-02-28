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
        public LayerMask GroundMask;
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
            if (GroundRaycast(frontLeftRaycastPointTransform.position, -frontLeftRaycastPointTransform.up, out RaycastHit raycastHit))
            {
                FrontLeftGroundHitLocalDown = raycastHit;
            }
            // Front Left World
            if (GroundRaycast(frontLeftRaycastPointTransform.position, -Vector3.up, out raycastHit))
            {
                FrontLeftGroundHitWorldDown = raycastHit;
            }
            // Front Right Local
            if (GroundRaycast(frontRightRaycastPointTransform.position, -frontRightRaycastPointTransform.up, out raycastHit))
            {
                FrontRightGroundHitLocalDown = raycastHit;
            }
            // Front Right World
            if (GroundRaycast(frontRightRaycastPointTransform.position, -Vector3.up, out raycastHit))
            {
                FrontRightGroundHitWorldDown = raycastHit;
            }
            // Back Left Local
            if (GroundRaycast(backLeftRaycastPointTransform.position, -backLeftRaycastPointTransform.up, out raycastHit))
            {
                BackLeftGroundHitLocalDown = raycastHit;
            }
            // Back Left World
            if (GroundRaycast(backLeftRaycastPointTransform.position, -Vector3.up,  out raycastHit))
            {
                BackLeftGroundHitWorldDown = raycastHit;
            }
            // Back Right Local
            if (GroundRaycast(backRightRaycastPointTransform.position, -backRightRaycastPointTransform.up, out raycastHit))
            {
                BackRightGroundHitLocalDown = raycastHit;
            }
            // Back Right World
            if (GroundRaycast(backRightRaycastPointTransform.position, -Vector3.up, out raycastHit))
            {
                BackRightGroundHitWorldDown = raycastHit;
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

        #region Private Methods
        private bool GroundRaycast(Vector3 position, Vector3 direction, out RaycastHit raycastHit)
        {
            if (Physics.Raycast(position, direction, out raycastHit, 50f, GroundMask, QueryTriggerInteraction.Ignore))
            {
                //Any debugging stuff needed
                if (Debug.isDebugBuild)
                {
                    Debug.DrawLine(position, position + (-direction * raycastHit.distance), Color.red);
                }

                // If the hit point normal is more than perpendicular to the world up then it is invalid
                if (Vector3.Angle(raycastHit.normal, Vector3.up) < 90f)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
