////////////////////////////////////////////////////////////
// File: PlayerProfile.cs
// Author: Jack Peedle, Charles Carter
// Date Created: 24/10/21
// Last Edited By: Charles Carter
// Date Last Edited: 06/05/22
// Brief: A data container for the current profile from file (without connections to scriptable objects)
//////////////////////////////////////////////////////////// 

using System.Collections.Generic;

// save it in file
[System.Serializable]
public class StoredPlayerProfile
{
    public string profileName;

    // int for currency
    public int iCurrency = 0;

    //THE IDs REPRESENT SCRIPTABLE OBJECT IDs
    // list of the saved hat ints and saved character ints
    public List<int> savedHatList = new List<int>();
    public List<int> savedCharacterList = new List<int>();
    public List<int> savedBoardList = new List<int>();

    public int equippedHat = 0;
    public int equippedCharacter = 0;
    public int equippedBoard = 0;

    public StoredPlayerProfile()
    {

    }
}