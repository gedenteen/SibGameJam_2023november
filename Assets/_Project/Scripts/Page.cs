using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Page
{
    public string text;
    public float spentTimeInHours;
    public List<CharacterName> charactersToShow = new List<CharacterName>();
}
