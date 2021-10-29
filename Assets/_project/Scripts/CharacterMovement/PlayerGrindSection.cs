////////////////////////////////////////////////////////////
// File: PlayerGrindSection.cs
// Author: Charles Carter
// Date Created: 26/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 26/10/21
// Brief: The trigger on the player to know whether they're on a grind rail or not
//////////////////////////////////////////////////////////// 

using UnityEngine;
using SleepyCat.Utility.Splines;

namespace SleepyCat.Movement
{
    public class PlayerGrindSection : MonoBehaviour
    {
        [SerializeField]
        private PlayerMovementController movementController;

        private void OnTriggerEnter(Collider other) 
        {
            //Debug.Log(other.name);

            if(other.transform.parent != null)
            {
                if(other.transform.parent.TryGetComponent(out SplineWrapper splineToGrindDown) && movementController && other.transform.parent.TryGetComponent(out GrindDetails grindCustomizable))
                {
                    if(Debug.isDebugBuild)
                    {
                        //Debug.Log("Player going to grind");
                    }

                    //Telling the grind condition that the player wants to start grinding
                    movementController.grindBelow.playerEnteredGrind(splineToGrindDown, grindCustomizable);
                }
            }  
        }

        private void OnTriggerExit(Collider other) 
        {
            if(other.transform.parent)
            {
                if(other.transform.parent.TryGetComponent(out SplineWrapper splineToGrindDown) && movementController && other.transform.parent.TryGetComponent(out GrindDetails grindCustomizable))
                {
                    if(Debug.isDebugBuild)
                    {
                        //Debug.Log("Player going off grind");
                    }

                    //Telling the grind condition that the player wants to start grinding
                    movementController.grindBelow.playerExitedGrind(splineToGrindDown, grindCustomizable);
                }

            }
        }
    }
}