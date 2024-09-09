using Grabbing;
using UnityEngine;
namespace Interact
{
    public class CreateGrabbableInteractable : TriggerInteractor
    {
        public Grabbable grabbablePrefab;
        public override void Interact(GameObject gameObject)
        {
            base.Interact(gameObject);

            Grabber grabber = gameObject.GetComponent<Grabber>();

            if (grabber != null && grabbablePrefab.tag != grabber.GetGrabbed()?.tag)
            {
                var newObj = Instantiate(grabbablePrefab);
                grabber.Grab(newObj);
            }
        }
    }
}