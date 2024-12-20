using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bullet : ExplodeOnImpact
{
    [SerializeField] Rigidbody body;
    [SerializeField] float startForce;
    [SerializeField] float duration;

    // Start is called before the first frame update
    void Start()
    {
        body.AddForce(startForce * transform.forward);
        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroyDuration()
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(DestroySelf());
    }
     
}
