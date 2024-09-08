using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grabbing
{
    public class Grabber: MonoBehaviour
    {
        [CanBeNull]
        private Grabbable currentlyGrabbed;
        public Vector3 grabbableOffset;
        public float throwForce = 5;
        public float throwUpForce = 5;
        public float throwRotationForce = 0.5f;

        public void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Throw();
            }
        }

        public void Grab(Grabbable grabbable)
        {
            if (currentlyGrabbed != null)
            {
                currentlyGrabbed.transform.SetParent(null);
                currentlyGrabbed.Release();
            }
            currentlyGrabbed = grabbable;
            currentlyGrabbed.transform.SetParent(this.transform);
            currentlyGrabbed.transform.localPosition = grabbableOffset + currentlyGrabbed.offset;
            currentlyGrabbed.transform.localRotation = Quaternion.identity;
            currentlyGrabbed.Grab();
        }

        public void Throw()
        {
            if (GetGrabbed())
            {
                var grabbed = GetGrabbed();
                grabbed.transform.SetParent(null);
                grabbed.Release();
                var rigidbody = grabbed.GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    rigidbody.AddForce(transform.forward * throwForce + Vector3.up * throwUpForce, ForceMode.Impulse);
                    rigidbody.AddTorque(
                        new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * throwRotationForce, ForceMode.Impulse);
                }
                currentlyGrabbed = null;
            }
        }

        public void DestroyGrabbed()
        {
            if (currentlyGrabbed)
            {
                Destroy(currentlyGrabbed.gameObject);
                currentlyGrabbed = null;
            }
        }
        
        [CanBeNull]
        public Grabbable GetGrabbed()
        {
            return currentlyGrabbed;
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.TransformVector(grabbableOffset), 0.1f);
        }
    }
}