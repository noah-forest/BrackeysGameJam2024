using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCar : MonoBehaviour
{
    private List<GameObject> _cars;

    public bool canSpawnNothing;
    public bool spawnWithCollider;

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
                Destroy(gameObject);
                return;
            }
        }
        
        var car = Instantiate(_cars[index], transform.parent);
        car.transform.SetPositionAndRotation(transform.position, transform.rotation);

        if (!spawnWithCollider)
        {
            var collider = car.GetComponent<BoxCollider>();
            collider.enabled = false;
        }
        
        Destroy(gameObject);
    }
}
