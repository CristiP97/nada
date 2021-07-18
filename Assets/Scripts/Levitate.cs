using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    public float startSpeed = 30f;
    private float speed;
    public float distance = 1f;
    private Vector3 lowDestination;
    private Vector3 highDestination;
    private Vector3 originalPosition;
    private Vector3 currentDestination;
    private float percent;
    private bool up = false;
    private float error = 0.15f;

    private void Start()
    {
        lowDestination = transform.position - new Vector3(0, distance / 2, 0);
        highDestination = transform.position + new Vector3(0, distance / 2, 0);
        originalPosition = transform.position;
        speed = startSpeed;
        StartCoroutine(Levitation());
    }

    IEnumerator Levitation()
    {
        while (true)
        {
            if (up)
            {
                currentDestination = highDestination;
            }
            else
            {
                currentDestination = lowDestination;
            }

            while (transform.position != currentDestination)
            {
                float moveAmount;
                if (Mathf.Abs(transform.position.y - currentDestination.y) <= error)
                {
                    moveAmount = Mathf.Sin(speed * Mathf.PI / 180) * Time.deltaTime / 2;
                }
                else
                {
                    moveAmount = Mathf.Sin(speed * Mathf.PI / 180) * Time.deltaTime;
                }

                //if ((currentDestination == highDestination && transform.position.y < originalPosition.y) || (currentDestination == lowDestination && transform.position.y < originalPosition.y))
                //{
                //    percent = Mathf.Max(0.2f, transform.position.y / originalPosition.y * 1.0f);
                //} else if ((currentDestination == lowDestination && transform.position.y >= originalPosition.y) || (currentDestination == highDestination && transform.position.y >= originalPosition.y))
                //{
                //    percent = Mathf.Max(0.2f, originalPosition.y / transform.position.y * 1.0f);
                //}

                //print(percent);

                //moveAmount = Mathf.Cos(speed * percent) * Time.deltaTime * 10;

                transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveAmount);
                yield return null;
            }

            yield return null;

            up = !up;
        }
    }
}
