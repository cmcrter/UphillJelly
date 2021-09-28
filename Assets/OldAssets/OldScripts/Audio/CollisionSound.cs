 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{

    //This script was used on objects that the player could collide with where there were several possible collision sounds and one would be randomly picked every time a collision occured

    private FMOD.Studio.EventInstance collisionSound;

    public int collisionNumber;

    int randomMin;
    int randomMax;

    float collisionTime;

    public float startCollisionTime;

   public bool canCollide;

    public string Sound;

    // Start is called before the first frame update
    void Start()
    {
        randomMin = 0;
        randomMax = 3;

        canCollide = false;

       
    }

    // Update is called once per frame
    void Update()
    {
        collisionSound.setParameterByName("HitVariation", collisionNumber);

        if(canCollide == false)
        {
            collisionTime -= Time.deltaTime;
        }

        if(collisionTime <=0)
        {
            collisionTime = startCollisionTime;

            canCollide = true;

            enabled = false;
        }
    }



    public void OnCollisionEnter(Collision collision)
    {
       if(canCollide)
        {

            int randomNumber = Random.Range(randomMin, randomMax);

            collisionNumber = randomNumber;

            collisionSound = FMODUnity.RuntimeManager.CreateInstance(Sound);

            collisionSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

            collisionSound.start();

            canCollide = false;

            enabled = true;
          

           

          
        }
    }

}
