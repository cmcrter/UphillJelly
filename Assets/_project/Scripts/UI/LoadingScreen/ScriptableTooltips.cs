////////////////////////////////////////////////////////////
// File: ScriptableTooltips.cs
// Author: Charles Carter
// Date Created: 13/04/22
// Last Edited By: Charles Carter
// Date Last Edited: 13/04/22
// Brief: A scriptable object to define the tooltips
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace L7Games
{
    [CreateAssetMenu(fileName = "Tooltips", menuName = "ScriptableObject/LoadingTooltip")]
    public class ScriptableTooltips : ScriptableObject
    {
        public string name;
        public string text;
        public float priority;
        public Sprite texture;
    }
}