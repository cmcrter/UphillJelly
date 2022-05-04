////////////////////////////////////////////////////////////
// File: MoneyCollectable.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A more specific script for money to be added to a player
//////////////////////////////////////////////////////////// 

using L7Games.Movement;
using UnityEngine;

namespace L7Games.Triggerables.Collectables
{
    public class MoneyCollectable : Collectables, ITriggerable
    {
        #region Public Fields

        [SerializeField]
        private float fCollectableScore = 0f;
        public float ReturnScoreValue
        {
            get => fCollectableScore;            
        }

        #endregion

            #region Public Events
        public delegate void MoneyPickedUpDelegate(PlayerController playerPickingUpMoney, MoneyCollectable money);
        public static event MoneyPickedUpDelegate MoneyPickedUp;
        #endregion

        #region Public Methods

        public override void PickupCollectable(PlayerController player)
        {
            //Debug.Log("Money Picked Up");

            //Add score onto player
            player.collectableScore += fCollectableScore;

            //Run collectable sound etc
            base.PickupCollectable(player);

            //Run the event to see if anything else needs to run
            if (MoneyPickedUp != null)
            {
                MoneyPickedUp(player, this);
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}