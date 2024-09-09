using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pizza))]
public class PizzaMaterialRenderer : MonoBehaviour
{
    public PizzaMaterial doughMaterial;
    public PizzaMaterial sauceMaterial;
    public PizzaMaterial cheeseMaterial;
    public PizzaMaterial pepperoniMaterial;
    private Renderer renderer;

    private Pizza pizza;

    void Start()
    {
        pizza = GetComponent<Pizza>();
        renderer = GetComponent<Renderer>();
        pizza.toppingAdded.AddListener(UpdateMaterial);
        pizza.onCooked.AddListener(UpdateMaterial);
        UpdateMaterial();
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
        List<Material> materials = new List<Material>();
        materials.Add(GetMaterial(doughMaterial));
        materials.Add(GetMaterial(doughMaterial));

        if (pizza.HasTopping(Pizza.Toppings.Sauce))
        {
            materials.Add(GetMaterial(sauceMaterial));
        }

        if (pizza.HasTopping(Pizza.Toppings.Cheese))
        {
            materials.Add(GetMaterial(cheeseMaterial));
        }

        if (pizza.HasTopping(Pizza.Toppings.Pepperoni))
        {
            materials.Add(GetMaterial(pepperoniMaterial));
        }

        Debug.Log(materials);

        renderer.materials = materials.ToArray();
    }
}
