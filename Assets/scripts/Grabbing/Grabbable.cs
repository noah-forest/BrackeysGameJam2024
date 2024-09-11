using Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Grabbing
{
    public class Grabbable : TriggerInteractor
    {
        public bool isGrabbed = false;
        public UnityEvent onRelease = new UnityEvent();
        public UnityEvent onGrab = new UnityEvent();
        public bool canGrab = true;
        public bool handMustBeEmpty = false;
        public Vector3 offset;
        public string tag = "";

        public void Start()
        {
            onRelease.AddListener(() => { isGrabbed = false; });
            onGrab.AddListener(() => { isGrabbed = true; });
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