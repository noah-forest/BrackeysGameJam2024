using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

	public TextMeshProUGUI wonScoreText;
	public TextMeshProUGUI lostScoreText;

	[SerializeField]AudioClip[] sounds;
	public AudioSource TextChangeSFX;

	public float postScreenDelay = 1.5f;

	public CanvasGroup winCanvas;
	public CanvasGroup lostCanvas;
	
	private GameManager gameManager;
	private FadeInOut fade;

	private void Start()
	{
		wonScreen.SetActive(false);
		lostScreen.SetActive(false);
		postLossText.SetActive(false);
		postWinText.SetActive(false);
		buttons.SetActive(false);

		gameManager = GameManager.singleton;

		Cursor.lockState = CursorLockMode.Confined;

		fade = GetComponent<FadeInOut>();
		
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

	private IEnumerator ShowPostScreen()
	{
        if (gameManager.gameState == GameManager.GameState.victory)
        {
	        fade.canvasGroup = winCanvas;
	        fade.FadeIn();
	        yield return new WaitForSeconds(postScreenDelay);
			// won
			TextChangeSFX.clip = sounds[0];
			TextChangeSFX.Play();
			fade.FadeOut();
			yield return new WaitForSeconds(1f);
			wonText.SetActive(false);
			fade.FadeIn();
            postWinText.SetActive(true);
            wonScoreText.text = $"Score: {gameManager.scoreAllTime}";
        }
        else
        {
            // lost
            fade.canvasGroup = lostCanvas;
            fade.FadeIn();
            yield return new WaitForSeconds(postScreenDelay);
            // won
            TextChangeSFX.clip = sounds[1];
            TextChangeSFX.Play();
            fade.FadeOut();
            yield return new WaitForSeconds(1f);
            lostText.SetActive(false);
            fade.FadeIn();
            postLossText.SetActive(true);
            lostScoreText.text = $"Score: {gameManager.scoreAllTime}";
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
