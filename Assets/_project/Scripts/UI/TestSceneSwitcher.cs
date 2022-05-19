////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7Games.Loading;
using UnityEngine.SceneManagement;

public class TestSceneSwitcher : MonoBehaviour
{
    #region Public Methods

    public static void SceneSwitched(string scene)
    {
        LoadingData.sceneToLoad = scene;
        LoadingData.waitForNextScene = true;
        
        SceneManager.LoadScene("LoadingScene");
    }

    #endregion
}
