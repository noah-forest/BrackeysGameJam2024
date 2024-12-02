using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    [SerializeField] float explodeSpeed;
    
    void Explode()
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > explodeSpeed)
        {
            Explode();
        }
    }
}
