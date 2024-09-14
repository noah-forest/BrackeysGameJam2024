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
    [SerializeField] MaterialCycler preExplodeFlicker;
    [SerializeField] float flickerInterval;
    float timeStamp;
    bool isFlickering;

    [SerializeField]
    [Range(0,1)]
    float flickerPercent;

    int matFlickerIdx;

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
        if( preExplodeFlicker && (timeStamp-Time.time)/fuseTime < flickerPercent && !isFlickering)
        {
            StartCoroutine(Flicker());
            isFlickering = true;
        }
    }

    IEnumerator Flicker()
    {

        yield return new WaitForSeconds((timeStamp - Time.time) / fuseTime * flickerInterval);
        matFlickerIdx = matFlickerIdx == 0 ? 1 : 0;
        preExplodeFlicker.ChangeMaterial(matFlickerIdx);
        StartCoroutine(Flicker()); // loops infinitley, but object will destroy itself soon.
    }

}
