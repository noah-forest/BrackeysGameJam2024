using UnityEngine;
using Interact;
using GameObject = Interact.GameObject;

public interface IInteractable
{
    public void Interact(GameObject gameObject);
    public bool CanInteract(GameObject gameObject);
}
