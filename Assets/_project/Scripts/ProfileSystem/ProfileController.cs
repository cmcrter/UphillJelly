////////////////////////////////////////////////////////////
// File: ProfileController.cs
// Author: Jack Peedle
// Date Created: 12/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 16/10/21
// Brief: A script to control the profile system
//////////////////////////////////////////////////////////// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ProfileController : MonoBehaviour
{

    #region Variables

    // array of gameobjects for the various hats
    public GameObject[] go_hats;

    // array of gameobjects for the various skateboards
    public GameObject[] go_skateboards;

    // array of materials for the various characters
    public Material[] mat_CharacterMeshes;

    // public int currency for the shop
    public int Currency;

    // public text to show the currency
    public Text CurrencyText;

    // Mesh rendered for the player
    public MeshRenderer playerMeshRenderer;

    // Gameobject for the previous hat
    public GameObject go_previousHat;


    //
    public GameObject currentActiveHat;

    // int for the current outfit
    private int iCurrentOutfit;

    [SerializeField]
    // int for the current hat
    private int iCurrentHat;

    // int for the current skateboard
    private int iCurrentSkateboard;

    #endregion

    #region Methods

    // On Start
    public void Start() {

        // set the currency to 0
        Currency = 0;

        // current outfit = 0
        iCurrentOutfit = 0;

        //
        iCurrentHat = 4;

        //
        go_hats[iCurrentHat].SetActive(true);

        //
        go_previousHat = go_hats[4];

        // set the players mesh renderer material to the character meshes current outfit
        playerMeshRenderer.material = mat_CharacterMeshes[iCurrentOutfit];

        //
        //go_previousHat = go_hats[iCurrentHat];

    }

    // Update
    public void Update() {

        // set the currency to the text to visualize the current currency to the player
        CurrencyText.text = "£" + Currency.ToString();

        go_previousHat.SetActive(false);


        currentActiveHat = go_hats[iCurrentHat];

        

    }

    #endregion

    #region ArrowShopButtons

    // Change the character mesh to the right
    public void ChangeCharacterMeshRight() {

        // current outfit + (move to the next one)
        iCurrentOutfit++;

        // for each character materials in the list
        for(int j = 0; j < mat_CharacterMeshes.Length; j++) {

            // if current outfit is = 1, show mat 1 etc
            if (iCurrentOutfit == j) {

                // change the material to the current integer of the outfit
                playerMeshRenderer.material = mat_CharacterMeshes[iCurrentOutfit];

            }

        }

        // if the current outfit is less than or = to the length of meshes
        // Show the first material / outfit
        if (iCurrentOutfit >= mat_CharacterMeshes.Length) {

            // set the current outfit to 0
            iCurrentOutfit = 0;

            // change the material to the current integer of the outfit
            playerMeshRenderer.material = mat_CharacterMeshes[iCurrentOutfit];

        }


    }

    // Change the character mesh to the left
    public void ChangeCharacterMeshLeft() {

        // current outfit -- (move to the next one)
        iCurrentOutfit--;

        // for each character materials in the list
        for (int i = 0; i < mat_CharacterMeshes.Length; i++) {

            // if current outfit is = 1, show mat 1 etc
            if (iCurrentOutfit == i) {

                // change the material to the current integer of the outfit
                playerMeshRenderer.material = mat_CharacterMeshes[iCurrentOutfit];

            }

        }

        // if the current outfit is less than 0
        // Show the last material / outfit
        if (iCurrentOutfit < 0) {

            // set the current outfit to 4
            iCurrentOutfit = 4;

            // change the material to the current integer of the outfit
            playerMeshRenderer.material = mat_CharacterMeshes[iCurrentOutfit];

        }


    }




    // Change the hat to the right
    public void ChangeHatRight() {



        // if the current outfit is less than or = to the length of meshes
        // Show the first material / outfit
        if (iCurrentHat > 10) {

            // set the current outfit to 0
            iCurrentHat = 1;


            //
            go_previousHat = go_hats[11];

        } else {

            // current outfit + (move to the next one)
            iCurrentHat++;

            //
            go_previousHat = go_hats[iCurrentHat - 1];

        }


        // for each character materials in the list
        for (int i = 0; i < go_hats.Length; i++) {

            // if current outfit is = 1, show mat 1 etc
            if (iCurrentHat == i) {

                // change the material to the current integer of the outfit
                //go_HatSpawnPoint = go_hats[iCurrentHat];
                go_hats[iCurrentHat].SetActive(true);

            }

        }

    }










    // Change the hat to the right
    public void ChangeHatLeft() {

        

        // if the current outfit is less than or = to the length of meshes
        // Show the first material / outfit
        if (iCurrentHat < 2) {

            // set the current outfit to 0
            iCurrentHat = 11;


            //
            go_previousHat = go_hats[1];

        } else {

            // current outfit + (move to the next one)
            iCurrentHat--;

            //
            go_previousHat = go_hats[iCurrentHat + 1];

        }


        // for each character materials in the list
        for (int i = 0; i < go_hats.Length; i++) {

            // if current outfit is = 1, show mat 1 etc
            if (iCurrentHat == i) {

                // change the material to the current integer of the outfit
                //go_HatSpawnPoint = go_hats[iCurrentHat];
                go_hats[iCurrentHat].SetActive(true);

            }

        }

    }











    // Change the hat to the right
    public void ChangeSkateboardRight() {

        // if the current outfit is less than or = to the length of meshes
        // Show the first material / outfit
        if (iCurrentSkateboard >= 11) {

            // set the current outfit to 0
            iCurrentSkateboard = 0;

        }

        // current outfit + (move to the next one)
        iCurrentHat++;

        //
        go_previousHat = go_hats[iCurrentHat - 1];

        // for each character materials in the list
        for (int i = 0; i < go_hats.Length; i++) {

            // if current outfit is = 1, show mat 1 etc
            if (iCurrentHat == i) {

                // change the material to the current integer of the outfit
                //go_HatSpawnPoint = go_hats[iCurrentHat];
                go_hats[iCurrentHat].SetActive(true);

            }

        }

    }



    #endregion

}
