using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CharacterName
{
    BEGIN = 0,
    MussWithoutWatchesAndLegs = 10,
    MussCalm = 11,
    MussCalmWithoutLegs = 12,
    MussFight = 13,
    PrincessCalm = 20,
    PrincessCalmWithoutLegs = 21,
    OwlCalm = 30,

    END = 99
}

[System.Serializable]
public class Character
{
    [SerializeField] public CharacterName name;
    [SerializeField] public Sprite sprite;
    [SerializeField] public float yRotation = 0f;
    [SerializeField] public Vector2 position;
    [SerializeField] public Vector2 size;
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
