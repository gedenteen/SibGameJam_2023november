using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadingFromCsv : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private Chapter chapterToFill;

    private void Start()
    {
        // Read();
    }

    public void Read()
    {
        StreamReader strReader = new StreamReader("Assets\\_Develop\\Other\\text.csv");
        bool endOfFile = false;
        int stringIndex = 0;

        while (!endOfFile)
        {
            string stringFromFile = strReader.ReadLine();
            if (stringFromFile == null)
            {
                endOfFile = true;
                break;
            }

            Debug.Log($"ReadingFromCsv: Read: stringFromFile={stringFromFile}");
            var dataValues = stringFromFile.Split(';');
            if (dataValues.Length >= 2)
            {
                Debug.Log($"ReadingFromCsv: Read: dataValues[0]={dataValues[0]} dataValues[1]={dataValues[1]}");

                if (stringIndex >= chapterToFill.pages.Count)
                {
                    chapterToFill.pages.Add(new Page(dataValues[0], dataValues[1]));
                }
                else
                {
                    chapterToFill.pages[stringIndex].upperText = dataValues[0];
                    chapterToFill.pages[stringIndex].upperText = dataValues[1];
                }
                stringIndex++;
            }
        }

    }
}
