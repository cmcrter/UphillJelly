////////////////////////////////////////////////////////////
// File: BrokenWallForce.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 29/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 22/05/22
// Brief: The shatterable glasses' trigger script
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games;
using L7Games.Movement;

namespace L7Games
{
    public class BrokenWallForce : MonoBehaviour, ITriggerable
    {
        #region Interface Contracts 

        //The interfaces have specific functions that need to be fulfilled within this script and any child of the script
        GameObject ITriggerable.ReturnGameObject() => gameObject;
        void ITriggerable.Trigger(PlayerController player) => TriggerWallBreak(player);
        void ITriggerable.UnTrigger(PlayerController player) => UnTriggerWallBreak();

        #endregion

        #region Variables

        public Rigidbody GlassBreakerRigidbody;
        public DestructableWall destructableWall;
        public GameObject explosionGO;

        public float radius = 5f;
        public float power = 10f;

        [SerializeField]
        private LayerMask glassMask;

        #endregion

        #region Public Methods

        public virtual void TriggerWallBreak(PlayerController player)
        {
            explosionGO.transform.position = player.transform.position + new Vector3(0, 0, 3f);
            GameObject wall = destructableWall.BreakGlassWall();

            //Adding a force so the window drops fast
            explosionGO.GetComponent<Rigidbody>().AddForce(transform.forward * 10000f);

            //going through the shards near the player and pushing them further
            for(int i = 0; i < wall.transform.childCount; ++i)
            {
                Transform child = wall.transform.GetChild(i);
                if(Vector3.Distance(child.transform.position, player.transform.position) < (radius * 0.5) && child.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(player.transform.forward * player.GetRB().velocity.magnitude * 1.5f);
                }
            }
        }

        public virtual void UnTriggerWallBreak()
        {
            //Doesn't do anything but in-case there's needed functionality later          
            StartCoroutine(destructableWall.WaitForAirTime());
        }

        #endregion
    }
}
