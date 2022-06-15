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

public class ProfanityFilter
{
    public TextAsset textBlockList;
    [SerializeField]
    string[] strBlockList;

    public void SetUpList()
    {
        strBlockList = textBlockList.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public bool isStringInList(string givenString)
    {
        for(int i = 0; i < strBlockList.Length; ++i)
        {
            string profanity = strBlockList[i];
            Regex word = new Regex("(?i)(\\b" + profanity + "\\b");

            if(word.IsMatch(givenString))
            {
                return true;
            }
        }

        return false;
    }
}
