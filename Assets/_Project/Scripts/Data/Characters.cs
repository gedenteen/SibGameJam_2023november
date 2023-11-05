using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CharacterName
{
    Mouse1,
    Mouse2,
    Snake
}

[System.Serializable]
public class Character
{
    [SerializeField] public CharacterName name;
    [SerializeField] public Sprite sprite;
}

[CreateAssetMenu(fileName = "CharactersData", menuName = "ScriptableObjects/Create characters data")]
public class CharactersData : ScriptableObject
{
    [SerializeField] private Character[] array;

    public Character[] Array
    {
        get
        {
            return array;
        }
    }
}
