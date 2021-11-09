//================================================================================================================================================================================================================================================================================================================================================
//  Name:               Trick.cs
//  Author:             Matthew Mason
//  Date Created:       09/11/2021
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 09/11/2021
//  Brief:              Script used to buffer input as it is performed by the player
//================================================================================================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trick", menuName = "ScriptableObject/Trick")]
public class Trick : ScriptableObject
{
    /// <summary>
    /// The input id required for the combo to be performed
    /// </summary>
    public List<int> inputCombo;

    /// <summary>
    /// The animation played with the trick
    /// </summary>
    public AnimationClip animationClipToPerform;

    /// <summary>
    /// The amount of score the trick gains per second
    /// </summary>
    public float baseScorePerSecond;
}
