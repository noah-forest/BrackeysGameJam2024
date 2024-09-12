using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeCigarette : MonoBehaviour
{

    [SerializeField] float cigSmokeDelayAfterPuff;

    public GameObject leftArm;
    public Animator armsAnim;

    public Cigarette cig;
    
    private AudioSource audioSource;
    public AudioClip inhaleSFX;
    public AudioClip exhaleSFX;

    public ParticleSystem exhaleParticles;
    
    private float animCooldown = 1.5f;
    private float animTime = 2f;
    private float lastButtonTime = 0f;

    private bool canPlay;
    public bool active;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        leftArm.SetActive(false);
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) && !active)
        { 
            if (Time.time >= lastButtonTime)
            {
                //add the current time to the cooldown
                lastButtonTime = Time.time + animTime;

                leftArm.SetActive(true);
                StartCoroutine(BringUp());
            }
        }
        
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) && active && cig.smokeable)
        {
            if (Time.time >= lastButtonTime)
            {
                //add the current time to the cooldown
                lastButtonTime = Time.time + animCooldown;

                StartCoroutine(Inhale());

                //var main = cig.smoke.main;
                //main.simulationSpace = ParticleSystemSimulationSpace.World;
                var emit = cig.smokeParticles.emission;
                emit.rateOverTime = 0;
                ParticleSystem.TrailModule trail = cig.smokeParticles.trails;
                trail.attachRibbonsToTransform = false;
                cig.emberParticles.Play();
            }
        }

        if (!cig.smokeable && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            active = false;
            cig.InitializeCig();
        }
        
        if (canPlay || canPlay && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            armsAnim.Play("Exhale");
            exhaleParticles.Play();
            audioSource.PlayOneShot(exhaleSFX);
            StartCoroutine(StartCigParticles());
            canPlay = false;
        }
    }

    IEnumerator Inhale()
    {
        audioSource.PlayOneShot(inhaleSFX);
        armsAnim.Play("Inhale");
        yield return new WaitForSeconds(animTime);
        cig.Smoke();
        canPlay = true;
    }
    
    
    IEnumerator BringUp()
    {
        armsAnim.Play("bringUp");
        yield return new WaitForSeconds(animTime);
        active = true;
    }

    IEnumerator StartCigParticles()
    {
        yield return new WaitForSeconds(cigSmokeDelayAfterPuff);
        //var main = cig.smoke.main;
        //main.simulationSpace = ParticleSystemSimulationSpace.Local;
        var emit = cig.smokeParticles.emission;
        emit.rateOverTime = 2;
        ParticleSystem.TrailModule trail = cig.smokeParticles.trails;
        trail.attachRibbonsToTransform = true;
        cig.emberParticles.Stop();
    }
}
