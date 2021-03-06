////////////////////////////////////////////////////////////
// File: TriggerableTrigger.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A script which triggers any triggerable when the collider touches them (generally it'll be only players that has this on them)
//////////////////////////////////////////////////////////// 

using L7Games.Movement;
using UnityEngine;

namespace L7Games.Triggerables
{
    public class TriggerableTrigger : MonoBehaviour
    {
        [SerializeField]
        PlayerController thisPlayer;

        #region Unity Methods

        //When something enters or leaves the player's zone
        private void OnTriggerEnter(Collider other)
        {
            if(!enabled)
                return;

            //If it can be triggerable
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                //And the object is on
                if (triggerable.ReturnGameObject().activeSelf)
                {
                    //Trigger it
                    triggerable.Trigger(thisPlayer);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!enabled)
                return;

            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                if (triggerable.ReturnGameObject().activeSelf)
                {
                    triggerable.UnTrigger(thisPlayer);
                }
            }
        }

        #endregion
    }
}
