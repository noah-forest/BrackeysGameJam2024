using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using UnityEngine;

public class PlacePizzaDough : MonoBehaviour
{
    RangeInteractable rangeInteractable;
    public GameObject pizza;

    private bool pizzaPlaced = false;
    void Start()
    {
        rangeInteractable = GetComponent<RangeInteractable>();
        
        rangeInteractable.onInteract.AddListener((interactor =>
        {
            if (!pizzaPlaced)
            {
                Grabber grabber = interactor.GetComponent<Grabber>();
                if (grabber && grabber.GetGrabbed()?.tag == "PizzaDough")
                {
                    pizzaPlaced = true;
                    pizza.SetActive(true);
                    grabber.DestroyGrabbed();
                }
            }
        }));
    }
}
