using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_Player : MonoBehaviour
{

    //
    public OutfitChanger outfitChanger;

    public int CurrentGameObjectInt;

    public int CurrentGameObjectMaterialInt;

    //
    //public Material playerHatMaterial;

    //
    //public Mesh playerHatMesh;


    //
    public void SavePlayer() {


        CurrentGameObjectInt = outfitChanger.currentGOint;

        CurrentGameObjectMaterialInt = outfitChanger.currentGOMaterialint;

        //
        //playerHatMaterial = outfitChanger.hatColour;

        //
        //playerHatMesh = outfitChanger.hatObject;

        //
        b_SaveSystem.SavePlayer(this);

        

    }

    //
    public void LoadPlayer() {

        //
        b_PlayerData data = b_SaveSystem.LoadPlayer();



        outfitChanger.currentGOint = CurrentGameObjectInt;

        outfitChanger.currentGOMaterialint = CurrentGameObjectMaterialInt;

        outfitChanger.LoadedHats();

        //
        // This Mat ==== b_PlayerData.data^^ ==== b_PlayerData.hatMaterial
        //playerHatMaterial = data.hatMaterial;
        //data.hatMaterial = playerHatMaterial;
        //
        //outfitChanger.hatColour = playerHatMaterial;

        //
        //playerHatMesh = data.hatMesh;
        //data.hatMesh = playerHatMesh;
        //
        //outfitChanger.hatObject = playerHatMesh;


        /*

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];


        */

    }


}
