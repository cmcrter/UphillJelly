using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment : MonoBehaviour
{
    //unused script to align the hoverboard to the ground, NOT the character, just the model
    //caused clipping issues without a feet IK system
    
    public HoverboardController control;

    void Update()
    {
        //don't align the board if we are in the air
        //if (!control.aerial)
        //{
        //    Align();
        //}
        Align();
    }

    private void Align()
    {

        //Raycast down from the center of the board, and rotate the board based on the hit.normal info
        RaycastHit hit;
        Vector3 theRay = -transform.up;


        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z),
            theRay, out hit, 20))
        {

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.parent.rotation;

            //Lerp the rotation so it doesn't look jerky
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / 0.15f);
        }
    }
}
