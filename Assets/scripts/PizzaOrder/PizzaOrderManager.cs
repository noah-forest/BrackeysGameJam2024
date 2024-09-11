using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PizzaOrder {
    public static class OrderManager
    {
        public static UnityEvent<Order> onOrderCreated = new UnityEvent<Order>();
        public static UnityEvent<Order> onOrderRemoved = new UnityEvent<Order>();
        public static UnityEvent ordersChanged = new UnityEvent();
        public static UnityEvent recipeBookChanged = new UnityEvent();
        public static List<Recipe> recipeBook = new List<Recipe>();
        public static List<Order> orders = new List<Order>();
        
        
        public static void AddOrder(Order order)
        {
            orders.Add(order);
            onOrderCreated.Invoke(order);
            
            ordersChanged.Invoke();
        }
        
        public static void RemoveOrder(Order order)
        {
            orders.Remove(order);
            onOrderRemoved.Invoke(order);
            
            ordersChanged.Invoke();
        }
        
        public static void ClearOrders()
        {
            foreach (Order order in orders)
            {
                onOrderRemoved.Invoke(order);
            }
            orders.Clear();
        }
        
        public static Order CreateRandomOrder()
        {
            Recipe randomRecipe = recipeBook[Random.Range(0, recipeBook.Count)];
            List<Pizza.Toppings> excludedToppings = new List<Pizza.Toppings>();
            foreach (Pizza.Toppings topping in randomRecipe.toppings)
            {
                if (Random.Range(0, 100) < 15)
                {
                    excludedToppings.Add(topping);
                }
            }
            Order order = new Order(randomRecipe.name, randomRecipe, excludedToppings);
            AddOrder(order);
            return order;
        }
        
        public static void SetRecipeBook(Recipe[] recipes)
        {
            recipeBook = new List<Recipe>(recipes);
            recipeBookChanged.Invoke();
        }
    }

}