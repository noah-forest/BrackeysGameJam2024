using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    Path path;
    Vector3 lastPoint;
    private int selectedWaypointIndex = -1;

    void OnSceneGUI()
    {
        path = target as Path;
        lastPoint = path.waypoints.Count > 0
            ? path.waypoints[path.waypoints.Count - 1].position
            : path.transform.position;
        Event guiEvent = Event.current;
        for (int i = 0; i < path.waypoints.Count; i++)
        {
            Handles.color = Color.blue;
            if (Handles.Button(path.waypoints[i].position, Quaternion.identity, 0.1f, 0.1f, Handles.DotHandleCap))
            {
                selectedWaypointIndex = i;
                Repaint();
                guiEvent.Use();
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control &&
                selectedWaypointIndex != -1)
            {
                Undo.RecordObject(path, "Delete Waypoint");
                path.waypoints.RemoveAt(selectedWaypointIndex);
                EditorUtility.SetDirty(path);
                selectedWaypointIndex = -1;
                guiEvent.Use();
            }

            if (i == selectedWaypointIndex)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(path.waypoints[i].position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path, "Move Waypoint");
                    path.waypoints[i].position = newPosition;
                    EditorUtility.SetDirty(path);
                }
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            AddWaypoint(GetMousePositionInWorld(guiEvent.mousePosition));
            guiEvent.Use();
        }
    }

    Vector3 GetMousePositionInWorld(Vector2 mousePosition)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition); // Create a ray from the mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // The ray hit something, and the hit information is stored in 'hit'
            return hit.point; // Return the position where the ray hit the object
        }

// Optionally return a default value if nothing is hit
        return Vector3.zero;
    }

    void AddWaypoint(Vector3 position)
    {
        Undo.RecordObject(path, "Add Waypoint");
        position.z = 0;
        Waypoint newWaypoint = new Waypoint { position = position };
        path.waypoints.Add(newWaypoint);
        int newWaypointIndex = path.waypoints.Count - 1;
        if (selectedWaypointIndex >= 0 && selectedWaypointIndex < path.waypoints.Count - 1)
        {
            newWaypoint.connectedWaypointsIndices.Clear();
            newWaypoint.connectedWaypointsIndices.Add(selectedWaypointIndex);
            path.waypoints[selectedWaypointIndex].connectedWaypointsIndices.Add(newWaypointIndex);
            newWaypoint.position.y = path.waypoints[selectedWaypointIndex].position.y;
        }

        selectedWaypointIndex = newWaypointIndex;
        EditorUtility.SetDirty(path);
    }
}