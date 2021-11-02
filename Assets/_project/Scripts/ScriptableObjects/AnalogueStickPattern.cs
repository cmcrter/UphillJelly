//================================================================================================================================================================================================================================================================================================================================================
//  Name:               AnalogueStickPattern.cs
//  Author:             Matthew Mason
//  Date Created:       02/11/2021
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 02/11/2021
//  Brief:              Scriptable Object used to store the patterns that the player can perform with the analogue stick 
//================================================================================================================================================================================================================================================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat.ScriptableObjects.Input
{
    [CreateAssetMenu(fileName = "AnaglogueStickPattern", menuName = "ScriptableObject/AnalogueStickPattern")]
    public class AnalogueStickPattern : ScriptableObject
    {
        [System.Serializable]
        public struct PatternPoint
        {
            /// <summary>
            /// The point on the 2D axis that will be next in the pattern
            /// </summary>
            public Vector2 pointOnAxis;
            /// <summary>
            /// The amount of time the player has to reach this point
            /// </summary>
            public float timeToReach;
            /// <summary>
            /// The distance the analogue stick must be from this point to be valid
            /// </summary>
            public float distanceTolerance;
        }

        public List<PatternPoint> patternPoints;
    }
}

