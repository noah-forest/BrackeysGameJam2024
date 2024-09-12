using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PizzaGameUI : MonoBehaviour
{
    PizzaModeManager modeManager;
    GameManager gameManager;
    public TextMeshProUGUI interactText;
    public GameObject prompt;

    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = PizzaModeManager.singleton;
    }
}
