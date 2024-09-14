using System;
using System.Collections;
using System.Collections.Generic;
using PizzaOrder;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EndOfDayUI : MonoBehaviour
{
    public TextMeshProUGUI currentDay;
    public TextMeshProUGUI daysLeft;
    public TextMeshProUGUI score;
    public TextMeshProUGUI neededScore;
    public TextMeshProUGUI timeScore;
    
    public ScrollRect scrollRect;

    public GameObject exitButton;
    public RectTransform content;
    public GameObject completedOrder;

    public AudioSource audioPlayer;
    
    private int daysLeftCount;
    
    private GameManager gameManager;

    private CarModeManager modeManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = CarModeManager.singleton;
        
        if (!gameManager) return;
        if (!modeManager) return;
        Cursor.lockState = CursorLockMode.Confined;
        
        currentDay.text = $"{gameManager.Day}";
        daysLeft.text = $"{gameManager.daysNeededToWin -  gameManager.Day}";
        
        scrollRect.onValueChanged.AddListener(CheckIfScrolledToEnd);
        
        //create the UI objects
        SetUpScrollRect();
        SetUpScoreText();
    }

    private void SetUpScoreText()
    {
        score.text = $"{gameManager.scoreToday}";
        neededScore.text = $"{gameManager.scoreRequiredToPass[Mathf.Clamp(gameManager.Day - 1, 0, gameManager.daysNeededToWin)]}";
        timeScore.text = $"{gameManager.scoreTime}";
    }
    
    private void SetUpScrollRect()
    {
        var counter = 0.2f;
        foreach (var order in gameManager.OrdersToDeliver)
        {
            StartCoroutine(addOrderSlowly(order, counter));
            ++counter;
        }
    }

    private IEnumerator addOrderSlowly(Order order, float counter)
    {
        yield return new WaitForSeconds(counter);
        audioPlayer.PlayOneShot(audioPlayer.clip);
        var orderUI = Instantiate(completedOrder, content);
        var uiInfo = orderUI.GetComponent<CompletedOrderUI>();

        if (!order.validForScoring)
        {
            uiInfo.orderName.text = $"{order.name} (Lost)";
            uiInfo.orderName.color = new Color32(250, 143, 143, 255);
        }
        else
        {
            uiInfo.orderName.text = order.name;
        }
            
        uiInfo.orderScore.text = $"{Mathf.Floor(order.score)}";
        if (!order.validForScoring)
        {
            uiInfo.orderScore.text = $"<s>{Mathf.Floor(order.score)}</s>";
            uiInfo.orderScore.color = new Color32(255, 0, 0, 255);
        }
        else
            uiInfo.orderScore.color = order.score switch
            {
                0 => new Color32(255, 70, 70, 255),
                < 50 => new Color32(250, 143, 143, 255),
                <= 75 => new Color32(255, 255, 143, 255),
                _ => new Color32(140, 255, 140, 255)
            };

        uiInfo.thumbsUp.SetActive(order.score >= 100 && order.validForScoring);
    }
    
    public void StartPizza()
    {
        gameManager.PostCarGame();
    }
    
    private void CheckIfScrolledToEnd(Vector2 vector)
    {
        if (scrollRect.verticalNormalizedPosition >= 0.05f) // scrolled to beginning
        {
            exitButton.SetActive(true);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(CurrentDayAnimation());
    }

    private IEnumerator CurrentDayAnimation()
    {
        yield return new WaitForSeconds(0.7f);
        currentDay.gameObject.SetActive(true);
        currentDay.GetComponent<Animator>().SetTrigger("pop");
    }
}
