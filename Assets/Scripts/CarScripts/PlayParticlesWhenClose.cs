using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticlesWhenClose : MonoBehaviour
{
    [SerializeField] float requiredProximty;
    [SerializeField] ParticleSystem particle;
    CarMaster target;

    private void Start()
    {
        target = CarMaster.singleton;
    }

    private void Update()
    {
        if( Vector3.Distance(transform.position,target.transform.position) <= requiredProximty && !particle.isPlaying)
        {
            particle.Play();
        }
        else if(particle.isPlaying) 
        {
            particle.Stop();
        }
    }
}
