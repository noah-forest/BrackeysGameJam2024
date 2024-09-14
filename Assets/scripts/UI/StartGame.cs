using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.singleton;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartGameButton()
    {
        gameManager.LoadPizzaScene();
        gameManager.Day = 0;
    }
}
