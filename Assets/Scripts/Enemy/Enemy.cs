using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //TODO: maybe a gameobject to keep the mesh that i want to instantiate

    public enum Auras {SPEED, ARMOR};

    [Header("General enemy stats")]
    public float health;
    public int goldYielded;
    public float attackDamage;
    public float physicalArmor;
    public float magicalArmor;

    [Header("Movement stats")]
    public float speed;
    public float maxSpeed;
    public float turnSpeed;

    [Header("Aura stats")]
    public bool hasAura;
    public Auras auraType;
    public float auraRange;
    [Range(0,1)]
    public float auraPercentage;

    private Animator animator;
    private bool death;


    Player playerScript;

    private float currentHealth;
    private Dictionary<string, float[]> dots;
    private ArrayList dotDuration;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<Player>();
        animator = gameObject.GetComponent<Animator>();
        dots = new Dictionary<string, float[]>();
        currentHealth = health;
    }

    private void Update()
    {
        // TODO: the way that the dots are managed seems awkard since i got to declare a new dictionary
        // at each frame; maybe modify and do it in a different way
        if (dots.Count != 0)
        {
            Dictionary<string, float[]> dotsAux = new Dictionary<string, float[]>();
            foreach(KeyValuePair<string, float[]> entry in dots)
            {
                TakeDamage(entry.Value[0] * Time.deltaTime, Tower.AttackType.NONE);
                if (entry.Value[1] - Time.deltaTime > 0)
                {
                    float[] dotInfo = new float[2];
                    dotInfo[0] = entry.Value[0];
                    dotInfo[1] = entry.Value[1] - Time.deltaTime;
                    dotsAux[entry.Key] = dotInfo;
                }
            }

            dots = dotsAux;
        }

    }

    public void TakeDamage(float amount, Tower.AttackType attackType)
    {
        // Reduce the damage received based on what type of attack it is
        // And how much resistance does the unit have
        if (attackType == Tower.AttackType.PHYSICAL)
        {
            amount = amount - physicalArmor; //TODO: Maybe change this to be a percentage?
            amount = Mathf.Max(0, amount);
        } else if (attackType == Tower.AttackType.MAGICAL)
        {
            amount = amount - magicalArmor; //TODO: Maybe change this to be a percentage?
            amount = Mathf.Max(0, amount);
        }

        currentHealth -= amount;
        if (IsDead() && !death)
        {
            death = true;
            EnemyDeath();
        }
    }
    
    private void EnemyDeath()
    {
        // Add the amount of resources yielded
        playerScript.GainMoney(goldYielded);

        // Set the camera back if the enemy was clicked
        EnemyCamera enemyCameraScript = gameObject.GetComponent<EnemyCamera>();
        if (enemyCameraScript != null) 
        { 
            if (enemyCameraScript.IsFollowing())
            {
                enemyCameraScript.SetCameraBack();
            }
        }

        // Remove the tag so it does not get targeted again
        gameObject.tag = "Untagged";

        // Play the death animation
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Destroy the enemy with a delay so we can play the death animation
        Destroy(gameObject, 2.0f);

       

        // Make sure to substract from the number of enemies alive
        WaveSpawner.enemiesAlive--;
        return;
    }

    public void ReachedGoal()
    {
        WaveSpawner.enemiesAlive--; //replace this so we don't acces directly
        playerScript.LoseLife();
        Destroy(gameObject);
        return;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (hasAura)
        {
            Gizmos.DrawWireSphere(transform.position, auraRange);
        }
    }

    public void UpdateDotList(string dotName, float[] dotInfo)
    {
        dots[dotName] = dotInfo;
    }

}
