using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarGameUI : MonoBehaviour
{
    public TextMeshProUGUI pizzaCountText;
    public TextMeshProUGUI timerText;
    public GameObject pizzaIcon;
    public GameObject textCont;
    public GameObject returnIcon;
    public GameObject returnText;
    public GameObject timer;

	private AudioSource sfxPlayer;

    private bool timerIsRunning = false;
    
    private GameManager gameManager;
    private CarModeManager modeManager;
    private FadeInOut fade;
    
    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = CarModeManager.singleton;

		sfxPlayer = GetComponent<AudioSource>();
		fade = GetComponent<FadeInOut>();

        timerIsRunning = true;
        
        if (modeManager)
        {
            modeManager.deliveryMade.AddListener(ShowDeliveryUI);
            modeManager.pizzasChanged.AddListener(UpdatePizzaCount);
            UpdatePizzaCount(gameManager.carManager.PizzasToDeliver);
        }
    }

    private void Update()
    {
        if (modeManager.timeToMakeDelivery > 0.1f)
        {
            DisplayTime(modeManager.timeToMakeDelivery);
        }
        else
        {
            DisplayTime(0);
        }
    }

    private void DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        var milliseconds = Mathf.FloorToInt(time % 1f * 100);

		timerText.color = seconds switch
        {
            < 5 => new Color32(255, 143, 143, 255),
            < 10 => new Color32(255, 255, 143, 255),
            _ => Color.white
        };


        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    private void ShowDeliveryUI()
    {
        StartCoroutine(FadeDeliveryText());
    }
    
    private IEnumerator FadeDeliveryText()
    {
        fade.FadeIn();
		yield return new WaitForSeconds(0.2f);
		sfxPlayer.Play();
		yield return new WaitForSeconds(1.3f);
        fade.FadeOut();
    }

    private void UpdatePizzaCount(uint count)
    {
        if (count == 0)
        {
            textCont.SetActive(false);
            pizzaIcon.SetActive(false);
            timer.SetActive(false);
            
            returnIcon.SetActive(true);
            returnText.SetActive(true);
            
            var returnAnim = returnIcon.GetComponent<Animator>();
            returnAnim.SetTrigger("return");

            return;
        }
        
        pizzaCountText.text = $"{count}";
        
        var anim = pizzaCountText.GetComponent<Animator>();
        anim.SetTrigger("pop");
        
        var iconAnim = pizzaIcon.GetComponent<Animator>();
        iconAnim.SetTrigger("shake");
    }
}
