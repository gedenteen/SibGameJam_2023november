using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class CommonThings
{
    public static float delayForPrintingText = 0.03f;
    public static bool textAnimataionIsRunning = false;

    public static IEnumerator AnimationForText(TextMeshProUGUI textmesh, string text)
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

}
