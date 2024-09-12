using Interact;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Grabbing
{
    public class Grabbable : PizzaModeInteractable
    {
        public bool isGrabbed = false;
        public UnityEvent onRelease = new UnityEvent();
        public UnityEvent onGrab = new UnityEvent();
        public bool canGrab = true;
        public bool handMustBeEmpty = false;
        public Vector3 offset;
        public Vector3 rotationOffset;
        //public Vector3 scaleOffset;
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