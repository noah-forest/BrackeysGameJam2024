using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PizzaOrder;
using UnityEngine;

public class PizzaModeManager : MonoBehaviour
{
    #region singleton

    public static PizzaModeManager singleton;

    public Recipe[] defaultRecipeBook;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }
            
        OrderManager.SetRecipeBook(defaultRecipeBook);
        
        singleton = this;
    }

    public void PizzaSubmission(GameObject obj)
    {
        Pizza pizza;

        if (obj.TryGetComponent<Pizza>(out pizza))
        {
        }
    }

    private void Update()
    {
        if (Mathf.FloorToInt(Time.time) % 40 == 0)
        {
            GenerateRandomOrders();
        }
    }
    
    private void GenerateRandomOrders()
    {
        OrderManager.ClearOrders();
        for (int i = 0; i < 4; i++)
        {
            OrderManager.CreateRandomOrder();
        }
    }
    
    #endregion
    public PlayerController player;
    public GameManager gameManager;
    /// <summary>
    /// When the player leaves the pizzaria, This value is passed to the Game manager, which is then passed to the car mode
    /// </summary>
    public uint ordersReadyToDeliver;
    /// <summary>
    /// This would be the total number of orders for the day the player needs to complete before they can leave the Pizzaria
    /// </summary>
    public uint ordersRequired;

    public Transform ambianceSoundLocation;
    public bool ReadyToLeave()
    {
        return ordersReadyToDeliver >= ordersRequired;
    }
}
