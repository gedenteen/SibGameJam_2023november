using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gameplay : MonoBehaviour
{
    [Header("Links on scene")]
    [SerializeField] TextMeshProUGUI textForDialogue;

    [Header("Links to assets")]
    [SerializeField] Chapter chapter;

    private int currentPageId = 0;

    private void Start()
    {
        textForDialogue.text = chapter.pages[currentPageId].text;
    }

    public void ShowNextPage()
    {
        Debug.Log("Gamepalay: ShowNextPage: begin");
        currentPageId++;
        textForDialogue.text = chapter.pages[currentPageId].text;
    }
}
