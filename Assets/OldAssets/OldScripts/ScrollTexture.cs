using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    //scroll the texture over time, used for the wrong way barriers.
    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;
 
    void Update()
    {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * ScrollY;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        
    }
}
