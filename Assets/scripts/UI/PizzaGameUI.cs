using System.Collections;
using System.Collections.Generic;
using Grabbing;
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

    public TextMeshProUGUI day;
    public TextMeshProUGUI dayNumber;
    
    private FadeInOut fade;
    
    private void Start()
    {
        gameManager = GameManager.singleton;
        modeManager = PizzaModeManager.singleton;
        
        fade = GetComponent<FadeInOut>();
        
        interactPrompt.SetActive(false);

        if (gameManager.Day == 5)
        {
            day.text = "Final";
            dayNumber.text = $"Day";
        }
        else
        {
            dayNumber.text = $"{gameManager.Day}";
        }
        
        StartCoroutine(ShowDay());
        
        interactor?.onLook.AddListener(ShowPrompt);
        interactor?.onLookAway.AddListener(HidePrompt);

        interactor?.onInput.AddListener(Validate);
        interactor?.onInteract.AddListener((GameObject v) => Validate());
        interactor?.Grabber?.onRelease.AddListener(Validate);
        modeManager.playerMaster.smoker.smokeStatus.AddListener(Validate);
    }


    private void ShowPrompt(GameObject obj, string prompt)
    {
        interactText.text = prompt;
        interactPrompt.SetActive(true);
    }

    private void HidePrompt(GameObject obj, string prompt)
    {
        EdibleIneractable drink = interactor?.Grabber?.currentlyGrabbed?.GetComponent<EdibleIneractable>();
        if (drink)
        {
            if(drink.CanEat())
            {
                interactText.text = "[LMB] Drink";
            }
            else
            {
                interactText.text = "[RMB] Throw";
            }
            StartCoroutine(ValidateDrinker());
        }
        else
        {
            interactPrompt.SetActive(false);

        }
    }

    private void Validate()
    {
        StartCoroutine(ValidateDrinker());
        StartCoroutine(ValidateSmoker());
    }

    public IEnumerator ValidateDrinker()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // wait 2 frames
        EdibleIneractable drink = interactor?.Grabber?.currentlyGrabbed?.GetComponent<EdibleIneractable>();

        if (drink)
        {
            if (!drink.CanEat())
            {
                interactText.text = "[RMB] Throw";
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    public IEnumerator ValidateSmoker()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (interactor?.objUnderCrosshair?.GetComponent<Ashtray>() != null)
        {
            if (modeManager.playerMaster.smoker.active)
            {
                interactPrompt.SetActive(true);
                interactText.text = "[LMB] Snuff Cig";

            }
            else
            {
                interactPrompt.SetActive(false);

            }
        }
    }

    private IEnumerator ShowDay()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1.5f);
        fade.FadeOut();
    }
}
