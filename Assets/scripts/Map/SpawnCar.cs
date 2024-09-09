using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnCar : MonoBehaviour
{
    private List<GameObject> _cars;

    public bool canSpawnNothing;

    private void Start()
    {
        LoadCarsFromResources();
        PickCar();
    }

    private void LoadCarsFromResources()
    {
        _cars = Resources.LoadAll<GameObject>("Prefabs/Cars").ToList();
    }

    private void PickCar()
    {
        var index = Random.Range(0, _cars.Count);

        //if it can spawn nothing
        if (canSpawnNothing)
        {
            //have a 1 in 3 chance of spawning nothing
            var newIndex = Random.Range(0, 3);
            if (newIndex == 0)
            {
                Debug.Log("Spawned nothing");
                Destroy(gameObject);
                return;
            }
        }
        
        var car = Instantiate(_cars[index], transform.parent);
        car.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Debug.Log("Spawned car");
        Destroy(gameObject);
    }
}
