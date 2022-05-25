////////////////////////////////////////////////////////////
// File: CreditsScreen.cs
// Author: Charles Carter
// Date Created: 24/05/22
// Last Edited By: Charles Carter
// Date Last Edited: 24/05/22
// Brief: A script to control the scrolling on the content on the credits
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Obsolete]
public class CreditsScreen : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private ScrollRect scrollingText;
    [SerializeField]
    private float scrollTime = 3f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(!scrollingText && Debug.isDebugBuild)
        {
            Debug.Log("No scroll rect selected for credits", this);
        }

        if(scrollingText)
        {
            scrollingText.normalizedPosition = Vector2.one;
        }
    }

    //private void OnEnable()
    //{
    //    Debug.Log("Scrolling credits");

    //    if(!scrollingText)
    //    {
    //        scrollingText = GetComponentInChildren<ScrollRect>();
    //    }

    //    StartCoroutine(Co_ScrollingText(scrollTime));
    //}

    //private void OnDisable()
    //{
    //    StopCoroutine(Co_ScrollingText(scrollTime));

    //    if(scrollingText)
    //    {
    //        scrollingText.verticalNormalizedPosition = 1;
    //    }
    //}

    #endregion

    #region Public Methods

    public void StartScroll()
    {
        scrollingText.verticalNormalizedPosition = 1f;
        StartCoroutine(Co_ScrollingText(scrollTime));
    }

    public void StopScroll()
    {
        StopCoroutine(Co_ScrollingText(scrollTime));
        scrollingText.verticalNormalizedPosition = 1f;
    }

    #endregion

    #region Private Methods

    private IEnumerator Co_ScrollingText(float textTimer)
    {
        for(float t = 1; t > 0; t -= Time.deltaTime / textTimer)
        {            
            scrollingText.verticalNormalizedPosition = t;
            yield return null;
        }
    }

    #endregion
}
