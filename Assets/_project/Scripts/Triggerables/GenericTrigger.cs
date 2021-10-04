////////////////////////////////////////////////////////////
// File: GenericTrigger.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A triggerable for unity events 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.Events;

namespace SleepyCat.Triggerables
{
    public class GenericTrigger : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger() => TriggerEnterUnityEvent();
        void ITriggerable.UnTrigger() => TriggerExitUnityEvent();

        #endregion

        #region Variables

        public UnityEvent eventsOnEnter;
        public UnityEvent eventsOnExit;

        #endregion

        #region Public Methods

        public void TriggerEnterUnityEvent()
        {
            eventsOnEnter.Invoke();
        }

        public void TriggerExitUnityEvent()
        {
            eventsOnExit.Invoke();
        }

        #endregion
    }
}