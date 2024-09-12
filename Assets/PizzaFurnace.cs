using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Interact;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PizzaFurnace : PizzaModeInteractable
{
    [SerializeField] ParticleSystem cookingParticles;
    [SerializeField] ParticleSystem burningParticles;

    public TextMeshProUGUI timerText;
    
    [Header("audio stuff")]
    public AudioSource dingAudioPlayer;
    public AudioSource cookingAudioPlayer;
    
    public Vector3 pizzaPosition;
    private Pizza currentPizza;

    public int timeToCook = 15;
    public int timeToBurn = 30;
    private float timeInOven = 0;

    private Color originalColor;

    private void Start()
    {
        originalColor = timerText.color;
    }

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
                
                displayText = "[LMB] Grab Pizza";
                gameObject.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
                
                GrabCurrentPizza(grabber);
            }
            currentPizza = pizza;
        }
        else
        {
            GrabCurrentPizza(grabber);
            
            displayText = "[LMB] Cook Pizza";
            gameObject.GetComponent<RaycastInteractor>().onLook.Invoke(gameObject, displayText);
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
                dingAudioPlayer.PlayOneShot(dingAudioPlayer.clip);
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

        if (!(timer < 0.1)) return;
        timerText.text = $"Done!";
        timerText.color = Color.green;
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
        cookingAudioPlayer.Play();
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

            timerText.text = $"00:00";
            timerText.color = originalColor;
            
            displayText = "[LMB] Cook Pizza";
            
            cookingAudioPlayer.Stop();
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
