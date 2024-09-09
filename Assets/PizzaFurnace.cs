using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using UnityEngine;

public class PizzaFurnace : TriggerInteractor
{
    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);

        Grabber grabber = gameObject.GetComponent<Grabber>();

        if (grabber != null && grabber.GetCurrentlyGrabbed().tag == "pizza")
        {
            var pizza = grabber.GetCurrentlyGrabbed().GetComponent<Pizza>();
            pizza.Cook();
        }
    }
}
