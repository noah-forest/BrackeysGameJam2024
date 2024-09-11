﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PizzaOrder
{
    [Serializable]
    public class Order
    {
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