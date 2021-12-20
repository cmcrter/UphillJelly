////////////////////////////////////////////////////////////
// File: Ghost.cs
// Author: Jack Peedle
// Date Created: 30/09/21
// Last Edited By: Jack Peedle
// Date Last Edited: 12/11/21
// Brief: Scriptable object to handle the visible ghost which will be shown on the screen
//////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// Create asset menu in the inspector for this class
[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    #region Public Fields

    // public bool for if the scene is recording the ghosts movement or if Unity is replaying the Ghosts movement
    public bool isRecording;
    public bool isReplaying;

    // Frequency of recording, how many times it is recording per second 10fps, 20fps etc, higher the frequency the smoother the ghost will be (Larger file size if exported)
    public float recordFrequency;

    // public list of a float to store the time which saves the ghosts position and rotation depending on the time stamp
    public List<float> timeStamp;

    // public lists for the vector3 position and rotation of the ghost
    public List<Vector3> position;
    public List<Vector3> rotation;

    #endregion

    #region Unity Methods

    //
    public void Start() {

        isRecording = false;

        isReplaying = false;

    }

    // On command reset the ghosts data
    public void ResetGhostData() {

        // Reset the saved ghost data for a new ghost to be recorded
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();

    }


    #endregion

    #region Private Methods
    #endregion
}
