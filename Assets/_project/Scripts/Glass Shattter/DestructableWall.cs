////////////////////////////////////////////////////////////
// File: Destructable Wall.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/05/22
// Brief: The glass shatter objects' script
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class DestructableWall : MonoBehaviour 
    {
        #region Variables

        [SerializeField]
        private Transform pfGlassWallBroken;
        public GameObject explosionGO;
        private FMOD.Studio.EventInstance shatterSound;

        #endregion

        #region Unity Methods

        private void Start() 
        {
            shatterSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/ObjectHitGlass");
        }

        #endregion

        #region Public Methods
        public GameObject BreakGlassWall()
        {
            GameObject newWindow = Instantiate(pfGlassWallBroken, transform.position, transform.rotation).gameObject;

            gameObject.transform.position = new Vector3(1000, 1000, 1000);
            shatterSound.start();

            return newWindow;
        }

        public IEnumerator WaitForAirTime()
        {
            yield return new WaitForSeconds(.3f);
            DestroyForceCollider();
        }

        public void DestroyForceCollider()
        {
            Destroy(explosionGO);
            Debug.Log("DestroyedExplosionGO");
        }

        #endregion
    }
}
