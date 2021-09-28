using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    //a trigger that if we enter, we kill the player. used in the bottomless pits.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            if (!player.GetComponent<HoverboardController>().wipedOut)
            {
                player.GetComponent<HoverboardController>().WipeOut();
            }

        }
    }

}
