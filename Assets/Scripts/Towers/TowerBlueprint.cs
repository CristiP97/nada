using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBlueprint
{
    public string name;
    public GameObject prefab;
    public float offset;

    public int buildCost;
    public int maxTowerLevel;

    public int[] upgradeCost;
    public GameObject[] upradedTowerVersions;

}
