using Grabbing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drinker : MonoBehaviour
{
    [SerializeField] Animator armsAnim;
    [SerializeField] Grabber grabber;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sipSound;
    [SerializeField] AudioClip postDrinkSound;

    EdibleIneractable drink;

    private void Update()
    {
        if (grabber.currentlyGrabbed && Input.GetMouseButtonDown(0))
        {

            drink = grabber.currentlyGrabbed.GetComponent<EdibleIneractable>();
            if (drink && drink.CanEat())
            {
                armsAnim.Play("Drank");

            }
        }
    }

    public void AlterEdible()
    {
       if(drink) drink.DeductUse();
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
