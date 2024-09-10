using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using UnityEngine;

public class PizzaFurnace : TriggerInteractor
{
    public Vector3 pizzaPosition;
    private Pizza currentPizza;
    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);

        Grabber grabber = gameObject.GetComponent<Grabber>();

        if (grabber.IsGrabbing())
        {

            Pizza pizza = grabber.GetCurrentlyGrabbed().gameObject.GetComponent<Pizza>();

            if (pizza != null)
            {
                grabber.ReleaseCurrentlyGrabbed();
                pizza.gameObject.transform.position = transform.position + pizzaPosition;
                pizza.gameObject.transform.rotation = Quaternion.identity;
                pizza.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                pizza.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                if (currentPizza)
                {
                    currentPizza.GetComponent<Rigidbody>().isKinematic = false;
                    grabber.Grab(currentPizza.GetComponent<Grabbable>());
                    currentPizza = pizza;
                }
            }
        }
        else
        {
            if (currentPizza)
            {
                currentPizza.GetComponent<Rigidbody>().isKinematic = false;
                grabber.Grab(currentPizza.GetComponent<Grabbable>());
                currentPizza = null;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + pizzaPosition, 0.1f);
    }
}
