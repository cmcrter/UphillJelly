////////////////////////////////////////////////////////////
// File: HUD.cs
// Author: Charles Carter
// Date Created: 15/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 15/02/22
// Brief: A script to manage the HUD of a player
//////////////////////////////////////////////////////////// 

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    #region Variables

    //The General texts visible on the HUD
    [SerializeField]
    private TextMeshProUGUI TrickText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    //For wiping out or finishing a combo, showing an image
    [SerializeField]
    private Image SoundImage;

    #endregion

    #region Unity Methods
    void Start()
    {
	
    }
 
    void Update()
    {
	
    }
    #endregion

    #region Public Methods

    public void AddTrickToText(string trickName)
    {
        
    }

    public void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString();
    }
    

    #endregion

    #region Private Methods
    #endregion
}
