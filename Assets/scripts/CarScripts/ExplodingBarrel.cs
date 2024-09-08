using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : CarHazard
{
    [SerializeField] float explosiveForce;
    [SerializeField] float explosionRadius;
    public override bool CanInteract(GameObject interactor)
    {
        return true;
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log("Barrel Interact");
        CarMaster car = interactor.GetComponent<CarMaster>();
        car?.body?.AddExplosionForce(explosiveForce, transform.position, explosionRadius);
        car?.health.TakeDamage();
        SpawnInteractEffect();
        Destroy(gameObject);
    }
}
