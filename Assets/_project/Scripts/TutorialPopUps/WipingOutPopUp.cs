using L7Games.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipingOutPopUp : UiPopUp
{
    public override bool CheckCondition(PlayerHingeMovementController player)
    {
        return player.IsWipedOut;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
