using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuSettingData
{
    public const string ResolutionPrefName = "Resolution";
    public const string ResolutionWidthPrefName = ResolutionPrefName + "Width";
    public const string ResolutionHeightPrefName = ResolutionPrefName + "Height";
    public const string ResolutionRefeshRatePrefName = ResolutionPrefName + "RefreshRate";

    public const string QualityPrefName = "Quality";
    public const string TexturePrefName = "Texture";
    public const string AAPrefName = "AA";

    public const string masterVolumePrefName = "MasterVolume";
    public const string musicVolumePrefName = "MusicVolume";
    public const string ambientVolumePrefName = "AmbientVolume";
    public const string sfxVolumePrefName = "SXFVolume";

    public Resolution resolution;
    public int qualityIndex;
    public int textureQuailityIndex;
    public int aaOptionIndex;

    public float masterVolume;
    public float musicVolume;
    public float ambientVolume;
    public float sfxVolume;

    /// <summary>
    /// Constructor with defaults
    /// </summary>
    public OptionsMenuSettingData()
    {
        resolution = Screen.currentResolution;
        qualityIndex = QualitySettings.GetQualityLevel();
        textureQuailityIndex = QualitySettings.masterTextureLimit;
        aaOptionIndex = QualitySettings.antiAliasing;

        masterVolume = 0.5f;
        musicVolume = 0.5f;
        ambientVolume = 0.5f;
        sfxVolume = 0.5f;
    }

    public static void SaveToPlayerPrefs(OptionsMenuSettingData optionsMenuSettingData)
    {
        PlayerPrefs.SetInt(ResolutionWidthPrefName, optionsMenuSettingData.resolution.width);
        PlayerPrefs.SetInt(ResolutionHeightPrefName, optionsMenuSettingData.resolution.height);
        PlayerPrefs.SetInt(ResolutionRefeshRatePrefName, optionsMenuSettingData.resolution.refreshRate);

        PlayerPrefs.SetInt(QualityPrefName, optionsMenuSettingData.qualityIndex);
        PlayerPrefs.SetInt(TexturePrefName, optionsMenuSettingData.textureQuailityIndex);
        PlayerPrefs.SetInt(AAPrefName, optionsMenuSettingData.aaOptionIndex);

        PlayerPrefs.SetFloat(masterVolumePrefName,  optionsMenuSettingData.masterVolume);
        PlayerPrefs.SetFloat(musicVolumePrefName,   optionsMenuSettingData.musicVolume);
        PlayerPrefs.SetFloat(ambientVolumePrefName, optionsMenuSettingData.ambientVolume);
        PlayerPrefs.SetFloat(sfxVolumePrefName,     optionsMenuSettingData.sfxVolume);

        PlayerPrefs.Save();
    }

    public static OptionsMenuSettingData LoadOptionsFromPlayerPrefs()
    {
        OptionsMenuSettingData newData = new OptionsMenuSettingData();
        if (PlayerPrefs.HasKey(ResolutionWidthPrefName))
        {
            newData.resolution.width = PlayerPrefs.GetInt(ResolutionWidthPrefName);
        }
        if (PlayerPrefs.HasKey(ResolutionHeightPrefName))
        {
            newData.resolution.height = PlayerPrefs.GetInt(ResolutionHeightPrefName);
        }
        if (PlayerPrefs.HasKey(ResolutionRefeshRatePrefName))
        {
            newData.resolution.refreshRate = PlayerPrefs.GetInt(ResolutionRefeshRatePrefName);
        }
        if (PlayerPrefs.HasKey(QualityPrefName))
        {
            newData.qualityIndex = PlayerPrefs.GetInt(QualityPrefName);
        }
        if (PlayerPrefs.HasKey(TexturePrefName))
        {
            newData.textureQuailityIndex = PlayerPrefs.GetInt(TexturePrefName);
        }
        if (PlayerPrefs.HasKey(AAPrefName))
        {
            newData.aaOptionIndex = PlayerPrefs.GetInt(AAPrefName);
        }

        if (PlayerPrefs.HasKey(masterVolumePrefName))
        {
            newData.masterVolume = PlayerPrefs.GetFloat(masterVolumePrefName);
        }
        if (PlayerPrefs.HasKey(musicVolumePrefName))
        {
            newData.musicVolume = PlayerPrefs.GetFloat(musicVolumePrefName);
        }
        if (PlayerPrefs.HasKey(ambientVolumePrefName))
        {
            newData.ambientVolume = PlayerPrefs.GetFloat(ambientVolumePrefName);
        }
        if (PlayerPrefs.HasKey(sfxVolumePrefName))
        {
            newData.sfxVolume = PlayerPrefs.GetFloat(sfxVolumePrefName);
        }
        return newData;
    }
}
