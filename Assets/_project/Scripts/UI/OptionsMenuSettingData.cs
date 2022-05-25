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
    public float ambientSlider;
    public float sfxSlider;

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
        PlayerPrefs.SetFloat(ambientVolumePrefName, optionsMenuSettingData.ambientSlider);
        PlayerPrefs.SetFloat(sfxVolumePrefName,     optionsMenuSettingData.sfxSlider);

        PlayerPrefs.Save();
    }

    public static OptionsMenuSettingData LoadOptionsFromPlayerPrefs()
    {
        OptionsMenuSettingData newData = new OptionsMenuSettingData();
        newData.resolution.width = PlayerPrefs.GetInt(ResolutionWidthPrefName);
        newData.resolution.height = PlayerPrefs.GetInt(ResolutionHeightPrefName);
        newData.resolution.refreshRate = PlayerPrefs.GetInt(ResolutionRefeshRatePrefName);

        newData.qualityIndex = PlayerPrefs.GetInt(QualityPrefName);
        newData.textureQuailityIndex = PlayerPrefs.GetInt(TexturePrefName);
        newData.aaOptionIndex = PlayerPrefs.GetInt(AAPrefName);

        newData.masterVolume = PlayerPrefs.GetInt(masterVolumePrefName);
        newData.musicVolume = PlayerPrefs.GetInt(musicVolumePrefName);
        newData.ambientSlider = PlayerPrefs.GetInt(ambientVolumePrefName);
        newData.sfxSlider = PlayerPrefs.GetInt(sfxVolumePrefName);
        return newData;
    }
}
