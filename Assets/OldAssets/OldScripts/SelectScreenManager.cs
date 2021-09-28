using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScreenManager : MonoBehaviour
{
    //unused script for the player selection screen that was canceled
    //checks if everyone has selected a player, then goes to the game level
    public PlayerSelector[] selectors;

    private bool allPlayersReady = false;

    public int playerCount = 1;

    void Start()
    {
        Debug.Log(Input.GetJoystickNames());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (PlayerSelector selector in selectors)
        {
            if (selector.isReady == false)
            {
                return;
            }
            else
            {
                StartGame();
            }
        }

    }

    void StartGame()
    {
        PlayerPrefs.SetInt("prefPlayerCount", playerCount);
        SceneManager.LoadScene(1);
    }
}
