using Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Grabbing
{
    public class Grabbable : TriggerInteractor
    {
        public UnityEvent onRelease;
        public UnityEvent onGrab;
        public bool canGrab = true;
        public bool handMustBeEmpty = false;
        public Vector3 offset;
        public string tag = "";

        public override void Interact(GameObject gameObject)
        {
            base.Interact(gameObject);
        }

        public virtual bool CanGrab(Grabber grabber)
        {
            if (handMustBeEmpty && grabber.GetCurrentlyGrabbed() != null)
            {
                return false;
            }
            return canGrab;
        }
    }

}