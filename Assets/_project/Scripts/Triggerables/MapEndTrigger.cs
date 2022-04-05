////////////////////////////////////////////////////////////
// File: MapEndTrigger.cs
// Author: Charles Carter
// Date Created: 01/04/22
// Last Edited By: Charles Carter
// Date Last Edited: 01/04/22
// Brief: The trigger when the player has finished the map
//////////////////////////////////////////////////////////// 

using L7Games.Movement;
using UnityEngine;

namespace L7Games
{
    public class MapEndTrigger : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => MapFinished(player);
        void ITriggerable.UnTrigger(PlayerController player) => MapUnfinished();

        #endregion

        #region Variables

        //public b_Player b_player;
        //public PlayFabManager playfabManager;

        [SerializeField]
        private GameObject endUI;

        #endregion

        #region Unity Methods

        void Start()
        {

        }

        #endregion

        #region Private Methods

        private void MapFinished(PlayerController player)
        {
            //Show End UI
            if(endUI)
            {
                endUI.SetActive(true);
            }
        }


        private void MapUnfinished()
        {

        }

        #endregion
    }
}
