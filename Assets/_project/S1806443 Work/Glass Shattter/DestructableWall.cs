using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class DestructableWall : MonoBehaviour {

        //public L7Games.Movement.PlayerHingeMovementController playerHingeController;

        [SerializeField] private Transform pfGlassWallBroken;

        public GameObject explosionGO;

        //private Vector3 forceApplied = new Vector3(0, 50, 0);

        public void BreakGlassWall() {
            Instantiate(pfGlassWallBroken, transform.position, transform.rotation);
            //Destroy(gameObject);
            gameObject.transform.position = new Vector3(0, 0, 0);
            Debug.Log("Glass Destroyed");

            //pfGlassWallBroken.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, forceApplied, 0);
        }

        public IEnumerator WaitForAirTime() {

            Debug.Log("WaitingForAirTime");

            yield return new WaitForSeconds(.2f);

            DestroyForceCollider();

        }

        public void DestroyForceCollider() {

            Destroy(explosionGO);

            Debug.Log("DestroyedExplosionGO");

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
