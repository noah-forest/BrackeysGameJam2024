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
        modeManager.ordersFinished.AddListener(StartGlow);
    }

    [SerializeField] GameObject glowEffect;

    public void StartGlow()
    {
        glowEffect.SetActive(true);
        displayText = "[LMB] Start Delivery";
    }


    public override bool CanInteract()
    {
        //Debug.Log(base.CanInteract(gameObject) && modeManager.ReadyToLeave());
        return base.CanInteract() && modeManager.ReadyToLeave();
    }

    public override void Interact(GameObject gameObject)
    {
        //Debug.Log("Interacted with Door");
        base.Interact(gameObject);
        modeManager.gameManager.pizzaCount = modeManager.numberOfCompletedOrders;
        modeManager.gameManager.LoadCarScene();
    }
}
