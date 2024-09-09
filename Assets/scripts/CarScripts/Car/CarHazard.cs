using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class CarHazard : MonoBehaviour, IInteractable
{
    CarController car;

    public abstract bool CanInteract(GameObject interactor);

    public abstract void Interact(GameObject interactor);

    [SerializeField] protected GameObject InteractParticleEffect;
    protected void SpawnInteractEffect()
    {
        if(InteractParticleEffect) Instantiate(InteractParticleEffect, transform.position, transform.rotation);
    }
}
