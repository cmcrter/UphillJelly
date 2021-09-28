using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class musicSwitch : MonoBehaviour
{
    //This script was used for getting the audio to fade out once the player reaches the finish line
    private StudioEventEmitter musicEmitter;

    public string parameterName;

    public float levelEnd;

    // Start is called before the first frame update
    void Start()
    {
        musicEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {
        if (levelEnd > 100) levelEnd = 100;

        if (levelEnd < 0) levelEnd = 0;
    }

    //simply fades the volume out with a float once the line is crossed

    public void finishRace()
    {
        if (musicEmitter != null)
        {
            levelEnd = 100;

            musicEmitter.SetParameter(parameterName, levelEnd);

           
        }

    }


}
