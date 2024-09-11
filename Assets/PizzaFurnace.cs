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

    public int timeToCook = 15;
    public int timeToBurn = 30;
    private float timeInOven = 0;

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
                PlacePizza(pizza.gameObject);
                GrabCurrentPizza(grabber);
            }
            currentPizza = pizza;
        }
        else
        {
            GrabCurrentPizza(grabber);
        }
    }

    void Update()
    {
        if (currentPizza)
        {
            timeInOven += 1 * Time.deltaTime;
            if (timeInOven >= timeToBurn)
            {
                currentPizza.Burn();
            }
            else if (timeInOven >= timeToCook && !currentPizza.IsCooked())
            {
                currentPizza.Cook();
            }
        }
    }

    void PlacePizza(GameObject pizza)
    {
        timeInOven = 0;
        pizza.transform.position = transform.position + pizzaPosition;
        pizza.transform.rotation = Quaternion.identity;
        pizza.GetComponent<Rigidbody>().velocity = Vector3.zero;
        pizza.GetComponent<Rigidbody>().isKinematic = true;
    }

    void GrabCurrentPizza(Grabber grabber)
    {

        if (currentPizza)
        {
            currentPizza.GetComponent<Rigidbody>().isKinematic = false;
            grabber.Grab(currentPizza.GetComponent<Grabbable>());
            currentPizza = null;
        }
    }

    public void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + pizzaPosition, 0.1f);
    }
}
