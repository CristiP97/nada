using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Sort of a singleton
    public static BuildManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in the scene!");
            return;
        }
        instance = this;
    }

    
    private Player playerScript;

    private TowerBlueprint towerToBuild;
    private Platform platformToBuildOn;
    private PlatformUpgradeUI platformUIScript;

    public GameObject towerShop;
    public GameObject upgradeSellMenu;


    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
        platformUIScript = upgradeSellMenu.GetComponent<PlatformUpgradeUI>();
    }

    //It's property? Got only a setter; TODO: expand it later
    public bool CanBuild { get { return towerToBuild != null; } }

    public void SelectTowerToBuild(TowerBlueprint tower)
    {
        towerToBuild = tower;
    }

    public void SelectPlatformToBuild(Platform platform)
    {
        // If we are clicking on the same platform deactivate the shop UI and remove the reference to the building platform
        if (platform.activeTower == null)
        {
            // If we were thinking of upgrading a tower before, hide the UI for that
            if (platformUIScript.IsUIEnabled())
            {
                platformUIScript.CancelSelection();
            }

            if (platformToBuildOn == platform)
            {
                platformToBuildOn = null;
                MakeShopInvisible();
            }
            else
            {
                platformToBuildOn = platform;
                MakeShopVisible();
            }
        } else
        {
            platformToBuildOn = platform;
        }
       
    }

    public void MakeShopVisible()
    {
        towerShop.SetActive(true);
    }

    public void MakeShopInvisible()
    {
        towerShop.SetActive(false);
    }

    public Platform GetPlatformToBuildOn()
    {
        return platformToBuildOn;
    }

    public void ToggleUpgradeSellMenu()
    {
        platformUIScript.SetPlatform(platformToBuildOn);
    }

    public void TellPlatformToBuildTower()
    {

        if (Player.money < towerToBuild.buildCost)
        {
            Debug.Log("Not enough resources to build tower!");
            return;
        }

        //Substract the money of the tower
        playerScript.SpendMoney(towerToBuild.buildCost);

        platformToBuildOn.BuildTower(towerToBuild);

        Debug.Log("Tower built!");
        Debug.Log("Money left: " + Player.money);

        // Reset the tower to build and the platform
        towerToBuild = null;
        platformToBuildOn = null;

        MakeShopInvisible();
    }

    public void SubstractUpgradeMoney(int amount)
    {
        playerScript.SpendMoney(amount);
    }

    public void AddSellMoney(int amount)
    {
        playerScript.GainMoney(amount);
    }
}
