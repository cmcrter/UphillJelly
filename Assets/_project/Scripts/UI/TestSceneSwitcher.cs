////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using UnityEngine;
using L7.Loading;
using UnityEngine.SceneManagement;

public class TestSceneSwitcher : MonoBehaviour
{
    #region Public Methods

    public void SceneSwitched(string scene)
    {
        LoadingData.sceneToLoad = scene;
        SceneManager.LoadScene("LoadingScene");
    }

    #endregion
}
