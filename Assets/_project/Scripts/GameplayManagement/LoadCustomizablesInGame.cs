////////////////////////////////////////////////////////////
// File: LoadCustomizablesInGame.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 15/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: A script to load the character materials in game
//////////////////////////////////////////////////////////// 

using System;
using System.Collections.Generic;
using L7Games;
using L7Games.Loading;
using UnityEngine;

[Serializable]
public class LoadCustomizablesInGame
{
    [Header("Needed Objects and Meshes")]
    public SkinnedMeshRenderer catBody;
    public Transform catHead;
    public MeshRenderer catBoardMesh;
    public MeshFilter catBoardFilter;

    [Header("Override Values")]
    [SerializeField]
    private bool OverrideValues = false;
    [SerializeField]
    private List<InventoryItemData> equipItems = new List<InventoryItemData>(3);

    public void ApplyCustomization()
    {
        if(LoadingData.shopItems == null)
        {
            if(OverrideValues)
            {
                if(equipItems[0] != null)
                {
                    //Applying Material To Body
                    catBody.material = equipItems[0].material;
                }

                if(equipItems[1] != null)
                {
                    //Adding Hat
                    GameObject.Instantiate<GameObject>(equipItems[1].prefab, catHead);
                }

                if(equipItems[2] != null)
                {
                    //Swapping Out Board
                    catBoardFilter.sharedMesh = equipItems[2].mesh;
                    catBoardMesh.material = equipItems[2].material;
                }
            }

            return;
        }

        //Applying Material To Body
        catBody.material = LoadingData.shopItems[0].material;

        //Adding Hat
        GameObject.Instantiate<GameObject>(LoadingData.shopItems[1].prefab, catHead);

        //Swapping Out Board
        catBoardFilter.sharedMesh = LoadingData.shopItems[2].mesh;
        catBoardMesh.material = LoadingData.shopItems[2].material;
    }

    public GameObject HatObject()
    {
        if(LoadingData.shopItems == null)
        {
            if(OverrideValues)
            {
                if(equipItems.Count <= 1)
                {
                    return null;
                }

                if(equipItems[1] != null)
                {
                    return equipItems[1].prefab;
                }
            }
        }

        return null;
    }
}