using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using FMODUnity;


public class PauseMenuOptionsPanel : MonoBehaviour
{
    [Header("Options Variables")]
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown textureDropdown;
    public TMP_Dropdown aaDropdown;

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
        Resolution[] resOptions = Screen.resolutions;
        for (int i = 0; i < resOptions.Length; ++i)
        {
            resolutionOptionsList.Add(resOptions[i].ToString());
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptionsList);

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

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }

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
}
