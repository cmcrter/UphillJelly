////////////////////////////////////////////////////////////
// File: HatObject.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hat Object", menuName = "Inventory System/Items/Hat")]
public class HatObject : ItemObjectSO
{

    public void Awake() {

        type = ItemType.Hat;

    }

}
