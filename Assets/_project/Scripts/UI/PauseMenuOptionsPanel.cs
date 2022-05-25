using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using L7Games.UI;


public class PauseMenuOptionsPanel : MonoBehaviour
{
    public OptionsMenuSettingData lastSavedOptionsData;

    [Header("Options Variables")]
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown textureDropdown;
    public TMP_Dropdown aaDropdown;
    public Toggle fullScreenToggle;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambientSlider;
    public Slider sfxSlider;

    [SerializeField]
    private Canvas UiCanvas;

    private float currentVolume;
    Resolution[] resolutions;

    public UnityEngine.EventSystems.EventSystem currentEventSystem;
    [SerializeField]
    private PauseMenuController pauseMenuController;

    private FMOD.Studio.Bus masterBus;
    private FMOD.Studio.Bus musicBus;
    private FMOD.Studio.Bus ambientBus;
    private FMOD.Studio.Bus sfxBus;

    #region Unity Method
    private void Awake()
    {
        List<string> resolutionOptionsList = new List<string>();
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            resolutionOptionsList.Add(resolutions[i].ToString());
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptionsList);


        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.AddOptions(new List<string>() { "Custom" });

        List<string> textureOptions = new List<string>() {"High", "Medium", "Low", "Lowest" };
        textureDropdown.ClearOptions();
        textureDropdown.AddOptions(textureOptions);

        List<string> aaOptions = new List<string>() { "0", "2", "4", "8"};
        aaDropdown.ClearOptions();
        aaDropdown.AddOptions(aaOptions);


        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambient Sounds");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Player Sounds");
    }

    private void OnEnable()
    {
        //SetSliderVolumeFromBus(masterVolumeSlider, masterBus);
        //SetSliderVolumeFromBus(musicVolumeSlider, musicBus);
        //SetSliderVolumeFromBus(ambientSlider, ambientBus);
        //SetSliderVolumeFromBus(sfxSlider, sfxBus);



        // Pull from player prefs
        lastSavedOptionsData = OptionsMenuSettingData.LoadOptionsFromPlayerPrefs();
        List<string> resolutionOptionsList = new List<string>();
        resolutions = Screen.resolutions;
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            if (resolutions[i].height == lastSavedOptionsData.resolution.height &&
                resolutions[i].width == lastSavedOptionsData.resolution.width &&
                resolutions[i].refreshRate == lastSavedOptionsData.resolution.refreshRate)
            {
                currentResIndex = i;
            }
            resolutionOptionsList.Add(resolutions[i].ToString());
        }
        resolutionDropdown.SetValueWithoutNotify(currentResIndex);
        qualityDropdown.SetValueWithoutNotify(lastSavedOptionsData.qualityIndex);
        textureDropdown.SetValueWithoutNotify(lastSavedOptionsData.textureQuailityIndex);
        aaDropdown.SetValueWithoutNotify(lastSavedOptionsData.aaOptionIndex);
        masterVolumeSlider.SetValueWithoutNotify(lastSavedOptionsData.masterVolume);
        musicVolumeSlider.SetValueWithoutNotify(lastSavedOptionsData.musicVolume);
        ambientSlider.SetValueWithoutNotify(lastSavedOptionsData.ambientVolume);
        sfxSlider.SetValueWithoutNotify(lastSavedOptionsData.sfxVolume);
    }

    private void Start()
    {
        currentEventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
    }
    #endregion

    #region Public Methods
    #region Options Section
    /// <summary>
    /// Setting the general master volume (TO DO: Expand this to the FMOD way)
    /// </summary>
    public void SetMasterVolume()
    {
        masterBus.setVolume(masterVolumeSlider.value);
    }
    public void SetMusicVolume()
    {
        musicBus.setVolume(musicVolumeSlider.value);
    }

    public void SetAmbientVolume()
    {
        ambientBus.setVolume(ambientSlider.value);
    }

    public void SetSfxVolume()
    {
        sfxBus.setVolume(sfxSlider.value);
    }

    public void OnResolutionSelectionChanged()
    {
        SetResolution(resolutionDropdown.value);
    }

    public void OnQualitySelectionChanged()
    {
        SetQuality(qualityDropdown.value);
    }

    public void OnTextureQualitySelectedChanged()
    {
        SetTextureQuality(textureDropdown.value);
    }

    public void OnAASelectedChanged()
    {
        SetAntiAliasing(aaDropdown.value);
    }

    public void OnFullScreenValueChanged()
    {
        //Screen.fullScreen = fullScreenToggle.isOn;
        if (fullScreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    private void SetSettingBasedOnData(OptionsMenuSettingData optionsMenuSettingData)
    {
        resolutions = Screen.resolutions;
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            if (resolutions[i].height == optionsMenuSettingData.resolution.height &&
                resolutions[i].width == optionsMenuSettingData.resolution.width &&
                resolutions[i].refreshRate == optionsMenuSettingData.resolution.refreshRate)
            {
                currentResIndex = i;
            }
        }
        resolutionDropdown.value = currentResIndex;
        qualityDropdown.value = optionsMenuSettingData.qualityIndex;
        textureDropdown.value = optionsMenuSettingData.textureQuailityIndex;
        aaDropdown.value = optionsMenuSettingData.aaOptionIndex;
        masterVolumeSlider.value = optionsMenuSettingData.masterVolume;
        musicVolumeSlider.value = optionsMenuSettingData.musicVolume;
        ambientSlider.value = optionsMenuSettingData.ambientVolume;
        sfxSlider.value = optionsMenuSettingData.sfxVolume;
    }

    public void OnRevert()
    {
        SetSettingBasedOnData(lastSavedOptionsData);
    }

    public void OnApply()
    {
        lastSavedOptionsData = new OptionsMenuSettingData();
        Resolution resolution = resolutions[resolutionDropdown.value];
        lastSavedOptionsData.resolution = resolution;
        lastSavedOptionsData.qualityIndex = qualityDropdown.value;
        lastSavedOptionsData.textureQuailityIndex = textureDropdown.value;
        lastSavedOptionsData.aaOptionIndex = aaDropdown.value;

        lastSavedOptionsData.masterVolume = masterVolumeSlider.value;
        lastSavedOptionsData.musicVolume = musicVolumeSlider.value;
        lastSavedOptionsData.ambientVolume = ambientSlider.value;
        lastSavedOptionsData.sfxVolume = sfxSlider.value;
        OptionsMenuSettingData.SaveToPlayerPrefs(lastSavedOptionsData);
    }

    public void OnWindowCloseButton()
    {
        WarningBox warningBox = WarningBox.CreateWarningBox(UiCanvas, currentEventSystem, "Do you want to save new setting");
        Button cancelButton = warningBox.AddButton("Cancel");
        cancelButton.onClick.AddListener(OnWarningBoxCancel);
        cancelButton.onClick.AddListener(warningBox.CloseBox);
        Button noButton = warningBox.AddCancelButton("No");
        noButton.onClick.AddListener(OnRevert);
        noButton.onClick.AddListener(CloseOption);
        noButton.onClick.AddListener(pauseMenuController.OnOptionMenuClose);
        noButton.onClick.AddListener(warningBox.CloseBox);
        Button yesButton = warningBox.AddCancelButton("Yes");
        yesButton.onClick.AddListener(OnApply);
        yesButton.onClick.AddListener(CloseOption);
        yesButton.onClick.AddListener(pauseMenuController.OnOptionMenuClose);
        yesButton.onClick.AddListener(warningBox.CloseBox);
    }
    public void OnWarningBoxCancel()
    {
        currentEventSystem.SetSelectedGameObject(resolutionDropdown.gameObject);
    }

    public void CloseOption()
    {
        gameObject.SetActive(false);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullScreenToggle.isOn);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.SetValueWithoutNotify(qualityDropdown.options.Count - 1);
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = GetSampleFromIndex(aaIndex);
        qualityDropdown.SetValueWithoutNotify(qualityDropdown.options.Count - 1);
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        //switch (qualityIndex)
        //{
        //    case 0: // quality level - very low
        //        textureDropdown.SetValueWithoutNotify(3);
        //        aaDropdown.SetValueWithoutNotify(0);
        //        break;
        //    case 1: // quality level - low
        //        textureDropdown.SetValueWithoutNotify(2);
        //        aaDropdown.SetValueWithoutNotify(0);
        //        break;
        //    case 2: // quality level - medium
        //        textureDropdown.SetValueWithoutNotify(1);
        //        aaDropdown.SetValueWithoutNotify(0);
        //        break;
        //    case 3: // quality level - high
        //        textureDropdown.SetValueWithoutNotify(0);
        //        aaDropdown.SetValueWithoutNotify(0);
        //        break;
        //    case 4: // quality level - very high
        //        textureDropdown.SetValueWithoutNotify(0);
        //        aaDropdown.SetValueWithoutNotify(1);
        //        break;
        //    case 5: // quality level - ultra
        //        textureDropdown.SetValueWithoutNotify(0);
        //        aaDropdown.SetValueWithoutNotify(2);
        //        break;
        //}
        textureDropdown.SetValueWithoutNotify(QualitySettings.masterTextureLimit);
        aaDropdown.SetValueWithoutNotify(GetIndexFromSample(QualitySettings.antiAliasing));
        qualityDropdown.value = qualityIndex;
    }
    #endregion
    #endregion

    private void SetSliderVolumeFromBus(Slider slider, FMOD.Studio.Bus bus)
    {
        if (bus.getVolume(out float currenVol) == FMOD.RESULT.OK)
        {
            slider.value = currenVol;
        }
        else
        {
            Debug.LogError("Bus Volume not gotten for bus: " + bus.ToString());
            slider.value = 1f;
        }
    }

    private int GetIndexFromSample(int sampleCount)
    {
        switch (sampleCount)
        {
            case (0):
                return 0;
            case (2):
                return 1;
            case (4):
                return 2;
            case (8):
                return 3;
            default:
                return -1;
        }
    }

    private int GetSampleFromIndex(int index)
    {
        switch (index)
        {
            case (0):
                return 0;
            case (1):
                return 2;
            case (2):
                return 4;
            case (3):
                return 8;
            default:
                return 0;
        }
    }
}
