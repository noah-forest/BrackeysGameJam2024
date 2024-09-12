using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : CarHazard
{
    [SerializeField] protected float explosiveForce;
    [SerializeField] protected float explosionRadius;
    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact(GameObject interactor)
    {
        // Debug.Log("Barrel Interact");
        CarMaster car = interactor.GetComponent<CarMaster>();
        car?.body?.AddExplosionForce(explosiveForce, transform.position, explosionRadius);
        car?.health.TakeDamage();
        SpawnInteractEffect();
        Destroy(gameObject);
    }
    protected IEnumerator DestroySelf()
    {
        yield return new WaitForFixedUpdate();
        SpawnInteractEffect();
        Destroy(gameObject);
    }
}
