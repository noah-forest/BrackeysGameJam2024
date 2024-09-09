using Grabbing;
using Interact;
using UnityEngine;

public class PlacePizzaDough : MonoBehaviour
{
    public Pizza pizzaPrefab;
    public Pizza currentlyPlacedPizza;

    public Material pizzaSauceMaterial;
    public Material pizzaSauceCheeseMaterial;

    public void DisableColliders()
    {
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void EnableColliders()
    {
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = true;
        }
    }

    void ClearPizza()
    {
        if (currentlyPlacedPizza)
        {
            currentlyPlacedPizza.GetComponent<Grabbable>().onGrab.RemoveListener(ClearPizza);
            currentlyPlacedPizza = null;
            EnableColliders();
        }
    }

    private void PlaceNewPizza()
    {
        currentlyPlacedPizza = Instantiate(pizzaPrefab);
        currentlyPlacedPizza.GetComponent<Grabbable>().onGrab.AddListener(ClearPizza);
        currentlyPlacedPizza.transform.position = transform.position;
        currentlyPlacedPizza.gameObject.SetActive(true);

        DisableColliders();
    }

    public void PlacePizza(GameObject interactor)
    {
        Grabber grabber = interactor.GetComponent<Grabber>();

        if (!currentlyPlacedPizza && grabber.GetCurrentlyGrabbed()?.tag == "PizzaDough")
        {
            PlaceNewPizza();
            grabber.DestroyGrabbed();
        }

        if (!currentlyPlacedPizza && grabber.GetCurrentlyGrabbed()?.tag == "pizza")
        {
            currentlyPlacedPizza = grabber.GetCurrentlyGrabbed().GetComponent<Pizza>();
            currentlyPlacedPizza.GetComponent<Grabbable>().onGrab.AddListener(ClearPizza);
            grabber.ReleaseCurrentlyGrabbed();
            currentlyPlacedPizza.transform.position = transform.position;
            currentlyPlacedPizza.transform.rotation = transform.rotation;
            DisableColliders();
        }
    }
}
