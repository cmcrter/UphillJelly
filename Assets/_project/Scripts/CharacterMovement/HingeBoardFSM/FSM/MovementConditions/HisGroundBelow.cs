////////////////////////////////////////////////////////////
// File: isGroundBelow.cs
// Author: Charles Carter
// Date Created: 10/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/01/22
// Brief: A condition for changing movement states, whether it's on the ground or not
//////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Utility.StateMachine;

namespace L7Games.Movement
{
    [Serializable]
    public class HisGroundBelow : Condition
    {
        #region Variables

        /// <summary>
        /// The Mask used for all anything that could count as ground
        /// </summary>
        public LayerMask GroundMask;
        public LayerMask floorMask;
        public LayerMask propMask;

        public RaycastHit FrontLeftGroundHitLocalDown;

        public RaycastHit FrontRightGroundHitLocalDown;
        public RaycastHit BackLeftGroundHitLocalDown;
        public RaycastHit BackRightGroundHitLocalDown;

        public RaycastHit FrontLeftGroundHitWorldDown;
        public RaycastHit FrontRightGroundHitWorldDown;
        public RaycastHit BackLeftGroundHitWorldDown;
        public RaycastHit BackRightGroundHitWorldDown;

        public RaycastHit FrontLeftFloorHit;
        public RaycastHit FrontRightFloorHit;
        public RaycastHit BackLeftFloorHit;
        public RaycastHit BackRightFloorHit;

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

        private bool frontLeftLocalUpToDate;
        private bool frontLeftWorldUpToDate;
        private bool frontRightLocalUpToDate;
        private bool frontRightWorldUpToDate;
        private bool backLeftLocalUpToDate;
        private bool backLeftWorldUpToDate;
        private bool backRightLocalUpToDate;
        private bool backRightWorldUpToDate;
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
            RaycastHit[] raycastHits;
            if (frontLeftLocalUpToDate = GroundRaycast(frontLeftRaycastPointTransform.position, -frontLeftRaycastPointTransform.up, out RaycastHit raycastHit))
            {
                FrontLeftGroundHitLocalDown = raycastHit;
            }
            // Front Left World
            if (frontLeftWorldUpToDate = GroundRaycastAll(frontLeftRaycastPointTransform.position, -Vector3.up, out raycastHits))
            {
                FrontLeftGroundHitWorldDown = raycastHits[0];
                // Check through the ray cast hits to see if any hit the floor
                if (TryGetClosestFloorLayerHit(raycastHits, out RaycastHit newRaycastHit))
                {
                    FrontLeftFloorHit = newRaycastHit;
                }
            }

            // Front Right Local
            if (frontRightLocalUpToDate = GroundRaycast(frontRightRaycastPointTransform.position, -frontRightRaycastPointTransform.up, out raycastHit))
            {
                FrontRightGroundHitLocalDown = raycastHit;
            }
            // Front Right World
            if (frontRightWorldUpToDate = GroundRaycastAll(frontRightRaycastPointTransform.position, -Vector3.up, out raycastHits))
            {
                FrontRightGroundHitWorldDown = raycastHits[0];
                // Check through the ray cast hits to see if any hit the floor
                if (TryGetClosestFloorLayerHit(raycastHits, out RaycastHit newRaycastHit))
                {
                    FrontRightFloorHit = newRaycastHit;
                }
            }

            // Back Left Local
            if (backLeftLocalUpToDate = GroundRaycast(backLeftRaycastPointTransform.position, -backLeftRaycastPointTransform.up, out raycastHit))
            {
                BackLeftGroundHitLocalDown = raycastHit;
            }
            // Back Left World
            if (backLeftWorldUpToDate = GroundRaycastAll(backLeftRaycastPointTransform.position, -Vector3.up,  out raycastHits))
            {
                BackLeftGroundHitWorldDown = raycastHits[0];
                // Check through the ray cast hits to see if any hit the floor
                if (TryGetClosestFloorLayerHit(raycastHits, out RaycastHit newRaycastHit))
                {
                    BackLeftFloorHit = newRaycastHit;
                }
            }

            // Back Right Local
            if (backRightLocalUpToDate = GroundRaycast(backRightRaycastPointTransform.position, -backRightRaycastPointTransform.up, out raycastHit))
            {
                BackRightGroundHitLocalDown = raycastHit;
            }
            // Back Right World
            if (backRightWorldUpToDate = GroundRaycastAll(backRightRaycastPointTransform.position, -Vector3.up, out raycastHits))
            {
                BackRightGroundHitWorldDown = raycastHits[0];
                // Check through the ray cast hits to see if any hit the floor
                if (TryGetClosestFloorLayerHit(raycastHits, out RaycastHit newRaycastHit))
                {
                    BackRightFloorHit = newRaycastHit;
                }
            }

            // If the local rotation raycast are touching ground then increase the distance the ground downwards check is allowed
            float worldDownGroundCheckDistance = groundDist;
            if ((FrontLeftGroundHitLocalDown.distance <= groundDist && frontLeftLocalUpToDate) || (FrontRightGroundHitLocalDown.distance <= groundDist && frontRightLocalUpToDate) 
                || (BackLeftGroundHitLocalDown.distance <= groundDist && backLeftLocalUpToDate) || (BackRightGroundHitLocalDown.distance <= groundDist && backRightLocalUpToDate))
            {
                worldDownGroundCheckDistance = localTransformExtendGroundDist;
            }

            //This hit may still be used for smoothing when the player is in the air
            if ((FrontLeftGroundHitWorldDown.distance <= worldDownGroundCheckDistance && frontLeftWorldUpToDate)  || (FrontRightGroundHitWorldDown.distance <= worldDownGroundCheckDistance && frontRightWorldUpToDate) 
                || (BackLeftGroundHitWorldDown.distance <= worldDownGroundCheckDistance && backLeftWorldUpToDate) || (BackRightGroundHitWorldDown.distance <= worldDownGroundCheckDistance && backRightWorldUpToDate))
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
                    //Debug.DrawLine(position, position + (-direction * raycastHit.distance), Color.red);
                }

                // If the hit point normal is more than perpendicular to the world up then it is invalid
                if (Vector3.Angle(raycastHit.normal, Vector3.up) < 90f)
                {
                    return true;
                }
            }
            return false;
        }

        private bool GroundRaycastAll(Vector3 position, Vector3 direction, out RaycastHit[] raycastHits)
        {
            raycastHits = Physics.RaycastAll(position, direction, 50f, GroundMask, QueryTriggerInteraction.Ignore);
            if (raycastHits.Length > 0)
            {
                List<RaycastHit> validRaycastHits = new List<RaycastHit>(raycastHits);
                for (int i = 0; i < validRaycastHits.Count; ++i)
                {
                    if (Vector3.Angle(validRaycastHits[i].normal, Vector3.up) >= 90f)
                    {
                        validRaycastHits.RemoveAt(i);
                        --i;
                    }
                }
                raycastHits = validRaycastHits.ToArray();
            }
            //Any debugging stuff needed
            if (Debug.isDebugBuild)
            {
                Color rayColour = Color.red;
                Vector3 startPoint = position;
                for (int i = -1; i < raycastHits.Length - 1; ++i)
                {
                    Debug.DrawLine(startPoint, raycastHits[i + 1].point, rayColour);
                    startPoint = raycastHits[i + 1].point;
                    rayColour = new Color(rayColour.b, rayColour.r, rayColour.g, 1f);
                }
            }

            return raycastHits.Length > 0;
        }

        private bool CheckIsFloorLayer(int layer)
        {
            int layermatch = (1 << layer) & floorMask;
            return layermatch != 0;
        }

        private bool TryGetClosestFloorLayerHit(RaycastHit[] hits, out RaycastHit floorHit)
        {
            if (hits.Length > 1)
            {
                int i = 0;
            }
            floorHit = new RaycastHit();
            float shortestDistance = float.MaxValue;
            bool floorHitFound = false;
            // Check through the ray cast hits too see if any hit the floor
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; ++i)
                {
                    if (CheckIsFloorLayer(hits[i].collider.gameObject.layer))
                    {
                        if (hits[i].distance < shortestDistance)
                        {
                            floorHit = hits[i];
                            shortestDistance = hits[i].distance;
                            floorHitFound = true;
                        }
                        // Save the floor hit once its found

                    }
                }
            }
            return floorHitFound;
        }
        #endregion
    }
}
