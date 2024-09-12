using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using PizzaOrder;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
            

        singleton = this;
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
    public uint ordersGenerated;

    public UnityEvent ordersFinished = new();

    [SerializeField][Range(1, 120)] float minOrdertime;
    [SerializeField][Range(1, 120)] float maxOrdertime;
    float timeStamp;


    [Serializable]
    struct stupid
    {
        [Range(1, 50)] public int minOrders;
        [Range(1, 50)] public int maxOrders;
        public uint GetTodaysOrderCount()
        {
            return (uint)UnityEngine.Random.Range(minOrders, maxOrders);
        }
    }

    [SerializeField] stupid[] DailyOrders = new stupid[5];

    public Transform ambianceSoundLocation;
    public bool ReadyToLeave()
    {
        return ordersReadyToDeliver >= ordersRequired;
    }

    public void PizzaSubmission(GameObject obj)
    {

        // THERE IS A BUG WHERE PIZZAS ARE COUNTED MULTIPLE TIMES
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

            Debug.Log(score);
            order.CompleteOrder(pizza);
            OrderManager.RemoveOrder(order);

            ++ordersReadyToDeliver;
            if (ordersReadyToDeliver >= ordersRequired) 
            {
                Debug.Log($"[PIZZA MODE][PIZZA SUBMISSION]: ORDER STATUS: {ordersReadyToDeliver} / {ordersRequired} : DAY COMPLETE");
                ordersFinished.Invoke();
            } 
        }
    }

    private void GenerateRandomOrders()
    {
        //Debug.Log($"[PIZZA MODE][ORDER GENERATOR]: attempting to generate order");
        for (int i = 0; i < pizzaBoxSpawners.Length; i++)
        {
            //Debug.Log($"[PIZZA MODE][ORDER GENERATOR]: searching for unassigned box");

            if (pizzaBoxSpawners[i].currentOrder == null)
            {

                var order = OrderManager.CreateRandomOrder();
                pizzaBoxSpawners[i].SetCurrentOrder(order);
                ordersGenerated++;
                Debug.Log($"[PIZZA MODE][ORDER GENERATOR]: Generated order {ordersGenerated} / {ordersRequired} for pizzaBox {i}");
                return;
            }
        }

    }

    private void Start()
    {
        OrderManager.SetRecipeBook(defaultRecipeBook);


        int day = Mathf.Clamp(gameManager.Day - 1, 0, DailyOrders.Length);

        ordersRequired = DailyOrders[day].GetTodaysOrderCount();
        foreach(PizzaBoxSpawner boxSpawners in pizzaBoxSpawners) boxSpawners.currentOrder = null;
        Debug.Log($"[PIZZA MODE] dayidx: {day} Todays Order Count: {ordersRequired}");

    }

    private void FixedUpdate()
    {
        if(OrderManager.orders.Count < 4 && ordersGenerated < ordersRequired && ordersReadyToDeliver < ordersRequired && Time.time > timeStamp)
        {
            GenerateRandomOrders();
            float waitTime = (ordersGenerated < 2) ? UnityEngine.Random.Range(2, 5) : UnityEngine.Random.Range(minOrdertime, maxOrdertime);
            timeStamp = Time.time + waitTime;
        }
    }
}
