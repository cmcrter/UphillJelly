using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindableEntity : MonoBehaviour
{
   //script attached to the first node of each grind rail, contains a sequence of nodes that are passed to the player. The first node passed to the player is based
   //on which node that the player hits first, so we don't teleport back to the start if we hit a rail in the middle.

    [SerializeField]
    private Transform[] nodes;


    //closest distance square, found it online, apparently works better than a regular closest distance? i dunno man im not a programmer.
    Transform GetClosestNode(Transform target, Transform[] nodes)
    {
        Transform closestNode = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = target.position;
        foreach (Transform potentialTarget in nodes)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestNode = potentialTarget;
            }
        }
        return closestNode;
    }

    
    public void BeginGrind(GameObject other)
    {
        //Input.GetKey(KeyCode.E) &&
        //Canceled input check, caused issues with only working half the time, but would be nice for more player control.
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<HoverboardController>().grinding == false)
            {
                //if we aren't already grinding, pass a list of nodes to the player, skip any nodes that the player has already passed based on the closest node.

                other.transform.position = GetClosestNode(other.transform, nodes).position;
                int MinIndex = System.Array.IndexOf(nodes, GetClosestNode(other.transform, nodes));
                Debug.Log(MinIndex);

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (i < MinIndex)
                    {
                        continue;
                    }
                    other.GetComponent<HoverboardController>().currentGrindTarget = nodes[MinIndex];
                    other.GetComponent<HoverboardController>().nodesToGrind.Add(nodes[i]);
                    Debug.Log(nodes[i].name);

                }

                //once we have got the nodes, we start the grind. the player controller is kinematic, and we generate a random trick animation to use for the grind
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.GetComponent<HoverboardController>().grinding = true;
                Random.InitState((int)System.DateTime.Now.Ticks);
                int randomNumber = Random.Range(1, 6);
                other.GetComponent<HoverboardController>().charAnim.SetTrigger("Grinding" + randomNumber);
                other.GetComponent<HoverboardController>().grindEffect.SetActive(true);



            }
        }
    }
}
