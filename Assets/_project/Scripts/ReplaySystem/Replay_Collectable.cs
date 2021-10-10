////////////////////////////////////////////////////////////
// File: Replay_collectable.cs
// Author: Jack Peedle
// Date Created: 10/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 10/10/21
// Brief: Script to change the collectables shape or colour to show the player if the ghost has collected it 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay_Collectable : MonoBehaviour
{



    #region Variables

    // reference to the child object
    public GameObject childCollectableObject;

    #endregion

    #region Methods

    // when collides with something
    void OnTriggerEnter(Collider col) {


        // if the player collides with this collectable
        if (col.gameObject.tag == "Player") {

            // get the render component in this gameobject and set it to cyan
            this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;

            // debug
            if (Debug.isDebugBuild) {

                Debug.Log("Collided with Player");

            }

        }


        // if the ghost collides with this collectable
        if (col.gameObject.tag == "Ghost") {

            // set the child object to active
            childCollectableObject.SetActive(true);

            // debug
            if (Debug.isDebugBuild) {

                Debug.Log("Collided with Ghost");

            }

        }


    }


    // Start is called before the first frame update
    void Start()
    {

        // on start set the child object to false
        childCollectableObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    #endregion

}
