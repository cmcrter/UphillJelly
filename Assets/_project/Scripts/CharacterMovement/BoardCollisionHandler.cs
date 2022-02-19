using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCollisionHandler : MonoBehaviour
{
    /// <summary>
    /// Called when collision has been detected that would wipe out the player
    /// </summary>
    public event System.Action<Vector3> lethalCollisionDetected;

    public Collider frontCollider;

    public Collider backCollider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).thisCollider == frontCollider)
        {

        }
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
