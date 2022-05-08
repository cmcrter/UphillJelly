////////////////////////////////////////////////////////////
// File: InventoryObject.cs
// Author: Jack Peedle
// Date Created: 06/05/22
// Last Edited By: Jack Peedle
// Date Last Edited: 06/05/22
// Brief: 
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{

    public string savePath;

    //
    //[SerializeField]
    public ItemDatabaseObject database;

    //
    public List<InventorySlot> container = new List<InventorySlot>();
    
    private void OnEnable() {

        //database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath
        //    ("Assets/Resources/DataBase.asset", typeof(ItemDatabaseObject));
        
        //database = Resources.Load<ItemDatabaseObject>("Database");    

    }
    
    public void AddItem(ItemObjectSO _item, int _amountOfItem) {

        for (int i = 0; i < container.Count; i++) {

            if (container[i].item == _item) {

                container[i].AddAmountOfItems(_amountOfItem);
                return;

            }

        }

        container.Add(new InventorySlot(database.GetId[_item], _item, _amountOfItem));
        

    }

    public void Save() {

        string saveData = JsonUtility.ToJson((this, true));
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath) + "");
        bf.Serialize(file, saveData);
        file.Close();

    }

    public void Load() {

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))){

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();

        }

    }


    public void OnAfterDeserialize() {

        for (int i = 0; i < container.Count; i++) {

            container[i].item = database.GetItem[container[i].ID];

        }
            

    }

    public void OnBeforeSerialize() {
        


    }

    /*
    public void RemoveItem(ItemObjectSO _item, int _amountOfItem) {

        bool hasItem = true;

        for (int i = 0; i > container.Count; i--) {

            if (container[i].item == _item) {

                container[i].RemoveAmountOfItems(_amountOfItem);
                hasItem = false;
                break;

            }

        }

        if (hasItem) {
            container.Remove(new InventorySlot(_item, _amountOfItem));
        }

    }
    */

}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObjectSO item;
    public int amountOfItem;

    //
    public InventorySlot(int _id, ItemObjectSO _item, int _amountOfItem) {

        ID = _id;   
        item = _item;
        amountOfItem = _amountOfItem;

    }

    public void AddAmountOfItems(int value) {

        amountOfItem += value;

    }

    /*
    public void RemoveAmountOfItems(int value) {

        amountOfItem -= value;

    }
    */

}