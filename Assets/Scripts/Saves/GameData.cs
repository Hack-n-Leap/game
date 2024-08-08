using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string playerLevel = "Level1";
    public Vector2 playerPosition;
    public List<KeyCode> playerFunctionsKey = new List<KeyCode> {KeyCode.None, KeyCode.D, KeyCode.A, KeyCode.Space, KeyCode.None, KeyCode.R, KeyCode.None, KeyCode.None };
    public List<bool> playerUnlockedFunctions = new List<bool>(new bool[10]);
    public int selectedLanguage = 0;
}
