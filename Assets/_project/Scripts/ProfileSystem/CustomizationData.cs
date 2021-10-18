////////////////////////////////////////////////////////////
// File: CustomizationData.cs
// Author: Jack Peedle
// Date Created: 18/10/21
// Last Edited By: Jack Peedle
// Date Last Edited: 18/10/21
// Brief: A script to save and load the players customization gameobjects and other data like currency
//////////////////////////////////////////////////////////// 

using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class CustomizationData : MonoBehaviour
{

    // Reference to the TextMeshPro input field
    public TMP_InputField TMPProfileTextInput;

    // players current material
    public Material playerCurrentMaterial;

    // current active hat on the player
    public GameObject theCurrentActiveHat;

    // public int currency for the shop
    public int Currency;

    // public float for the hat spawn position
    public float[] hatSpawningPosition;

    // public float for the skateboard spawn position
    //public float[] skateboardSpawnPosition;

    // Customization data controller
    public CustomizationData(ProfileController profileController) {

        //
        TMPProfileTextInput = profileController.TMPProfileTextInput;

        //
        playerCurrentMaterial = profileController.CurrentActivePlayerMaterial;

        //
        theCurrentActiveHat = profileController.currentActiveHat;

        // Add current active skateboard
        // In this space

        //
        Vector3 hatSpawnPosition = profileController.hatSpawnPosition.transform.position;

        //
        hatSpawningPosition = new float[] {

            //
            hatSpawnPosition.x, hatSpawnPosition.y, hatSpawnPosition.z

        };

        //
        Currency = profileController.Currency;

    }

}
