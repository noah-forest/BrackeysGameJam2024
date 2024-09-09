using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Modifed version of sebastion lauges FOW tutorial
/// </summary>

[RequireComponent(typeof(SphereCollider))]
public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obsticalMask;
    public float meshResolution;
    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector] public bool searchingForTargets = false;
    public float searchInterval = 0.2f;
    Coroutine searchCoroutine;

    public MeshFilter viewVisualFilter;
    Mesh viewVisualMesh;
    public MeshRenderer meshRenderer;
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;
        public ViewCastInfo( bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _dist;
            angle = _angle;
        }

    }

    private void Start()
    {
        viewVisualMesh = new Mesh();
        viewVisualMesh.name = "View Mesh";
        viewVisualFilter.mesh = viewVisualMesh;
        StopTargetSearch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent?.gameObject && LayerMask.GetMask(LayerMask.LayerToName(other.transform.parent.gameObject.layer)) == targetMask) StartTargetSearch();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent?.gameObject && LayerMask.GetMask(LayerMask.LayerToName(other.transform.parent.gameObject.layer)) == targetMask) StopTargetSearch();
    }
    private void LateUpdate()
    {
        if (searchingForTargets)
        {
            DrawFoW();
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (searchingForTargets)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    
    void DrawFoW()
    {
        int stepCount = (int) (viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for(int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle * 0.5f + stepAngleSize * i;
            ViewCastInfo newView = ViewCast(angle);
            viewPoints.Add(newView.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] verts = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];
        verts[0] = Vector3.zero;  // verts are parented so they are already in local

        for(int i = 0; i < vertexCount-1; i++)
        {
            verts[i+1] = transform.InverseTransformPoint( viewPoints[i]);

            //build triangle verts
            if (i < vertexCount-2)
            {
                triangles[i * 3] = 0; // all trianges touch the origin
                //the two far points of the triangle
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 +2] = i + 2;
            }
        }

        viewVisualMesh.Clear();
        viewVisualMesh.vertices = verts;
        viewVisualMesh.triangles = triangles;
        viewVisualMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, dir, out hit, viewRadius,obsticalMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 distToTarg = target.position - transform.position;
            Vector3 dirToTarg = distToTarg.normalized;
            if(Vector3.Angle(transform.forward,dirToTarg) < viewAngle * 0.5f)
            {
                if (!Physics.Raycast(transform.position, dirToTarg, distToTarg.magnitude, obsticalMask))
                {
                    visibleTargets.Add(target);
                }
            }

        }
        Debug.Log("Searching for target");
    }

    void StartTargetSearch()
    {
        meshRenderer.enabled = true;
        searchingForTargets = true;
        searchCoroutine = StartCoroutine(FindTargetsWithDelay(searchInterval));
    }

    void StopTargetSearch()
    {
        visibleTargets.Clear();
        viewVisualMesh.Clear();
        viewVisualMesh.RecalculateNormals();
        meshRenderer.enabled = false;
        searchingForTargets = false;
        if(searchCoroutine != null) StopCoroutine(searchCoroutine);
    }
}
