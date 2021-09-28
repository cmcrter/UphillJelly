using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{

    //unused code for a player selection screen. the characters would stand in a lineup and you can select them by moving a selector label left and right over the characters.

    public Camera cam;
    public Transform[] selectionPoints; //0 = cat, 1 = bird, 2 = croc, 3 = turtle
    private int currentSelection = 0;

    [SerializeField]
    private string playerPrefix = "";

    private bool isAxisInUse = false;

    public int selectedCharacter = 0;

    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = selectionPoints[0].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw(playerPrefix + "Horizontal") != 0)
        {
            if (isAxisInUse == false)
            {
                if (Input.GetAxisRaw(playerPrefix + "Horizontal") == -1)
                {
                    //move left
                    currentSelection -= 1;
                    if (currentSelection < 0)
                    {
                        currentSelection = (selectionPoints.Length -1);
                    }
                    gameObject.transform.position = selectionPoints[currentSelection].position;
                }
                if (Input.GetAxisRaw(playerPrefix + "Horizontal") == 1)
                {
                    //move right
                    currentSelection += 1;
                    if (currentSelection > selectionPoints.Length -1)
                    {
                        currentSelection = 0;
                    }
                    gameObject.transform.position = selectionPoints[currentSelection].position;
                }
                isAxisInUse = true;
            }
        }

        if (Input.GetAxisRaw(playerPrefix + "Horizontal") == 0)
        {
            isAxisInUse = false;
        }

        if (Input.GetButtonDown(playerPrefix + "Jump"))
        {

            PlayerPrefs.SetInt(playerPrefix + "selectedCharacter", currentSelection);
            isReady = true;
        }
        transform.LookAt(cam.transform.position);
    }
}
