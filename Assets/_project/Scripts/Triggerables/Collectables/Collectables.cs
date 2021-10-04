////////////////////////////////////////////////////////////
// File: Collectables.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: An abstract script so that money/ secret items can be interacted with the same way
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Triggerables.Collectables
{
    public abstract class Collectables : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger() => PickupCollectable();
        void ITriggerable.UnTrigger() => PutdownCollectable();

        #endregion

        #region Public Methods

        public virtual void PickupCollectable()
        {
            //Turning off item
            gameObject.SetActive(false);
        }

        public virtual void PutdownCollectable()
        {
            //Doesn't do anything but in-case there's needed functionality later
        }

        #endregion
    }
}
