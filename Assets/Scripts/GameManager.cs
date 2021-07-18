using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded;
    public static GameObject selectedEnemy;
    public GameObject gameOverUI;
    public GameObject gameWinUI;

    private EnemyStatsUI enemyStatsUIScript;

    private void Start()
    {
        gameEnded = false;
        selectedEnemy = null;
        enemyStatsUIScript = gameObject.GetComponent<EnemyStatsUI>();
        enemyStatsUIScript.enabled = false;


        FindObjectOfType<Player>().OnPlayerDeath += LoseGame;
        FindObjectOfType<WaveSpawner>().OnWinLevel += WinGame;
    }

    private void Update()
    {
        //Debug.Log("FPS: " + 1 / Time.deltaTime);

        if (selectedEnemy)
        {
            if(enemyStatsUIScript.enabled == false)
            {
                enemyStatsUIScript.SetEnemy(selectedEnemy);
                enemyStatsUIScript.enabled = true;
            }
        }
    }

    private void LoseGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            gameOverUI.SetActive(true);
            Debug.Log("Level lost. Please try again");
        }
    }

    private void WinGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            gameWinUI.SetActive(true);
            Debug.Log("Level won! This is a message from the game master!");
        }
    }
}
