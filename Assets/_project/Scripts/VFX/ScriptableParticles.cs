////////////////////////////////////////////////////////////
// File: ScriptableParticles.cs
// Author: Charles Carter
// Date Created: 25/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 25/03/22
// Brief: A scriptable object for each particle system
//////////////////////////////////////////////////////////// 

using UnityEngine;

[CreateAssetMenu(fileName = "ParticleObject", menuName = "ScriptableObject/ParticleObject")]
public class ScriptableParticles : ScriptableObject
{
    public string vfx_name;

    public GameObject vfx_prefab;

    public float vfx_duration;
    public bool vfx_isLooping;
}
