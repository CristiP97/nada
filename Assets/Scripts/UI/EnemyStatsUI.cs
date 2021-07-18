using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsUI : MonoBehaviour
{
    public GameObject enemyStatsUI;
    private Text[] stats;
    private EnemyCamera enemyCameraScript;
    private EnemyMovement enemyMovementScript;
    private EnemyMovementAnimator enemyMovementAnimator;
    private Enemy enemyScript;
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        stats = enemyStatsUI.GetComponentsInChildren<Text>();

        if(stats.Length == 0 )
        {
            Debug.LogError("Couldn't get the text components from the EnemyStatsUI!");
        }
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        enemyCameraScript = enemy.GetComponent<EnemyCamera>();
        enemyMovementScript = enemy.GetComponent<EnemyMovement>();
        enemyMovementAnimator = enemy.GetComponent<EnemyMovementAnimator>();
        enemyScript = enemy.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        // I know that i wanna display exactly 4 stats at the moment
        if (enemyCameraScript != null && enemyCameraScript.IsFollowing())
        {
            if (!enemyStatsUI.activeSelf)
                enemyStatsUI.SetActive(true);
            stats[0].text = "Health: " + Mathf.RoundToInt(enemyScript.GetCurrentHealth()).ToString();

            try
            {
                stats[1].text = "Speed: " + enemyMovementScript.GetCurrentSpeed().ToString();

            }
            catch
            {
                stats[1].text = "Speed: " + enemyMovementAnimator.GetCurrentSpeed().ToString();

            }
            stats[2].text = "Physical armor: " + enemyScript.physicalArmor.ToString();
            stats[3].text = "Magical armor: " + enemyScript.magicalArmor.ToString();
        } else
        {
            enemyStatsUI.SetActive(false);
            this.enabled = false;
        }

    }
}
