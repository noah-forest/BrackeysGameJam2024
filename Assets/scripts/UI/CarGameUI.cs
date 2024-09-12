using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarGameUI : MonoBehaviour
{
    public TextMeshProUGUI pizzaCountText;
    public GameObject pizzaIcon;
    
    private GameManager gameManager;
    private CarModeManager modeManager;

    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = CarModeManager.singleton;
        if (modeManager)
        {
            modeManager.pizzasChanged.AddListener(UpdatePizzaCount);
            UpdatePizzaCount(gameManager.carManager.PizzasToDeliver);
        }
    }

    private void UpdatePizzaCount(uint count)
    {
        pizzaCountText.text = $"{count}";
        
        var anim = pizzaCountText.GetComponent<Animator>();
        anim.SetTrigger("pop");
        
        var iconAnim = pizzaIcon.GetComponent<Animator>();
        iconAnim.SetTrigger("shake");
    }
}
