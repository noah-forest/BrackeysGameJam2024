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
            if (hit.collider.gameObject == rb.gameObject) {
                rb.AddForce((forceDirection).normalized * force, ForceMode.Acceleration);
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
