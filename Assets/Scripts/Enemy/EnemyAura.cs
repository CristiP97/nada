using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAura : MonoBehaviour
{
    private Enemy enemy;
    private EnemyMovementAnimator enemyMovementScript;
    private string enemyTag = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemyMovementScript = gameObject.GetComponent<EnemyMovementAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy doesn't have an aura just disable the script
        if (!enemy.hasAura)
        {
            enabled = false;
            return;
        }

        switch (enemy.auraType)
        {
            case Enemy.Auras.SPEED:
                SpeedBoost();
                break;
        }
    }

    private void SpeedBoost()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= this.enemy.auraRange && enemy != gameObject)
            {
                enemyMovementScript = enemy.GetComponent<EnemyMovementAnimator>();
                enemyMovementScript.UpdateSpeedBuffs(this.enemy.auraPercentage);
            }
        }
    }
}
