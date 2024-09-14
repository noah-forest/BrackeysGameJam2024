using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
	public GameObject wonScreen;
	public GameObject lostScreen;

	public GameObject wonText;
	public GameObject postWinText;
    public GameObject lostText;
    public GameObject postLossText;
	public GameObject buttons;

	[SerializeField]AudioClip[] sounds;
	public AudioSource TextChangeSFX;

	public float postScreenDelay = 1.5f;
	
	private GameManager gameManager;

	private void Start()
	{
		wonScreen.SetActive(false);
		lostScreen.SetActive(false);
		postLossText.SetActive(false);
		postWinText.SetActive(false);
		buttons.SetActive(false);

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
		StartCoroutine(ShowPostScreen());
	}

	IEnumerator ShowPostScreen()
	{
		yield return new WaitForSeconds(postScreenDelay);
        if (gameManager.gameState == GameManager.GameState.victory)
        {
			// won
			TextChangeSFX.clip = sounds[0];
			TextChangeSFX.Play();
			wonText.SetActive(false);
            postWinText.SetActive(true);
        }
        else
        {
            // lost
            TextChangeSFX.clip = sounds[1];
            TextChangeSFX.Play();
            lostText.SetActive(false);
            postLossText.SetActive(true);
        }
		StartCoroutine(ShowButtons());
    }

	IEnumerator ShowButtons()
	{
		yield return new WaitForSeconds(postScreenDelay/2);
        TextChangeSFX.clip = sounds[2];
        buttons.SetActive(true);
	}

	public void BackToMainMenu()
	{
		gameManager.LoadMenuScene();
	}

}
