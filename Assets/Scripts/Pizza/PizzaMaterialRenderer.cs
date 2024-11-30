using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pizza))]
public class PizzaMaterialRenderer : MonoBehaviour
{
    public PizzaMaterial[] materials;
    
    public Material rawDoughMaterial;
    public Material cookedDoughMaterial;
    // public PizzaMaterial sauceMaterial;
    // public PizzaMaterial cheeseMaterial;
    // public PizzaMaterial pepperoniMaterial;
    // public PizzaMaterial sausageMaterial;

    public Material burntDoughMaterial;
    private Renderer renderer;

    private Pizza pizza;

    void Start()
    {
        pizza = GetComponent<Pizza>();
        renderer = GetComponent<Renderer>();
        pizza.toppingAdded.AddListener(UpdateMaterial);
        pizza.onCooked.AddListener(UpdateMaterial);
        pizza.onBurned.AddListener(() => renderer.materials = new Material[] { burntDoughMaterial, burntDoughMaterial });
        UpdateMaterial();
    }
    
    public Material GetDoughMaterial()
    {
        if (pizza.IsCooked())
        {
            return cookedDoughMaterial;
        }
        else
        {
            return rawDoughMaterial;
        }
    }

    public Material GetMaterial(PizzaMaterial material)
    {
        if (pizza.IsCooked())
        {
            return material.cookedMaterial;
        }
        else
        {
            return material.rawMaterial;
        }
    }

    public void UpdateMaterial()
    {
        List<Material> materialsToPlace = new List<Material>();
        materialsToPlace.Add(GetDoughMaterial());
        materialsToPlace.Add(GetDoughMaterial());
        
        foreach (var material in materials)
        {
            if (pizza.HasTopping(material.topping))
            {
                materialsToPlace.Add(GetMaterial(material));
            }
        }
        //
        // foreach (var topping in pizza.GetToppings())
        // {
        //     foreach (var material in materials)
        //     {
        //         if (material.topping == topping)
        //         {
        //             materialsToPlace.Add(GetMaterial(material));
        //         }
        //     }
        // }
        //
        // if (pizza.HasTopping(Pizza.Toppings.Sauce))
        // {
        //     materialsToPlace.Add(GetMaterial(sauceMaterial));
        // }
        //
        // if (pizza.HasTopping(Pizza.Toppings.Cheese))
        // {
        //     materialsToPlace.Add(GetMaterial(cheeseMaterial));
        // }
        //
        // if (pizza.HasTopping(Pizza.Toppings.Pepperoni))
        // {
        //     materialsToPlace.Add(GetMaterial(pepperoniMaterial));
        // }
        //
        // if (pizza.HasTopping(Pizza.Toppings.Sausage))
        // {
        //     materialsToPlace.Add(GetMaterial(sausageMaterial));
        // }

        //Debug.Log(materials);

        renderer.materials = materialsToPlace.ToArray();
    }
}
