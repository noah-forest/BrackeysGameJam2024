using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EndOfDayUI : MonoBehaviour
{
    public TextMeshProUGUI currentDay;
    public TextMeshProUGUI daysLeft;

    public ScrollRect scrollRect;

    public GameObject exitButton;
    public RectTransform content;
    public GameObject completedOrder;
    
    private int daysLeftCount;
    
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        
        if (!gameManager) return;
        Cursor.lockState = CursorLockMode.Confined;
        
        currentDay.text = $"{gameManager.Day}";
        
        scrollRect.onValueChanged.AddListener(CheckIfScrolledToEnd);
        
        //create the UI objects
        foreach (var order in gameManager.OrdersToDeliver)
        {
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
    }

    public void StartPizza()
    {
        gameManager.LoadPizzaScene();
    }
    
    private void CheckIfScrolledToEnd(Vector2 vector)
    {
        if (scrollRect.verticalNormalizedPosition >= 0.95f) // scrolled to end
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
