using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using Unity.VisualScripting;
using UnityEngine;

public class PlacePizzaDough : MonoBehaviour
{
    RangeInteractable rangeInteractable;
    public Pizza pizza;

    private bool pizzaPlaced = false;

    public Material pizzaSauceMaterial;
    public Material pizzaSauceCheeseMaterial;

    public void PlacePizza(GameObject interactor)
    {
        Grabber grabber = interactor.GetComponent<Grabber>();
        if (grabber)
        {
            if (!pizza.gameObject.activeSelf && grabber.GetGrabbed()?.tag == "PizzaDough")
            {
                pizza.gameObject.SetActive(true);
                grabber.DestroyGrabbed();
            }
            if (pizza.gameObject.activeSelf && !pizza.HasSauce() && grabber.GetGrabbed()?.tag == "PizzaSauce")
            {
                pizza.AddSauce();
                grabber.DestroyGrabbed();
            }
            if (pizza.gameObject.activeSelf && !pizza.HasCheese() && grabber.GetGrabbed()?.tag == "Cheese")
            {
                pizza.AddCheese();
                grabber.DestroyGrabbed();
            }
        }
    }
}
