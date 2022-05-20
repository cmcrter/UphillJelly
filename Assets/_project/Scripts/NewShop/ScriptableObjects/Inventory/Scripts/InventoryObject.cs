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
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public string savePath;

    //
    //[SerializeField]
    public ItemDatabaseObject database;

    public Inventory Container;

    //
    //public List<InventorySlot> container = new List<InventorySlot>();
    
    
    public void AddItem(Item _item) {

        for (int i = 0; i < Container.Items.Count; i++) {

            

            if (Container.Items[i].item.Id == _item.Id) {

                //Container.Items[i].AddAmountOfItems(_amountOfItem);
                return;

            }

            

        }


        Container.Items.Add(new InventorySlot(_item.Id, _item)); //_amountOfItem));

    }

    [ContextMenu("Save")]
    public void Save() {

        //string saveData = JsonUtility.ToJson((this, true));
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath) + "");
        //bf.Serialize(file, saveData);
        //file.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load() {

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))){

            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();

        }

    }

    [ContextMenu("Clear")]
    public void Clear() {

        Container = new Inventory();

    }

    /*
    public void OnAfterDeserialize() {

        for (int i = 0; i < Container.Items.Count; i++) {

            Container.Items[i].item = database.GetItem[Container.Items[i].ID];

        }
            

    }

    public void OnBeforeSerialize() {
        


    }
    */
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
public class Inventory {

    public List<InventorySlot> Items = new List<InventorySlot>();

}
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    //public int amountOfItem;

    //
    public InventorySlot(int _id, Item _item)  { //int _amountOfItem)

        ID = _id;   
        item = _item;
        //amountOfItem = _amountOfItem;

    }

    /*
    public void AddAmountOfItems(int value) {

        amountOfItem += value;

    }

    
    public void RemoveAmountOfItems(int value) {

        amountOfItem -= value;

    }
    */

}