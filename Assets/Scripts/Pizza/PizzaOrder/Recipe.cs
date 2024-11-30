using System;

namespace PizzaOrder
{
    [Serializable]
    public class Recipe
    {
        public string name;
        public Pizza.Toppings[] toppings;
    }
}