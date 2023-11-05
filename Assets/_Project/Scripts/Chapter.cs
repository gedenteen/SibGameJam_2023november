using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChapter", menuName = "ScriptableObjects/Create Chapter")]
public class Chapter : ScriptableObject
{
    public List<Page> pages;
    public string[] textForActions;
    public Chapter[] nextChapters;
}
