using System;
using System.Collections;
using System.Collections.Generic;
using Grabbing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRemover : MonoBehaviour
{
    public UnityEvent<GameObject> onObjectRemoved;
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Grabbable>() != null )
        {
            onObjectRemoved.Invoke(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
