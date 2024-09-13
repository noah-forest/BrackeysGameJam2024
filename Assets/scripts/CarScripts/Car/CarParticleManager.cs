using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleManager : MonoBehaviour
{

    [SerializeField] GameObject[] turboVisuals;
    [SerializeField] ParticleSystem hurtParticles;
    [SerializeField] CarMaster car;
    [SerializeField] int lastPizzaCount;

    private void Start()
    {
        lastPizzaCount = (int)car.modeManager.PizzasToDeliver - 1;
        car.health.carDamaged.AddListener(PlayPizzaParticles);

    }

    private void Update()
    {
        turboVisuals[0].SetActive(car.controller.FootOnTurbo && car.controller.VerticalInput > 0);
        turboVisuals[1].SetActive(car.controller.FootOnTurbo && car.controller.VerticalInput < 0);
        
    }


    public void PlayPizzaParticles()
    {
        Debug.Log("[PIZZA PARTICLE]"+lastPizzaCount);
        if (lastPizzaCount > 0)
        {
            hurtParticles.Play();
        }
        lastPizzaCount--;
    }
}
