using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using TMPro;
using UnityEngine;

public class PizzaFurnace : PizzaModeInteractable
{
    [SerializeField] ParticleSystem cookingParticles;
    [SerializeField] ParticleSystem burningParticles;

    public TextMeshProUGUI timerText;
    
    public Vector3 pizzaPosition;
    private Pizza currentPizza;

    public int timeToCook = 15;
    public int timeToBurn = 30;
    private float timeInOven = 0;

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);

        Grabber grabber = gameObject.GetComponent<Grabber>();

        if (grabber.IsGrabbing())
        {
            Pizza pizza = grabber.GetCurrentlyGrabbed().gameObject.GetComponent<Pizza>();
            if (pizza != null)
            {
                grabber.ReleaseCurrentlyGrabbed();
                PlacePizza(pizza.gameObject);
                GrabCurrentPizza(grabber);
            }
            currentPizza = pizza;
        }
        else
        {
            GrabCurrentPizza(grabber);
        }
    }

    void Update()
    {
        if (currentPizza)
        {
            timeInOven += 1 * Time.deltaTime;
            
            SetUpTimerText(Mathf.Ceil(timeToCook - timeInOven));
            
            if (timeInOven >= timeToBurn)
            {
                currentPizza.Burn();
                if (!burningParticles.isPlaying)
                {
                    burningParticles.Play();
                }
            }
            else if (timeInOven >= timeToCook && !currentPizza.IsCooked())
            {
                currentPizza.Cook();
            }
        }
    }

    private void SetUpTimerText(float timer)
    {
        var anim = timerText.GetComponent<Animator>();
        
        timerText.text = $"00:{timer}";
        anim.SetFloat("timer", timer);
        
        if (timer < 10)
        {
            timerText.text = $"00:0{timer}";
        }

        if (!(timer < 0)) return;
        timerText.text = $"00:00";
        anim.SetFloat("timer", timer);
    }

    void PlacePizza(GameObject pizza)
    {
        timeInOven = 0;
        pizza.transform.position = transform.position + pizzaPosition;
        pizza.transform.rotation = Quaternion.identity;
        pizza.GetComponent<Rigidbody>().velocity = Vector3.zero;
        pizza.GetComponent<Rigidbody>().isKinematic = true;
        cookingParticles.Play();
    }

    void GrabCurrentPizza(Grabber grabber)
    {

        if (currentPizza)
        {
            currentPizza.GetComponent<Rigidbody>().isKinematic = false;
            grabber.Grab(currentPizza.GetComponent<Grabbable>());
            currentPizza = null;
            
            var anim = timerText.GetComponent<Animator>();
            anim.SetFloat("timer", 0.02f);
            
            cookingParticles.Stop();
            burningParticles.Stop();
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + pizzaPosition, 0.1f);
    }
}
