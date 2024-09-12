using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : PizzaModeInteractable
{

    PizzaModeManager modeManager;

    private void Start()
    {
        modeManager = PizzaModeManager.singleton;
    }
    public override bool CanInteract(GameObject gameObject)
    {
        //Debug.Log(base.CanInteract(gameObject) && modeManager.ReadyToLeave());
        return base.CanInteract(gameObject) && modeManager.ReadyToLeave();
    }

    public override void Interact(GameObject gameObject)
    {
        //Debug.Log("Interacted with Door");
        base.Interact(gameObject);
        modeManager.gameManager.pizzaCount = modeManager.ordersReadyToDeliver;
        modeManager.gameManager.LoadCarScene();
    }
}
