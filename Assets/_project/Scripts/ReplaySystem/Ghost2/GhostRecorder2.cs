////////////////////////////////////////////////////////////
// File: GhostRecorder.cs
// Author: Jack Peedle
// Date Created: 29/09/21
// Last Edited By: Jack Peedle
// Date Last Edited: 08/01/22
// Brief:   Ghost replay script which controls the temporary movement of a test character as well as recording and replaying 
//          the characters movement and rotation to act as a "Ghost"
//////////////////////////////////////////////////////////// 

using UnityEngine;

public class GhostRecorder2 : MonoBehaviour
{
    #region Public Fields

    // public reference to the ghost scriptable object
    public Ghost2 ghost2;

    // timer for the ghost to correlate with the timer of the player for frequency and data
    private float timer;

    // store actual time value
    private float timeValue;

    // gameobject for if this gameobjects position has changed
    public GameObject HasChangedGO;

    #endregion

    #region Unity Methods

    // On awake
    private void Start()
    {
        
        // if the ghost is currently recording
        if (ghost2.isRecording) {

            //reset the ghost data to allow for new data to be recorded
            ghost2.ResetGhostData();

            // set time value and timer to 0
            timeValue = 0f;
            timer = 0f;

        }

    }

    // Update
    public void Update() {



        // calculates the most recent frame of the timer and time value in intervals of seconds relevant to time
        timer += Time.unscaledDeltaTime;
        timeValue += Time.unscaledDeltaTime;

        // if the ghost is recording and the timer is less than or = to 1 divided by the ghost recording frequency and the players position has changed
        if (ghost2.isRecording & timer >= 1 / ghost2.recordFrequency && HasChangedGO.transform.hasChanged) {

            //add the time value to the time stamp of the ghost
            ghost2.timeStamp.Add(timeValue);

            // add this position onto the ghost depending on the timestamp
            ghost2.position.Add(this.transform.position);

            // add this rotation onto the ghost depending on the timestamp
            ghost2.rotation.Add(this.transform.eulerAngles);

            // set the timer to 0
            timer = 0f;

            // set the has changed bool to false
            HasChangedGO.transform.hasChanged = false;
            
            // else if the players position has not changed
        } else if (!HasChangedGO.transform.hasChanged) {

            // print debug log
            if (Debug.isDebugBuild) {

            }

        }

    }

    #endregion

    #region Private Methods

    


    #endregion
}
