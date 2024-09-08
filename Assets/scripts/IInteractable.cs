using UnityEngine;
using Interact;

public interface IInteractable
{
    public void Interact(GameObject interactor);
    public bool CanInteract(GameObject interactor);
}
