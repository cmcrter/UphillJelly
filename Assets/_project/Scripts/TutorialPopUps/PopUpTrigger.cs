using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games.Movement;
using L7Games;

public class PopUpTrigger : MonoBehaviour, ITriggerable
{

    public event System.Action<PlayerController> triggerHit;


    public GameObject ReturnGameObject()
    {
        return this.gameObject;
    }

    public void Trigger(PlayerController player)
    {
        if (triggerHit != null)
        {
            triggerHit(player);
        }
    }

    public void UnTrigger(PlayerController player)
    {
    }
}
