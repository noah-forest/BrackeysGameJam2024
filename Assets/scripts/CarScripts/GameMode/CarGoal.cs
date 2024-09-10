using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGoal : MonoBehaviour
{
    CarModeManager carModeManager;
    public BoxCollider boundingBox;
    private void Start()
    {
        carModeManager = CarModeManager.singleton;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<CarMaster>())
        {
            carModeManager.DeliverPizza();
        }
    }
}
