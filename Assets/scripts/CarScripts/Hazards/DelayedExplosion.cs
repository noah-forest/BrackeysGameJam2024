using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedExplosion : ExplodeOnImpact
{
    [SerializeField] SphereCollider explosionCollider;
    [SerializeField] float fuseTime = 1;
    [SerializeField] GameObject telegraphEffectPrefab;
    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;
    float timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time + fuseTime;
        telegraphEffectPrefab.transform.localScale = startScale;
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
        telegraphEffectPrefab.transform.localScale = Vector3.Lerp(telegraphEffectPrefab.transform.localScale, endScale, Time.deltaTime);
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForFixedUpdate();
        SpawnInteractEffect();
        Destroy(gameObject);
    }
}
