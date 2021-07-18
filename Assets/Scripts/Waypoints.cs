using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class used to store the waypoints
//All the enemies take the path from here
public class Waypoints : MonoBehaviour
{
    public static Transform[] waypoints;

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            waypoints[i] = transform.GetChild(i);
        }

    }
}
