using UnityEngine;
using Interact;

public interface IInteractable
{
    public void Interact(GameObject interactor);
    public bool CanInteract(GameObject interactor);
    /// <summary>
    /// Onlook should typically enable any custom visuals you want to show when the player is looking at this interactable
    /// </summary>
    /// <returns></returns>
    public void OnLook();
    /// <summary>
    /// Typically onLookaway should be used to hide whatever visuals were shown via onlook
    /// </summary>
    public void OnLookAway();

    public string GetDisplayString();
}
