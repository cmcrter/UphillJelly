using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//i set up inhertibles for the collectibles but since we only have 1 type of collectible it was pointless
    public interface ICollectable
    {
        void PickupCollectable();
    }

    public interface IKillTrigger
    {
        void OnKillTrigger();
    }
