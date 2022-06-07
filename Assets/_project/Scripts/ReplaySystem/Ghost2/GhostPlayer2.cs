////////////////////////////////////////////////////////////
// File: GhostPlayer.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 30/09/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/05/22
// Brief: Script in charge of controlling the ghost of the player with the movement and positioning
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.InputSystem;

//namespace SleepyCat.GhostPlayer
//{

    public class GhostPlayer2 : MonoBehaviour
    {
        #region Public Fields

        // public reference to the ghost
        public Ghost2 ghost2;
        
        // private float for the time value
        private float timeValue;

        // linear interpolation to account for timestamps that are not stored e.g. 0.49, 0.27, 0.82,
        // timestamps that are stored e.g. 0.5, 1, 1.5
        // for example 1 and 1.5 are known timestamps, interpolation is used to find out the position and rotation of the object
        // between 1 and 1.5 using the position and rotation and comparing both and essentially filling in the gaps
        private int index1;
        private int index2;

        public GameObject modelObject;

        #endregion

        #region Unity Methods

        // private on awake function
        private void Awake() 
        {
            // on awake set time value to 0
            timeValue = 0f;
        }

        // on update
        void Update() 
        {
            if (PauseManager.IsInstancePaused)
            {
                return;
            }
            // calculates the most recent frame of the timer and time value in intervals of seconds relevant to time
            timeValue += Time.unscaledDeltaTime;

            // if ghost replaying is true
            if (ghost2.isReplaying)
            {
                //Debug.Log("IsReplaying");    

                // Get the ghost index
                GetGhostIndex();

                // set the ghosts transform position
                SetGhostTransform();
            }
        }

        #endregion

        #region Private Methods

        // get the ghosts index depending on the position of the ghost
        private void GetGhostIndex() 
        {
            // Look for the timestamp that is stored in the ghost
            for (int i = 0; i < ghost2.timeStamp.Count - 2; i++)
            {
                // if the current time value is = to stored time stamp
                if (ghost2.timeStamp[i] == timeValue)
                {
                    // interpolation not needed so we set indexes to i value
                    index1 = i;
                    index2 = i;
                    return;

                } // if the current time value is not stored 
                else if (ghost2.timeStamp[i] < timeValue & timeValue < ghost2.timeStamp[i + 1]) 
                {
                    // take the index before and the index after to work out the interpolation
                    index1 = i;
                    index2 = i + 1;
                    return;                  
                }               
            }

            //Debug.Log("GottenGhostIndex");

            // increment the index of timestamps
            index1 = ghost2.timeStamp.Count - 1;
            index2 = ghost2.timeStamp.Count - 1;
        }

        // get and set the ghosts transform depending on the transform of the ghost
        private void SetGhostTransform() 
        {
            // if both indexes are equal
            if (index1 == index2)
            {
                // take the value from the ghost object
                transform.position = ghost2.position[index1] + modelObject.transform.localPosition;
                transform.eulerAngles = ghost2.rotation[index1] + modelObject.transform.localRotation.eulerAngles;
            } 
            else
            {
                // interpolation factor float is the (timevalue - ghosts index 1) divided by the (ghosts index 2 - ghosts index 1)
                float interpolationFactor = (timeValue - ghost2.timeStamp[index1]) / (ghost2.timeStamp[index2] - ghost2.timeStamp[index1]);

                // this position = ghosts position of both indexes and the interpolation of both indexes 
                // to understand the position between index 1 and index 2
                transform.position = Vector3.Lerp(ghost2.position[index1], ghost2.position[index2], interpolationFactor) + modelObject.transform.localPosition;

                // this rotation = ghosts rotation of both indexes and the interpolation of both indexes 
                // to understand the rotation between index 1 and index 2
                transform.eulerAngles = Vector3.Lerp(ghost2.rotation[index1], ghost2.rotation[index2], interpolationFactor) + modelObject.transform.localRotation.eulerAngles;
        }
        }

        #endregion
    }



