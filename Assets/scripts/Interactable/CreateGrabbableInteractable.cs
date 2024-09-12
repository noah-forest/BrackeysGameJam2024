using Grabbing;
using UnityEngine;
namespace Interact
{
    public class CreateGrabbableInteractable : PizzaModeInteractable
    {
        public Grabbable grabbablePrefab;
        public override void Interact(GameObject gameObject)
        {
            base.Interact(gameObject);

            Grabber grabber = gameObject.GetComponent<Grabber>();

            if (grabber != null && grabbablePrefab.tag != grabber.GetCurrentlyGrabbed()?.tag)
            {
                var newObj = Instantiate(grabbablePrefab);
                grabber.Grab(newObj);
            }
        }
    }
}