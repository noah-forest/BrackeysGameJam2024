using Grabbing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drinker : MonoBehaviour
{
    [SerializeField] Animator armsAnim;
    [SerializeField] Grabber grabber;
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
        drink.DeductUse();
    }
}
