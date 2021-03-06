////////////////////////////////////////////////////////////
// File: InventoryItemData.cs
// Author: Jack Peedle
// Date Created: 19/04/22
// Last Edited By: Jack Peedle
// Date Last Edited: 19/04/22
// Brief: 
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace L7Games {

    [CreateAssetMenu(menuName = "Inventory Item Data")]
    public class InventoryItemData : ScriptableObject
    {

        public string id;
        public string displayName;
        public Sprite icon;
        public int price;
        //public Text description;
        //public string type;
        public GameObject prefab;

        public Material material;
        public Mesh mesh;
    }

}


    