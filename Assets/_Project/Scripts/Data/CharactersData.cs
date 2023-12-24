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
    MussFightWithoutLegs = 14,
    PrincessCalm = 20,
    PrincessCalmWithoutLegs = 21,
    PrincessSniffy = 22,
    PrincessSniffyWithoutLegs = 23,
    OwlCalm = 30,
    OwlSleep = 31,
    OwlBadEnd = 32,
    LaskaCalm = 40,
    LaskaWithHand = 41,
    LaskaBadEnd = 42,
    SnakeCalm = 50,
    SnakeWithFlower = 51,
    SnakeBadEnd = 52,
    HedgehogCalm = 60,
    HedgehogAngry = 61,
    HedgehogBadEnd = 62,
    MouseKing = 70,
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
