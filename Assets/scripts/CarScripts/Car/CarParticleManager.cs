using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleManager : MonoBehaviour
{

    [SerializeField] GameObject[] turboVisuals;
    [SerializeField] ParticleSystem hurtParticles;
    [SerializeField] CarMaster car;
    [SerializeField] int lastPizzaCount;
    [SerializeField] AudioSource carHorn;
    [SerializeField] AudioSource carHurt;
    [SerializeField] AudioClip[] hurtSounds;

    float startingPitch;

    private void Start()
    {
        lastPizzaCount = (int)car.modeManager.PizzasToDeliver - 1;
        car.health.carDamaged.AddListener(PlayPizzaParticles);
        car.modeManager.deliveryMade.AddListener(PlayHorn);
        startingPitch = carHurt.pitch;

    }

    private void Update()
    {
        turboVisuals[0].SetActive(car.controller.FootOnTurbo && car.controller.VerticalInput > 0);
        turboVisuals[1].SetActive(car.controller.FootOnTurbo && car.controller.VerticalInput < 0);
        
    }


    public void PlayPizzaParticles()
    {
       // Debug.Log("[PIZZA PARTICLE]"+lastPizzaCount);
        if (lastPizzaCount > 0)
        {
            hurtParticles.Play();
        }
        else
        {
            hurtParticles.Stop();
        }
        lastPizzaCount--;
        carHurt.pitch = startingPitch + Random.Range(-0.05f, 0.05f);
        carHurt.PlayOneShot(hurtSounds[Random.Range(0,hurtSounds.Length)]);
    }

    public void PlayHorn()
    {
        carHorn.Play();
    }
}
