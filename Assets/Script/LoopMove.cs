using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMove : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public bool iscanMove = true;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length > 0&&iscanMove)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}
