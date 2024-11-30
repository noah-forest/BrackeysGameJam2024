using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using PizzaOrder;
using TMPro;
using UnityEngine;

public class PizzaBox : Grabbable
{
    private Pizza pizzaInBox = null;
    private Animator animator;
    private Order order;

    public override bool CanInteract(GameObject interactor)
    {
        Grabber grabber = interactor.GetComponent<Grabber>();
        if (grabber && grabber.GetCurrentlyGrabbed() != null && grabber.GetCurrentlyGrabbed().GetComponent<Pizza>() != null)
        {
            displayText = "[LMB] Insert Pizza";
            return true;
        }
        else if (CanGrab(grabber))
        {
            displayText = "[LMB] Grab Order";
            return true;
        }
        return false;
    }

    public override bool CanGrab(Grabber grabber)
    {
        return pizzaInBox != null && base.CanGrab(grabber);
    }
    
    public Pizza GetPizzaInBox()
    {
        return pizzaInBox;
    }
    
    public void Start()
    {
        canGrab = false;
        animator = GetComponent<Animator>();
        animator.SetBool("Open", true);
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
                Invoke("DisableAnimator", 1f);
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

    void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    
    void EnableGrabbable()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        canGrab = true;
    }
}
