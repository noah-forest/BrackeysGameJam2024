using UnityEngine;
using Interact;

public interface IInteractable
{
    public void Interact(Interactor interactor);
    public bool CanInteract(Interactor interactor);
}
