////////////////////////////////////////////////////////////
// File: ItemDatabaseObject.cs
// Author: Jack Peedle
// Date Created: 08/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 08/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver {

    public ItemObjectSO[] Items;
    public Dictionary<ItemObjectSO, int> GetId = new Dictionary<ItemObjectSO, int>();
    public Dictionary<int, ItemObjectSO> GetItem = new Dictionary<int, ItemObjectSO>();

    public void OnAfterDeserialize() {

        GetId = new Dictionary<ItemObjectSO, int>();
        GetItem = new Dictionary<int, ItemObjectSO>();

        for (int i = 0; i < Items.Length; i++) {

            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);

        }

    }

    public void OnBeforeSerialize() {
        
    }
}
