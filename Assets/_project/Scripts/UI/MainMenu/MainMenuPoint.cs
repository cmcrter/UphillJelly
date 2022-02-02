////////////////////////////////////////////////////////////
// File: MainMenuPoint.cs
// Author: Charles Carter
// Date Created: 02/02/22
// Last Edited By: Charles Carter
// Date Last Edited: 02/02/22
// Brief: The point at which the camera looks at for specific menu options (used as a reference for the state machine too)
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;


public class MainMenuPoint : MonoBehaviour
{
    #region Variables

    //The camera uses this objects transform for it's view
    //May have a disabled camera on the object to test the view
    [SerializeField]
    List<GameObject> objectsToShow;

    public MainMenuPoint LeftPoint;
    public MainMenuPoint RightPoint;
    public MainMenuPoint UpPoint;
    public MainMenuPoint DownPoint;

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        if(objectsToShow == null)
        {
            return;
        }

        //Show objects needed
        foreach(GameObject UIElement in objectsToShow)
        {
            UIElement.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if(objectsToShow == null)
        {
            return;
        }

        //Hide Objects
        foreach(GameObject UIElement in objectsToShow)
        {
            UIElement.SetActive(false);
        }
    }

    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion
}
