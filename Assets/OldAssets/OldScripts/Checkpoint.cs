using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //checkpoint script attached to the bus stops in the level 

    //this used to be an array for canceled multiplayer feature, now each checkpoint has only 1 respawn point
    public Transform[] RespawnPoints;

    //once the player passes the checkpoint trigger, we grab the respawn point associated with the checkpoint and set that as the players current respawn point
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            other.gameObject.GetComponent<HoverboardController>().lastCheckpoint = RespawnPoints[Random.Range(0, RespawnPoints.Length)];
        }
    }
}
