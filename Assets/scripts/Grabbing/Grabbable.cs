using UnityEngine;
using UnityEngine.Events;

namespace Grabbing
{
    public class Grabbable : MonoBehaviour
    {
        public UnityEvent onRelease;
        public UnityEvent onGrab;
        public bool enabled = true;
        public Vector3 offset;
        public string tag = "";

        public void Grab(GameObject grabber)
        {
            Grabber grabberComp = grabber.GetComponent<Grabber>();

            if (grabberComp != null)
            {
                grabberComp.Grab(this);
            }
        }

        public void Grab()
        {
            onGrab.Invoke();
            Collider collider = GetComponent<Collider>();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
        }

        public void Release()
        {
            onRelease.Invoke();
            Collider collider = GetComponent<Collider>();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (collider != null)
            {
                collider.enabled = true;
            }
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }
        }
    }

}