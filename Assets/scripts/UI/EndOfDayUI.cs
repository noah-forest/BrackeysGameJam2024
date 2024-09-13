using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        foreach (var order in gameManager.AllOrders)
        {
            var orderUI = Instantiate(completedOrder, content);
            var uiInfo = orderUI.GetComponent<CompletedOrderUI>();

            uiInfo.orderName.text = order.name;
            uiInfo.orderScore.text = $"{order.score}";
            uiInfo.thumbsUp.SetActive(order.score >= 100);
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
