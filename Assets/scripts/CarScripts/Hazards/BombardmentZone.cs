using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardmentZone : ObsitcalGenerator
{
    [SerializeField] float spawnInterval;
    [SerializeField] int spawnBurst;
    [SerializeField] float requiredProximity;
    [SerializeField] float minBurstInterval = 0;
    [SerializeField] float maxBurstInterval = 0.5f;
    bool isBursting;
    float timeStamp;
    CarMaster car;

    private void Start()
    {
        car = CarMaster.singleton;
    }

    private void FixedUpdate()
    {
        if(CanSpawn())
        {
            StartCoroutine( BurstSpawn());
        }
    }

    IEnumerator BurstSpawn()
    {
        isBursting = true;
        timeStamp = Time.time + spawnInterval;
        for(int i = 0; i < spawnBurst; i++)
        {
            if (RollChanceToSpawnNothing()) continue;
            yield return new WaitForSeconds(Random.Range(minBurstInterval, maxBurstInterval));
            Spawn();
        }
        isBursting = false;
    }

    bool CanSpawn()
    {
        if(!car) car = CarMaster.singleton;
        return !isBursting && Time.time > timeStamp && car ? Vector3.Distance(car.transform.position, transform.position) < requiredProximity : true;
    }
}
