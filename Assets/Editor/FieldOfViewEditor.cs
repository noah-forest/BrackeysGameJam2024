using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        SphereCollider collider = target.GameObject().GetComponent<SphereCollider>();
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirectionFromAngle(-fow.viewAngle * 0.5f, false);
        Vector3 viewAngleB = fow.DirectionFromAngle(fow.viewAngle * 0.5f, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        if (collider)
        {
            collider.radius = fow.viewRadius * 0.5f;
            collider.isTrigger = true;
        }

        Handles.color = Color.red;
        foreach(Transform visibleTarg in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarg.position);
        }
    }
}
