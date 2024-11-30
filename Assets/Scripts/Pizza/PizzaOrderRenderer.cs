using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaOrder;
using TMPro;
using UnityEngine.Events;

public class PizzaOrderRenderer : MonoBehaviour
{
    public UnityEvent onOrderAdded = new UnityEvent();
    public GameObject orderChit;
    public GameObject finishedText;
    public GameObject waitingText;
    
    private PizzaModeManager pizzaModeManager;
    private Dictionary<Order, GameObject> orderChits = new Dictionary<Order, GameObject>();
    bool doneFortheDay;

    public void Start()
    {
        Initialize();
        pizzaModeManager = PizzaModeManager.singleton;
        
        finishedText.SetActive(false);
        
        pizzaModeManager.ordersFinished.AddListener(OnOrdersFinished);
    }
    
    public void AddOrder(Order order)
    {
        GameObject orderChitInstance = Instantiate(orderChit, transform);
        
        var orderText = order.GetOrderString();
        
        orderChitInstance.GetComponentInChildren<TextMeshProUGUI>().text = orderText;
        
        orderChits.Add(order, orderChitInstance);
        onOrderAdded.Invoke();
        if (OrderManager.orders.Count > 0)
        {
            waitingText.SetActive(false);
        }
    }

    void RemoveOrder(Order order)
    {
        GameObject obj;
        if (orderChits.TryGetValue(order, out obj))
        {
            Destroy(obj);
            orderChits.Remove(order);
        }

        if(OrderManager.orders.Count == 0 && !doneFortheDay)
        {
            waitingText.SetActive(true);
        }

    }

    public void Initialize()
    {
        foreach(var child in transform)
        {
            Destroy(((Transform)child).gameObject);
        }
        foreach (Order order in OrderManager.orders)
        {
            AddOrder(order);
        }
        OrderManager.onOrderCreated.AddListener(AddOrder);
        OrderManager.onOrderRemoved.AddListener(RemoveOrder);
    }

    private void OnOrdersFinished()
    {
        waitingText.SetActive(false);
        finishedText.SetActive(true);
        doneFortheDay = true;
    }

}
