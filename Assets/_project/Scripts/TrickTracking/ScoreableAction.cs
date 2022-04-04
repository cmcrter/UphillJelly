//================================================================================================================================================================================================================================================================================================================================================
//  Name:               ScoreableAction.cs
//  Author:             Matthew Mason
//  Date Created:       31/03/2022
//  Last Modified By:   Matthew Mason
//  Date Last Modified: 31/03/2022
//  Brief:              A script-able object used to store data about a given kind of score-able action that can be passed to the trick buffer
//================================================================================================================================================================================================================================================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreableAction", menuName = "ScriptableObject/ScoreableAction")]
public class ScoreableAction : ScriptableObject
{
    [Tooltip("How much score the action rewards when it started being performed")]
    public float initalScoreValue;

    [Tooltip("How much score the action will gain initial")]
    public float scorePerSecond;

    [Tooltip("The name of the trick as it will appear in game")]
    public string trickName;
}
