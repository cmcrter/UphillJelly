////////////////////////////////////////////////////////////
// File: VFXPlayer.cs
// Author: Charles Carter
// Date Created: 25/03/22
// Last Edited By: Charles Carter
// Date Last Edited: 25/03/022
// Brief: A way to play the different particle system scriptable objects
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.Pool;

public class VFXPlayer : MonoBehaviour
{
    #region Variables

    public static VFXPlayer instance;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    public Transform oneShotVFXParent;

    ObjectPool<GameObject> vfxPool;

    public ObjectPool<GameObject> Pool
    {
        get
        {
            if(vfxPool == null)
            {
                vfxPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }

            return vfxPool;
        }
    }
    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
        }

        instance = this;
    }

    #endregion

    #region Public Methods

    public static void ClearAllVFX()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="particle"></param>
    public GameObject PlayVFX(ScriptableParticles particle)
    {
        GameObject go = /* Pool.Get(); */ Instantiate(particle.vfx_prefab);
        go.name = particle.vfx_name;

        //Put Particle System Under Object

        return go;
    }

    public GameObject PlayVFX(ScriptableParticles particle, Vector3 position)
    {
        GameObject go = Instantiate(particle.vfx_prefab, position, particle.vfx_prefab.transform.rotation, null);
        go.name = particle.vfx_name;

        return go;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="particle"></param>
    /// <param name="vfxParent"></param>
    public GameObject PlayVFX(ScriptableParticles particle, Transform vfxParent)
    {
        GameObject go = Instantiate(particle.vfx_prefab, vfxParent);
        go.name = particle.vfx_name;

        //Put Particle System Under Object

        return go;
    }

    #endregion

    #region Private Methods

    GameObject CreatePooledItem()
    {
        return new GameObject();
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject system)
    {
        gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject system)
    {
        gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(gameObject);
    }

    #endregion
}
