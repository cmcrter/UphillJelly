using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PresetNameSelector : MonoBehaviour
{
    public const int characterLimit = 8;

    public TMP_Dropdown firstDropdown;
    public TMP_Dropdown secondDropdown;

    public TMP_InputField textInputField;

    private string[] firstOptions;
    private string[] secondOptions;

    // Start is called before the first frame update
    void Start()
    {
        firstOptions = new string[firstDropdown.options.Count];
        for (int i = 0; i < firstOptions.Length; ++i)
        {
            firstOptions[i] = firstDropdown.options[i].text;
        }

        secondOptions = new string[secondDropdown.options.Count];
        for (int i = 0; i < secondOptions.Length; ++i)
        {
            secondOptions[i] = secondDropdown.options[i].text;
        }
    }

    public void ApplyName()
    {
        textInputField.text = firstDropdown.options[firstDropdown.value].text + secondDropdown.options[secondDropdown.value].text;
    }

    public void OnFirstOptionSelected()
    {
        OnOptionSelected(firstDropdown, secondDropdown, secondOptions);
    }

    public void OnSecondOptionSelected()
    {
        OnOptionSelected(secondDropdown, firstDropdown, firstOptions);
    }

    private void OnOptionSelected(TMP_Dropdown selectedDropDown, TMP_Dropdown otherDropDown, string[] allOtherDropdownOptions)
    {
        // Get the second options selection
        string otherOptionSelection = otherDropDown.options[otherDropDown.value].text;

        SetOptions(GetApplicableOptions(characterLimit - selectedDropDown.options[selectedDropDown.value].text.Length, allOtherDropdownOptions), otherDropDown);

        // find the select option new value and reset it
        bool optionFound = false;
        for (int i = 0; i < otherDropDown.options.Count; ++i)
        {
            if (otherOptionSelection == otherDropDown.options[i].text)
            {
                otherDropDown.SetValueWithoutNotify(i);
                optionFound = true;
                break;
            }
        }
        if (!optionFound)
        {
            otherDropDown.value = 0;
        }
    }

    private void SetFirstOptions(List<string> options)
    {
        firstDropdown.ClearOptions();
        firstDropdown.AddOptions(options);
    }
    private void SetSecondOptions(List<string> options)
    {
        secondDropdown.ClearOptions();
        secondDropdown.AddOptions(options);
    }

    private void SetOptions(List<string> options, TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    private List<string> GetAllApplicableFirstOptions(int characterLimit)
    {
        List<string> presets = new List<string>();
        for (int i = 0; i < firstOptions.Length; ++i)
        {
            if (firstOptions[i].Length <= characterLimit)
            {
                presets.Add(firstOptions[i]);
            }
        }
        return presets;
    }

    private List<string> GetAllApplicableSecondOptions(int characterLimit)
    {
        List<string> presets = new List<string>();
        for (int i = 0; i < secondOptions.Length; ++i)
        {
            if (secondOptions[i].Length <= characterLimit)
            {
                presets.Add(secondOptions[i]);
            }
        }
        return presets;
    }

    private List<string> GetApplicableOptions(int characterLimit, string[] allOption)
    {
        List<string> presets = new List<string>();
        for (int i = 0; i < allOption.Length; ++i)
        {
            if (allOption[i].Length <= characterLimit)
            {
                presets.Add(allOption[i]);
            }
        }
        return presets;
    }
}
