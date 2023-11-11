using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    [Header("Links on scene")]
    [SerializeField] TextMeshProUGUI textForName;
    [SerializeField] TextMeshProUGUI textForDialogue;
    [SerializeField] GameObject backgroundImage;
    [SerializeField] GameObject placeForActions;
    [SerializeField] Clock clock;
    [SerializeField] Image[] imagesForChars;
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] CutsceneHandler cutsceneHandler;

    [Header("Links to assets")]
    [SerializeField] Chapter chapter;
    [SerializeField] GameObject prefabButtonForAction;
    [SerializeField] CharactersData charactersData;

    [Header("Changable values")]
    [SerializeField] float currentTimeInHours = 0f;

    public bool gameWasStarted = false;

    private int currentPageId = -1;
    private List<GameObject> containerForButtonActions = new List<GameObject>();
    private Dictionary<CharacterName, Character> dictForCharacters = new Dictionary<CharacterName, Character>();
    private IEnumerator coroutineForTextAnimation = null;
    private bool isUserSelectsAction = false;

    private void Awake()
    {
        foreach (Character character in charactersData.Array)
        {
            dictForCharacters[character.name] = character;
            Debug.Log($"Gameplay: Awake: add {character.name} to dictForCharacters");
        }
    }

    public void StartGame()
    {
        if (!gameWasStarted)
        {
            gameWasStarted = true;
            ShowNextPage();
        }
    }

    public void ShowNextPage()
    {
        Debug.Log("Gamepalay: ShowNextPage: begin");

        if (CommonThings.textAnimataionIsRunning)
        {
            textForDialogue.text = chapter.pages[currentPageId].mainText; // моментальное отображение текста
            if (coroutineForTextAnimation is not null)
                StopCoroutine(coroutineForTextAnimation);
            CommonThings.textAnimataionIsRunning = false;
            return;
        }

        currentPageId++;

        // Это последняя страница (фраза)?
        if (currentPageId < chapter.pages.Count)
        {
            // Смена музыки?
            if (chapter.pages[currentPageId].musicToPlay != null)
            {
                audioSourceMusic.clip = chapter.pages[currentPageId].musicToPlay;
                audioSourceMusic.Play();
            }

            // Катсцена?
            if (chapter.pages[currentPageId].itIsCutscene)
            {
                cutsceneHandler.Handle(chapter.pages[currentPageId].cutscene, chapter.pages[currentPageId].mainText);
                return;
            }
            else
            {
                cutsceneHandler.Hide();
            }

            var chapterBackground = chapter.pages[currentPageId].backgroundImage;

            // Менять бакгроунд, если задан в тексте
            if (chapterBackground)
                backgroundImage.GetComponent<Image>().sprite = chapterBackground;

            // Если нет, то показываем страницу
            if (coroutineForTextAnimation is not null)
                StopCoroutine(coroutineForTextAnimation);

            textForName.text = chapter.pages[currentPageId].upperText;
            coroutineForTextAnimation = CommonThings.AnimationForText(textForDialogue, chapter.pages[currentPageId].mainText); //
            StartCoroutine(coroutineForTextAnimation);
            //textForDialogue.text = chapter.pages[currentPageId].text; // моментальное отображение текста

            // меняем время
            currentTimeInHours += chapter.pages[currentPageId].spentTimeInHours;
            clock.SetNewTime(currentTimeInHours);

            // показываем персонажей
            if (chapter.pages[currentPageId].changeCharsToShow)
            {
                int ind = 0;

                if (chapter.pages[currentPageId].charactersToShow.Count == 0)
                {
                    Debug.LogError($"Gameplay: page have no characters to show");

                    // Это вот старая версия показа персонажей:
                    if (chapter.pages[currentPageId].spritesOfCharsToShow.Count > 0)
                    {
                        foreach (Sprite charSprite in chapter.pages[currentPageId].spritesOfCharsToShow)
                        {
                            imagesForChars[ind].gameObject.SetActive(true);

                            RectTransform rectTransform = imagesForChars[ind].gameObject.GetComponent<RectTransform>();
                            rectTransform.sizeDelta = charSprite.textureRect.size;
                            imagesForChars[ind].sprite = charSprite;

                            ind++;
                        }
                    }
                }
                else
                {
                    foreach (CharacterName charName in chapter.pages[currentPageId].charactersToShow)
                    {
                        imagesForChars[ind].gameObject.SetActive(true); //активируем Image, который на сцене
                        imagesForChars[ind].sprite = dictForCharacters[charName].sprite; //устанавливаем в Image спрайт персонажа

                        RectTransform rectTransform = imagesForChars[ind].gameObject.GetComponent<RectTransform>(); //берем RectTransform у Image
                        rectTransform.anchoredPosition = dictForCharacters[charName].position; //меняем его позицию
                        rectTransform.sizeDelta = dictForCharacters[charName].size; //и размер
                        Vector3 rotation = rectTransform.rotation.eulerAngles; // и поворот
                        rotation.y = dictForCharacters[charName].yRotation;
                        rectTransform.transform.eulerAngles = rotation;

                        ind++; //с этим Image закончили, идем дальше
                    }
                }

                // Отключаем лишние изображения
                while (ind < imagesForChars.Length - 1)
                {
                    imagesForChars[ind].gameObject.SetActive(false);
                    ind++;
                }
            }
            // показываем персонажей - КОНЕЦ
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

        if (isUserSelectsAction)
        {
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
            button.onClick.AddListener(() =>
            {
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

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
}
