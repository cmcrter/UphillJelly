////////////////////////////////////////////////////////////
// File: ItemObjectSO.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public enum ItemType
{

    Hat,
    CharacterSkin,
    Skateboard,
    Default

}

public abstract class ItemObjectSO : ScriptableObject
{

    //
    public GameObject prefab;
    public ItemType type;

    // item description text area string with a size of 15 and 20
    [TextArea(15,20)]
    public string itemDescription;

}
