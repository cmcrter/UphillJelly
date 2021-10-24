using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class b_SaveSystem
{
    
    //
    public static void SavePlayer(b_Player playerr) {

        //
        BinaryFormatter formatter = new BinaryFormatter();

        //
        string path = Application.persistentDataPath + "/Player.txt";

        //
        FileStream stream = new FileStream(path, FileMode.Create);

        //
        b_PlayerData data = new b_PlayerData(playerr);

        //
        formatter.Serialize(stream, data);

        //
        stream.Close();

    }


    //
    public static b_PlayerData LoadPlayer() {

        //
        string path = Application.persistentDataPath + "/Player.txt";

        //
        if (File.Exists(path)) {

            //
            BinaryFormatter formatter = new BinaryFormatter();

            //
            FileStream stream = new FileStream(path, FileMode.Open);

            //
            b_PlayerData data = formatter.Deserialize(stream) as b_PlayerData;

            //
            stream.Close();

            //
            return data;

            

        } else {

            //
            Debug.LogError("Save file not found in " + path);
            return null;

        }


    }


}
