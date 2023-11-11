using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneHandler : MonoBehaviour
{
    //public static CutsceneHandler instance;
    [SerializeField] private Gameplay gameplay;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Sprite[] spritesOfScroll;
    [SerializeField] private float scrollAnimationDelay = 0.15f;
    [SerializeField] private Image imageForScroll;
    [SerializeField] private TextMeshProUGUI textOnScroll;
    private bool animationWasShown = false;

    private IEnumerator coroutineForTextAnimation = null;

    private void Awake()
    {
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/

        //Handle(Cutscenes.introScroll);
    }

    public void Handle(Cutscenes cutscene, string text)
    {
        switch (cutscene)
        {
            case Cutscenes.introScroll:
                StartCoroutine(IntroScroll(text));
                break;
            default:
                Debug.LogError($"CutsceneHandler: Handle: unexpected {cutscene}");
                return;
        }

        Show();
    }

    public void Hide()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public void Show()
    {
        canvasGroup.gameObject.SetActive(true);
    }


    private IEnumerator IntroScroll(string text)
    {
        if (!animationWasShown)
        {
            int ind = 0;
            while (ind < spritesOfScroll.Length)
            {
                imageForScroll.sprite = spritesOfScroll[ind];
                yield return new WaitForSeconds(scrollAnimationDelay);
                ind++;
            }

            animationWasShown = true;
        }

        //coroutineForTextAnimation = CommonThings.AnimationForText(textOnScroll, text);
        //StartCoroutine(coroutineForTextAnimation);
        textOnScroll.text = text;
    }
}
