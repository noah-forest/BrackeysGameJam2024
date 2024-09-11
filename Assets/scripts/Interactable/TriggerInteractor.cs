using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interact
{
    public class TriggerInteractor : MonoBehaviour, IInteractable
    {
        public UnityEvent<GameObject> onInteract;
        public float interactionRange = 2f;

        public bool CanInteract(GameObject gameObject)
        {
            if (Vector3.Distance(transform.position, gameObject.transform.position) < interactionRange)
            {
                return true;
            }

            return false;
        }

        public virtual void Interact(GameObject gameObject)
        {
            this.onInteract.Invoke(gameObject);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}