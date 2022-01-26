////////////////////////////////////////////////////////////
// File: PlayerAimingAssist.cs
// Author: Charles Carter
// Date Created: 07/01/22
// Last Edited By: Charles Carter
// Date Last Edited: 07/01/22
// Brief: A script to help with aiming to jump on wall rides and grind rails
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace L7Games.Movement
{
    public class PlayerAimingAssist : MonoBehaviour
    {
        #region Public Fields

        [SerializeField]
        private PlayerHingeMovementController movementController;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            movementController = movementController ?? GetComponent<PlayerHingeMovementController>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        #endregion

        #region Private Methods



        #endregion
    }
}