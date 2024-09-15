using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using PizzaOrder;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PizzaModeManager : MonoBehaviour
{
    #region singleton

    public static PizzaModeManager singleton;

    public RecipeBook[] dailyRecipieBook;

    [Serializable]
    public struct RecipeBook
    {
        public Recipe[] recipies;
    }
    
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
    public uint numberOfCompletedOrders
    {
        get
        {
            return OrderManager.GetNumberOfCompletedOrders();
        }
    }

    /// <summary>
    /// This would be the total number of orders for the day the player needs to complete before they can leave the Pizzaria
    /// </summary>
    public uint ordersRequired;

    int currentNumberOfOrders
    {
        get
        {
            return OrderManager.orders.Count;
        }
    }

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
    
    private void Start()
    {
        Clean();
        Setup();
        int day = Mathf.Clamp(gameManager.Day - 1, 0, DailyOrders.Length);

        ordersRequired = DailyOrders[day].GetTodaysOrderCount();
        //Debug.Log($"[PIZZA MODE] dayidx: {day} Todays Order Count: {ordersRequired}");
    }
    
    void OnOrderManagerCompletedOrder(Order order)
    {
        //Debug.Log($"[PIZZA MODE][PIZZA SUBMISSION]: ORDER STATUS: {numberOfCompletedOrders} / {ordersRequired} : DAY COMPLETE");
        if (numberOfCompletedOrders >= ordersRequired)
        {
            ordersFinished.Invoke();
        }
    }

    void Setup()
    {
        OrderManager.SetRecipeBook(dailyRecipieBook[gameManager.Day-1].recipies);
        OrderManager.onOrderCompleted.AddListener(OnOrderManagerCompletedOrder);

        StartCoroutine(NextOrder());
    }

    void Clean()
    {
        OrderManager.ClearOrders();
        OrderManager.ClearCompletedOrders();
        ClearPizzaBoxSpawnerOrders();
        OrderManager.onOrderCompleted.RemoveListener(OnOrderManagerCompletedOrder);
        
        StopCoroutine(NextOrder());
    }
    
    public bool ReadyToLeave()
    {
        return numberOfCompletedOrders >= ordersRequired;
    }

    public void PizzaSubmission(GameObject obj)
    {

        // THERE IS A BUG WHERE PIZZAS ARE COUNTED MULTIPLE TIMES
        PizzaBox pizzaBox;

        if (obj.TryGetComponent<PizzaBox>(out pizzaBox))
        {
            if (pizzaBox.GetOrder() == null || pizzaBox.GetOrder().IsCompleted())
            {
                return;
            }
            
            var pizza = pizzaBox.GetPizzaInBox();
            Order order = pizzaBox.GetOrder();
            order.CompleteOrder(pizza);
            gameManager.OrdersToDeliver.Add(order);
            OrderManager.RemoveOrder(order);
        }
    }

    private void CreateNextPizzaOrder()
    {
        if (currentNumberOfOrders < 4)
        {
            var emptyBox = GetEmptyPizzaBoxSpawner();
            if (emptyBox != null)
            {
                var order = OrderManager.CreateRandomOrder();
                emptyBox.SetCurrentOrder(order);
            }
        }
    }
    
    public PizzaBoxSpawner GetEmptyPizzaBoxSpawner()
    {
        foreach (PizzaBoxSpawner boxSpawner in pizzaBoxSpawners)
        {
            if (boxSpawner.currentOrder == null)
            {
                return boxSpawner;
            }
        }

        return null;
    }

    private void ClearPizzaBoxSpawnerOrders()
    {
        foreach(PizzaBoxSpawner boxSpawners in pizzaBoxSpawners) boxSpawners.ClearOrder();
    }

    private IEnumerator NextOrder()
    {
        while (true)
        {
            if (currentNumberOfOrders < 4 && (currentNumberOfOrders + numberOfCompletedOrders) < ordersRequired)
            {
                CreateNextPizzaOrder();
            }
            float waitTime = (currentNumberOfOrders < 2) ? UnityEngine.Random.Range(2, 5) : UnityEngine.Random.Range(minOrdertime, maxOrdertime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
