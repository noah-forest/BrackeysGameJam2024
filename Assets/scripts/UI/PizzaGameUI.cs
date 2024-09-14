using System.Collections;
using System.Collections.Generic;
using Interact;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PizzaGameUI : MonoBehaviour
{
    public RaycastInteractor interactor;
    
    PizzaModeManager modeManager;
    GameManager gameManager;
    public TextMeshProUGUI interactText;
    public GameObject interactPrompt;

    public TextMeshProUGUI dayNumber;
    
    private FadeInOut fade;
    
    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = PizzaModeManager.singleton;
        
        fade = GetComponent<FadeInOut>();
        
        interactPrompt.SetActive(false);

        dayNumber.text = $"{gameManager.Day}";
        StartCoroutine(ShowDay());
        
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

    private IEnumerator ShowDay()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1.5f);
        fade.FadeOut();
    }
}
