using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteractor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       //Debug.Log(collision.gameObject);
        collision.gameObject.GetComponent<IInteractable>()?.Interact(gameObject);
    }
}
