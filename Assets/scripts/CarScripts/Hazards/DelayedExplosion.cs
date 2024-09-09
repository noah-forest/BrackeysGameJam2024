using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedExplosion : MonoBehaviour
{

    [SerializeField] float fuseTime = 1;
    float timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        timeStamp = Time.time + fuseTime;
    }

    private void Update()
    {
        if (Time.time > timeStamp) 
        {
            
        }
    }

    protected virtual void Explode()
    {

    }
    protected virtual void UpdateVisuals()
    {

    }
}
