using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grabbing
{
    public class Grabbable : MonoBehaviour
    {
        public Vector3 offset;
        private Rigidbody rb;
        private Collider collider;

        private void Start()
        {
            collider = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }

        public void Grab()
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        public void Release()
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

}