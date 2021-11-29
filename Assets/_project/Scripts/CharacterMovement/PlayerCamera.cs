////////////////////////////////////////////////////////////
// File: CharacterController.cs
// Author: Charles Carter
// Date Created: 28/09/21
// Last Edited By: Charles Carter
// Date Last Edited: 30/09/21
// Brief: A quick script to register the inputs for the character
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Movement
{
    public class PlayerCamera : MonoBehaviour
	{
        #region Variables

        public Transform target;

		public float smoothSpeed = 1f;
		public float followDist = 2.5f;
		private Vector3 offset;

		public bool FollowRotation = true;
		public bool FollowY = true;
		public bool FollowZ = true;

		public bool bMovingBackwards = false;

        #endregion

        #region Unity Methods

        private void Start()
		{
			offset = ( -target.forward * followDist ) + new Vector3(0, 1f, 0);
			transform.position = target.position + offset;
		}

		private void FixedUpdate()
		{
			if (FollowRotation)
			{
				if(!bMovingBackwards)
				{
					offset = (-target.forward.normalized * followDist) + new Vector3(0, 1f, 0);
				}
				else
				{
					offset = (target.forward.normalized * followDist) + new Vector3(0, 1f, 0);
				}
			}

			Vector3 speed = Vector3.zero;
			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref speed, smoothSpeed, 1000f);
			transform.position = smoothedPosition;

			transform.LookAt(target);
		}

        #endregion
    }
}