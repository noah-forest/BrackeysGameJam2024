using System;
using System.Collections.Generic;

namespace PizzaOrder
{
    [Serializable]
    public class Order
    {
        public string name;
        public Recipe recipe;
        public List<Pizza.Toppings> excludedToppings;
        
        public Order(string name, Recipe recipe, List<Pizza.Toppings> excludedToppings)
        {
            this.name = name;
            this.recipe = recipe;
            this.excludedToppings = excludedToppings;
        }
    }
}