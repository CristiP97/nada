using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    // Keeps track of how many enemies are currently alive
    public static int enemiesAlive;

    // Event that signals that the level was won
    public event System.Action OnWinLevel;

    public enum SpawnState {START, SPAWNING, WAITING, COUNTING };

    //Maybe make this an array, to have more customazation
    public float[] timeBetweenWaves;
    public Text countdownText;

    public float countdown; //leave it temporary as public to make sure things aren't broken
    private int waveIndex;
    private bool levelSpawnCompleted;
    private SpawnState state;
    private Player playerScript;

    public GameObject forceNextWaveButton;

    public Wave[] waves;

    // Start is called before the first frame update
    void Start()
    {
        // Check if each wave has an associated countdown except for the final one
        if (timeBetweenWaves.Length != waves.Length - 1)
        {
            Debug.LogError("ERROR! Not every wave has an associated wait time attached!");
        }

        playerScript = FindObjectOfType<Player>();

        countdown = 5f; // TODO: This is a default time attached; later let the player build his defences first before sending waves at him
        state = SpawnState.START;
        countdownText.text = "Press start button to begin";
        forceNextWaveButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "Press when ready to start the level";
    }

    // Update is called once per frame
    void Update()
    {
        // We are waiting for the player to press the start button
        if (state == SpawnState.START)
        {
            return;
        }

        //If we won the level stop(only for debug/testing; delete this otherwise)
        if (levelSpawnCompleted)
            return;

        // Do not start counting down while we are still spawning enemies
        if (state == SpawnState.SPAWNING)
            return;

        // If we reach the last wave and all the enemies are dead
        if (waveIndex == waves.Length)
        {
            if (enemiesAlive == 0)
            {
                if (OnWinLevel != null)
                {
                    OnWinLevel();
                }

                LevelCompleted();
            }
            //TODO: call a function that switches to the next level(scene); or do this in the levelCompleted() function above
            return; //temporary, to not go beyond this and crash since the index will be higher than the no of waves
        }

        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave(waves[waveIndex]));
        } else
        {
            countdown -= Time.deltaTime;
        }

        countdownText.text = "Next wave in " + Mathf.Ceil(countdown).ToString();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        countdown = 0; // Just to make sure it doesn't diplay a negative number
        forceNextWaveButton.SetActive(false);
        state = SpawnState.SPAWNING;
        float waitingTime = 0;
        float maximumWaitTime = Mathf.NegativeInfinity;

        // We are cycling through all the miniwaves
        for (int i = 0; i < wave.miniWave.Length; ++i)
        {
            StartCoroutine(SpawnMiniWave(wave.miniWave[i]));

            // If there is a small wait time between the waves wait and reset the final waiting counter
            if (wave.miniWave[i].waitTimeBeforeNextMiniWave > 0)
            {
                maximumWaitTime = Mathf.NegativeInfinity;
                yield return new WaitForSeconds(wave.miniWave[i].waitTimeBeforeNextMiniWave);
            } else
            {
                waitingTime = wave.miniWave[i].numberOfEnemies * (1.0f / wave.miniWave[i].spawnRate);
                if ( maximumWaitTime < waitingTime)
                {
                    maximumWaitTime = waitingTime;
                }
            }
        }

        // Just to make sure that we don't wait for an infinite amount of time
        if(maximumWaitTime == Mathf.NegativeInfinity)
        {
            maximumWaitTime = 0;
        }

        yield return new WaitForSeconds(maximumWaitTime);

        state = SpawnState.COUNTING;
        
        if (waveIndex < waves.Length - 1)
            countdown = timeBetweenWaves[waveIndex];

        waveIndex++;

        if (waveIndex != waves.Length)
            forceNextWaveButton.SetActive(true);
    }

    IEnumerator SpawnMiniWave(Wave.MiniWave miniWave)
    {
        enemiesAlive += miniWave.numberOfEnemies;

        for ( int i = 0; i < miniWave.numberOfEnemies; ++i)
        {
            GameObject spawnedEnemy = SpawnEnemy(miniWave.enemyType , miniWave.spawnPoint);
            SetEnemyPath(spawnedEnemy, miniWave.spawnPoint, miniWave.wayPoints);
            yield return new WaitForSeconds(1.0f / miniWave.spawnRate);
        }
    }

    void LevelCompleted()
    {
        levelSpawnCompleted = true;
        Debug.Log("Level completed! Congratulations!");
    }

    GameObject SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {
        GameObject spawnedEnemy = (GameObject)Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        return spawnedEnemy;
       
    }

    void SetEnemyPath(GameObject enemy, Transform spawnPoint, Transform wayPoints)
    {
        EnemyMovement enemyMovementScript = enemy.GetComponent<EnemyMovement>();

       

        Transform[] waypointsTransform = new Transform[wayPoints.childCount];

        for (int i = 0; i < wayPoints.childCount; ++i)
        {
            waypointsTransform[i] = wayPoints.GetChild(i);
        }

        //------------------------TO WORK WITH BLENDER ANIMATIONS---------------------------
        if (enemyMovementScript == null)
        {
            EnemyMovementAnimator enemyAnimatorScript = enemy.GetComponent<EnemyMovementAnimator>();
            enemyAnimatorScript.SetPath(spawnPoint, waypointsTransform);
            return;
        }
        //------------------------TO WORK WITH BLENDER ANIMATIONS---------------------------

        enemyMovementScript.SetPath(spawnPoint, waypointsTransform);
        //enemyMovementScript.BeginMovement();
    }

    public void ForceNextWaveEarly()
    {
        // When the button is pressed for the first time just start the game
        if (state == SpawnState.START)
        {
            state = SpawnState.COUNTING;
            forceNextWaveButton.SetActive(false);
            forceNextWaveButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "Force next wave";
            return;
        }

        // Daredevil bonus; currently based purely on time and nothing else; TODO: Change it later
        // Don't award bonus for the first round since later the player will have all the time in the world to set up the defences
        if (waveIndex > 0)
        {
            playerScript.GainMoney(Mathf.RoundToInt(countdown));
        }
        countdown = 0;
    }
}
