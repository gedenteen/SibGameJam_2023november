using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Page
{
    [Header("CUTSCENE?")]
    [SerializeField] public bool itIsCutscene = false;
    [SerializeField] public Cutscenes cutscene;

    [Header("TEXT")]
    [SerializeField] public string upperText;
    [SerializeField] [TextAreaAttribute] public string mainText;
    [SerializeField] public float spentTimeInHours;

    [Header("BACKGROUND")]
    [SerializeField] public Sprite backgroundImage;


    [Header("CHARACTERS")]
    [SerializeField] public bool changeCharsToShow = false;
    [SerializeField] public List<Sprite> spritesOfCharsToShow = new List<Sprite>();
    [SerializeField] public List<CharacterName> charactersToShow = new List<CharacterName>();

    [Header("AUDIO")]
    [SerializeField] public AudioClip musicToPlay;

    [SerializeField] public bool gameEnd = false;
    // [SerializeField] public bool gameEnd = false;

    public Page(string upperText, string mainText)
    {
        this.upperText = upperText;
        this.mainText = mainText;
        spentTimeInHours = 0f;
    }
}
