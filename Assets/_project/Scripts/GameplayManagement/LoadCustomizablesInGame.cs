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

    public GameObject defaultBoard;

    public MeshRenderer catBoardMesh;
    public MeshFilter catBoardFilter;

    public SkinnedMeshRenderer ghostCatBody;
    public Transform ghostCatHead;
    public MeshRenderer ghostCatBoardMesh;
    public MeshFilter ghostCatBoardFilter;

    [Header("Override Values")]
    [SerializeField]
    private bool OverrideValues = false;
    [SerializeField]
    private List<ItemObjectSO> equipItems = new List<ItemObjectSO>(3);

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
                    GameObject.Instantiate<GameObject>(equipItems[1].objectPrefab, catHead);
                }

                if(equipItems[2] != null)
                {
                    defaultBoard.SetActive(false);

                    //Swapping Out Board
                    catBoardFilter.sharedMesh = equipItems[2].objectPrefab.GetComponent<MeshFilter>().sharedMesh;
                    catBoardMesh.material = equipItems[2].material;
                }
            }

            return;
        }

        //Personally this way is super inefficient but it's the only way it works with the shop system as currently is
        if (LoadingData.shopItems[0])
        {
            //Applying Material To Body
            catBody.material = LoadingData.shopItems[0].material;
        }

        if (LoadingData.shopItems[1])
        {
            //Adding Hat
            catHead.GetComponent<MeshFilter>().sharedMesh = LoadingData.shopItems[1].objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            catHead.GetComponent<MeshRenderer>().material = LoadingData.shopItems[1].material;
        }

        if (LoadingData.shopItems[2])
        {
            defaultBoard.SetActive(false);

            //Swapping Out Board
            catBoardFilter.sharedMesh = LoadingData.shopItems[2].objectPrefab.GetComponent<MeshFilter>().sharedMesh;
            catBoardMesh.material = LoadingData.shopItems[2].material;
        }
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
                    return equipItems[1].objectPrefab;
                }
            }
        }

        return null;
    }
}