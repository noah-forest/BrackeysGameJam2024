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

    [SerializeField] float minBurstInterval = 0;
    [SerializeField] float maxBurstInterval = 0.5f;
    bool isBursting;

    private void Start()
    {
        car = CarMaster.singleton;
    }

    private void FixedUpdate()
    {
        if (CanSpawn())
        {
            StartCoroutine(BurstSpawn());
        }
    }

    IEnumerator BurstSpawn()
    {
        isBursting = true;
        timeStamp = Time.time + spawnInterval;
        for (int i = 0; i < spawnBurst; i++)
        {
            yield return new WaitForSeconds(Random.Range(minBurstInterval, maxBurstInterval));
            Spawn();
        }
        isBursting = false;
    }

    bool CanSpawn()
    {
        float dist = 0;
        if (car)
        {
            dist = Vector3.Distance(car.transform.position, transform.position);
        }

        return !isBursting && Time.time > timeStamp && dist < requiredProximity;
    }

    protected void Spawn()
    {
        //Debug.Log("BulletSpammer spawn");
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
        return  obsticalesGenerated.Count > minObsticales && roll <= chanceToSpawnNothing;
    }
}
