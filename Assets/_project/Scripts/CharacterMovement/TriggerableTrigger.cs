////////////////////////////////////////////////////////////
// File: TriggerableTrigger.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A script which triggers any triggerable when the collider touches them (generally it'll be only players that has this on them)
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Triggerables
{
    public class TriggerableTrigger : MonoBehaviour
    {
        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                if (triggerable.ReturnGameObject().activeSelf)
                {
                    triggerable.Trigger();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITriggerable triggerable))
            {
                if (triggerable.ReturnGameObject().activeSelf)
                {
                    triggerable.UnTrigger();
                }
            }
        }

        #endregion
    }
}
