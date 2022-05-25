////////////////////////////////////////////////////////////
// File: ProfileSelector.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 20/11/21
// Last Edited By: Charles Carter
// Date Last Edited: 23/05/22
// Brief: A script to control the text for all of the profiles
//////////////////////////////////////////////////////////// 

using UnityEngine;
using TMPro;
using System.IO;
using L7Games.Loading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ProfileSelector : MonoBehaviour
{
    public const int MAX_PROFILE_AMOUNT = 3;

    static ProfileSelector instance;
    public StoredPlayerProfile b_player;

    // text input field
    public TMP_InputField tmp_Input;

    [SerializeField]
    private GameObject profileSelectionScreen;
    [SerializeField]
    private GameObject profileEditingScreen;

    [Header("Profile Selection UI Buttons")]
    [SerializeField]
    private List<TMP_Text> ProfileButtonTexts = new List<TMP_Text>();

    private string thisName;

    [SerializeField]
    private ShopManager shop;

    [SerializeField]
    private int defaultStartingAmount = 15;

    private void Awake()
    {
        //Not a singleton
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        LoadingData.currentLevel = LEVEL.MAINMENU;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Going through and making the right local file and text changes
        for(int i = 0; i < MAX_PROFILE_AMOUNT; ++i)
        {
            if(Directory.Exists(Application.persistentDataPath + "/CurrentProfile" + (i + 1).ToString()))
            {
                // Delete the file
                Directory.Delete(Application.persistentDataPath + "/CurrentProfile" + (i + 1).ToString());
            }

            // create a string called "path" which is the persistent data path %AppData% and call the file Player.txt
            string path = Application.persistentDataPath + "/Profile" + (i + 1).ToString() + "/ProfileData/Profile" + (i + 1).ToString() + "Data.sdat";

            // if a file exists in the "path"
            if(File.Exists(path))
            {
                // profile text = Load Profile 1
                ProfileButtonTexts[i].text = "Load Profile" + " " + (i + 1).ToString();
            }
            else
            {
                // load profile text = new game
                ProfileButtonTexts[i].text = "New Game";
            }
        }
    }

    //Profile Selected On Initial Screen
    public void ProfileSelected(int profileSlot)
    {
        LoadingData.playerSlot = profileSlot;

        // set the starting background to false
        profileSelectionScreen.SetActive(false);
        profileEditingScreen.SetActive(true);

        // if no directory exists
        if(!Directory.Exists(Application.persistentDataPath + "/CurrentProfile" + LoadingData.playerSlot.ToString()))
        {
            // create a directory for "/CurrentProfile"
            Directory.CreateDirectory(Application.persistentDataPath + "/CurrentProfile" + LoadingData.playerSlot.ToString());
            //b_SaveSystem.SavePlayer(LoadingData.playerSlot);
        }

        LoadingData.player = b_SaveSystem.LoadPlayer(LoadingData.playerSlot);

        if(LoadingData.player == null)
        {
            LoadingData.player = new StoredPlayerProfile();
        }

        tmp_Input.text = LoadingData.player.profileName;
        tmp_Input.textComponent.text = LoadingData.player.profileName;

        thisName = LoadingData.player.profileName;
    }

    //Being able to change a profile name based on text input
    public void ChangePlayerName(string newName)
    {
        thisName = newName;
    }

    public void BackButton()
    {
        profileEditingScreen.SetActive(false);
        profileSelectionScreen.SetActive(true);
    }

    public void ConfirmProfile()
    {
        LoadingData.player.profileName = thisName;
        b_SaveSystem.SavePlayer(LoadingData.playerSlot);

        PushValuesToShop();

        // set the starting background to false
        profileSelectionScreen.SetActive(false);
    }

    //Definitely making a new profile (overwrite functionality)
    public void NewProfile()
    {
        LoadingData.player = new StoredPlayerProfile();
        LoadingData.player.profileName = thisName;
        LoadingData.player.iCurrency = defaultStartingAmount;

        b_SaveSystem.SavePlayer(LoadingData.playerSlot);

        PushValuesToShop();

        // set the starting background to false
        profileSelectionScreen.SetActive(false);
    }

    public static void QuitButton()
    {
        Application.Quit();
    }

    public void PushValuesToShop()
    {
        if (!shop) return;

        shop.playerCurrency = LoadingData.player.iCurrency;
        shop.UpdateCurrencyText();

        shop.currentEquippedCharacter = shop.FindChar(LoadingData.player.equippedCharacter);
        shop.currentEquippedHat = shop.FindHat(LoadingData.player.equippedHat);
        shop.currentEquippedSkateboard = shop.FindSkateboard(LoadingData.player.equippedBoard);

        for (int i = 0; i < LoadingData.player.savedCharacterList.Count; ++i)
        {
            shop.shopInventory.purchasedItems.Add(shop.FindChar(i));
        }

        for (int i = 0; i < LoadingData.player.savedHatList.Count; ++i)
        {
            shop.shopInventory.purchasedItems.Add(shop.FindHat(i));
        }

        for (int i = 0; i < LoadingData.player.savedBoardList.Count; ++i)
        {
            shop.shopInventory.purchasedItems.Add(shop.FindSkateboard(i));
        }
    }

    public void GetValuesFromShop()
    {
        if (!shop) return;
        if (LoadingData.player == null) return;

        LoadingData.player.iCurrency = shop.playerCurrency;
        LoadingData.shopItems = new ItemObjectSO[3];
        ItemObjectSO[] currentEquip = shop.ReturnEquippedObjects();

        if (currentEquip[0] != null)
        {
            LoadingData.shopItems[0] = currentEquip[0];
            LoadingData.player.equippedCharacter = currentEquip[0].Id;
        }

        if (currentEquip[1] != null)
        {
            LoadingData.player.equippedHat = currentEquip[1].Id;
            LoadingData.shopItems[1] = currentEquip[1];
        }

        if (currentEquip[2] != null)
        {
            LoadingData.player.equippedBoard = currentEquip[2].Id;
            LoadingData.shopItems[2] = currentEquip[2];
        }

        for (int i = 0; i < shop.shopInventory.purchasedItems.Count; ++i)
        {
            ItemObjectSO thisItem = (ItemObjectSO)shop.shopInventory.purchasedItems[i];

            switch (thisItem.type)
            {
                case ItemType.CharacterSkin:
                    LoadingData.player.savedCharacterList.Add(thisItem.Id);
                    break;
                case ItemType.Hat:
                    LoadingData.player.savedHatList.Add(thisItem.Id);
                    break;
                case ItemType.Skateboard:
                    LoadingData.player.savedBoardList.Add(thisItem.Id);
                    break;
            }
        }
    }
}