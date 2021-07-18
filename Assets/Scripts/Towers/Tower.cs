using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tower : MonoBehaviour
{

    public enum AttackType { PHYSICAL, MAGICAL, NONE};
    public enum AttackableEnemyTypes { GROUND, UDNERGROUND, FLYING, NONE}; //TODO: Maybe mmodify this later

    [Header("Tower stats")]
    public float range = 15f;
    public float turnSpeed = 360f;
    public float fireRate = 2;
    public float damage;
    public float projectileSpeed;
    [Range(0, 1)]
    public float sellPercent;
    public AttackType attackType;

    [Header("Radius for AOE damage")]
    public float areaOfDamage;

    [Header("Slow percent")]
    [Range(0,1)]
    public float slowPercent;

    [Header("DOT details")]
    public float dotDamage;
    public float dotDuration;
    
    [Header("Unity components")]
    public GameObject projectile;
    public Transform firePoint;
    public string enemyTag = "Enemy";

    private Transform target;
    private float fireCountdown;
    private float timeBetweenTargetSearches = 0.5f;
    private float updateTargetCooldown;

    private void Start()
    {
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        updateTargetCooldown = timeBetweenTargetSearches;
    }

    private void Update()
    {
        if (target == null)
        {
            // Search for a target ONLY if i don't have one
            if (updateTargetCooldown <= 0)
            {
                print("Searching for new target...");
                UpdateTarget();
                updateTargetCooldown = timeBetweenTargetSearches;
            }
            else
            {
                updateTargetCooldown -= Time.deltaTime;
            }

            // Keep decreasing the fire cooldown even if we don't have a target
            if (fireCountdown > 0)
            {
                fireCountdown -= Time.deltaTime;
            }
            return;
        }
            
        // If we have a target rotate to it and shoot when the fireCountdown is ready
        //RotateToTarget();

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = fireRate;
        } else
        {
            fireCountdown -= Time.deltaTime;
        }

        if (target.tag != enemyTag)
        {
            target = null;
            return;
        }

        // If the current enemy goes outside the range of the tower it means we don't have a target anymore
        if (Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject currentProjectile = (GameObject)Instantiate(projectile, firePoint.position, firePoint.rotation);
        Projectile projectileComponent = currentProjectile.GetComponent<Projectile>();

        if (projectileComponent != null)
         projectileComponent.SetProjectileTarget(target, damage, areaOfDamage, projectileSpeed, dotDamage , dotDuration, attackType);
    }

    void RotateToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float amount = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
        transform.eulerAngles = Vector3.up * amount;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        } else
        {
            target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    

}
