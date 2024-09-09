using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PizzaMaterial
{
    public Material rawMaterial;
    public Material cookedMaterial;
}

[RequireComponent(typeof(PizzaMaterialRenderer))]
public class Pizza : MonoBehaviour
{
    public UnityEvent toppingAdded;
    public UnityEvent onCooked;
    private bool isCooked = false;

    public enum Toppings
    {
        Pepperoni,
        Cheese,
        Sauce,
        Sausage
    }

    List<Toppings> toppings = new List<Toppings>();

    public bool HasTopping(Toppings topping)
    {
        return toppings.Contains(topping);
    }

    public void AddTopping(Toppings topping)
    {
        toppings.Add(topping);
        toppingAdded.Invoke();
    }

    public void Cook()
    {
        if (!isCooked)
        {
            isCooked = true;
            onCooked.Invoke();
        }
    }

    public bool IsCooked()
    {
        return isCooked;
    }
}
