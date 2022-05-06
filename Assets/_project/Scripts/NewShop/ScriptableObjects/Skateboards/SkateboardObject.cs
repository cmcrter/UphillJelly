////////////////////////////////////////////////////////////
// File: SkateboardObject.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skateboard Object", menuName = "Inventory System/Items/Skateboard")]
public class SkateboardObject : ItemObjectSO
{

    public void Awake() {

        type = ItemType.Skateboard;

    }

}