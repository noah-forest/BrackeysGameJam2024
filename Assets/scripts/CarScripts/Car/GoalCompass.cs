using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCompass : MonoBehaviour
{
    public Transform currentGoal;
    public Path mapPath;
    public Transform arrow;
    [SerializeField] private float intermediatePointDistance = 2f; // Distance of intermediate point from current position

    private Pathfinding pathfinding;
    private List<int> path = new List<int>();
    private List<Vector3> waypoints = new List<Vector3>();
    public LineRenderer lineRenderer;

    private void Start()
    {
        pathfinding = new Pathfinding(mapPath.waypoints);
    }

    void UpdateWaypoints()
    {
        waypoints.Clear();
        for (int i = 0; i < path.Count; i++)
        {
            var currentPos = pathfinding.waypoints[path[i]].position;
            if (pathfinding.waypoints.Count == 1)
            {
                waypoints.Add(currentGoal.position);
            }
            else
            {
                waypoints.Add(currentPos);
            }
        }
    }
    
    void UpdateLinerRenderer()
    {
        lineRenderer.positionCount = waypoints.Count;
        lineRenderer.SetPositions(waypoints.ToArray());
    }

    private void Update()
    {
        path = pathfinding.FindPath(transform.position, currentGoal.position);
        if (path != null)
        {
            UpdateWaypoints();
            RemovePassedWaypoints();
            // UpdateLinerRenderer();
            // AddIntermediatePoint();
            RotateArrow();
        }
    }

    private void RemovePassedWaypoints()
    {
        while (waypoints.Count > 1)
        {
            Vector3 playerToWaypoint = waypoints[0] - transform.position;
            Vector3 waypointToNextWaypoint = waypoints[1] - waypoints[0];

            // Check if the player has passed the waypoint
            if (Vector3.Dot(playerToWaypoint, waypointToNextWaypoint) < 0)
            {
                waypoints.RemoveAt(0);
                path.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
    }

    private void AddIntermediatePoint()
    {
        if (waypoints.Count > 0)
        {
            Vector3 directionToNextWaypoint = (waypoints[0] - transform.position).normalized;
            Vector3 intermediatePoint = transform.position + directionToNextWaypoint * intermediatePointDistance;
            waypoints.Insert(0, intermediatePoint);
        }
    }

    private void RotateArrow()
    {
        if (waypoints.Count > 0)
        {
            Vector3 nextWaypoint = waypoints[0];
            
            if (Vector3.Distance(transform.position, currentGoal.position) < 15)
            {
                arrow.gameObject.SetActive(false);
            }
            else
            {
                arrow.gameObject.SetActive(true);
            }

            if (Vector3.Distance(transform.position, currentGoal.position) < 50)
            {
                nextWaypoint = currentGoal.position;
            } else if (Vector3.Distance(transform.position, nextWaypoint) < 50)
            {
                if (waypoints.Count == 1)
                {
                    nextWaypoint = currentGoal.position;
                }
                else
                {
                    nextWaypoint = waypoints[1];
                }
            }
            
            Vector3 directionToNextPoint = nextWaypoint - transform.position;
            directionToNextPoint.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToNextPoint);
            Quaternion smoothRotation = Quaternion.Slerp(arrow.rotation, targetRotation, 1 * Time.deltaTime);
            arrow.transform.position = transform.position;
            arrow.rotation = smoothRotation;
        }
    }
}