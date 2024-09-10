using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGoal : MonoBehaviour
{
    CarModeManager carModeManager;
    public BoxCollider boundingBox;
    float timeTouching;
    [SerializeField] float timeToClear;
    [SerializeField] GameObject clearEffect;
    [SerializeField] Transform clearingVisual;
     Vector3 clearingVisualStartScale;
    [SerializeField] Vector3 clearingVisualEndScale;
    private void Start()
    {
        carModeManager = CarModeManager.singleton;
        clearingVisualStartScale = clearingVisual.localScale;
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        timeTouching += Time.deltaTime;
        clearingVisual.transform.localScale = Vector3.Lerp(clearingVisual.transform.localScale, clearingVisualEndScale, Time.deltaTime);
        if (timeTouching > timeToClear)
        {
            if (clearEffect) Instantiate(clearEffect, transform.position, transform.rotation, transform.parent);
            carModeManager.DeliverPizza();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        timeTouching = 0;
        clearingVisual.localScale = clearingVisualStartScale;
    }
}