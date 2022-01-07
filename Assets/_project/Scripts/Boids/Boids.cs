//=====================================================================================================================================================================================================================================================================================================================
//  Name:               Boids.cs
//  Author:             Matthew Mason
//  Date Created:       07/01/2022
//  Date Last Modified: 07/01/2022
//  Brief:              Class for general functionality of all boids
//=====================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Boids
{
    public class Boids : MonoBehaviour
    {
        //private Vector3 currentVelocity;

        /// <summary>
        /// Returns a velocity that is either pushing towards or away from a point
        /// </summary>
        /// <param name="isSeeking">If the force is pushing towards the point, otherwise it will being pushing away</param>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="targetPosition">The point to push either towards or away from</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <param name="currentVelocity">The current velocity of the boid agent</param>
        /// <returns>A velocity that is either pushing towards or away from a point</returns>
        protected static Vector3 GetSeekOrFleeForce(bool isSeeking, float maxSpeed, Vector3 targetPosition, Vector3 agentPosition, Vector3 currentVelocity)
        {
            Vector3 targetDirection  = isSeeking ? Vector3.Normalize(targetPosition - agentPosition) : Vector3.Normalize(targetPosition - agentPosition);
            Vector3 newVelocity = targetDirection * maxSpeed;
            return newVelocity - currentVelocity;
        }

        /// <summary>
        /// Returns a velocity that is pushing towards a point
        /// </summary>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="targetPosition">The point to push towards</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <param name="currentVelocity">The current velocity of the boid agent</param>
        /// <returns>A velocity that is pushing towards a point</returns>
        protected static Vector3 GetSeekForce(float maxSpeed, Vector3 targetPosition, Vector3 agentPosition, Vector3 currentVelocity)
        {
            return GetSeekOrFleeForce(true, maxSpeed, targetPosition, agentPosition, currentVelocity);
        }

        /// <summary>
        /// Returns a velocity that is pushing away from a point
        /// </summary>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="targetPosition">The point to push away from</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <param name="currentVelocity">The current velocity of the boid agent</param>
        /// <returns>A velocity that is pushing away from a point</returns>
        protected static Vector3 GetFleeForce(float maxSpeed, Vector3 targetPosition, Vector3 agentPosition, Vector3 currentVelocity)
        {
            return GetSeekOrFleeForce(false, maxSpeed, targetPosition, agentPosition, currentVelocity);
        }

        /// <summary>
        /// Returns a velocity that is wandering in a randomized direction
        /// </summary>
        /// <param name="wanderSphereRadius">The radius that the wander circle will remain on</param>
        /// <param name="jitterRadius">The radius for the jitter circle around the initially found point</param>
        /// <param name="forwardDistance">The distance the found wander point is moved in front of the agent</param>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="currentVelocity">The current velocity of the agent</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <returns>a velocity that is wandering in a randomized direction</returns>
        protected static Vector3 GetWanderForce(float wanderSphereRadius, float jitterRadius, float forwardDistance, float maxSpeed, Vector3 currentVelocity, Vector3 agentPosition)
        {
            // Get the wander point around the agent
            Vector3 pointOnSphere = Random.onUnitSphere * wanderSphereRadius;
            Vector3 jitteredPoint = Random.onUnitSphere * jitterRadius;
            Vector3 scaledJitteredPoint = jitteredPoint.normalized * wanderSphereRadius;
            // Move the wander point forwards relative to the agent by a given amount and seek towards the point
            return GetSeekForce(maxSpeed, jitteredPoint + currentVelocity.normalized * forwardDistance, agentPosition, currentVelocity); 
        }

        /// <summary>
        /// Returns a velocity that is heading towards another moving target to intercept
        /// </summary>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="targetPosition">The position of the target </param>
        /// <param name="targetVelocity">The moving target's current velocity</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <param name="currentVelocity">The current velocity of the agent</param>
        /// <returns>A velocity that is heading towards another moving target to intercept</returns>
        protected static Vector3 GetPursueForce(float maxSpeed, Vector3 targetPosition, Vector3 targetVelocity, Vector3 agentPosition, Vector3 currentVelocity)
        {
            return GetSeekForce(maxSpeed, targetPosition + targetVelocity, agentPosition, currentVelocity);
        }

        /// <summary>
        /// Returns a velocity that is moving away from a target to dodge its movement
        /// </summary>
        /// <param name="maxSpeed">The max speed the new velocity is allowed</param>
        /// <param name="targetPosition">The position of the target </param>
        /// <param name="targetVelocity">The moving target's current velocity</param>
        /// <param name="agentPosition">The current position of the boid agent</param>
        /// <param name="currentVelocity">The current velocity of the agent</param>
        /// <returns>A velocity that is heading towards another moving target to intercept</returns>
        protected static Vector3 GetEvadeForce(float maxSpeed, Vector3 targetPosition, Vector3 targetVelocity, Vector3 agentPosition, Vector3 currentVelocity)
        {
            return GetFleeForce(maxSpeed, targetPosition + targetVelocity, agentPosition, currentVelocity);
        }
    }
}


