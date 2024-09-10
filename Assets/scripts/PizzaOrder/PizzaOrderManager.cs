using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PizzaOrder {
    public class OrderManager : MonoBehaviour
    {
        public static bool ready = false;
        public static UnityEvent onReady = new UnityEvent();
        public static OrderManager instance;
        
        public UnityEvent<Order> onOrderCreated;
        public UnityEvent<Order> onOrderRemoved;
        public List<Recipe> recipeBook = new List<Recipe>();
        public List<Order> orders = new List<Order>();
        
        void Awake()
        {
            instance = this;
            ready = true;
            onReady.Invoke();
            
            CreateRandomOrder();
            CreateRandomOrder();
            CreateRandomOrder();
            CreateRandomOrder();
        }
        
        public void AddOrder(Order order)
        {
            orders.Add(order);
            onOrderCreated.Invoke(order);
        }
        
        public void RemoveOrder(Order order)
        {
            orders.Remove(order);
            onOrderRemoved.Invoke(order);
        }
        
        public void CreateRandomOrder()
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
        }
    }

}