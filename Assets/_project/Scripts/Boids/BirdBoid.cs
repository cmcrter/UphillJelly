using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Boids
{
    public class BirdBoid : Boid
    {

        public float seekWeight;
        public float fleeWeight;
        public float wanderWeight;

        public float wanderJitter;
        public float wanderForward;
        public float wanderRadius;

        public float maxSpeed;

        Vector3 currentVelocity;

        public Transform fleeTransform;

        // Update is called once per frame
        void Update()
        {
            Vector3 newVelocity = Vector3.zero;

            //if (fleeWeight > 0f)
            //{
            //    Vector3 fleeVelocity = GetFleeForce(maxSpeed, fleeTransform.position, transform.position, currentVelocity) * fleeWeight;
            //    newVelocity += fleeVelocity;
            //    if (newVelocity.magnitude > maxSpeed)
            //    {
            //        currentVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
            //        transform.position += currentVelocity;
            //        return;

            //    }
            //}
            if (seekWeight > 0f)
            {
                Vector3 seekVelocity = GetSeekForce(maxSpeed, transform.position + Vector3.up, transform.position, currentVelocity) * seekWeight;
                newVelocity += seekVelocity;
                if (newVelocity.magnitude > maxSpeed)
                {
                    currentVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
                    transform.position += currentVelocity;
                    return;
                }
            }

            if (wanderWeight > 0f)
            {
                Vector3 wanderVelocity = GetWanderForce(wanderRadius, wanderJitter, wanderForward, maxSpeed, currentVelocity, transform.position) * wanderWeight;
                newVelocity += wanderVelocity;
                if (newVelocity.magnitude > 10f)
                {
                    currentVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
                    transform.position += currentVelocity;
                    return;
                }
            }

            currentVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);
            transform.position += currentVelocity;
            return;
        }
    }
}
