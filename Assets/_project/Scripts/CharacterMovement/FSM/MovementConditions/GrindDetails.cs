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
        public int ExitGrindForce = 5;
    }
}