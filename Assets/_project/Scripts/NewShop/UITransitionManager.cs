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
using Cinemachine;

namespace SleepyCat
{
    public class UITransitionManager : MonoBehaviour
    {
        public CinemachineVirtualCamera currentCamera;

        // Start is called before the first frame update
        void Start() {

            currentCamera.Priority++;

        }

        public void UpdateCamera(CinemachineVirtualCamera target) {

            currentCamera.Priority--;

            currentCamera = target;

            currentCamera.Priority++;

        }

    }
}
