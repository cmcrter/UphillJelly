//================================================================================================================================================================================================================================================================================================================================================
// File: HumanoidCollisionHandler.cs
// Author: Matthew Mason
// Date Created: 09/02/22
// Last Edited By: Matthew Mason
// Date Last Edited: 09/02/22
// Brief: Used to detected collisions against the humanoid portion of the controller
//================================================================================================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidCollisionHandler : MonoBehaviour
{
    /// <summary>
    /// Called when collision has been detected that would wipe out the player
    /// </summary>
    public event System.Action<Vector3> lethalCollisionDetected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
