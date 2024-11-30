using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        bool completed = false;
        public float score;
        public bool validForScoring = true;

        public Order(string name, Recipe recipe, List<Pizza.Toppings> excludedToppings)
        {
            this.name = name;
            this.recipe = recipe;
            this.excludedToppings = excludedToppings;
            
            toppings.AddRange(recipe.toppings);
            for (int i = 0; i < excludedToppings.Count; i++)
            {
                toppings.Remove(excludedToppings[i]);
            }
        }

        public Order(string name, List<Pizza.Toppings> toppings, List<Pizza.Toppings> excludedToppings)
        {
            this.name = name;
            this.excludedToppings = excludedToppings;
            
            toppings.AddRange(toppings);
            for (int i = 0; i < excludedToppings.Count; i++)
            {
                toppings.Remove(excludedToppings[i]);
            }
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
            float pizzaCookValue = 25;
            // plus 1 for being cooked
            float bestScore = toppings.Count + pizzaCookValue;
            int toppingsRequsted = toppings.Count;
            int toppingsReceived = pizza.GetToppings().Length;
            //Debug.Log($"Order: {name} |  Requested Toppings: {toppingsRequsted}  | Received Toppings {toppingsReceived}");



            score = bestScore;
            for(int i = 0;  i < toppings.Count; i++)
            {
                if (!pizza.HasTopping(toppings[i]))
                {
                    score--;
                    //.Log($"Order: {name} | 1 Score lost for missing topping");
                }
            }
            if(toppingsReceived > toppingsRequsted)
            {
                //Debug.Log($"Order: {name} | {(toppingsReceived - toppingsRequsted) * 0.5f} Score lost for extra toppings ");
                score -= (toppingsReceived - toppingsRequsted) * 0.5f;
            }

            if (!pizza.IsCooked())
            {
                score -= pizzaCookValue;
                //Debug.Log($"Order: {name} | Score lost for raw pizza");
            }
            if (pizza.IsBurned())
            {
                score = 0;
                //Debug.Log($"Order: {name} | Score eliminated for burnt pizza");
            }
            //Debug.Log($"Order: {name} | Score: {score}/{bestScore} ");


            return Math.Max(0.1f,(Mathf.Clamp(Mathf.Ceil(score), 0, bestScore) / bestScore)) * 100;
        }

        public void CompleteOrder(Pizza pizza)
        {
            if (completed)
            {
                return;
            }
            onOrderCompleted.Invoke();
            score = CalculatePizzaScore(pizza);
            completed = true;
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