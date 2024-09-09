using System;
using Interact;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grabbing
{
    [RequireComponent(typeof(RaycastInteractor))]
    public class Grabber : MonoBehaviour
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
                ThrowCurrentlyGrabbed();
            }
        }

        public void Start()
        {
            GetComponent<RaycastInteractor>()?.onInteract.AddListener((gameObject) =>
            {
                Grabbable grabbable = gameObject.GetComponent<Grabbable>();
                if (grabbable != null)
                {
                    Grab(grabbable);
                }
            });
        }

        public void Grab(Grabbable grabbable)
        {
            if (grabbable.CanGrab(this))
            {
                ReleaseCurrentlyGrabbed();

                currentlyGrabbed = grabbable;
                currentlyGrabbed.transform.SetParent(this.transform);
                currentlyGrabbed.transform.localPosition = grabbableOffset + currentlyGrabbed.offset;
                currentlyGrabbed.transform.localRotation = Quaternion.identity;

                // Disable collider and rigidbody
                Collider collider = currentlyGrabbed.GetComponent<Collider>();
                Rigidbody rigidbody = currentlyGrabbed.GetComponent<Rigidbody>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }

                currentlyGrabbed.onGrab.Invoke();
            }
        }

        public void ReleaseCurrentlyGrabbed()
        {
            if (currentlyGrabbed != null)
            {
                currentlyGrabbed.transform.SetParent(null);

                // Enable collider and rigidbody
                Collider collider = currentlyGrabbed.GetComponent<Collider>();
                Rigidbody rigidbody = currentlyGrabbed.GetComponent<Rigidbody>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = false;
                }

                currentlyGrabbed.onRelease.Invoke();
                currentlyGrabbed = null;
            }
        }

        public void ThrowCurrentlyGrabbed()
        {
            if (GetCurrentlyGrabbed())
            {
                var grabbed = GetCurrentlyGrabbed();
                ReleaseCurrentlyGrabbed();
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
        public Grabbable GetCurrentlyGrabbed()
        {
            return currentlyGrabbed;
        }

        public bool IsGrabbing()
        {
            return currentlyGrabbed != null;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.TransformVector(grabbableOffset), 0.1f);
        }
    }
}