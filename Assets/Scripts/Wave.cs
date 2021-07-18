using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public MiniWave[] miniWave;

    [System.Serializable]
    public class MiniWave
    {
        [Header("Wave properties")]
        public int numberOfEnemies;
        public float spawnRate;
        public float waitTimeBeforeNextMiniWave;

        [Header("References")]
        public GameObject enemyType;
        public Transform spawnPoint;
        public Transform wayPoints;

    }
    
}
