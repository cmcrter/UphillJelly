using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SleepyCat.Utility.Splines;

namespace SleepyCat.Movement
{
    public class PlayerGrindSection : MonoBehaviour
    {
        [SerializeField]
        PlayerMovementController movementController;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out SplineWrapper splineToGrindDown) && movementController)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Player going to grind");
                }

                //Telling the grind condition that the player wants to start grinding
                movementController.grindBelow.playerEnteredGrind(splineToGrindDown);
            }    
        }

        private void OnTriggerExit(Collider other) 
        {
            if (other.TryGetComponent(out SplineWrapper splineToGrindDown) && movementController) 
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Player going off grind");
                }

                //Telling the grind condition that the player wants to start grinding
                movementController.grindBelow.playerExitedGrind(splineToGrindDown);
            }
        }
    }
}