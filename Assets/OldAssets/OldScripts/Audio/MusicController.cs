using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicController : Singleton<MusicController>
{

    //Originally this music controller script was used for every sound in the scene, however, that caused some issues with fmod later on so don't worry about this as right now it only controls the velocity audio

    private StudioEventEmitter musicEmitter; 
    public string parameterName;

    public string parametreName2;
    

    void Start()
    {
        musicEmitter = GetComponent<FMODUnity.StudioEventEmitter>(); 
    }



    public void ChangeVelocity(float velocity)
    {
        if (musicEmitter != null)
        {
            musicEmitter.SetParameter(parameterName, velocity);
        }
    }



}