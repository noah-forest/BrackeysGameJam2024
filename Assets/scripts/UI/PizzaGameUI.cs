using System.Collections;
using System.Collections.Generic;
using Interact;
using TMPro;
using UnityEngine;

public class PizzaGameUI : MonoBehaviour
{
    public RaycastInteractor interactor;
    
    PizzaModeManager modeManager;
    GameManager gameManager;
    public TextMeshProUGUI interactText;
    public GameObject interactPrompt;

    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = PizzaModeManager.singleton;
        
        interactPrompt.SetActive(false);
        
        interactor?.onLook.AddListener(ShowPrompt);
        interactor?.onLookAway.AddListener(HidePrompt);
    }

    private void ShowPrompt(GameObject obj, string prompt)
    {
        interactText.text = prompt;
        interactPrompt.SetActive(true);
    }

    private void HidePrompt(GameObject obj, string prompt)
    {
        interactPrompt.SetActive(false);
    }
}
