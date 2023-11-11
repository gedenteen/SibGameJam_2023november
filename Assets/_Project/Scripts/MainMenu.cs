using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject myCanvasGroup;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Gameplay gameplay;

    private bool isActive = true;
    private bool exitedFromMainMenu = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowOrHide();
        }
    }

    public void ShowOrHide()
    {
        isActive = !isActive;
        myCanvasGroup.SetActive(isActive);

        if (isActive == false && exitedFromMainMenu == false)
        {
            exitedFromMainMenu = true;
            backgroundImage.sprite = null;
            backgroundImage.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void ButtonStart()
    {
        if (!gameplay.gameWasStarted)
            gameplay.StartGame();

        ShowOrHide();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
