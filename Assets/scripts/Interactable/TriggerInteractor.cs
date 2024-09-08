using UnityEngine;
using UnityEngine.Events;

namespace Interact
{
    [RequireComponent(typeof(Collider))]
    public class TriggerInteractor: MonoBehaviour, IInteractable
    {
        public UnityEvent<GameObject> onInteract;
        private bool playerInCollider = false;
        
        public bool CanInteract(GameObject gameObject)
        {
            return playerInCollider;
        }

        public virtual void Interact(GameObject gameObject)
        {
            this.onInteract.Invoke(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInCollider = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInCollider = false;
            }
        }
    }
}