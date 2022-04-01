////////////////////////////////////////////////////////////
// File: Collectables.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: An abstract script so that money/ secret items can be interacted with the same way
//////////////////////////////////////////////////////////// 

using L7Games.Movement;
using UnityEngine;

namespace L7Games.Triggerables.Collectables
{
    public abstract class Collectables : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => PickupCollectable(player);
        void ITriggerable.UnTrigger(PlayerController player) => PutdownCollectable();

        #endregion

        #region Variables

        PlayerController playerPickedUp;
        private FMOD.Studio.EventInstance pickupSound;

        #endregion

        #region Public Methods

        private void Start()
        {
            pickupSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/ItemPickup");
        }

        private void OnEnable()
        {
            playerPickedUp = null;
        }

        private void OnDisable()
        {
            if(!playerPickedUp) return;

            playerPickedUp.onRespawn -= TurnCollectableOn;
            playerPickedUp = null;
        }

        public virtual void PickupCollectable(PlayerController player)
        {
            //Turning off item
            gameObject.SetActive(false);

            //For testing reasons
            player.onRespawn += TurnCollectableOn;

            //Debug.Log(player.collectableCounter);
            pickupSound.setParameterByName("ItemCombo", player.collectableCounter);
            pickupSound.start();

            player.CollectableSoundCooldown();

            playerPickedUp = player;

            //For multiplayer - list of the players that have picked it up, and when they respawn... remove them from the list
            //Get more and more ghost as more players pick it up (1 time pickup per player per collectable)
            //When list is full with every player, disable object
        }

        public virtual void PutdownCollectable()
        {
            //Doesn't do anything but in-case there's needed functionality later          
        
        }

        public void TurnCollectableOn()
        {
            gameObject.SetActive(true);
        }

        #endregion
    }
}
