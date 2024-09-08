using Grabbing;
using UnityEngine;
namespace Interact
{
    public class CreateGrabbableInteractable: RangeInteractable
    {
        public Grabbable grabbablePrefab;
        public override void Interact(GameObject interactor)
        {
            Grabber grabber = interactor.GetComponent<Grabber>();

            if (grabber != null)
            {
                var newObj = Instantiate(grabbablePrefab);
                grabber.Grab(newObj);
            }
        }
    }
}