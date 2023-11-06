using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class Gameplay : MonoBehaviour
{
    [Header("Links on scene")]
    [SerializeField] TextMeshProUGUI textForName;
    [SerializeField] TextMeshProUGUI textForDialogue;
    [SerializeField] GameObject placeForActions;
    [SerializeField] Clock clock;
    [SerializeField] Image[] imagesForChars;

    [Header("Links to assets")]
    [SerializeField] Chapter chapter;
    [SerializeField] GameObject prefabButtonForAction;
    [SerializeField] CharactersData charactersData;

    [Header("Changable values")]
    [SerializeField] float currentTimeInHours = 0f;
    [SerializeField] float delayForPrintingText = 0.04f;

    private int currentPageId = -1;
    private List<GameObject> containerForButtonActions = new List<GameObject>();
    private Dictionary<CharacterName, Character> dictForCharacters = new Dictionary<CharacterName, Character>();
    private IEnumerator coroutineForTextAnimation = null;
    private bool textAnimataionIsRunning = false;
    private bool isUserSelectsAction = false;

    private void Awake()
    {
        foreach (Character character in charactersData.Array)
        {
            dictForCharacters[character.name] = character;
            Debug.Log($"Gameplay: Awake: add {character.name} to dictForCharacters");
        }
    }

    private void Start()
    {
        ShowNextPage();
    }

    IEnumerator AnimationForText(TextMeshProUGUI textmesh, string text)
    {
        textAnimataionIsRunning = true;

        textmesh.text = "";
        foreach (char c in text)
        {
            textmesh.text += c;
            yield return new WaitForSeconds(delayForPrintingText);
        }

        textAnimataionIsRunning = false;
    }

    public void ShowNextPage()
    {
        Debug.Log("Gamepalay: ShowNextPage: begin");

        if (textAnimataionIsRunning)
        {
            textForDialogue.text = chapter.pages[currentPageId].mainText; // моментальное отображение текста
            if (coroutineForTextAnimation is not null)
                StopCoroutine(coroutineForTextAnimation);
            textAnimataionIsRunning = false;
            return;
        }

        currentPageId++;

        // Это последняя страница (фраза)?
        if (currentPageId < chapter.pages.Count)
        {
            // Если нет, то показываем страницу
            if (coroutineForTextAnimation is not null)
                StopCoroutine(coroutineForTextAnimation);
            textForName.text = chapter.pages[currentPageId].upperText;
            coroutineForTextAnimation = AnimationForText(textForDialogue, chapter.pages[currentPageId].mainText); //
            StartCoroutine(coroutineForTextAnimation);
            //textForDialogue.text = chapter.pages[currentPageId].text; // моментальное отображение текста

            // меняем время
            currentTimeInHours += chapter.pages[currentPageId].spentTimeInHours;
            clock.SetNewTime(currentTimeInHours);

            // показываем персонажей
            int ind = 0;
            foreach (CharacterName charname in chapter.pages[currentPageId].charactersToShow)
            {
                imagesForChars[ind].gameObject.SetActive(true);

                RectTransform rectTransform = imagesForChars[ind].gameObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = dictForCharacters[charname].sprite.textureRect.size;
                imagesForChars[ind].sprite = dictForCharacters[charname].sprite;

                ind++;
            }

            // Отключаем лишние изображения
            while (ind < imagesForChars.Length - 1)
            {
                imagesForChars[ind].gameObject.SetActive(false);
                ind++;
            }
        }
        else
        {
            // Если да, то показываем варианты действий
            ShowOptionsForAction();
        }
    }

    public void ShowOptionsForAction()
    {
        if (chapter.textForActions.Length != chapter.nextChapters.Length)
        {
            SelectNextChapter(0);
            return;
        }

        if (isUserSelectsAction) {
            return;
        }

        isUserSelectsAction = true;

        placeForActions.SetActive(true);
        for (int i = 0; i < chapter.textForActions.Length; i++)
        {
            // Создаем кнопку
            GameObject action = Instantiate(prefabButtonForAction, placeForActions.transform);
            containerForButtonActions.Add(action);

            // Устанавливаем для нее текст
            TextMeshProUGUI textmesh = action.GetComponentInChildren<TextMeshProUGUI>();
            textmesh.text = chapter.textForActions[i];

            // Для клика по кнопке задаем SelectNextChapter с соответсвующим индексом        
            int chapterId = i;
            Button button = action.GetComponent<Button>();
            button.onClick.AddListener(() => {
                isUserSelectsAction = false;
                SelectNextChapter(chapterId);
            });
        }
    }

    private void SelectNextChapter(int chapterId)
    {
        Debug.Log($"Click action with id={chapterId}");
        currentPageId = -1;
        chapter = chapter.nextChapters[chapterId];
        ShowNextPage();

        // Удаляем кнопки для действий
        foreach (GameObject go in containerForButtonActions)
        {
            Destroy(go);
        }
        containerForButtonActions.Clear();
    }
}
