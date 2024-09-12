using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PizzaOrder
{
    [Serializable]
    public class Order
    {
        public UnityEvent onOrderCompleted = new UnityEvent();
        public string name;
        public Recipe recipe;
        public List<Pizza.Toppings> excludedToppings;
        public List<Pizza.Toppings> toppings = new List<Pizza.Toppings>();
        public bool completed = false;
        public float score;

        public Order(string name, Recipe recipe, List<Pizza.Toppings> excludedToppings)
        {
            this.name = name;
            this.recipe = recipe;
            this.excludedToppings = excludedToppings;
            
            toppings.AddRange(recipe.toppings);
        }

        public string GetOrderString()
        {
            var excludedStr = "";
            foreach (var excluded in this.excludedToppings)
            {
                excludedStr += excluded.ToString() + ", ";
            }
            excludedStr = excludedStr.TrimEnd(',', ' ');
            var orderText = this.name;
        
            if (excludedStr.Length > 0)
            {
                orderText += "\n(no " + excludedStr + ")";
            }

            return orderText;
        }

        public float CalculatePizzaScore(Pizza pizza)
        {
            // plus 1 for being cooked
            float bestScore = excludedToppings.Count + toppings.Count + 1;
            
            completed = true;
            score = 0;
            foreach (var topping in pizza.GetToppings())
            {
                if (toppings.Contains(topping))
                {
                    score += 1;
                }
                else
                {
                    score -= 1;
                }
            }
            foreach (var excluded in excludedToppings)
            {
                if (recipe.toppings.Contains(excluded))
                {
                    score -= 1;
                }
            }

            if (pizza.IsBurned() || !pizza.IsCooked())
            {
                score -= 1;
            }
            
            return Mathf.Clamp(score, 0, bestScore);
        }

        public void CompleteOrder(Pizza pizza)
        {
            if (completed)
            {
                return;
            }
            onOrderCompleted.Invoke();
            score = CalculatePizzaScore(pizza);
        }
        
        public bool IsCompleted()
        {
            return completed;
        }
        
        public float GetScore()
        {
            return score;
        }
    }
}