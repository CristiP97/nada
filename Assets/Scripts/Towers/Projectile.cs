using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private float travelSpeed = 50f;
    private float error = 0.05f;
    private float damage;
    private float damageRadius;
    private float dotDamage;
    private float dotDuration;
    private Tower.AttackType attackType;

    private Transform target;

    public void SetProjectileTarget(Transform target, float damage, float damageRadius, float travelSpeed, float dotDamage, float dotDuration, Tower.AttackType attackType)
    {
        this.target = target;
        this.damage = damage;
        this.damageRadius = damageRadius;
        this.travelSpeed = travelSpeed;
        this.dotDamage = dotDamage;
        this.dotDuration = dotDuration;
        this.attackType = attackType;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position);
        float amountMovedPerFrame = travelSpeed * Time.deltaTime;

        if (direction.magnitude <= amountMovedPerFrame)
        {
            HitTarget();
        }

        transform.Translate(direction.normalized * amountMovedPerFrame, Space.World);
        transform.LookAt(target);

    }

    public void HitTarget()
    {
        Debug.Log("We hit something!");

        if (damageRadius > 0) {
            AreaDamage();
        } else
        {
            SingleDamage(target);
        }

        
        Destroy(gameObject);
        return;
    }

    void AreaDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                SingleDamage(collider.transform);
            }
        }
    }

    void SingleDamage(Transform target)
    {
        Enemy enemyScript = target.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage, attackType);

            if (dotDuration > 0)
            {
                ApplyDot(enemyScript);
                
            } 
        }

        return;

    }

    void ApplyDot(Enemy enemyScript)
    {
        float[] dotInfo = new float[2];
        dotInfo[0] = dotDamage;
        dotInfo[1] = dotDuration;
        enemyScript.UpdateDotList(gameObject.name, dotInfo);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
