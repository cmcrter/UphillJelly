////////////////////////////////////////////////////////////
// File: TempScoreSystem.cs
// Author: Jack Peedle
// Date Created: 10/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 10/10/21
// Brief: Script to track the player and ghosts score as the game goes on
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempScoreSystem : MonoBehaviour
{
    
    //
    public int PlayerScore;

    //
    public Text PlayerScoreText;



    //
    public int GhostScore;

    //
    public Text GhostScoreText;



    // Start is called before the first frame update
    public void Start()
    {

        PlayerScore = 0;
        GhostScore = 0;

    }

    // increase player score
    public void IncreasePlayerScore() {

        //
        PlayerScore = PlayerScore + 5;

    }

    // increase ghost score
    public void IncreaseGhostScore() {

        //
        GhostScore = GhostScore + 5;

    }

    private void Update() {

        //
        PlayerScoreText.text = "Player Score : " + PlayerScore;

        //
        GhostScoreText.text = "Ghost Score : " + GhostScore;

    }

}
