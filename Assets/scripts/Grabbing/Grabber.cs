﻿using System;
using Interact;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Grabbing
{
    [RequireComponent(typeof(RaycastInteractor))]
    public class Grabber : MonoBehaviour
    {
        [CanBeNull]
        public Grabbable currentlyGrabbed;
        public Vector3 grabbableOffset;
        public float throwForce = 5;
        public float throwUpForce = 5;
        public float throwRotationForce = 0.5f;
        public Transform targetTransform;
        public int defaultLayer = 0;
        public int grabbedLayer = 1;
        public UnityEvent onRelease = new();
        public bool canThrow = true;
        public bool canGrab = true;

        public void Update()
        {
            if (Input.GetMouseButtonDown(1) && canThrow)
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
            if (grabbable.CanGrab(this) && canGrab)
            {
                ReleaseCurrentlyGrabbed();

                currentlyGrabbed = grabbable;
                currentlyGrabbed.transform.SetParent(targetTransform);
                currentlyGrabbed.transform.rotation = targetTransform.rotation;
                currentlyGrabbed.transform.position = targetTransform.position;
                currentlyGrabbed.transform.localPosition = grabbable.offset;
                currentlyGrabbed.transform.localRotation = Quaternion.Euler(grabbable.rotationOffset);

                RecursiveSetLayer(currentlyGrabbed.gameObject, grabbedLayer);

                //currentlyGrabbed.transform.localScale = grabbable.scaleOffset;


                // Disable collider and rigidbody
                foreach (Collider c in currentlyGrabbed.GetComponents<Collider>())
                {
                    
                    c.enabled = false;
                }
                
                Rigidbody rigidbody = currentlyGrabbed.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }

                currentlyGrabbed.onGrab.Invoke();
                currentlyGrabbed.isGrabbed = true;

                if (currentlyGrabbed.GetComponent<PizzaBox>())
                {
                    PizzaModeManager.singleton.chuteLight.SetActive(true);
                }
                else
                {
                    PizzaModeManager.singleton.chuteLight.SetActive(false);
                }
            }
        }


        void RecursiveSetLayer(GameObject obj, int layer)
        {
            obj.layer = layer;
            if(obj.transform.childCount > 0)
            {
                foreach(Transform child in obj.transform)
                {
                    RecursiveSetLayer(child.gameObject, layer);
                }
            }
        }

        public void ReleaseCurrentlyGrabbed()
        {
            if (currentlyGrabbed != null)
            {
                currentlyGrabbed.transform.SetParent(null);
                RecursiveSetLayer(currentlyGrabbed.gameObject, defaultLayer);



                // Enable collider and rigidbody
                Rigidbody rigidbody = currentlyGrabbed.GetComponent<Rigidbody>();
                foreach (Collider c in currentlyGrabbed.GetComponents<Collider>())
                {
                    c.enabled = true;
                }
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = false;
                }

                currentlyGrabbed.onRelease.Invoke();
                currentlyGrabbed.isGrabbed = false;
                onRelease.Invoke();
                currentlyGrabbed = null;
            }
            PizzaModeManager.singleton.chuteLight.SetActive(false);
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
            PizzaModeManager.singleton.chuteLight.SetActive(false);
        }

        public void DestroyGrabbed()
        {
            if (currentlyGrabbed)
            {
                Destroy(currentlyGrabbed.gameObject);
                currentlyGrabbed = null;
            }
            PizzaModeManager.singleton.chuteLight.SetActive(false);
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