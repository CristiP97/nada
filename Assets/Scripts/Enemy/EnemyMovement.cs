using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Error used when aproximating the rotation
    private float delta = 0.05f;

    private Enemy enemy;
    private Transform spawnPoint;
    private Transform[] waypoints;
    private float currentSpeed;
    private float currentTurnSpeed;

    private ArrayList speedBuffs;
    private ArrayList buffPercentage;

    private ArrayList speedDebuffs;
    private ArrayList debuffPercentage;

    // Start is called before the first frame update
    private void Start()
    {
        speedBuffs = new ArrayList();
        speedDebuffs = new ArrayList();
        enemy = GetComponent<Enemy>();

        // Initialize the vector of waypoints and populate it with the positions
        Vector3[] targetPositions = new Vector3[waypoints.Length];

        for (int i = 0; i < waypoints.Length; ++i)
        {
            targetPositions[i] = waypoints[i].position;
        }

        transform.LookAt(targetPositions[0]);
        currentSpeed = enemy.speed;
        currentTurnSpeed = enemy.turnSpeed;

        // Start moving
        StartCoroutine(MoveToTarget(targetPositions));
    }

    private void Update()
    {
        
    }

    public void SetPath(Transform spawnPoint, Transform[] waypoints)
    {
        this.spawnPoint = spawnPoint;
        this.waypoints = waypoints;
    }

    IEnumerator MoveToTarget(Vector3[] targetPositions)
    {
        for(int i = 0; i < targetPositions.Length; ++i)
        {
            yield return StartCoroutine(TurnToTarget(targetPositions[i]));

            while(transform.position != targetPositions[i])
            {
                // If the enemy is dead stop moving
                if (enemy.IsDead())
                {
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPositions[i], currentSpeed * Time.deltaTime);
                yield return null;
                currentSpeed = enemy.speed;
                UpdateCurrentSpeedWithBuffs();
                speedBuffs.Clear();
                speedDebuffs.Clear();
            }

            // Set back to the original speed in case it was modified by external factors
            
        }

        //If it reaches this part, it means that the enemy has reached the final waypoint
        enemy.ReachedGoal();
    }


    IEnumerator TurnToTarget(Vector3 targetPosition)
    {

        // If the enemy is dead stop moving
        if (enemy.IsDead())
        {
            yield break;
        }

        // Get the direction that the enemy should face
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Get the target angle that the enemy should turn in degrees
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float angle = 0f;

        // While the difference between the target angle and the current angle is greater than an error delta, keep turning
        while (true)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > delta)
            {
                angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, enemy.turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
            else
            {
                break;
            }

        }

    }

    private void UpdateCurrentSpeedWithBuffs()
    {
        foreach (float p in speedBuffs)
        {
            currentSpeed += currentSpeed * p;
        }
    }

    public void UpdateSpeedBuffs(float percentage)
    {
        // This should take only different buffs; the exact same buffs should not stack
        if (!speedBuffs.Contains(percentage))
        {
            speedBuffs.Add(percentage);
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
