using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using UnityEngine;

[RequireComponent(typeof(Pizza))]
public class PizzaGrabbable : Grabbable
{
    Pizza pizza;

    void Start()
    {
        pizza = GetComponent<Pizza>();
    }

    public override bool CanGrab(Grabber grabber)
    {
        var canGrab = base.CanGrab(grabber);

        if (pizza.IsCooked())
        {
            return canGrab;
        }

        var currentlyGrabbed = grabber.GetCurrentlyGrabbed();

        if (currentlyGrabbed is PizzaToppingGrabbable)
        {
            var topping = currentlyGrabbed.GetComponent<PizzaToppingGrabbable>().topping;
            if (!pizza.HasTopping(topping))
            {
                pizza.AddTopping(topping);
                grabber.DestroyGrabbed();
            }

            return false;
        }

        return canGrab;
    }
}
