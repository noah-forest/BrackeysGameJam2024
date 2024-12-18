using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Wind : MonoBehaviour
{
    public UnityEvent onSucked;
    public float force;
    public bool shrink = false;
    public float shrinkSpeed = 1f;
    public float minimumScale = 0.1f;
    public bool shake = false;
    public float shakingForce;
    Dictionary<Transform, Vector3> objectOriginalScale = new Dictionary<Transform, Vector3>();

    public void ShrinkObject(Transform obj, float shrinkSpeed)
    {
        if (!objectOriginalScale.ContainsKey(obj))
        {
            objectOriginalScale.Add(obj, obj.localScale);
        }
        obj.localScale = Vector3.Lerp(obj.localScale, Vector3.one * minimumScale, Time.deltaTime * shrinkSpeed);
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
                rb.AddForce(transform.forward * force, ForceMode.Acceleration);
                
                if (shrink)
                {
                    ShrinkObject(other.transform, shrinkSpeed);
                }
                if (shake)
                {
                    ShakeObject(hit.collider.gameObject.GetComponent<BoxCollider>(), rb, shakingForce);
                }
            }
        }
    }

    void ShakeObject(BoxCollider mainCollider, Rigidbody body, float shakeForce )
    {
        if (!mainCollider) return;
        float xPos = Random.Range(mainCollider.bounds.min.x, mainCollider.bounds.max.x);
        float yPos = Random.Range(mainCollider.bounds.min.y, mainCollider.bounds.max.y);
        float zPos = Random.Range(mainCollider.bounds.min.z, mainCollider.bounds.max.z);
        Vector3 forceAxis = Vector3.zero;
        switch (Random.Range(0, 2))
        {
            case 0:
                forceAxis = Vector3.forward;
                break;
            case 1:
                forceAxis = Vector3.right;
                break;
            case 2:
                forceAxis = Vector3.up;
                break;
        }
        forceAxis *= (Random.value > 0.5 ? 1 : -1);
        body.AddForceAtPosition(shakeForce * forceAxis, new Vector3(xPos, yPos, zPos));
    }
    
    public void OnTriggerEnter(Collider other)
    {
        onSucked.Invoke();
    }
    public void OnTriggerExit(Collider other)
    {
        if (shrink && objectOriginalScale.ContainsKey(other.transform))
        {
            other.transform.localScale = objectOriginalScale[other.transform];
        }
    }

    public void OnDrawGizmosSelected()
    {
        var transPosition = transform.position;
        var normDir = transform.forward;
        Gizmos.DrawLine(transform.position, transPosition + normDir);
        Gizmos.DrawCube(transform.position + normDir, Vector3.one * 0.1f);
        Gizmos.DrawCube(transform.position + transform.forward * force, Vector3.one * 0.1f);
    }
}
