using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaOrder;
using TMPro;
using Unity.VisualScripting;

public class RecipeBookRenderer : MonoBehaviour
{
    private OrderManager orderManager;
    public GameObject recipePage;
    void Start()
    {
        if (OrderManager.ready == false)
        {
            OrderManager.onReady.AddListener(Render);
        }
        else
        {
            Render();
        }
    }

    void Render()
    {
        // clear all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        orderManager = OrderManager.instance;
        foreach (var recipe in orderManager.recipeBook)
        {
            var page = Instantiate(recipePage, transform);
            page.GetComponentInChildren<TextMeshProUGUI>().text = recipe.name;
            string toppings = "";
            foreach (var topping in recipe.toppings)
            {
                toppings += "- " + topping.ToString() + "\n";
            }
            page.GetComponentInChildren<TextMeshProUGUI>().text += "\n" + toppings;
        }
    }
}
