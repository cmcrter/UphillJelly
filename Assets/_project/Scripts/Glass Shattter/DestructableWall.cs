using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class DestructableWall : MonoBehaviour {

        //public L7Games.Movement.PlayerHingeMovementController playerHingeController;

        [SerializeField] private Transform pfGlassWallBroken;

        public GameObject explosionGO;

        //public float shrinkSpeed = 2f;

        //private Vector3 targetScale = new Vector3(0, 0, 0);

        //private bool shrinkWallBool;

        //private Vector3 forceApplied = new Vector3(0, 50, 0);

        private FMOD.Studio.EventInstance shatterSound;

        private void Start() 
        {
            shatterSound = FMODUnity.RuntimeManager.CreateInstance("event:/PlayerSounds/ObjectHitGlass");
        }

        public void BreakGlassWall() {
            Instantiate(pfGlassWallBroken, transform.position, transform.rotation);
            //Destroy(gameObject);
            gameObject.transform.position = new Vector3(1000, 1000, 1000);
            Debug.Log("Glass Destroyed");
            shatterSound.start();
            //pfGlassWallBroken.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, forceApplied, 0);
        }

        public IEnumerator WaitForAirTime() {

            Debug.Log("WaitingForAirTime");

            yield return new WaitForSeconds(.3f);

            DestroyForceCollider();

            //ShrinkWall();

        }

        public void DestroyForceCollider() {

            Destroy(explosionGO);

            Debug.Log("DestroyedExplosionGO");

        }

        public void ShrinkWall() {

            //shrinkWallBool = true;

        }

        private void Update() {
            
            //if (shrinkWallBool) {
            //
            //    pfGlassWallBroken.localScale = Vector3.Lerp(pfGlassWallBroken.localScale, targetScale, shrinkSpeed * Time.deltaTime);
            //
            //}

        }

        //public Transform playerTransform;

        /*
        private void OnCollisionEnter(Collision collision) {

            //playerHingeController.bWipeOutLocked = true;

            Instantiate(pfGlassWallBroken, transform.position, transform.rotation);
            Destroy(gameObject);
            Debug.Log("Glass Destroyed");

            pfGlassWallBroken.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, forceApplied, 0);

            if (collision.gameObject.tag == "JacksFinish") {

                

            }

        }
        */

    }
}
