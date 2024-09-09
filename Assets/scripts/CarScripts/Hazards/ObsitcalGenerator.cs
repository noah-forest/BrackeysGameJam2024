using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObsitcalGenerator : MonoBehaviour
{

    [SerializeField] protected List<GameObject> possibleObsticales;
    [SerializeField] protected BoxCollider boundingBox;
    [SerializeField] protected float verticalOffset;
    [SerializeField] protected int obsticalesGenerated;

    protected void Spawn()
    {
        GameObject toGenerate = possibleObsticales[Random.Range(0, possibleObsticales.Count)];
        if (toGenerate == null) return;
        float xBound = Random.Range(boundingBox.bounds.min.x, boundingBox.bounds.max.x);
        float zBound = Random.Range(boundingBox.bounds.min.z, boundingBox.bounds.max.z);
        Vector3 spawnPoint = new Vector3(xBound, 0, zBound);

        RaycastHit hit;
        Physics.Raycast(new Vector3(xBound, boundingBox.bounds.max.y, zBound), Vector3.down, out hit);
        if (hit.collider)
        {
            spawnPoint.y = hit.point.y + verticalOffset;
            toGenerate = Instantiate(toGenerate, spawnPoint, transform.rotation, transform);
            toGenerate.transform.Rotate(Vector3.up, Random.Range(0, 180));
            toGenerate.transform.up = hit.normal;
            toGenerate.transform.RotateAround(toGenerate.transform.position, toGenerate.transform.up, toGenerate.transform.localEulerAngles.y);
        }
        obsticalesGenerated++; // incremented regardless of success to prevent infnite loop when placed in invalid locations
    }
}
