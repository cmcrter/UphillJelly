////////////////////////////////////////////////////////////
// File: ProfileSaveSystem.cs
// Author: Jack Peedle
// Date Created: 22/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 22/10/21
// Brief: A script to control the profile system
//////////////////////////////////////////////////////////// 

using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class ProfileSaveSystem
{
    
    //
    public static void SaveProfile(ProfileController profile) {

        //
        BinaryFormatter formatter = new BinaryFormatter();

        //
        string path = Application.persistentDataPath + "/Profile.txt";

        //
        FileStream stream = new FileStream(path, FileMode.Create);


        //
        PlayerData data = new PlayerData(profile);


        //
        formatter.Serialize(stream, data);

        //
        stream.Close();


    }


    //
    public static PlayerData LoadProfile() {

        //
        string path = Application.persistentDataPath + "/Profile.txt";

        //
        if (File.Exists(path)) {

            //
            BinaryFormatter formatter = new BinaryFormatter();

            //
            FileStream stream = new FileStream(path, FileMode.Open);

            //
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            //
            stream.Close();

            //
            return data;

        } else {

            //
            //if (Debug.isDebugBuild) {

                Debug.Log("Save file not found in " + path);
                return null;

            //}

        }

    }



}
