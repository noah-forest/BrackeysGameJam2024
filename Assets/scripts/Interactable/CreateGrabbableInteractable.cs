using Grabbing;
using UnityEngine;
namespace Interact
{
    public class CreateGrabbableInteractable: RangeInteractable
    {
        public Grabbable grabbablePrefab;
        public override void Interact(Interactor interactor)
        {
            Instantiate(grabbablePrefab);
        }
    }
}