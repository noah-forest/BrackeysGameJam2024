using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCar : MonoBehaviour
{
    private List<GameObject> _cars;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
        
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
        var car = Instantiate(_cars[index], transform);
        car.transform.position = transform.position;
    }
}
