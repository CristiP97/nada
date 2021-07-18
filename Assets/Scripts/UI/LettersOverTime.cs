using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LettersOverTime : MonoBehaviour
{
    public float displaySpeed = 0.1f;
    private string textToDisplay = "\tThe Kingdom of Nada has formed 200 hundred years ago with it's first king Dufinus Lerdo. " +
                                    "Since the kingdom was formed, it has only known peace and prosperity." +
                                    "\n\tRecently though, under the rule of king Dufinus Parvus IV, riots have broken out for uknown reason. " +
                                    "Now he must discover the root of the problem and resolve it before the kingdom falls into anarchy..." +
                                    "\n\tThe king has tasked you, the general of his army, with the investigation. " +
                                    "Your first mission is to go to search around the farms near the edge of the kingdom and stop the riots that are happening there.";
    public Text displayText;

    private string currentDisplayedText = "";

    private void Start()
    {
        StartCoroutine("ShowText");
    }


    IEnumerator ShowText()
    {
        for (int i = 0; i < textToDisplay.Length; ++i)
        {
            currentDisplayedText += textToDisplay[i];
            displayText.text = currentDisplayedText;
            yield return new WaitForSeconds(displaySpeed);
        }

    }
}
