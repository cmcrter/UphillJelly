////////////////////////////////////////////////////////////
// File: GrindDetails.cs
// Author: Charles Carter
// Date Created: 26/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 26/10/21
// Brief: A way of customizing how much force a grind rail will apply to a player that grinds on it.
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace SleepyCat.Movement
{
    public class GrindDetails : MonoBehaviour
    {
        public int DuringGrindForce = 10;

        /// <summary>
        /// the vector parts are relative to the player, z being forward, y being up, x being right
        /// </summary>
        [Header("vector is relative to player... x being right, y being up, z being forward")]
        public Vector3 ExitForce = new Vector3(0, 1.5f, 10f);
    }
}