using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    BuildManager buildManager;
    //public TowerBlueprint archerTower;
    //public TowerBlueprint mageTower;

    public TowerBlueprint[] availableTowers;

    private Button[] childrenButtons;

    private void Start()
    {

        if (availableTowers.Length != gameObject.GetComponentsInChildren<Button>().Length)
        {
            Debug.LogError("The number of towers available does not match the number of buttons in the shop!");
        }

        childrenButtons = gameObject.GetComponentsInChildren<Button>();

        buildManager = BuildManager.instance;

        // Set the UI to have the price and name of the tower visible
        SetTowerNameAndPrice();

        // Set the identifying prefab for each button
        SetButtonsPrefabs(); // EXPERIMENTAL - MIGHT DELETE IT LATER

        // Check every half second what towers can the player buy
        InvokeRepeating("CheckIfPurchaseAvailable", 0, 0.5f);

        // This should start as inactive until the player presses on a platform
        gameObject.SetActive(false);
    }

    public void SetTowerNameAndPrice()
    {
        for (int i = 0; i < childrenButtons.Length; ++i)
        {
            // Should be only 2 components: the name of the tower and it's cost
            Text[] textZones = childrenButtons[i].gameObject.GetComponentsInChildren<Text>();
            
            if (textZones.Length != 2)
            {
                Debug.LogError("There is a number of text fields on one of the buttons different from 2!");
            }

            // Set the name and the cost
            textZones[0].text = availableTowers[i].name;
            textZones[1].text = availableTowers[i].buildCost.ToString() + "g";

        }
    }

    //-----------------------------EXPERIMENTAL - MIGHT DELETE IT--------------------------------
    public void SetButtonsPrefabs()
    {
        for (int i = 0; i < childrenButtons.Length; ++i)
        {
            PreviewTower previewTowerScript = childrenButtons[i].GetComponent<PreviewTower>();
            previewTowerScript.previewPrefab = availableTowers[i];

        }
    }
    //-----------------------------EXPERIMENTAL - MIGHT DELETE IT--------------------------------

    public void CheckIfPurchaseAvailable()
    {
        for (int i = 0; i < childrenButtons.Length; ++i)
        {
            if(Player.money < availableTowers[i].buildCost)
            {
                childrenButtons[i].interactable = false;
                //childrenButtons[i].image.color = new Vector4(255, 255, 255, 25);
            } else
            {
                childrenButtons[i].interactable = true;
                //childrenButtons[i].image.color = new Vector4(255, 255, 255, 255);
            }
        }
    }

    public void SelectArcherTower()
    {
        Debug.Log("Archer tower selected!");
        buildManager.SelectTowerToBuild(availableTowers[0]);
        buildManager.TellPlatformToBuildTower();
    }

    public void SelectMageTower()
    {
        Debug.Log("Mage tower selected!");
        buildManager.SelectTowerToBuild(availableTowers[1]);
        buildManager.TellPlatformToBuildTower();
    }

    public void SelectDartTower()
    {
        Debug.Log("Dart tower selected!");
        buildManager.SelectTowerToBuild(availableTowers[2]);
        buildManager.TellPlatformToBuildTower();
    }
}
