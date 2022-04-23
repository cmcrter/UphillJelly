using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L7Games;
using L7Games.Movement;

namespace SleepyCat
{
    public class BrokenWallForce : MonoBehaviour, ITriggerable
    {

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;

        void ITriggerable.Trigger(PlayerController player) => TriggerWallBreak(player);
        void ITriggerable.UnTrigger(PlayerController player) => UnTriggerWallBreak();

        public Rigidbody GlassBreakerRigidbody;

        public DestructableWall destructableWall;

        public GameObject explosionGO;

        public float radius = 5f;
        public float power = 10f;

        //public bool shrink = false;
        //public float targetscale = 0;
        //public float shrinkSpeed = 2f;

        public virtual void TriggerWallBreak(PlayerController player) {


            explosionGO.transform.position = player.transform.position + new Vector3(0, 0, 3f);

            destructableWall.BreakGlassWall();

            Vector3 explosionPosition = transform.position;

            //explosionGO.GetComponent<Rigidbody>().AddExplosionForce(power, explosionPosition, radius, 5f);

            explosionGO.GetComponent<Rigidbody>().AddForce(transform.forward * 10000f);

            //shrink = true;

            

            

            //explosionGO.GetComponent<Rigidbody>().AddExplosionForce(power, explosionPosition, radius, 5f);
            //explosionGO.GetComponent<Rigidbody>().AddForce(transform.forward * 4000f);



        }

        public virtual void UnTriggerWallBreak() {
            //Doesn't do anything but in-case there's needed functionality later          

            StartCoroutine(destructableWall.WaitForAirTime());

        }
        /*
        public void Update() {
            
            if (!shrink) {
                // do nothing
            }

            if (shrink) {
                explosionGO.transform.localScale = Vector3.Lerp(explosionGO.transform.localScale, new Vector3(targetscale, targetscale,
                    targetscale), Time.deltaTime * shrinkSpeed);
            }

            if (explosionGO.transform.localScale.x <= 0) {

                shrink = false;

            }


        }
        */
        public void OnCollisionEnter(Collision collision) {

            //playerHingeController.bWipeOutLocked = true;

            //if (collision = gameObject.characterReference) {
            //
            //}

            //Instantiate(pfGlassWallBroken, transform.position, transform.rotation);
            //Destroy(gameObject);
            Debug.Log("Collided with something M8");

            //collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, forceApplied, 0);


        }

        

        /*
        private void OnTriggerEnter(Collider other) {

            if (other.gameObject.tag == "Player") {
                Debug.Log("CollidedWithPlayer");

                StartCoroutine(WaitForAirTime());
            }

            if (other.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Character")) {

                

            }


        }

        */
        

    }
}
