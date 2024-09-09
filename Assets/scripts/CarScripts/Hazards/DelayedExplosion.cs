using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedExplosion : ExplodeOnImpact
{
    [SerializeField] SphereCollider explosionCollider;
    [SerializeField] float fuseTime = 1;
    [SerializeField] GameObject telegraphEffectPrefab;
    float timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time + fuseTime;
        telegraphEffectPrefab.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (Time.time > timeStamp) 
        {
            Explode();
        }
        else
        {
            UpdateVisuals();
        }
    }

    protected virtual void Explode()
    {
        explosionCollider.enabled = true;
        StartCoroutine(DestroySelf());
    }
    protected virtual void UpdateVisuals()
    {
        telegraphEffectPrefab.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Time.time / timeStamp);
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForFixedUpdate();
        SpawnInteractEffect();
        Destroy(gameObject);
    }
}
