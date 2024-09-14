using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
	public GameObject wonScreen;
	public GameObject lostScreen;
	
	private GameManager gameManager;

	private void Start()
	{
		wonScreen.SetActive(false);
		lostScreen.SetActive(false);
		
		gameManager = GameManager.singleton;

		Cursor.lockState = CursorLockMode.Confined;
		
		if (gameManager.gameState == GameManager.GameState.victory)
		{
			// won
			wonScreen.SetActive(true);
		}
		else
		{
			// lost
			lostScreen.SetActive(true);
		}
	}

	public void BackToMainMenu()
	{
		gameManager.LoadMenuScene();
	}
}
