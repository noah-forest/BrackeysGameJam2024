using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using PizzaOrder;
using TMPro;
using UnityEngine;

public class PizzaBox : TriggerInteractor
{
    private Pizza pizzaInBox = null;
    private Animator animator;
    private Grabbable thisGrabbable;
    private Order order;
    
    public Pizza GetPizzaInBox()
    {
        return pizzaInBox;
    }
    
    public void Start()
    {
        thisGrabbable = GetComponent<Grabbable>();
        animator = GetComponent<Animator>();
        thisGrabbable.canGrab = false;
        animator.SetBool("Open", true);
    }
    
    public override bool CanInteract(GameObject gameObject)
    {
        return pizzaInBox == null;
    }
    
    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        Grabber grabber;
        
        if (gameObject.TryGetComponent<Grabber>(out grabber) && grabber.GetCurrentlyGrabbed() != null)
        {
            Pizza pizza;

            if (grabber.GetCurrentlyGrabbed().TryGetComponent<Pizza>(out pizza))
            {
                grabber.ReleaseCurrentlyGrabbed();

                Grabbable pizzaGrabber = pizza.GetComponent<Grabbable>();
                pizzaGrabber.canGrab = false;
                pizzaInBox = pizza;

                pizza.GetComponent<Rigidbody>().isKinematic = true;
                pizza.transform.SetParent(this.transform);
                pizza.transform.localPosition = Vector3.zero;
                pizza.transform.localRotation = Quaternion.identity;
                pizza.GetComponent<Collider>().enabled = false;
                
                animator.SetBool("Open", false);
                Invoke("EnableGrabbable", 0.1f);
            }
        }
    }

    public void SetOrderText(string order)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = order;
    }

    public void SetOrder(Order order)
    {
        this.order = order;
        SetOrderText(order.GetOrderString());
    }
    public Order GetOrder()
    {
        return order;
    }

    void EnableGrabbable()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        thisGrabbable.canGrab = true;
    }
}
