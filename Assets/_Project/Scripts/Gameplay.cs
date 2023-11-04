using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class Gameplay : MonoBehaviour
{
    [Header("Links on scene")]
    [SerializeField] TextMeshProUGUI textForDialogue;
    [SerializeField] GameObject placeForActions;
    [SerializeField] Clock clock;
    [SerializeField] Image[] imagesForChars;

    [Header("Links to assets")]
    [SerializeField] Chapter chapter;
    [SerializeField] GameObject prefabButtonForAction;
    [SerializeField] CharactersData charactersData;

    [SerializeField] float currentTimeInHours = 0f;
    private int currentPageId = -1;
    private List<GameObject> containerForButtonActions = new List<GameObject>();
    private Dictionary<CharacterName, Character> dictForCharacters = new Dictionary<CharacterName, Character>();

    private void Awake()
    {
        foreach (Character character in charactersData.array)
        {
            dictForCharacters[character.name] = character;
            Debug.Log($"Gameplay: Awake: add {character.name} to dictForCharacters");
        }
    }

    private void Start()
    {
        ShowNextPage();
    }

    public void ShowNextPage()
    {
        Debug.Log("Gamepalay: ShowNextPage: begin");
        currentPageId++;

        // Это последняя страница (фраза)?
        if (currentPageId != chapter.pages.Length)
        {
            // Если нет, то показываем страницу
            textForDialogue.text = chapter.pages[currentPageId].text;

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
            Debug.LogError("Gamepalay: ShowOptionsForAction: count of actions != count of chapters");
            return;
        }

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
            button.onClick.AddListener(() => SelectNextChapter(chapterId));
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
