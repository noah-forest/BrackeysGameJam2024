using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PizzaOrder;
using TMPro;
using Unity.VisualScripting;

public class RecipeBookRenderer : MonoBehaviour
{
    public GameObject recipePage;
    void Start()
    {
        OrderManager.recipeBookChanged.AddListener(Render);
        Render();
    }

    void Render()
    {
        // clear all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var recipe in OrderManager.recipeBook)
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
