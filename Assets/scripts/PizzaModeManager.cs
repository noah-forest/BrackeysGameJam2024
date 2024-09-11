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
        
        GenerateRandomOrders();
        singleton = this;
    }

    public void PizzaSubmission(GameObject obj)
    {
        PizzaBox pizzaBox;

        if (obj.TryGetComponent<PizzaBox>(out pizzaBox))
        {
            if (pizzaBox.GetPizzaInBox() == null)
            {
                return;
            }
            var pizza = pizzaBox.GetPizzaInBox();
            float highestScore = -1;
            Order highestOrder = null;
            foreach (var order in OrderManager.orders)
            {
                float score = order.CalculatePizzaScore(pizza);
                if (score > highestScore)
                {
                    highestScore = score;
                    highestOrder = order;
                }
            }
            
            if (highestOrder != null)
            {
                highestOrder.CompleteOrder(pizza);
                OrderManager.RemoveOrder(highestOrder);
            }
            
            if (OrderManager.orders.Count == 0)
            {
                GenerateRandomOrders();
            }
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
