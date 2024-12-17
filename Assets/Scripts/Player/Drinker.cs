using Grabbing;
using Interact;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drinker : MonoBehaviour
{
    [SerializeField] Animator armsAnim;
    [SerializeField] Grabber grabber;
    [SerializeField] RaycastInteractor interactor;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sipSound;
    [SerializeField] AudioClip postDrinkSound;

    EdibleIneractable drink;
    bool canEat;


    private void Start()
    {
        interactor.onLook.AddListener(DisableDrinker);
        interactor.onLookAway.AddListener(EnableDrinker);

    }

    void DisableDrinker(GameObject obj, string prompt)
    {
        canEat = false; 
    }
    void EnableDrinker(GameObject obj, string prompt)
    {
        canEat = true;
    }

    private void Update()
    {
        if (canEat && grabber.currentlyGrabbed && Input.GetMouseButtonDown(0))
        {

            drink = grabber.currentlyGrabbed.GetComponent<EdibleIneractable>();
            if (drink && drink.CanEat())
            {
                grabber.canThrow = false;
                grabber.canGrab = false;
                armsAnim.Play("Drank");

            }
        }
    }

    public void AlterEdible()
    {
        if (drink)
        {
            drink.DeductUse();
            interactor.onInteract.Invoke(drink.gameObject);
        }
        grabber.canThrow = true;
        grabber.canGrab = true;
        
    }

    public void PlaySip()
    {
        audioSource.PlayOneShot(sipSound);
    }
    public void PlayPostDrink()
    {
        audioSource.PlayOneShot(postDrinkSound);
    }
}
