using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Page
{
    [SerializeField] public string upperText;
    [SerializeField] public string mainText;
    [SerializeField] public float spentTimeInHours;
    [SerializeField] public Sprite backgroundImage;
    // [SerializeField] public List<CharacterName> charactersToShow = new List<CharacterName>();

    [SerializeField] public List<Sprite> spritesOfCharsToShow = new List<Sprite>();
    [SerializeField] public bool changeCharsToShow = false;

    [SerializeField] public AudioClip musicToPlay;

    public Page(string upperText, string mainText)
    {
        this.upperText = upperText;
        this.mainText = mainText;
        spentTimeInHours = 0f;
    }
}
