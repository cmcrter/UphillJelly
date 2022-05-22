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

    public int Id;
    //
    public Sprite uiDisplay;
    public ItemType type;

    public bool isPurchased;

    public GameObject objectPrefab;

    // item description text area string with a size of 15 and 20
    [TextArea(15,20)]
    public string itemDescription;

}

[System.Serializable]
public class Item
{

    public string Name;
    public int Id;  
    public Item(ItemObjectSO item) {

        Name = item.name;
        Id = item.Id;

    }

}
