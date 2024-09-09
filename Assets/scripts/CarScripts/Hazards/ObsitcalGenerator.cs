using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObsitcalGenerator : MonoBehaviour
{

    [SerializeField] protected List<GameObject> possibleObsticales;
    [SerializeField] protected BoxCollider boundingBox;
    [SerializeField] protected float verticalOffset;
    [SerializeField] protected List<GameObject> obsticalesGenerated;
    [SerializeField] protected int minObsticales;
    [SerializeField] protected int maxObsticales;
    [SerializeField][Range(0, 1)] protected float chanceToSpawnNothing;
    
    protected void Spawn()
    {
        obsticalesGenerated.RemoveAll((x) => x == null); // clean out list of null values
        if (obsticalesGenerated.Count >= maxObsticales) return;
        GameObject toGenerate = possibleObsticales[Random.Range(0, possibleObsticales.Count)];
        if (toGenerate == null) return;
        float xBound = Random.Range(boundingBox.bounds.min.x, boundingBox.bounds.max.x);
        float zBound = Random.Range(boundingBox.bounds.min.z, boundingBox.bounds.max.z);
        Vector3 spawnPoint = new Vector3(xBound, 0, zBound);

        RaycastHit hit;
        Physics.Raycast(new Vector3(xBound, boundingBox.bounds.max.y, zBound), Vector3.down, out hit,boundingBox.size.y+1,LayerMask.GetMask("Ground"));
        if (hit.collider)
        {
            spawnPoint.y = hit.point.y + verticalOffset;
            toGenerate = Instantiate(toGenerate, spawnPoint, transform.rotation, transform);
            toGenerate.transform.Rotate(Vector3.up, Random.Range(0, 180));
            toGenerate.transform.up = hit.normal;
            toGenerate.transform.RotateAround(toGenerate.transform.position, toGenerate.transform.up, toGenerate.transform.localEulerAngles.y);
            obsticalesGenerated.Add(toGenerate); ; // incremented regardless of success to prevent infnite loop when placed in invalid locations
        }
        else
        {
            Debug.Log($" {gameObject.name}: Spawn did not collide. No object Created");
        }
        
    }

    protected bool RollChanceToSpawnNothing()
    {
        float roll = Random.value;
        Debug.Log($" {gameObject.name}: Obsticale Count: {obsticalesGenerated.Count} ==> {roll}/{chanceToSpawnNothing}");
        return obsticalesGenerated.Count > minObsticales && roll <= chanceToSpawnNothing;
    }
}
