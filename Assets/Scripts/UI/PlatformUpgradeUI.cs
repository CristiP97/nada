using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformUpgradeUI : MonoBehaviour
{
    private Platform platform;
    public Button upgradeButton;
    public Text upgradeText;

    public void Start()
    {
        upgradeText = upgradeButton.GetComponentInChildren<Text>();
    }

    public void SetPlatform(Platform platform)
    {
        if (this.platform == platform)
        {
            this.platform = null;
            gameObject.SetActive(false);
        }
        else
        {
            this.platform = platform;
            gameObject.SetActive(true);
            transform.position = platform.transform.position + Vector3.up * platform.towerBlueprint.offset;

        }
    }

    private void Update()
    {
        // If we have a platform selected for upgrade, visually signal if the player has enough money for the upgrade or not
        if (platform != null)
        {
            // Check if the tower is already at max level
            if(platform.IsActiveTowerMaxLevel())
            {
                upgradeText.text = "MAX";
                upgradeButton.interactable = false;

            } else
            {
                upgradeText.text = "Upgrade";
                if (platform.CurrentUpgradeCost() > Player.money)
                {
                    upgradeButton.interactable = false;
                }
                else
                {
                    upgradeButton.interactable = true;
                }
            }

        }
    }

    public void Upgrade()
    {
        platform.UpgradeTower();
        platform = null;
        gameObject.SetActive(false);
    }

    public void Sell()
    {
        platform.SellTower();
        platform = null;
        gameObject.SetActive(false);
    }

    public void CancelSelection()
    {
        platform = null;
        gameObject.SetActive(false);
    }

    public Platform GetPlatformSelected()
    {
        return platform;
    }

    public bool IsUIEnabled()
    {
        return gameObject.activeSelf;
    }
}
