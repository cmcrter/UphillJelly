using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRecievers : MonoBehaviour
{

    //hacky script used for animation recievers for each trick, once the trick plays we display the trick label text at the bottom of the screen.
    public HoverboardController playerController;

    //each trick label is manually entered - this sucks and can probably be improved
    public GameObject treflipLabel;
    public GameObject heelflipLabel;
    public GameObject kickflipLabel;
    public GameObject hardflipLabel;
    public GameObject shoveitLabel;

    public GameObject bsBoardSlideLabel;
    public GameObject nosegrindLabel;
    public GameObject bsCrookedLabel;
    public GameObject fiveOhGrindLabel;
    public GameObject fiftyFiftyGrindLabel;

    //Popup text is hidden after a certain amount of time
    public float PopupDecayTime = 0.1f;
    private float timerTime;
    private bool PopupActive = false;
    public void IsTricking()
    {
        playerController.isTricking = false;
        
    }

    //each of these functions is triggered by an anim notify on the individual trick animations

    //Reset all labels just in case, set the specific trick label on, and start the decay timer
    public void Treflip()
    {
        ResetLabels();
        treflipLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void Heelflip()
    {
        ResetLabels();
        heelflipLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void Kickflip()
    {
        ResetLabels();
        kickflipLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void Hardflip()
    {
        ResetLabels();
        hardflipLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void ShoveIt()
    {
        ResetLabels();
        shoveitLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void bsBoardslide()
    {
        ResetLabels();
        bsBoardSlideLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void Nosegrind()
    {
        ResetLabels();
        nosegrindLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void bsCrooked()
    {
        ResetLabels();
        bsCrookedLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }

    public void fiveOhGrind()
    {
        ResetLabels();
        fiveOhGrindLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }


    public void fiftyFiftyGrind()
    {
        ResetLabels();
        fiftyFiftyGrindLabel.SetActive(true);
        PopupActive = true;
        timerTime = Time.time + PopupDecayTime;
    }
    private void Update()
    {
        //once our decay timer runs out, reset all labels.
        if (PopupActive && (Time.time >= timerTime))
        {
            ResetLabels();
               
        }
    }

    //generic function to hide all labels once the popup decay time has passed
    public void ResetLabels()
    {
        treflipLabel.SetActive(false);
        shoveitLabel.SetActive(false);
        hardflipLabel.SetActive(false);
        kickflipLabel.SetActive(false);
        heelflipLabel.SetActive(false);
        bsBoardSlideLabel.SetActive(false);
        nosegrindLabel.SetActive(false);
        bsCrookedLabel.SetActive(false);
        fiveOhGrindLabel.SetActive(false);
        fiftyFiftyGrindLabel.SetActive(false);
    }
}
