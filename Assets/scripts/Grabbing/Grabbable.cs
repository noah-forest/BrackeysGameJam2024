using UnityEngine;

namespace Grabbing
{
    public class Grabbable : MonoBehaviour
    {
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