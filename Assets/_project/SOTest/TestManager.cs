////////////////////////////////////////////////////////////
// File: 
// Author: 
// Date Created: 
// Last Edited By:
// Date Last Edited:
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using UnityEngine;

namespace L7Games
{
    [CreateAssetMenu(fileName = "TestManager", menuName = "ScriptableObjects/TestObjectManager", order = 1)]
    public class TestManager : ScriptableObject
    {
        public static ScriptableTest[] scriptables;
        public List<ScriptableTest> testObjs = new List<ScriptableTest>();

        private void OnValidate()
        {
            scriptables = testObjs.ToArray();
        }

        public static ScriptableTest FindTest(int ID)
        {
            for(int i = 0; i < scriptables.Length; ++i)
            {
                if(scriptables[i].i == ID)
                {
                    return scriptables[i];
                }
            }

            return null;
        }
    }
}
