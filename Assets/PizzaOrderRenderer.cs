using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaOrder;
using TMPro;
public class PizzaOrderRenderer : MonoBehaviour
{
    public GameObject orderChit;

    private Dictionary<Order, GameObject> orderChits = new Dictionary<Order, GameObject>();

    public void AddOrder(Order order)
    {
        GameObject orderChitInstance = Instantiate(orderChit, transform);
        
        var orderText = order.GetOrderString();
        
        orderChitInstance.GetComponentInChildren<TextMeshProUGUI>().text = orderText;
        
        orderChits.Add(order, orderChitInstance);
    }

    void RemoveOrder(Order order)
    {
        GameObject obj;
        if (orderChits.TryGetValue(order, out obj))
        {
            Destroy(obj);
            orderChits.Remove(order);
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

    public void Start()
    {
        Initialize();
    }
}
