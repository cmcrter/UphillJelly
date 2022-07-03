////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using UnityEngine;

namespace L7Games
{
    public class TestMono : MonoBehaviour
    {   
        void Awake()
        {
            foreach(ScriptableTest test in TestManager.scriptables)
            {
                Debug.Log(test.i);
            }
        }

    }
}
