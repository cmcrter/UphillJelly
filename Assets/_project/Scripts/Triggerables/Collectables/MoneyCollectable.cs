////////////////////////////////////////////////////////////
// File: MoneyCollectable.cs
// Author: Charles Carter
// Date Created: 04/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 04/10/21
// Brief: A more specific script for money to be added to a player
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Triggerables.Collectables
{
    public class MoneyCollectable : Collectables
    {
        #region Public Fields

        [SerializeField]
        private float fCollectableScore = 0f;

        #endregion

        #region Public Methods

        public override void PickupCollectable(/*Player player*/)
        {
            //Add score onto player
            //player.AddMoney(fCollectableScore);

            base.PickupCollectable();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}