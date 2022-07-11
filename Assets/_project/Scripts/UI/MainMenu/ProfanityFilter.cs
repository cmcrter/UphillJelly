////////////////////////////////////////////////////////////
// File: ProfanityFilter.cs
// Author: Charles Carter
// Date Created: 15/06/22
// Last Edited By: Charles Carter
// Date Last Edited: 15/06/22
// Brief: A script for the purpose of checking a given string against a list of banned words
//////////////////////////////////////////////////////////// 

using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
public class ProfanityFilter
{
    public TextAsset textBlockList;
    [SerializeField]
    private string[] strBlockList;

    private static string[] staticBlockList;

    public void SetUpList()
    {
        if(!textBlockList)
        {
            if(Debug.isDebugBuild)
            {
                Debug.Log("No Banned Words List!");
            }

            return;
        }

        strBlockList = textBlockList.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        staticBlockList = strBlockList;
    }

    private void OnValidate()
    {
        if (!textBlockList)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("No Banned Words List!");
            }

            return;
        }

        strBlockList = textBlockList.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        staticBlockList = strBlockList;
    }

    //Searching through the static list 
    public static bool isStringInList(string givenString)
    {
        if(staticBlockList == null)
        {
            if (Debug.isDebugBuild)
            {
                Debug.LogWarning("No profanity words list");
            }
            return false;
        }

        //The lists will generally be lower case to make it easier
        givenString.ToLower();

        for (int i = 0; i < staticBlockList.Length; ++i)
        {
            string profanity = staticBlockList[i].ToLower();

            //Getting profanity
            Regex word = new Regex("(?i)(\\b" + profanity + "\\b)");

            //Removing spaces
            givenString = Regex.Replace(givenString, @"\s+", "");
            string givenProfanity = Regex.Replace(givenString, @"\s+", "");

            //Checking if the string is a variant of the profanity
            if (word.IsMatch(givenString) || givenProfanity.Equals(givenString))
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log(" match " + givenString);
                }

                return true;
            }
        }

        return false;
    }
}
