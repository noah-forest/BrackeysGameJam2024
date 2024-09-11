using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wind : MonoBehaviour
{
    public UnityEvent onSucked;
    public Vector3 forceDirection;
    public float force;
    public bool shrink = false;
    public float shrinkSpeed = 1f;

    public void ShrinkObject(Transform obj, float shrinkSpeed)
    {
        obj.localScale = Vector3.Lerp(obj.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);
    }
    
    public void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        // raycast to see if anything is in the way
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.position - transform.position, out hit))
        {
            if (hit.collider.gameObject == rb.gameObject)
            {
                var t = this.transform.position + forceDirection - rb.transform.position;
                rb.AddForce((forceDirection + t).normalized * force, ForceMode.Acceleration);
                
                if (shrink)
                {
                    ShrinkObject(other.transform, shrinkSpeed);
                }
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        onSucked.Invoke();
    }

    public void OnDrawGizmosSelected()
    {
        var transPosition = transform.position;
        var normDir = forceDirection.normalized;
        Gizmos.DrawLine(transform.position, transPosition + normDir);
        Gizmos.DrawCube(transform.position + normDir, Vector3.one * 0.1f);
        Gizmos.DrawCube(transform.position + forceDirection, Vector3.one * 0.1f);
    }
}