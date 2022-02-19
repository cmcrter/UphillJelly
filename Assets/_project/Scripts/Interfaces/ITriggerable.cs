////////////////////////////////////////////////////////////
// File: ITriggerable.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: An interface to be used by anything that can be triggered by on trigger collider (Collectables and Checkpoints)
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Movement;

namespace L7Games
{
    public interface ITriggerable
    {
        GameObject ReturnGameObject();

        void Trigger(PlayerController player);
        void UnTrigger(PlayerController player);
    }
}
