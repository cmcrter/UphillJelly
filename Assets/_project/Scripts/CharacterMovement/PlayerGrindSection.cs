////////////////////////////////////////////////////////////
// File: PlayerGrindSection.cs
// Author: Charles Carter
// Date Created: 26/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 26/10/21
// Brief: The trigger on the player to know whether they're on a grind rail or not
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Utility.Splines;

namespace L7Games.Movement
{
    public class PlayerGrindSection : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovementController movementController;

        [SerializeField]
        private PlayerHingeMovementController movementHingeController;

        private void OnTriggerEnter(Collider other) 
        {
            //Debug.Log(other.name);

            if(other.transform.parent != null)
            {
                if(other.transform.parent.TryGetComponent(out SplineWrapper splineToGrindDown) && other.transform.parent.TryGetComponent(out GrindDetails grindCustomizable))
                {
                    if(Debug.isDebugBuild)
                    {
                        //Debug.Log("Player going to grind");
                    }

                    if(movementController)
                    {
                        //Telling the grind condition that the player wants to start grinding
                        movementController.grindBelow.playerEnteredGrind(splineToGrindDown, grindCustomizable);
                    }
                    else if(movementHingeController)
                    {
                        movementHingeController.grindBelow.playerEnteredGrind(splineToGrindDown, grindCustomizable);
                    }
                }
            }  
        }

        private void OnTriggerExit(Collider other) 
        {
            if(other.transform.parent)
            {
                if(other.transform.parent.TryGetComponent(out SplineWrapper splineToGrindDown))
                {
                    if(Debug.isDebugBuild)
                    {
                        //Debug.Log("Player going off grind");
                    }

                    if(movementController)
                    {
                        //Telling the grind condition that the player wants to start grinding
                        movementController.grindBelow.PlayerLeftGrindArea(splineToGrindDown);
                    }
                    else if(movementHingeController)
                    {
                        movementHingeController.grindBelow.PlayerLeftGrindArea(splineToGrindDown);
                    }
                }

            }
        }
    }
}