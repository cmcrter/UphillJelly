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

        #endregion

        #region Public Events
        public delegate void MoneyPickedUpDelegate(PlayerController playerPickingUpMoney);
        public static event MoneyPickedUpDelegate MoneyPickedUp;
        #endregion

        #region Public Methods

        public override void PickupCollectable(PlayerController player)
        {
            //Debug.Log("Money Picked Up");

            //Add score onto player
            //player.AddMoney(fCollectableScore);

            base.PickupCollectable(player);

            if (MoneyPickedUp != null)
            {
                MoneyPickedUp(player);
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}