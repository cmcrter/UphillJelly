////////////////////////////////////////////////////////////
// File: DefaultObject.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObjectSO
{

    public void Awake() {

        type = ItemType.Default;

    }

}
