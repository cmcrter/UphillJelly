////////////////////////////////////////////////////////////
// File: UITransitionManager.cs
// Author: Jack Peedle
// Date Created: 12/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 12/04/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using L7Games.Loading;

namespace L7Games
{
    public class UITransitionManager : MonoBehaviour
    {
        #region Variables

        public CinemachineVirtualCamera currentCamera;
        public CinemachineVirtualCamera secondaryCamera;

        public EventSystem eventSystemInScene;

        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            if(LoadingData.player != null)
            {
                currentCamera = secondaryCamera;
            }

            currentCamera.Priority++;
        }

        #endregion

        #region Public Methods

        public void UpdateCamera(CinemachineVirtualCamera target) 
        {
            currentCamera.Priority--;

            currentCamera = target;

            currentCamera.Priority++;
        }
        #endregion
    }
}
