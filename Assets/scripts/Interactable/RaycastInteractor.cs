using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Interact
{

    public class RaycastInteractor : MonoBehaviour
    {
        public LayerMask layerMask;
        void Update()
        {
            // on mouse down
            if (Input.GetMouseButtonDown(0))
            {
                // raycast from camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, layerMask, QueryTriggerInteraction.Ignore))
                {
                    IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                    if (interactable != null && interactable.CanInteract(this.gameObject))
                    {
                        interactable.Interact(this.gameObject);
                    }
                }
            }
        }
    }

}