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
    
    public PizzaBoxSpawner[] pizzaBoxSpawners;

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
            Order order = pizzaBox.GetOrder();
            var score = order.CalculatePizzaScore(pizza);
            order.CompleteOrder(pizza);
            OrderManager.RemoveOrder(order);
            
            if (OrderManager.orders.Count == 0)
            {
                GenerateRandomOrders();
            }
        }
    }
    
    private void GenerateRandomOrders()
    {
        OrderManager.ClearOrders();
        for (int i = 0; i < pizzaBoxSpawners.Length; i++)
        {
            var order = OrderManager.CreateRandomOrder();
            pizzaBoxSpawners[i].SetCurrentOrder(order);
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
