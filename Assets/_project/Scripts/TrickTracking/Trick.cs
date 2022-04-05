using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7Games.Tricks
{
    [System.Serializable]
    public class Trick
    {
        [Tooltip("The scoreable action associated with this trick")]
        public ScoreableAction scoreableDetails;

        [Tooltip("The name of the trick's animation state")]
        public string TrickAnimStateName;
    }
}
