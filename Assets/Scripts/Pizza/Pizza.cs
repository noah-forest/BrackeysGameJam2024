using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PizzaMaterial
{
    public Pizza.Toppings topping;
    public Material rawMaterial;
    public Material cookedMaterial;
}

[RequireComponent(typeof(PizzaMaterialRenderer))]
public class Pizza : MonoBehaviour
{
    public UnityEvent toppingAdded;
    public UnityEvent onCooked;
    public UnityEvent onBurned;
    private bool isCooked = false;
    private bool isBurned = false;

    private float timeCooked = 0;

    public enum Toppings
    {
        Pepperoni,
        Cheese,
        Sauce,
        Sausage,
        Pineapple,
        Mushroom,
        Pepper,
        Olives,
		Bacon
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

    public Toppings[] GetToppings()
    {
        return toppings.ToArray();
    }

    public bool IsCooked()
    {
        return isCooked;
    }

    public bool IsBurned()
    {
        return isBurned;
    }

    public void Burn()
    {
        isBurned = true;
        onBurned.Invoke();
    }
    
    public float GetTimeCooked()
    {
        return timeCooked;
    }
    
    public void SetTimeCooked(float time)
    {
        timeCooked = time;
    }
}
