using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SleepyCat
{
    public class DestructableWall : MonoBehaviour {

        [SerializeField] private Transform pfGlassWallBroken;

        private Vector3 forceApplied = new Vector3(0, 50, 0);


        private void OnCollisionEnter(Collision collision) {
            
            if (collision.gameObject.tag == "JacksFinish") {

                Instantiate(pfGlassWallBroken, transform.position, transform.rotation);
                Destroy(gameObject);
                Debug.Log("Glass Destroyed");

                pfGlassWallBroken.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, forceApplied, 0);

            }

        }

    }
}
