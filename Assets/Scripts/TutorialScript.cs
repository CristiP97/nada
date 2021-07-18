using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public GameObject mainPanel;
    public Text tutorialText;
    public float waitTimeBetweenLetters = 0.05f;

    // Need a reference to see if the player clicked on the platform
    public GameObject towerShop;

    private string[] tutorialTexts;
    private int tutorialIndex;

    private string skipTutorialText = "\tWelcome to the first level of the game. The next section is the tutorial. " +
                                    "If you have already done it, press X at any point to skip it. Press enter to continue...";
    private string welcomeGeneral = "\tGeneral, we have arrived at the outskirts of our kingdom. " +
                                    "We are currently located near the village Greutar. Let me show you " +
                                    "a report of the situation. Press enter to continue...";
    private string resources = "\tAs you can see in the top left corner those are the resources that our army has. " +
                               "We can use those to build our defenses when the enemies attack. Press enter to continue...";
    private string countdown = "\tNext to the right it's the estimated time until the enemy arrives. Press enter to continue...";
    private string lives = "\tNext to the right is the damage that we can sustain. If too many enemies pass and it reaches 0 " +
                            "we have lost the battle. Press enter to continue...";
    private string buildingPlatforms = "\tGeneral, do you see the those platforms scattered across the land? I think " +
                                        "we can use them to build our defenses. Hover over one of them and click it!";
    private string shop = "\tGreat! Now you can choose the structure that you want to build. At the moment we only " +
                            "have available our archer tower blueprints. Let's build one!";
    private string archerTowers = "\tArcher towers have low damage, but they compensate with their high fire rate! Press enter to continue...";
    private string upgrade = "\tYou can click again on the platform that you have your tower built on. We can now either " +
                            "upgrade it or sell it to build another tower. Keep that in mind in for the future. Press enter to continue...";
    private string scoutTerrain = "\tYou can scout the terrain by pressing W,A,S,D. By Pressing F you can go into free mode " +
                                    "and explore the land as you wish. Rotate your camera while pressing your right mouse button and W,A,S,D,Q,E to move. " +
                                    "Press F again to return to the normal view. Press enter to continue...";
    private string enemyinfo = "\tWhile you are in free mode with the camera, you can press on an enemy to see aditional info about it! " +
                                "Press SPACE to stop following the enemy. Press enter to continue...";
    private string reportEnding = "\tThat's the whole report general! The villagers are approaching. Let's get ready to stop this riot. " +
                                    "Press the button in the lower left corner after finishing reading the report to start the level. Press enter to continue...";

    private string currentlyDisplayedText;
    private string currentTextToDisplay;
    private bool onGoingExplanation = false;
    private bool skipTutorial = false;
    private BuildManager buildManager;
    private Platform tutorialPlatform;

    private void Start()
    {
        tutorialTexts = new string[12];

        tutorialTexts[0] = skipTutorialText;
        tutorialTexts[1] = welcomeGeneral;
        tutorialTexts[2] = resources;
        tutorialTexts[3] = countdown;
        tutorialTexts[4] = lives;
        tutorialTexts[5] = buildingPlatforms;
        tutorialTexts[6] = shop;
        tutorialTexts[7] = archerTowers;
        tutorialTexts[8] = upgrade;
        tutorialTexts[9] = scoutTerrain;
        tutorialTexts[10] = enemyinfo;
        tutorialTexts[11] = reportEnding;

        tutorialText.text = currentlyDisplayedText;
        buildManager = BuildManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            skipTutorial = true;
        }

        if (skipTutorial || tutorialTexts.Length == tutorialIndex)
        {
            mainPanel.SetActive(false);
            this.enabled = false;
            return;
        }

        if (onGoingExplanation)
        {
            return;
        }

        for (int i = 0; i < tutorialTexts.Length; ++i)
        {
            if (i == tutorialIndex)
            {
                currentTextToDisplay = tutorialTexts[i];
            }
        }

        StartCoroutine(ShowTextWithDelay(currentTextToDisplay));
    }

    IEnumerator ShowTextWithDelay(string targetText)
    {
        onGoingExplanation = true;
        for (int i = 0; i < targetText.Length; ++i)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                skipTutorial = true;
                yield break;
            }

            currentlyDisplayedText += targetText[i];
            tutorialText.text = currentlyDisplayedText;
            yield return new WaitForSeconds(waitTimeBetweenLetters);
        }

        if (skipTutorial)
            yield break;

        if (tutorialIndex != 5 && tutorialIndex != 6)
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        } else if (tutorialIndex == 5)
        {
            while (towerShop.activeSelf != true)
            {
                yield return null;
            } 
        } else if (tutorialIndex == 6)
        {
            tutorialPlatform = buildManager.GetPlatformToBuildOn();
         
            // If the player builds the tower before finishing the text just skip ahead
            if (tutorialPlatform != null)
            {
                while (tutorialPlatform.activeTower == null)
                {
                    // In case the player clicks another platform get a reference to that oane instead
                    if (buildManager.GetPlatformToBuildOn() && buildManager.GetPlatformToBuildOn() != tutorialPlatform)
                    {
                        tutorialPlatform = buildManager.GetPlatformToBuildOn();
                    }

                    yield return null;
                }
            }
        }
        
        currentlyDisplayedText = "";
        tutorialIndex++;
        onGoingExplanation = false;

    }




}
