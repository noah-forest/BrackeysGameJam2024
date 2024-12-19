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
	[SerializeField] ParticleSystem smokeParticles;

    public TextMeshProUGUI timerText;
    
    [Header("audio stuff")]
    public AudioSource dingAudioPlayer;
    public AudioSource cookingAudioPlayer;
	public AudioSource burningAudioPlayer;
    
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
    public override bool CanInteract(GameObject interactor)
    {
        Grabber grabber = interactor.GetComponent<Grabber>();
        if (grabber.IsGrabbing())
        {
            Pizza pizza = grabber.GetCurrentlyGrabbed().gameObject.GetComponent<Pizza>();
            return pizza || currentPizza;
        }
        else
        {
            return currentPizza;
        }
    }
    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);

        Grabber grabber = interactor.GetComponent<Grabber>();
        if (!grabber.canGrab) return;
        if (grabber.IsGrabbing())
        {
            Pizza pizza = grabber.GetCurrentlyGrabbed().gameObject.GetComponent<Pizza>();
            if (pizza != null)
            {
                grabber.ReleaseCurrentlyGrabbed();
                PlacePizza(pizza.gameObject);
                
                displayText = "[LMB] Grab Pizza";
                interactor.GetComponent<RaycastInteractor>().onLook.Invoke(interactor, displayText);
                
                GrabCurrentPizza(grabber);
                timeInOven = pizza.GetTimeCooked();
                cookingAudioPlayer.Play();
                cookingParticles.Play();
                currentPizza = pizza;
            }
            else
            {
                grabber.ReleaseCurrentlyGrabbed();
                GrabCurrentPizza(grabber);
            }

        }
        else
        {
            GrabCurrentPizza(grabber);
            
            displayText = "[LMB] Cook Pizza";
            interactor.GetComponent<RaycastInteractor>().onLook.Invoke(interactor, displayText);
        }
    }

    void Update()
    {
        if (currentPizza)
        {
            timeInOven += 1 * Time.deltaTime;
            currentPizza.SetTimeCooked(timeInOven);
            
            SetUpTimerText(Mathf.Ceil(timeToCook - timeInOven));
            
            if (timeInOven >= timeToBurn)
            {
                currentPizza.Burn();
                if (!burningParticles.isPlaying)
                {
                    burningParticles.Play();
					burningAudioPlayer.Play();
					smokeParticles.Play();
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
			burningAudioPlayer.Stop();
			smokeParticles.Stop();
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + pizzaPosition, 0.1f);
    }
}
