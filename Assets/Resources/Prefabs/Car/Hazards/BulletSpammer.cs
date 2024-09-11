using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BulletSpammer : MonoBehaviour
{
    CarMaster car;
    [SerializeField] protected List<GameObject> possibleObsticales;
    [SerializeField] protected BoxCollider boundingBox;
    [SerializeField] protected List<GameObject> obsticalesGenerated;
    [SerializeField] protected int minObsticales;
    [SerializeField] protected int maxObsticales;
    [SerializeField][Range(0, 1)] protected float chanceToSpawnNothing;
    [SerializeField] Transform building;

    [SerializeField] float spawnInterval;
    [SerializeField] int spawnBurst;
    [SerializeField] float requiredProximity;
    float timeStamp;


    private void Start()
    {
        car = CarMaster.singleton;
    }

    private void FixedUpdate()
    {
        if (CanSpawn())
        {
            BurstSpawn();
        }
    }

    void BurstSpawn()
    {
        timeStamp = Time.time + spawnInterval;
        for (int i = 0; i < spawnBurst; i++)
        {
            Spawn();
        }
    }

    bool CanSpawn()
    {
        if (!car) car = CarMaster.singleton;
        float dist = Vector3.Distance(car.transform.position, transform.position);
        return Time.time > timeStamp; //&& Vector3.Distance(car.transform.position, transform.position) > requiredProximity;
    }

    protected void Spawn()
    {
        Debug.Log("BulletSpammer spawn");
        if (RollChanceToSpawnNothing()) return;
        obsticalesGenerated.RemoveAll((x) => x == null); // clean out list of null values
        if (obsticalesGenerated.Count >= maxObsticales) return;
        GameObject toGenerate = possibleObsticales[Random.Range(0, possibleObsticales.Count)];
        if (toGenerate == null) return;
        float xBound = Random.Range(boundingBox.bounds.min.x, boundingBox.bounds.max.x);
        float yBound = Random.Range(boundingBox.bounds.min.y, boundingBox.bounds.max.y);
        float zBound = Random.Range(boundingBox.bounds.min.z, boundingBox.bounds.max.z);
        Vector3 spawnPoint = new Vector3(xBound, yBound, zBound);
        Vector3 dir = (spawnPoint - building.transform.position).normalized;

        toGenerate = Instantiate(toGenerate, spawnPoint, transform.rotation, transform);
        toGenerate.transform.forward = dir;
        obsticalesGenerated.Add(toGenerate); ; // incremented regardless of success to prevent infnite loop when placed in invalid locations

    }

    protected bool RollChanceToSpawnNothing()
    {
        float roll = Random.value;
        // Debug.Log($" {gameObject.name}: Obsticale Count: {obsticalesGenerated.Count} ==> {roll}/{chanceToSpawnNothing}");
        return obsticalesGenerated.Count > minObsticales && roll <= chanceToSpawnNothing;
    }
}
