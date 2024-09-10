using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaOrder;
using TMPro;
public class PizzaOrderRenderer : MonoBehaviour
{
    public GameObject orderChit;
    private OrderManager orderManager;

    private Dictionary<Order, GameObject> orderChits = new Dictionary<Order, GameObject>();

    public void AddOrder(Order order)
    {
        GameObject orderChitInstance = Instantiate(orderChit, transform);
        var excludedStr = "";
        foreach (var excluded in order.excludedToppings)
        {
            excludedStr += excluded.ToString() + ", ";
        }
        excludedStr = excludedStr.TrimEnd(',', ' ');
        var orderText = order.name;
        
        if (excludedStr.Length > 0)
        {
            orderText += "\n(no " + excludedStr + ")";
        }
        
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
        foreach (Order order in orderManager.orders)
        {
            AddOrder(order);
        }
        orderManager.onOrderCreated.AddListener(AddOrder);
        orderManager.onOrderRemoved.AddListener(RemoveOrder);
    }

    public void Update()
    {
        if (orderManager == null)
        {
            orderManager = OrderManager.instance;
            if (orderManager != null)
            {
                Initialize();
            }
        }
    }
}
