using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardmentZone : ObsitcalGenerator
{
    [SerializeField] float spawnInterval;
    [SerializeField] int spawnBurst;
    [SerializeField] float requiredProximity;
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
            BurstSpawn();
        }
    }

    void BurstSpawn()
    {
        timeStamp = Time.time + spawnInterval;
        for(int i = 0; i < spawnBurst; i++)
        {
            if (RollChanceToSpawnNothing()) return;
            Spawn();
        }
    }

    bool CanSpawn()
    {
        if(!car) car = CarMaster.singleton;
        return Time.time > timeStamp && Vector3.Distance(car.transform.position, transform.position) > requiredProximity;
    }
}
