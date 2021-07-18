using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour
{
    public Color hoverColor;

    [Header("Optional")]
    public GameObject activeTower;
    [HideInInspector]
    public TowerBlueprint towerBlueprint;

    private Color startColor;
    private Renderer renderer;
    private int currentUpgradeLevel;
    private int value;
    private Material[] platformMaterials;

    BuildManager buildManager;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        platformMaterials = renderer.materials;
        startColor = platformMaterials[2].color;
        buildManager = BuildManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called when presiing the mouse down while hovering over the mesh
    private void OnMouseDown()
    {
        buildManager.SelectPlatformToBuild(this);

        if (activeTower != null)
        {
            buildManager.MakeShopInvisible();
            buildManager.ToggleUpgradeSellMenu();
            return;
        }

        //buildManager.MakeShopVisible();

    }

    public void BuildTower(TowerBlueprint towerBlueprint)
    {

        value += towerBlueprint.buildCost;

        //Instantiate the turret in the scene
        GameObject tower = (GameObject)Instantiate(towerBlueprint.prefab, transform.position + new Vector3(0, towerBlueprint.offset, 0), transform.rotation);

        //Set the active tower on the received platform
        activeTower = tower;
        this.towerBlueprint = towerBlueprint;
    }

    public void UpgradeTower()
    {
        // If we are at maximum level, we can't upgrade anymore
        if (currentUpgradeLevel == towerBlueprint.maxTowerLevel) // TODO: Remove this from here since we are doing the check directly in the upgradeUI
            return;

        // TODO: MOVE THIS LOGIC IN THE PLATFORM UPGRADEUI!
        // If the player doesn't have enough money we can't upgrade
        if (Player.money < towerBlueprint.upgradeCost[currentUpgradeLevel])
        {
            Debug.Log("Don't have enough money to upgrade this tower!");
            return;
        }

        buildManager.SubstractUpgradeMoney(towerBlueprint.upgradeCost[currentUpgradeLevel]);
        value += towerBlueprint.upgradeCost[currentUpgradeLevel];

        // Destroy the currently active tower
        Destroy(activeTower);

        // Instantiate the new one
        GameObject tower = (GameObject)Instantiate(towerBlueprint.upradedTowerVersions[currentUpgradeLevel], transform.position + new Vector3(0, towerBlueprint.offset, 0), transform.rotation);
        activeTower = tower;

        // Increment the upgrade level
        currentUpgradeLevel++;
    }

    public void SellTower()
    {
        buildManager.AddSellMoney(Mathf.RoundToInt(value * activeTower.GetComponent<Tower>().sellPercent));

        Destroy(activeTower);
        activeTower = null;
        currentUpgradeLevel = 0;
        value = 0;

        
    }

    public int ActiveTowerLevel()
    {
        return currentUpgradeLevel;
    }

    public bool IsActiveTowerMaxLevel()
    {
        return currentUpgradeLevel == towerBlueprint.maxTowerLevel;
    }

    public int CurrentUpgradeCost()
    {
        return towerBlueprint.upgradeCost[currentUpgradeLevel];
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        platformMaterials[2].color = hoverColor;
    }

    private void OnMouseExit()
    {
        platformMaterials[2].color = startColor;
    }
}
