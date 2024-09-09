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

        var grabberTag = grabber.GetCurrentlyGrabbed()?.tag;

        if (grabberTag == "PizzaSauce" || grabberTag == "Cheese")
        {
            if (grabberTag == "PizzaSauce")
            {
                if (!pizza.HasSauce())
                {
                    pizza.AddSauce();
                    grabber.DestroyGrabbed();
                }
            }
            else if (grabberTag == "Cheese")
            {
                if (!pizza.HasCheese())
                {
                    pizza.AddCheese();
                    grabber.DestroyGrabbed();
                }
            }
        }
        if (grabberTag == "PizzaSauce" || grabberTag == "Cheese")
        {
            return false;
        }

        return canGrab;
    }
}
