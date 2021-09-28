using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    //a pretty basic follow camera, tracks a point behind the player, and uses transform.lookat to look at the player over time
    public GameObject Player;
    public GameObject child;

    public float yOffset;
    public float speed;
    public float sensitivity;

    //unused for a potential wall avoidance feature
    RaycastHit camHit;

    public Vector3 camDist;

    private void FixedUpdate() 
    {
        follow();
    }

    private void follow()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, child.transform.position, Time.deltaTime * speed);
        gameObject.transform.LookAt(Player.gameObject.transform.position);
    }
}
