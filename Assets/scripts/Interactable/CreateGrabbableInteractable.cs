using Grabbing;
using UnityEngine;
namespace Interact
{
    public class CreateGrabbableInteractable: RangeInteractable
    {
        public Grabbable grabbablePrefab;
        public override void Interact(GameObject gameObject)
        {
            Grabber grabber = gameObject.GetComponent<Grabber>();

            if (grabber != null)
            {
                var newObj = Instantiate(grabbablePrefab);
                grabber.Grab(newObj);
            }
        }
    }
}