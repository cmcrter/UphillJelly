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
using L7Games;
using Cinemachine;

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

    [Header("Profanity Filter")]
    public ProfanityFilter wordFilter;

    [Header("Profile Selection UI Buttons")]
    [SerializeField]
    private List<TMP_Text> ProfileButtonTexts = new List<TMP_Text>();

    private string thisName;

    [SerializeField]
    private ShopManager shop;

    [SerializeField]
    private int defaultStartingAmount = 15;

    [Header("Variables for warning box")]
    [SerializeField]
    private UITransitionManager transitionManager;
    [SerializeField]
    private CinemachineVirtualCamera mainCamera;
    [SerializeField]
    private Canvas profileCanvas;
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private GameObject levelButton;

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
        wordFilter.SetUpList();
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

        if(LoadingData.player != null)
        {
            PushValuesToShop();
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

        thisName = LoadingData.player.profileName.ToUpper();
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
        if(thisName.Length == 0)
        {
            WarningBox.CreateConfirmOnlyWarningBox(profileCanvas, eventSystem, "Must Include A Name", Empty);
            return;
        }

        if(ProfanityFilter.isStringInList(thisName))
        {
            WarningBox.CreateConfirmOnlyWarningBox(profileCanvas, eventSystem, "Inappropriate Name", Empty);
            return;
        }

        LoadingData.player.profileName = tmp_Input.text.ToUpper();
        b_SaveSystem.SavePlayer(LoadingData.playerSlot);

        PushValuesToShop();
        GetValuesFromShop();
        MoveToMenu();
    }

    //Definitely making a new profile (overwrite functionality)
    public void NewProfile()
    {
        if(thisName.Length == 0)
        {
            WarningBox.CreateConfirmOnlyWarningBox(profileCanvas, eventSystem, "Must Include A Name", Empty);
            return;
        }

        if(ProfanityFilter.isStringInList(thisName))
        {
            WarningBox.CreateConfirmOnlyWarningBox(profileCanvas, eventSystem, "Inappropriate Name", Empty);
            return;
        }

        LoadingData.player = new StoredPlayerProfile
        {
            profileName = tmp_Input.text.ToUpper(),
            iCurrency = defaultStartingAmount
        };

        b_SaveSystem.SavePlayer(LoadingData.playerSlot);

        //Slow way of doing it right now
        shop.ResetItemPurchases();

        PushValuesToShop();
        MoveToMenu();
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
            ItemObjectSO item = shop.FindChar(i);
            shop.shopInventory.purchasedItems.Add(item);
            item.isPurchased = true;
        }

        for (int i = 0; i < LoadingData.player.savedHatList.Count; ++i)
        {
            ItemObjectSO item = shop.FindHat(i);
            shop.shopInventory.purchasedItems.Add(shop.FindHat(i));
            item.isPurchased = true;
        }

        for (int i = 0; i < LoadingData.player.savedBoardList.Count; ++i)
        {
            ItemObjectSO item = shop.FindSkateboard(i);
            shop.shopInventory.purchasedItems.Add(shop.FindSkateboard(i));
            item.isPurchased = true;
        }

        shop.ApplyCurrentToPreview();
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

    private void MoveToMenu()
    {
        // set the starting background to false
        profileSelectionScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(levelButton);
        transitionManager.UpdateCamera(mainCamera);
    }

    private void Empty()
    {

    }
}