using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PizzaOrder
{
    public static class OrderManager
    {
        public static UnityEvent<Order> onOrderCreated = new UnityEvent<Order>();
        public static UnityEvent<Order> onOrderRemoved = new UnityEvent<Order>();
        public static UnityEvent<Order> onOrderCompleted = new UnityEvent<Order>();
        public static UnityEvent ordersChanged = new UnityEvent();
        public static UnityEvent recipeBookChanged = new UnityEvent();
        public static List<Recipe> recipeBook = new List<Recipe>();
        public static List<Order> orders = new List<Order>();
        public static List<Order> completedOrders = new List<Order>();

        public static void AddOrder(Order order)
        {
            orders.Add(order);
            onOrderCreated.Invoke(order);

            UnityAction listener = null;
            listener = () =>
            {
                completedOrders.Add(order);
                onOrderCompleted.Invoke(order);
                order.onOrderCompleted.RemoveListener(listener);
            };
            order.onOrderCompleted.AddListener(listener);

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
        
        public static uint GetNumberOfCompletedOrders()
        {
            return (uint) completedOrders.Count;
        }
        
        public static List<Order> GetCompletedOrders()
        {
            return completedOrders; 
        }

        public static void ClearCompletedOrders()
        {
            completedOrders.Clear();
        }

        public static Order CreateRandomOrder()
        {
            Recipe randomRecipe = recipeBook[Random.Range(0, recipeBook.Count)];
            List<Pizza.Toppings> excludedToppings = new List<Pizza.Toppings>();
            var toppingsToPut = new List<Pizza.Toppings>(randomRecipe.toppings);
            foreach (Pizza.Toppings topping in randomRecipe.toppings)
            {
                if (Random.Range(0, 100) < GameManager.singleton.Day * 3.5)
                {
                    excludedToppings.Add(topping);
                }
                else
                {
                    toppingsToPut.Add(topping);
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