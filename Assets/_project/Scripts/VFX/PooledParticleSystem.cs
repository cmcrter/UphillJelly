////////////////////////////////////////////////////////////
// File: PooledParticleSystem.cs
// Author: Charles Carter
// Date Created: 25/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 
// Brief: A script to return a particle system back to the object pool when it's done
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.Pool;

public class PooledParticleSystem : MonoBehaviour
{
    public ParticleSystem system;
    public IObjectPool<GameObject> pool;

    void Start()
    {
        system = system ?? GetComponent<ParticleSystem>();
        var main = system.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        // Return to the pool
        pool.Release(gameObject);
    }
}
