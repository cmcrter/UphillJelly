using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindNode : MonoBehaviour
{

    //script attached to each grind node within a grind entity, that can refer back to the parent grind entity
    //once the player hits the rail point, we tell the grind entity that this node got hit.

    public GrindableEntity myGrindable;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player")){
            //make sure the player is aerial before attaching to rail, otherwise the player can teleport up onto trails or get pulled onto them which sucks
            if (other.gameObject.GetComponent<HoverboardController>().canGrind && other.gameObject.GetComponent<HoverboardController>().aerial == true)
            {
                myGrindable.BeginGrind(other.gameObject);
            }

        }

    }
}
