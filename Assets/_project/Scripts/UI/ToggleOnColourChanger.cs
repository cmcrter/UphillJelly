using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOnColourChanger : MonoBehaviour
{
    public Toggle toggleToUpdate;

    public ColorBlock colorBlockForOnState;

    private ColorBlock unToggleColourBlock;

    // Start is called before the first frame update
    void Awake()
    {
        unToggleColourBlock = toggleToUpdate.colors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Bind this function to the toggle on value changed event
    /// </summary>
    public void OnToggle()
    {
        toggleToUpdate.colors = toggleToUpdate.isOn ? colorBlockForOnState : unToggleColourBlock;
    }
}
