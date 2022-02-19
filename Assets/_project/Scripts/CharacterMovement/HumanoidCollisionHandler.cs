//================================================================================================================================================================================================================================================================================================================================================
// File:                HumanoidCollisionHandler.cs
// Author:              Matthew Mason
// Date Created:        09/02/22
// Last Edited By:      Matthew Mason
// Date Last Edited:    11/02/22
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

    private void OnCollisionEnter(Collision collision)
    {
        if (lethalCollisionDetected != null)
        {
            // Get the average diection of the collsion
            Vector3 avaerageDirection = Vector3.zero;
            ContactPoint[] contactPoints = new ContactPoint[0];
            int numberOfContacts = collision.GetContacts(contactPoints);
            for (int i = 0; i < numberOfContacts; ++i)
            {
                avaerageDirection += (contactPoints[i].point - contactPoints[i].thisCollider.transform.position).normalized;
            }
            lethalCollisionDetected(avaerageDirection.normalized);
        }
    }
}
