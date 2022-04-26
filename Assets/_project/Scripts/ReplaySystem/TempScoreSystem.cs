////////////////////////////////////////////////////////////
// File: TempScoreSystem.cs
// Author: Jack Peedle
// Date Created: 10/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 12/11/21
// Brief: Script to track the player and ghosts score as the game goes on
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempScoreSystem : MonoBehaviour
{
    
    /// <summary>
    /// Temporary script to showcase the score system with the replay
    /// </summary>

    // int for the player score
    public int PlayerScore;

    // text object for the player text
    public Text PlayerScoreText;



    // int for the ghost score
    public int GhostScore;

    // text object for the ghost text
    public Text GhostScoreText;


    // int for the ghost score
    public float PlayerTime;

    // text object for the ghost text
    public Text TimeValueText;

    //
    public bool TimerActive = false;



    // Start is called before the first frame update
    public void Start()
    {
        // set both scores to 0 on game start
        PlayerScore = 0;
        GhostScore = 0;
        PlayerTime = 0;

        //TimerActive = true;

    }

    // increase player score
    public void IncreasePlayerScore() {

        // increase the player score by 5
        PlayerScore = PlayerScore + 5;

    }

    // increase ghost score
    public void IncreaseGhostScore() {

        // increase the ghost score by 5
        GhostScore = GhostScore + 5;

    }

    private void Update() {

        if (TimerActive) {

            //
            PlayerTime += Time.deltaTime;

        } else if (!TimerActive) {

            //
            //do nothing

        }

        // set the player score to the player score text + "Player Score : "
        PlayerScoreText.text = "Player Score : " + PlayerScore;

        // set the ghost score to the ghost score text + "Ghost Score : "
        GhostScoreText.text = "Ghost Score : " + GhostScore;

        TimeValueText.text = "Time : " + PlayerTime;

    }

}
