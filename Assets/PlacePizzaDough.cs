using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlacePizzaDough : MonoBehaviour
{
    RangeInteractable rangeInteractable;
    public Pizza pizzaPrefab;
    public Pizza currentPizza;

    public Material pizzaSauceMaterial;
    public Material pizzaSauceCheeseMaterial;

    void ClearPizza()
    {
        if (currentPizza)
        {
            currentPizza.GetComponent<Grabbable>().onGrab.RemoveListener(ClearPizza);
            currentPizza = null;
        }
    }

    public void PlacePizza(GameObject interactor)
    {
        Grabber grabber = interactor.GetComponent<Grabber>();
        if (grabber)
        {
            if (!currentPizza && grabber.GetGrabbed()?.tag == "pizza")
            {
                currentPizza = grabber.GetGrabbed().GetComponent<Pizza>();
                grabber.GetGrabbed().onGrab.AddListener(ClearPizza);
                grabber.Release();
                currentPizza.transform.position = transform.position;
                currentPizza.transform.rotation = transform.rotation;
            }

            if (!currentPizza && grabber.GetGrabbed()?.tag == "PizzaDough")
            {
                currentPizza.gameObject.SetActive(true);
                grabber.DestroyGrabbed();
            }
            if (currentPizza && !currentPizza.HasSauce() && grabber.GetGrabbed()?.tag == "PizzaSauce")
            {
                currentPizza.AddSauce();
                grabber.DestroyGrabbed();
            }
            if (currentPizza && !currentPizza.HasCheese() && grabber.GetGrabbed()?.tag == "Cheese")
            {
                currentPizza.AddCheese();
                grabber.DestroyGrabbed();
            }
        }
    }
}
