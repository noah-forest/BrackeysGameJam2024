using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Waypoint> waypoints;
    
    public Pathfinding(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
    }

    // Heuristic: Straight-line distance between two points (Euclidean distance)
    private float Heuristic(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    // A* algorithm to find the shortest path between start and goal
    public List<int> FindPath(Vector3 startPosition, Vector3 goalPosition)
    {
        int startIdx = GetClosestWaypointIndex(startPosition);
        int goalIdx = GetClosestWaypointIndex(goalPosition);

        if (startIdx == -1 || goalIdx == -1) return null;

        // Open set of nodes to explore
        HashSet<int> openSet = new HashSet<int> { startIdx };
        // Came from map to reconstruct path
        Dictionary<int, int> cameFrom = new Dictionary<int, int>();
        // Cost from start to a given node
        Dictionary<int, float> gScore = new Dictionary<int, float> { [startIdx] = 0 };
        // Estimated total cost (g + heuristic)
        Dictionary<int, float> fScore = new Dictionary<int, float> { [startIdx] = Heuristic(waypoints[startIdx].position, goalPosition) };

        while (openSet.Count > 0)
        {
            // Get node in openSet with the lowest fScore
            int current = GetNodeWithLowestFScore(openSet, fScore);
            if (current == goalIdx)
                return ReconstructPath(cameFrom, current); // Reached the goal

            openSet.Remove(current);

            foreach (int neighbor in waypoints[current].connectedWaypointsIndices)
            {
                float tentativeGScore = gScore[current] + Vector3.Distance(waypoints[current].position, waypoints[neighbor].position);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    // This path is the best so far
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(waypoints[neighbor].position, goalPosition);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    // Helper: Reconstructs the path from cameFrom map
    private List<int> ReconstructPath(Dictionary<int, int> cameFrom, int current)
    {
        List<int> totalPath = new List<int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    // Helper: Gets the waypoint index closest to a given position
    private int GetClosestWaypointIndex(Vector3 position)
    {
        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // Helper: Gets the node with the lowest fScore in the openSet
    private int GetNodeWithLowestFScore(HashSet<int> openSet, Dictionary<int, float> fScore)
    {
        int bestNode = -1;
        float lowestFScore = Mathf.Infinity;

        foreach (int node in openSet)
        {
            if (fScore.ContainsKey(node) && fScore[node] < lowestFScore)
            {
                lowestFScore = fScore[node];
                bestNode = node;
            }
        }

        return bestNode;
    }
}
