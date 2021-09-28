using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickup : MonoBehaviour, ICollectable
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickupCollectable();
            other.gameObject.GetComponent<HoverboardController>().radicalScore += 10;
        }
    }

    public void PickupCollectable()
    {
        gameObject.SetActive(false);
    }
}
