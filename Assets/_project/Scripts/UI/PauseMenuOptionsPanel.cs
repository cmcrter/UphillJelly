using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using FMODUnity;


public class PauseMenuOptionsPanel : MonoBehaviour
{
    public OptionsMenuSettingData currentOptionsData;

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

    private float currentVolume;
    Resolution[] resolutions;

    public UnityEngine.EventSystems.EventSystem currentEventSYstem;

    private FMOD.Studio.Bus masterBus;
    private FMOD.Studio.Bus musicBus;
    private FMOD.Studio.Bus ambientBus;
    private FMOD.Studio.Bus sfxBus;

    #region Unity Method
    private void Awake()
    {
        List<string> resolutionOptionsList = new List<string>();
        resolutions = Screen.resolutions;
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            if (resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResIndex = i;
            }
            resolutionOptionsList.Add(resolutions[i].ToString());
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptionsList);
        resolutionDropdown.SetValueWithoutNotify(currentResIndex);

        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.AddOptions(new List<string>() { "Custom" });
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());

        List<string> textureOptions = new List<string>() {"High", "Medium", "Low", "Lowest" };
        textureDropdown.ClearOptions();
        textureDropdown.AddOptions(textureOptions);
        textureDropdown.SetValueWithoutNotify(QualitySettings.masterTextureLimit);

        List<string> aaOptions = new List<string>() { "0", "2", "4", "8"};
        aaDropdown.ClearOptions();
        aaDropdown.AddOptions(aaOptions);
        aaDropdown.SetValueWithoutNotify(GetIndexFromSample(QualitySettings.antiAliasing));


        masterBus = RuntimeManager.GetBus("bus:/");
        ambientBus = RuntimeManager.GetBus("bus:/Ambient Sounds");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Player Sounds");
    }

    private void OnEnable()
    {
        SetSliderVolumeFromBus(masterVolumeSlider, masterBus);
        SetSliderVolumeFromBus(musicVolumeSlider, musicBus);
        SetSliderVolumeFromBus(ambientSlider, ambientBus);
        SetSliderVolumeFromBus(sfxSlider, sfxBus);

        //currentOptionsData = new OptionsMenuSettingData();
        //for (int i = 0; i < resolutions.Length; ++i)
        //{
        //    if (resolutions[i].height == Screen.currentResolution.height &&
        //        resolutions[i].width == Screen.currentResolution.width &&
        //        resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
        //    {
        //        currentResIndex = i;
        //    }
        //}
        //currentOptionsData.resolution = 
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
