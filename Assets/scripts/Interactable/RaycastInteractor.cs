using UnityEngine;
using UnityEngine.Events;

namespace Interact
{

    public class RaycastInteractor : MonoBehaviour
    {
        public UnityEvent<GameObject> onInteract = new();
        public LayerMask layerMask;
        [SerializeField] Transform head;
        [SerializeField] float distance;
        void Update()
        {
            // on mouse down
            if (Input.GetMouseButtonDown(0))
            {
                // raycast from camera
                
                RaycastHit hit;
                if (Physics.Raycast(head.position, head.forward, out hit, distance, layerMask))
                {
                    IInteractable[] interactables = hit.collider.gameObject.GetComponents<IInteractable>();

                    foreach (var interactable in interactables)
                    {
                        if (interactable != null && interactable.CanInteract(this.gameObject))
                        {
                            interactable?.Interact(this.gameObject);
                            onInteract.Invoke(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }

}